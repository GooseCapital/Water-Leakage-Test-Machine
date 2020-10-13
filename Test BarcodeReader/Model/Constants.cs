using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Windows.Documents;

namespace Test_BarcodeReader.Model
{
    public class Constants
    {
        public enum ErrorCode
        {
            Success, NotOpened
        }

        public const string TableName = "testhistory";
        public const int StationId = 101;
        public const string TurnOffText = "Đang tắt";
        public const string TurnOnText = "Đang mở";
        public const string Valve3Way_Way1 = "Ngả 1";
        public const string Valve3Way_Way2 = "Ngả 2";

        public const string ProductCodeFileName = "ProductCode.txt";
        public const string ConfigFileName = "config.json";
        // public static List<string> ModelNameList = new List<string>() {"Multi", "Slim", "Slimv2"};

        public const string OkButtonDevice = "X015";
        public const string EntryButtonDevice = "X016";
        public static List<string> ModelDeviceList = new List<string>() {"M100", "M101", "M102"};
    }
}
