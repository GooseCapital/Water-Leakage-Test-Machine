using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using ActUtlTypeLib;
using Newtonsoft.Json;
using Test_BarcodeReader.Controller;
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
            LvProducts.ItemsSource = ProductsList;
        }

        #region Global Variables

        public PLCCommand Plc = new PLCCommand(Constants.StationId);
        public int ModelSelected = 0;
        public AdvancedForm AdvancedForm = new AdvancedForm();
        public bool IsNewBarcode = false;
        private ObservableCollection<Product> ProductsList = new ObservableCollection<Product>();
        public ConfigurationInfo Configuration = new ConfigurationInfo();
        public Product Product = null;
        public bool IsCheckingProduct = false;

        #endregion

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            AdvancedForm.Close();
            Plc.Plc.Close();
            Environment.Exit(0);
        }

        private bool ImportData(ref ConfigurationInfo configuration)
        {
            if (File.Exists(Constants.ConfigFileName))
            {
                try
                {
                    configuration =
                        JsonConvert.DeserializeObject<ConfigurationInfo>(File.ReadAllText(Constants.ConfigFileName));
                    Product.ProductCodeList = configuration.ProductCode;
                    DataSql.TestID = configuration.TestID;
                    DataSql.TestMachine = configuration.TestMachine;
                    configuration.MySqlConnection.ConnectMySql();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //AdvancedForm.Show();
            // File.WriteAllText(Constants.ConfigFileName, JsonConvert.SerializeObject(configuration, Formatting.Indented));
            if (!ImportData(ref Configuration))
            {
                MessageBox.Show("Lỗi trong quá trình nạp dữ liệu");
                Environment.Exit(0);
            }

            // Product product = new Product("3520018-V01234567890123");
            // product.AnalyzeBarcode();
            // DataSql data = new DataSql(product);
            // SqlCommand.SendProduct(data, Configuration.MySqlConnection);

            // nts.ConfigFileName, JsonConvert.SerializeObject(configuration, Formatting.Indented));
            // if (!Product.ImportProductCode())
            // {
            //     MessageBox.Show("Chưa có ProductCode mẫu");
            //     Environment.Exit(0);
            // }

            if (Plc.Error == Constants.ErrorCode.NotOpened)
            {
                MessageBox.Show("Kết nối PLC không thành công");
                Environment.Exit(0);
            }

            Task.Run(UpdateData);
            

            // Product product = new Product("319507-V084202040500002");
            // product.ConvertBarcodeToObject();
        }

        private void ReSendSql()
        {
            List<Product> products = ProductsList.Where(n => n.SentToServer == false).ToList();
            // if (!IsCheckingProduct)
            // {
            //     for (int i = 0; i < products.Count; i++)
            //     {
            //         SendToSql(products[i], false);
            //         CollectionViewSource.GetDefaultView(ProductsList).Refresh();
            //     }
            // }

            for (int i = 0; i < products.Count; i++)
            {
                SendToSql(products[i], false);
                // Thread.Sleep(1000);
            }
        }

        public void UpdateData()
        {
            while (true)
            {
                bool visible = false;
                BtnStart.Dispatcher.Invoke(() => visible = BtnStart.Visibility != Visibility.Hidden);
                if (visible)
                {
                    bool model1 = UpdateModelSelectUI(BtnModel1, Constants.ModelDeviceList[0], 1);
                    bool model2 = UpdateModelSelectUI(BtnModel2, Constants.ModelDeviceList[1], 2);
                    bool model3 = UpdateModelSelectUI(BtnModel3, Constants.ModelDeviceList[2], 3);
                    if (!model1 && !model2 && !model3)
                    {
                        ModelSelected = 0;
                        BtnStart.Dispatcher.Invoke(() => BtnStart.Content = $"Please Scan Product");
                    }
                    if (ModelSelected != 0)
                    {
                        BtnStart.Dispatcher.Invoke(() => BtnStart.Content = $"Start Auto - Model {ModelSelected}");
                    }

                    if (Plc.CheckDevice("M1") || Plc.CheckDevice("M2"))
                    {
                        BtnStart.Dispatcher.Invoke(() => BtnStart.Background = Brushes.Green);
                    }
                    else
                    {
                        BtnStart.Dispatcher.Invoke(() =>
                            BtnStart.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221)));
                    }
                }

                ReSendSql();
                // this.Dispatcher.Invoke(() => LvProducts.ItemsSource = ProductsList);
                CheckProductResult();
                TblNumberCheckedProduct.Dispatcher.Invoke(() => TblNumberCheckedProduct.Text = ProductsList.Count.ToString());
                Thread.Sleep(100);
            }
        }

        private bool UpdateModelSelectUI(Button button, string device, int model)
        {
            try
            {
                if (Plc.CheckDevice(device))
                {
                    button.Dispatcher.Invoke(() =>
                    {
                        button.Background = Brushes.Green;
                    });
                    if (model != 0)
                    {
                        ModelSelected = model;
                    }

                    return true;
                }
                else
                {
                    button.Dispatcher.Invoke(() =>
                    {
                        button.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
                    });
                    return false;
                }
            }
            catch (Exception e)
            {
                //pass
            }

            return false;
        }

        #region Button

        private void BtnModel1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Plc.SetDevice("M300", true);
        }

        private void BtnModel1_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Plc.SetDevice("M300", false);
        }

        private void BtnModel2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Plc.SetDevice("M301", true);
        }

        private void BtnModel2_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Plc.SetDevice("M301", false);
        }

        private void BtnModel3_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Plc.SetDevice("M302", true);
        }

        private void BtnModel3_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Plc.SetDevice("M302", false);
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
                        Plc.SetDevice("M1", true);
                        break;
                    }
                case 2:
                case 3:
                    {
                        Plc.SetDevice("M2", true);
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

        #endregion

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IsNewBarcode = true;
                Product = new Product(TbSerialCode.Text);
                bool isCorrect = Product.AnalyzeBarcode();
                if (isCorrect)
                {
                    TblTextBarcode.Text = $@"Barcode: {TbSerialCode.Text}";
                    ChooseModel(Product.Type);
                    IsCheckingProduct = true;
                }
                else
                {
                    Product = null;
                }
            }
            else
            {
                TbSerialCode.Focus();
            }
            if (IsNewBarcode)
            {
                TbSerialCode.Text = "";
                IsNewBarcode = false;
            }
        }

        private void CheckProductResult()
        {
            if (Product != null)
            {
                if (Plc.CheckDevice(Constants.OkButtonDevice))
                {
                    Product.IsGoodProduct = true;
                    SendToSql(Product);
                    Product = null;
                }

                if (Plc.CheckDevice(Constants.EntryButtonDevice))
                {
                    Product.IsGoodProduct = false;
                    SendToSql(Product);
                    Product = null;
                }
            }
        }

        private void SendToSql(Product product, bool insertToListView = true)
        {
            DataSql data = new DataSql(product);
            product.SentToServer = SqlCommand.SendProduct(data, Configuration.MySqlConnection);
            product.No = ProductsList.Count + 1;
            if (insertToListView)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() => ProductsList.Insert(0, product)));
                TblTextBarcode.Dispatcher.Invoke(() => TblTextBarcode.Text = "Please Scan Product");
                IsCheckingProduct = false;
            }
        }

        // private bool IsDuplicateProduct(Product product)
        // {
        //     for (int i = 0; i < ProductsList.Count; i++)
        //     {
        //         if (product == ProductsList[i])
        //         {
        //             return true;
        //         }
        //     }
        //     
        //     return false;
        // }

        private void ChooseModel(int type)
        {
            switch (type)
            {
                case 0:
                    {
                        Plc.SetDevice("M300", true);
                        Thread.Sleep(100);
                        Plc.SetDevice("M300", false);
                        break;
                    }
                case 1:
                    {
                        Plc.SetDevice("M301", true);
                        Thread.Sleep(100);
                        Plc.SetDevice("M301", false);
                        break;
                    }
                case 2:
                    {
                        Plc.SetDevice("M302", true);
                        Thread.Sleep(100);
                        Plc.SetDevice("M302", false);
                        break;
                    }
            }
        }
    }
}
