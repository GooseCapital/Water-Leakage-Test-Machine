using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Test_BarcodeReader.Controller;
using Test_BarcodeReader.Model;

namespace Test_BarcodeReader
{
    /// <summary>
    /// Interaction logic for Advanced_Form.xaml
    /// </summary>
    public partial class AdvancedForm : Window
    {
        public AdvancedForm()
        {
            InitializeComponent();
        }

        #region Global Variables

        // public bool StatusPressurePump = false;
        // public bool StatusAirPush2 = false;
        // public bool StatusAirPush1 = false;
        // public bool StatusFlowrateValve = false;
        // public bool StatusPressureOut2 = false;
        // public bool StatusPressureIn2 = false;
        // public bool StatusWaterPressure = false;
        // public bool StatusWaterPump = false;
        // public bool StatusValve3Way = false;
        public PLCCommand Plc = new PLCCommand(Constants.StationId);
        public DeviceInfo PressurePumpDeviceInfo = new DeviceInfo("M506", "M602", "Y006", "Y006");
        public DeviceInfo AirPush2DeviceInfo = new DeviceInfo("M516", "M516", "Y016", "Y016");
        public DeviceInfo AirPush1DeviceInfo = new DeviceInfo("M515", "M515", "Y015", "Y015");
        public DeviceInfo FlowrateValveDeviceInfo = new DeviceInfo("M526", "M527", "Y026", "Y027");
        public DeviceInfo PressureOut2DeviceInfo = new DeviceInfo("M513", "M514", "Y013", "Y014");
        public DeviceInfo PressureIn2DeviceInfo = new DeviceInfo("M511", "M512", "Y011", "Y012");
        public DeviceInfo WaterPressureDeviceInfo = new DeviceInfo("M524", "M525", "Y024", "Y025");
        public DeviceInfo WaterPumpDeviceInfo = new DeviceInfo("M517", "M517", "Y017", "Y017");
        public DeviceInfo Valve3WayDeviceInfo = new DeviceInfo("M522", "M523", "Y022", "Y023");
        public DeviceInfo JigProductDeviceInfo = new DeviceInfo("M520", "M521", "Y020", "Y021");

        #endregion

        #region Get and change status device

        private void ChangeStatusDevice(string deviceOpen, string deviceClose, ref bool status)
        {
            if (status)
            {
                // Device is on - turn off
                Plc.SetDevice(deviceClose, true);
                Thread.Sleep(50);
                Plc.SetDevice(deviceClose, false);
            }
            else
            {
                //Turn On
                Plc.SetDevice(deviceOpen, true);
                Thread.Sleep(50);
                Plc.SetDevice(deviceOpen, false);
            }
        }

        private void ChangeStatusValve(DeviceInfo deviceInfo)
        {
            if (deviceInfo.Status)
            {
                // Device is on - turn off
                Plc.SetDevice(deviceInfo.TurnOffAddress, true);
                Thread.Sleep(50);
                Plc.SetDevice(deviceInfo.TurnOffAddress, false);
            }
            else
            {
                //Turn On
                Plc.SetDevice(deviceInfo.TurnOnAddress, true);
                Thread.Sleep(50);
                Plc.SetDevice(deviceInfo.TurnOnAddress, false);
            }

            deviceInfo.Status = !deviceInfo.Status;
        }

        private void ChangeStatusPump(DeviceInfo deviceInfo)
        {
            if (deviceInfo.Status)
            {
                Plc.SetDevice(deviceInfo.TurnOnAddress, false);
            }
            else
            {
                Plc.SetDevice(deviceInfo.TurnOnAddress, true);
            }

            deviceInfo.Status = !deviceInfo.Status;
        }

        private void GetStatusValve(string deviceOpen, string deviceClose, ref bool status)
        {
            if (Plc.CheckDevice(deviceOpen))
            {
                status = true;
            }

            if (Plc.CheckDevice(deviceClose))
            {
                status = false;
            }
        }

        private void GetStatusValve(DeviceInfo deviceInfo)
        {
            if (Plc.CheckDevice(deviceInfo.CheckOnAddress))
            {
                deviceInfo.Status = true;
            }

            if (Plc.CheckDevice(deviceInfo.CheckOffAddress))
            {
                deviceInfo.Status = false;
            }
        }

        private void GetStatusPump(string device, ref bool status)
        {
            if (Plc.CheckDevice(device))
            {
                status = true;
            }
            else
            {
                status = false;
            }
        }

        private void GetStatusPump(DeviceInfo deviceInfo)
        {
            if (Plc.CheckDevice(deviceInfo.CheckOnAddress))
            {
                deviceInfo.Status = true;
            }
            else
            {
                deviceInfo.Status = false;
            }
        }

        #endregion

        /// <summary>
        /// Task to update status device
        /// </summary>
        public void UpdateDevice()
        {
            while (true)
            {
                GetStatusValve(PressureIn2DeviceInfo);
                GetStatusValve(PressureOut2DeviceInfo);
                GetStatusValve(Valve3WayDeviceInfo);
                GetStatusValve(WaterPressureDeviceInfo);
                GetStatusValve(FlowrateValveDeviceInfo);


                // GetStatusValve("Y011", "Y012", ref StatusPressureIn2);
                // GetStatusValve("Y013", "Y014", ref StatusPressureOut2);
                // // GetStatusValve("Y020", "Y021", Status); //Clamp Status
                // GetStatusValve("Y022", "Y023", ref StatusValve3Way);
                // GetStatusValve("Y024", "Y025", ref StatusWaterPressure);
                // GetStatusValve("Y026", "Y027", ref StatusFlowrateValve);

                GetStatusPump(PressurePumpDeviceInfo);
                GetStatusPump(WaterPumpDeviceInfo);
                GetStatusPump(AirPush1DeviceInfo);
                GetStatusPump(AirPush2DeviceInfo);

                // GetStatusPump("Y006", ref StatusPressurePump);
                // GetStatusPump("Y017", ref StatusWaterPump);
                // GetStatusPump("Y015", ref StatusAirPush1);
                // GetStatusPump("Y016", ref StatusAirPush2);

                // UpdateControl(StPressurePump, StatusPressurePump);
                // UpdateControl(StAirPush2, StatusAirPush2);
                // UpdateControl(StAirPush1, StatusAirPush1);
                // UpdateControl(StFlowrateValve, StatusFlowrateValve);
                // UpdateControl(StPressureOut2, StatusPressureOut2);
                // UpdateControl(StPressureIn2, StatusPressureIn2);
                // UpdateControl(StWaterPressure, StatusWaterPressure);
                // UpdateControl(StWaterPump, StatusWaterPump);
                // UpdateControl(StValve3Way, StatusValve3Way);

                UpdateControl(StPressurePump, PressurePumpDeviceInfo);
                UpdateControl(StAirPush2, BtnAirPush2_2, AirPush2DeviceInfo);
                UpdateControl(StAirPush1, BtnAirPush1_2, AirPush1DeviceInfo);
                UpdateControl(StFlowrateValve, BtnFlowrateValve_2, FlowrateValveDeviceInfo);
                UpdateControl(StPressureOut2, BtnPressureOut2_2, PressureOut2DeviceInfo);
                UpdateControl(StPressureIn2, BtnPressureIn2_2, PressureIn2DeviceInfo);
                UpdateControl(StWaterPressure, BtnWaterPressure_2, WaterPressureDeviceInfo);
                UpdateControl(StWaterPump, WaterPumpDeviceInfo);
                UpdateValve3Way(StValve3Way, BtnValve3Way_2, Valve3WayDeviceInfo);
                UpdateControl(StJigProduct, BtnJigProduct_2, JigProductDeviceInfo);
                // Thread.Sleep(50);
            }
        }

        #region Update control and overload

        public void UpdateControl(Ellipse ellipse, bool status)
        {
            if (status)
            {
                ellipse.Dispatcher.Invoke(() => ellipse.Fill = new SolidColorBrush(Colors.Green));
            }
            else
            {
                ellipse.Dispatcher.Invoke(() => ellipse.Fill = new SolidColorBrush(Colors.Red));
            }
        }

        public void UpdateControl(Ellipse ellipse, DeviceInfo deviceInfo)
        {
            if (deviceInfo.Status)
            {
                ellipse.Dispatcher.Invoke(() => ellipse.Fill = new SolidColorBrush(Colors.Green));
            }
            else
            {
                ellipse.Dispatcher.Invoke(() => ellipse.Fill = new SolidColorBrush(Colors.Red));
            }
        }

        public void UpdateControl(Button button, DeviceInfo deviceInfo)
        {
            if (deviceInfo.Status)
            {
                button.Dispatcher.Invoke(() => button.Content = Constants.TurnOnText);
            }
            else
            {
                button.Dispatcher.Invoke(() => button.Content = Constants.TurnOffText);
            }
        }

        public void UpdateControl(Ellipse ellipse, Button button, DeviceInfo deviceInfo)
        {
            if (deviceInfo.Status)
            {
                ellipse.Dispatcher.Invoke(() => ellipse.Fill = new SolidColorBrush(Colors.Green));
                button.Dispatcher.Invoke(() => button.Content = Constants.TurnOnText);
            }
            else
            {
                ellipse.Dispatcher.Invoke(() => ellipse.Fill = new SolidColorBrush(Colors.Red));
                button.Dispatcher.Invoke(() => button.Content = Constants.TurnOffText);
            }
        }

        public void UpdateValve3Way(Ellipse ellipse, Button button, DeviceInfo deviceInfo)
        {
            if (deviceInfo.Status)
            {
                ellipse.Dispatcher.Invoke(() => ellipse.Fill = new SolidColorBrush(Colors.Green));
                button.Dispatcher.Invoke(() => button.Content = Constants.Valve3Way_Way1);
            }
            else
            {
                ellipse.Dispatcher.Invoke(() => ellipse.Fill = new SolidColorBrush(Colors.Red));
                button.Dispatcher.Invoke(() => button.Content = Constants.Valve3Way_Way2);
            }
        }

        #endregion

        #region UI Render Function

        private void AdvancedMode_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(UpdateDevice);
        }

        private void AdvancedMode_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void BtnPressurePump_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusValve(PressurePumpDeviceInfo);
        }

        private void BtnValve3Way_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusValve(Valve3WayDeviceInfo);
        }

        private void BtnAirPush2_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusPump(AirPush2DeviceInfo);
        }

        private void BtnAirPush1_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusPump(AirPush1DeviceInfo);
        }

        private void BtnFlowrateValve_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusValve(FlowrateValveDeviceInfo);
        }

        private void BtnPressureOut2_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusValve(PressureOut2DeviceInfo);
        }

        private void BtnPressureIn2_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusValve(PressureIn2DeviceInfo);
        }

        private void BtnWaterPressure_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusValve(WaterPressureDeviceInfo);
        }

        private void BtnWaterPump_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusPump(WaterPumpDeviceInfo);
        }

        private void BtnPressureIn2_2_Click(object sender, RoutedEventArgs e)
        {
            BtnPressureIn2_Click(this, null);
        }

        private void BtnPressureOut2_2_Click(object sender, RoutedEventArgs e)
        {
            BtnPressureOut2_Click(this, null);
        }

        private void BtnAirPush1_2_Click(object sender, RoutedEventArgs e)
        {
            BtnAirPush1_Click(this, null);
        }

        private void BtnAirPush2_2_Click(object sender, RoutedEventArgs e)
        {
            BtnAirPush2_Click(this, null);
        }

        private void BtnWaterPressure_2_Click(object sender, RoutedEventArgs e)
        {
            BtnWaterPressure_Click(this, null);
        }

        private void BtnFlowrateValve_2_Click(object sender, RoutedEventArgs e)
        {
            BtnFlowrateValve_Click(this, null);
        }

        private void BtnJig_Click(object sender, RoutedEventArgs e)
        {
            BtnJigProduct_Click(this, null);
        }

        private void BtnJigProduct_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusValve(JigProductDeviceInfo);
        }

        private void BtnValve3Way_2_Click(object sender, RoutedEventArgs e)
        {
            BtnValve3Way_Click(this, null);
        }

        #endregion
    }
}
