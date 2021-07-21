using SquirtingElephant.Helpers;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace SquirtingElephant.ConfigurableRoomStats
{
    public class SE_Settings : Mod
    {
        #region Fields
        
        public static SettingsData Settings;

        private const float SCROLL_AREA_OFFSET_TOP = BUTTON_HEIGHT + ROW_HEIGHT;
        private const float COL_WIDTH = 350f;
        private const float SLIDER_COL_WIDTH = 300f;
        private const float ROW_HEIGHT = 32f;
        private const float BUTTON_Y = 0f;
        private const float BUTTON_HEIGHT = ROW_HEIGHT;
        public static TableData Table = new TableData(
            new Vector2(50f, ROW_HEIGHT * 2),
            new Vector2(0f, 2f),
            new float[] { COL_WIDTH, SLIDER_COL_WIDTH },
            new float[] { ROW_HEIGHT },
            -1,
            44);

        // For scrolling:
        private Vector2 ScrollPosition = Vector2.zero;
        private Rect ViewRect = new Rect(0.0f, 0.0f, 100.0f, 200.0f);
        
        #endregion

        public SE_Settings(ModContentPack content) : base(content) { Settings = GetSettings<SettingsData>(); }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();
            ls.Begin(inRect);

            CreateTopButtons(inRect.width);
            Widgets.Label(new Rect(0f, BUTTON_HEIGHT, inRect.width, ROW_HEIGHT), "SECRS_AllSettingsAppliedLive".TC());

            ls.BeginScrollView(new Rect(0, SCROLL_AREA_OFFSET_TOP, inRect.width, inRect.height - SCROLL_AREA_OFFSET_TOP), ref ScrollPosition, ref ViewRect);

            ls.GetRect(Table.Bottom); // Ensures the scrollview works.
            CreateAllSettings();

            ls.EndScrollView(ref ViewRect);
            ls.End();

            Main.ApplySettingsToDefs();
        }
        
        private void CreateAllSettings()
        {
            int rowIdx = 0;
            foreach (EStatType statType in Settings.StatData.Keys)
            {
                Widgets.Label(Table.GetFieldRect(0, rowIdx++), statType.HeaderKey().TC());

                foreach (StatData sd in Settings.StatData[statType])
                    MakeInputs(rowIdx++, sd.TranslationKey, ref sd.Value, sd.MinValue, sd.MaxValue);

                if (Widgets.ButtonText(Table.GetFieldRect(0, rowIdx++).RightHalf(), "SECRS_Reset".TC()))
                    Settings.Reset(statType);

                rowIdx++;
            }
        }

        private void CreateTopButtons(float menuWidth)
        {
            float dividedWidth = menuWidth / 4;
            if (Widgets.ButtonText(new Rect(0, BUTTON_Y, dividedWidth, BUTTON_HEIGHT), "SECRS_ApplyPreset".TC()))
            {
                Find.WindowStack.Add(new FloatMenu(new List<FloatMenuOption>
                {
                    new FloatMenuOption("SECRS_PresetVanilla".TC(), () => ChosePreset(0)),
                    new FloatMenuOption("SECRS_PresetRealistic".TC(), () => ChosePreset(1)),
                    new FloatMenuOption("SECRS_PresetMin".TC(), () => ChosePreset(2)),
                    new FloatMenuOption("SECRS_PresetMax".TC(), () => ChosePreset(3)),
                    new FloatMenuOption("SECRS_PresetCustom1".TC(), () => ChosePreset(4)),
                    new FloatMenuOption("SECRS_PresetCustom2".TC(), () => ChosePreset(5))
                }));
            }

            if (Widgets.ButtonText(new Rect(dividedWidth, BUTTON_Y, dividedWidth, BUTTON_HEIGHT), "SECRS_SavePreset".TC()))
            {
                Find.WindowStack.Add(new FloatMenu(
                    new List<FloatMenuOption>()
                    {
                        new FloatMenuOption(string.Format("{0} 1", "SECRS_SavePreset".TC()), () => Settings.SaveToPreset(1)),
                        new FloatMenuOption(string.Format("{0} 2", "SECRS_SavePreset".TC()), () => Settings.SaveToPreset(2))
                    }));
            }

            if (Widgets.ButtonText(new Rect(dividedWidth * 2, BUTTON_Y, dividedWidth, BUTTON_HEIGHT), "SECRS_OpenModSettingsFolder".TC()))
                System.Diagnostics.Process.Start(Utils.GetModSettingsFolderPath());

            if (Widgets.ButtonText(new Rect(dividedWidth * 3, BUTTON_Y, dividedWidth, BUTTON_HEIGHT), "SECRS_ResetAll".TC()))
                Settings.ResetAll();
        }

        private void ChosePreset(int preset)
        {
            if (Settings.StatData == null)
            {
                Log.Error("ChosePreset() detected a null-value for Settings.StatData.");
                return;
            }

            foreach (var kvp in Settings.StatData)
                kvp.Value.ForEach(s => s.ApplyPreset(preset));
        }

        private void MakeInputs(int rowIdx, string translationKey, ref float setting, float min, float max)
        {
            string buffer = setting.ToString();
            Widgets.TextFieldNumericLabeled(Table.GetFieldRect(0, rowIdx), translationKey.Translate().CapitalizeFirst() + " ", ref setting, ref buffer, min, max);

            float count = Widgets.HorizontalSlider(Table.GetFieldRect(1, rowIdx), setting, min, max);
            if (count != setting)
                setting = count;
        }

        public override string SettingsCategory() => "SECRS_SettingsCategory".Translate();
    }
}
