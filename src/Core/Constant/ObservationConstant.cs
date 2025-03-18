using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Constant
{
    public static class ObservationConstant
    {
        public static int THANH_CONG = 200;
        public static int THAT_BAI = 500;
        public static string PREFIX_DO_MUA_HOA_BINH = "AQRLG";
        public static string MA_TRAM_MUA_HOA_BINH = "MUA_HOABINH";

        public static Dictionary<string, (int, int)> PARAMS_API_REPORT = new Dictionary<string, (int, int)>()
        {
            {"5min", (1, 5) },
            {"10min", (1,10) },
            {"30min", (1,30) },
            {"hour", (1,60) },
            {"3hour", (1,180) },
            {"6hour", (1,360 )},
            {"12hour", (1,720) },
            {"24hour", (1,1440) },
            {"1day", (2,1) },
            {"2day", (2,2) },
            {"3day", (2,3) },
            {"4day", (2,4) },
            {"5day", (2,5) },
            {"6day", (2,6) },
            {"7day", (2,7) },
        };
    }
}
