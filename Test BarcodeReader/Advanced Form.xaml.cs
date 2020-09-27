using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
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

        public bool StatusPressurePump = false;
        public bool StatusAirPush2 = false;
        public bool StatusAirPush1 = false;
        public bool StatusFlowrateValve = false;
        public bool StatusPressureOut2 = false;
        public bool StatusPressureIn2 = false;
        public bool StatusWaterPressure = false;
        public bool StatusWaterPump = false;
        public bool StatusValve3Way = false;
        public PLCCommand Plc = new PLCCommand(Constants.StationId);

        #endregion

        private void BtnPressurePump_Click(object sender, RoutedEventArgs e)
        {
            if (StatusPressurePump)
            {
                //Turn off
                Plc.SetDevice("M601", true);
                Thread.Sleep(100);
                Plc.SetDevice("M601", false);
            }
            else
            {
                //Turn On
                Plc.SetDevice("M506", true);
                Thread.Sleep(100);
                Plc.SetDevice("M506", false);
            }
        }

        public void GetStatusValve(string deviceOpen, string deviceClose, ref bool status)
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

        public void GetStatusPump(string device, ref bool status)
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

        public void UpdateDevice()
        {
            while (true)
            {
                GetStatusValve("Y011", "Y012", ref StatusPressureIn2);
                GetStatusValve("Y013", "Y014", ref StatusPressureOut2);
                // GetStatusValve("Y020", "Y021", Status); //Clamp Status
                GetStatusValve("Y022", "Y023", ref StatusValve3Way);
                GetStatusValve("Y024", "Y025", ref StatusWaterPressure);
                GetStatusValve("Y026", "Y027", ref StatusFlowrateValve);

                GetStatusPump("Y006", ref StatusPressurePump);
                GetStatusPump("Y017", ref StatusWaterPump);
                GetStatusPump("Y015", ref StatusAirPush1);
                GetStatusPump("Y016", ref StatusAirPush2);

                UpdateControl(StPressurePump, StatusPressurePump);
                UpdateControl(StAirPush2, StatusAirPush2);
                UpdateControl(StAirPush1, StatusAirPush1);
                UpdateControl(StFlowrateValve, StatusFlowrateValve);
                UpdateControl(StPressureOut2, StatusPressureOut2);
                UpdateControl(StPressureIn2, StatusPressureIn2);
                UpdateControl(StWaterPressure, StatusWaterPressure);
                UpdateControl(StWaterPump, StatusWaterPump);
                UpdateControl(StValve3Way, StatusValve3Way);
                Thread.Sleep(100);
            }
        }

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

        private void AdvancedMode_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(UpdateDevice);
        }

        private void AdvancedMode_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }
    }
}
