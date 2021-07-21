using System.Collections.Generic;

namespace SquirtingElephant.ConfigurableRoomStats
{
    public static class Misc
    {
        private static readonly List<string> HeaderKeys = new List<string>
        {
            "SECRS_RoomSpaceHeader",
            "SECRS_RoomBeautyHeader",
            "SECRS_RoomWealthHeader",
            "SECRS_RoomImpHeader",
            "SECRS_RoomCleanHeader"
        };

        private static readonly List<string> RoomStatDefNames = new List<string>
        {
            "Space",
            "Beauty",
            "Wealth",
            "Impressiveness",
            "Cleanliness"
        };

        public static string HeaderKey(this EStatType statType) => HeaderKeys[(int)statType];
        public static string RoomStatDefName(this EStatType statType) => RoomStatDefNames[(int)statType];
    }
}
