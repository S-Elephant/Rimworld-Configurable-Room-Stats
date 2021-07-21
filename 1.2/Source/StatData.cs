using System.Collections.Generic;
using System.Linq;
using Verse;

namespace SquirtingElephant.ConfigurableRoomStats
{
    public enum EStatType
    {
        Space = 0,
        Beauty = 1,
        Wealth = 2,
        Imp = 3,
        Clean = 4
    }

    public class StatData : IExposable
    {
        private static readonly List<(float, float)> MinMaxConsts = new List<(float, float)>()
        {
            (2f, 2500f), // Space.
            (-1000f, 1000f), // Beauty.
            (1f, 1000000f), // Wealth.
            (1f, 2500f), // Imp.
            (-5f, 1f) // Clean.
        };

        private readonly EStatType _StatType;
        public EStatType StatType { get { return _StatType; } }


        private string _TranslationKey;
        /// <summary>
        /// Note that this is also the expose label.
        /// </summary>
        public string TranslationKey { get { return _TranslationKey; } private set { _TranslationKey = value; } }
        public float Value;
        public float MinValue;
        public float MaxValue;
        private const string PREFIX = "SECRS_";

        /// <summary>
        /// Note that the first value is always the default value.
        /// Vanilla, Realistic, Min, Max, Custom1, Custom2.
        /// </summary>
        public List<float> Presets;

        public float CustomPreset1;
        public float CustomPreset2;

        public StatData(EStatType statType, string translationKeyNoPrefix, params float[] presets)
        {
            _StatType = statType;
            TranslationKey = PREFIX + translationKeyNoPrefix;
            Value = presets[0];
            Presets = presets.ToList();
            CustomPreset1 = CustomPreset2 = Presets[0];
            MinValue = MinMaxConsts[(int)_StatType].Item1;
            MaxValue = MinMaxConsts[(int)_StatType].Item2;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref Value, TranslationKey + "_Value", Presets[0], true);
            Scribe_Values.Look(ref CustomPreset1, TranslationKey + "_CustomPreset1", Presets[0], true);
            Scribe_Values.Look(ref CustomPreset2, TranslationKey + "_CustomPreset2", Presets[0], true);
        }

        public void CreateDefaultPresets()
        {
            Presets.Add(MinValue);
            Presets.Add(MaxValue);
        }

        public void DuplicateVanillaPreset()
        {
            DuplicatePreset(0);
        }

        public void DuplicatePreset(int presetIdx)
        {
            Presets.Add(Presets[presetIdx]);
        }

        public void ApplyPreset(int preset)
        {
            if (preset < 4)
                Value = Presets[preset];
            else if (preset == 4)
                Value = CustomPreset1;
            else
                Value = CustomPreset2;
        }

        public void Reset()
        {
            Value = Presets[0];
        }

        public void SaveToPreset(int customPresetNumber)
        {
            if (customPresetNumber == 1)
                CustomPreset1 = Value;
            else
                CustomPreset2 = Value;
        }
    }
}
