using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActUtlTypeLib;
using Test_BarcodeReader.Model;

namespace Test_BarcodeReader.Controller
{
    public class PLCCommand
    {
        public ActUtlType Plc = new ActUtlType();
        public Constants.ErrorCode Error { get; private set; }
        public PLCCommand(int stationId)
        {
            Plc.ActLogicalStationNumber = stationId;
            if (Plc.Open() != 0)
            {
                Error = Constants.ErrorCode.NotOpened;
            }
        }

        public bool CheckDevice(string device)
        {
            if (Plc.GetDevice(device, out int status) == 0)
            {
                return status == 1;
            }

            return false;
        }

        public int GetDeviceValue(string device)
        {
            int x = Plc.GetDevice(device, out int data);
            return data;
        }

        public bool SetDevice(string device, bool isOn)
        {
             return Plc.SetDevice(device, isOn ? 1 : 0) == 0;
        }
    }
}
