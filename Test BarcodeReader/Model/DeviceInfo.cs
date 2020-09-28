namespace Test_BarcodeReader.Model
{
    public class DeviceInfo
    {
        public DeviceInfo(string turnOnAddress, string turnOffAddress)
        {
            TurnOnAddress = turnOnAddress;
            TurnOffAddress = turnOffAddress;
        }

        public DeviceInfo(string turnOnAddress, string turnOffAddress, string checkOnAddress, string checkOffAddress)
        {
            TurnOnAddress = turnOnAddress;
            TurnOffAddress = turnOffAddress;
            CheckOnAddress = checkOnAddress;
            CheckOffAddress = checkOffAddress;
        }

        public string TurnOnAddress { get; private set; }
        public string TurnOffAddress { get; private set; }
        public bool Status = false;
        public string CheckOnAddress { get; set; }
        public string CheckOffAddress { get; set; }
    }
}
