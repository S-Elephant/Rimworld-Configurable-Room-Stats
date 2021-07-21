using System.Collections.Generic;
using Verse;

namespace SquirtingElephant.ConfigurableRoomStats
{
    public class SettingsData : ModSettings
    {
        public Dictionary<EStatType, List<StatData>> StatData = null;

        private void AddPreset(EStatType statType, params float[] values)
        {
            if (values.Length != StatData[statType].Count)
            {
                Log.Error("Length mismatch in AddPreset(). for type: " + statType.ToString() + ". Got: " + string.Join(", ", values));
                return;
            }

            for (int i = 0; i < StatData[statType].Count; i++)
            {
                float value = (i < values.Length) ? values[i] : values[values.Length - 1];
                StatData[statType][i].Presets.Add(value);
            }
        }

        public void CreateStatData()
        {
            StatData = new Dictionary<EStatType, List<StatData>>()
                    {
                        { EStatType.Space, new List<StatData>() {
                                    new StatData(EStatType.Space, "Space_RatherTight", 12.5f),
                                    new StatData(EStatType.Space, "Space_AverageSized", 29f),
                                    new StatData(EStatType.Space, "Space_SomewhatSpacious", 55f),
                                    new StatData(EStatType.Space, "Space_QuiteSpacious", 70f),
                                    new StatData(EStatType.Space, "Space_VerySpacious", 130f),
                                    new StatData(EStatType.Space, "Space_ExtremelySpacious", 349.5f)
                        }},

                        { EStatType.Beauty, new List<StatData>() {
                                    new StatData(EStatType.Beauty, "Beauty_Ugly", -3.5f),
                                    new StatData(EStatType.Beauty, "Beauty_Neutral", 0f),
                                    new StatData(EStatType.Beauty, "Beauty_Pretty", 2.4f),
                                    new StatData(EStatType.Beauty, "Beauty_Beautiful", 5f),
                                    new StatData(EStatType.Beauty, "Beauty_VeryBeautiful", 15f),
                                    new StatData(EStatType.Beauty, "Beauty_ExtremelyBeautiful", 50f),
                                    new StatData(EStatType.Beauty, "Beauty_UnbelievablyBeautiful", 100f)
                        }},

                        { EStatType.Wealth, new List<StatData>() {
                                    new StatData(EStatType.Wealth, "Wealth_SomewhatPoor", 500f),
                                    new StatData(EStatType.Wealth, "Wealth_Mediocre", 700f),
                                    new StatData(EStatType.Wealth, "Wealth_SomewhatRich", 2000f),
                                    new StatData(EStatType.Wealth, "Wealth_Rich", 4000f),
                                    new StatData(EStatType.Wealth, "Wealth_Luxurious", 10000f),
                                    new StatData(EStatType.Wealth, "Wealth_ExtremelyLuxurious", 100000f),
                                    new StatData(EStatType.Wealth, "Wealth_UnbelievablyLuxurious", 1000000f)
                        }},

                        { EStatType.Imp, new List<StatData>() {
                                    new StatData(EStatType.Imp, "Imp_Dull", 20f),
                                    new StatData(EStatType.Imp, "Imp_Mediocre", 30f),
                                    new StatData(EStatType.Imp, "Imp_Decent", 40f),
                                    new StatData(EStatType.Imp, "Imp_SlightlyImpressive", 50f),
                                    new StatData(EStatType.Imp, "Imp_SomewhatImpressive", 65f),
                                    new StatData(EStatType.Imp, "Imp_VeryImpressive", 85f),
                                    new StatData(EStatType.Imp, "Imp_ExtremelyImpressive", 120f),
                                    new StatData(EStatType.Imp, "Imp_UnbelievablyImpressive", 170f),
                                    new StatData(EStatType.Imp, "Imp_WondrouslyImpressive", 240f)
                        }},

                        { EStatType.Clean, new List<StatData>() {
                                    new StatData(EStatType.Clean, "Clean_Dirty", -1.1f),
                                    new StatData(EStatType.Clean, "Clean_SlightlyDirty", -0.4f),
                                    new StatData(EStatType.Clean, "Clean_Clean", -0.05f),
                                    new StatData(EStatType.Clean, "Clean_Sterile", 0.4f)
                        }},
                    };

            AddPreset(EStatType.Space, 6f, 12f, 28f, 45f, 75f, 175f); // Realistic Room values.

            foreach (var kvp in StatData)
            {
                // Add vanilla values for the Realistic Room presets.
                foreach (StatData sd in kvp.Value)
                {
                    if (sd.StatType != EStatType.Space)
                        sd.DuplicateVanillaPreset();
                }

                kvp.Value.ForEach(s => s.CreateDefaultPresets());
            }
        }

        public override void ExposeData()
        {
            if (StatData == null) { CreateStatData(); }
            foreach (var kvp in StatData)
                kvp.Value.ForEach(s => s.ExposeData());
        }

        public void Reset(EStatType statType)
        {
            StatData[statType].ForEach(s => s.Reset());
        }

        public void ResetAll()
        {
            foreach (var kvp in StatData)
                kvp.Value.ForEach(s => s.Reset());
        }

        public void SaveToPreset(int customPresetNumber)
        {
            foreach (var kvp in StatData)
                kvp.Value.ForEach(s => s.SaveToPreset(customPresetNumber));
        }
    }
}
