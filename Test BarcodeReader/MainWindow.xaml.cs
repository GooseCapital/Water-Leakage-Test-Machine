using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ActUtlTypeLib;
using Test_BarcodeReader.Model;

namespace Test_BarcodeReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Global Variables

        public ActUtlType Plc = new ActUtlType();
        public static bool IsConnected = false;
        public int ModelSelected = 0;
        public AdvancedForm AdvancedForm = new AdvancedForm();

        #endregion

        private void BtnConnection_Click(object sender, RoutedEventArgs e)
        {
            if (!IsConnected)
            {
                Plc.ActLogicalStationNumber = Constants.StationId;
                int x = Plc.Open();
                if (x != 0)
                {
                    MessageBox.Show("Connection Error");
                    return;
                }
                BtnConnection.Content = "Connected";
                IsConnected = true;
            }
            else
            {
                Plc.Close();
                BtnConnection.Content = "Disconnected";
                IsConnected = false;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AdvancedForm.Close();
            Plc.Close();
            Environment.Exit(0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(UpdateData);
        }

        public void UpdateData()
        {
            while (true)
            {
                if (IsConnected)
                {
                    UpdateModelSelectUI(BtnModel1, "M100", 1);
                    UpdateModelSelectUI(BtnModel2, "M101", 2);
                    UpdateModelSelectUI(BtnModel3, "M102", 3);
                    if (ModelSelected != 0)
                    {
                        BtnStart.Dispatcher.Invoke(() => BtnStart.Content = $"Start Auto - Model {ModelSelected}");
                    }

                    if (CheckDevice("M1") || CheckDevice("M2"))
                    {
                        BtnStart.Dispatcher.Invoke(() => BtnStart.Background = Brushes.Green);
                    }
                    else
                    {
                        BtnStart.Dispatcher.Invoke(() =>
                            BtnStart.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221)));
                    }
                }
                System.Threading.Thread.Sleep(100);
            }
        }

        private void UpdateModelSelectUI(Button button, string device, int model)
        {
            try
            {
                if (CheckDevice(device))
                {
                    button.Dispatcher.Invoke(() =>
                    {
                        button.Background = Brushes.Green;
                    });
                    if (model != 0)
                    {
                        ModelSelected = model;
                    }
                }
                else
                {
                    button.Dispatcher.Invoke(() =>
                    {
                        button.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
                    });
                }
            }
            catch (Exception e)
            {
                //pass
            }
        }

        private bool CheckDevice(string device)
        {
            if (Plc.GetDevice(device, out int status) == 0)
            {
                return status == 1;
            }

            return false;
        }

        private void HandleClickEvent(string device, bool isOn)
        {
            if (IsConnected)
            {
                Plc.SetDevice(device, isOn ? 1 : 0);
            }
            else
            {
                MessageBox.Show("Not Connected");
            }
        }

        private void BtnModel1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            HandleClickEvent("M300", true);
        }

        private void BtnModel1_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HandleClickEvent("M300", false);
        }

        private void BtnModel2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            HandleClickEvent("M301", true);
        }

        private void BtnModel2_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HandleClickEvent("M301", false);
        }

        private void BtnModel3_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            HandleClickEvent("M302", true);
        }

        private void BtnModel3_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HandleClickEvent("M302", false);
        }

        private void BtnStart_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            switch (ModelSelected)
            {
                case 1:
                {
                    Plc.SetDevice("M1", 1);
                    break;
                }
                case 2:
                case 3:
                {
                    Plc.SetDevice("M2", 1);
                    break;
                }
                default:
                    return;
            }
        }

        private void BtnAdvanced_Click(object sender, RoutedEventArgs e)
        {
            AdvancedForm.Show();
        }
    }
}
