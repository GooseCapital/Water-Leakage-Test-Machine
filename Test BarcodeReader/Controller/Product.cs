using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Test_BarcodeReader.Model;

namespace Test_BarcodeReader.Controller
{
    public class Product : INotifyPropertyChanged
    {
        public int No { get; set; }
        public string ProductCode { get; private set; }
        public string PlantCode { get; private set; }
        public int Year { get; private set; }
        public int Day { get; private set; }
        public string LineCode { get; private set; }
        public string SerialNumber { get; private set; }
        public int Type { get; private set; }

        // public string TypeName => Constants.ModelNameList[Type];

        private bool _sendToServer = false;

        public bool SentToServer
        {
            get
            {
                return _sendToServer;
            }
            set
            {
                _sendToServer = value;
                RaisePropertyChanged();
            }
        }

        public static List<string> ProductCodeList = new List<string>();


        public string RawBarCode { get; set; }

        public bool IsGoodProduct { get; set; }


        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            Volatile.Read(ref PropertyChanged)?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        // private string _rawBarCode = null;
        private const int NumberProductCode = 3;

        public static int Counting = 1;

        public Product(string productCode, string plantCode, int year, int day, string lineCode, string serialNumber)
        {
            ProductCode = productCode;
            PlantCode = plantCode;
            Year = year;
            Day = day;
            LineCode = lineCode;
            SerialNumber = serialNumber;
        }

        public Product(string rawBarCode)
        {
            RawBarCode = rawBarCode;
        }

        public static bool ImportProductCode()
        {
            if (File.Exists(Constants.ProductCodeFileName))
            {
                ProductCodeList = File.ReadAllLines(Constants.ProductCodeFileName).ToList();
                if (ProductCodeList.Count < 3)
                    return false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AnalyzeBarcode()
        {
            try
            {
                if (ConvertBarcodeToObject())
                {
                    for (int i = 0; i < NumberProductCode; i++)
                    {
                        if (ProductCode == ProductCodeList[i])
                        {
                            Type = i;
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //pass
            }

            return false;
        }

        public bool ConvertBarcodeToObject()
        {
            if (String.IsNullOrWhiteSpace(RawBarCode))
                return false;
            try
            {
                string serialNumber = RawBarCode.Substring(RawBarCode.Length - 5, 5);
                string lineCode = RawBarCode.Substring(RawBarCode.Length - 7, 2);
                string day = RawBarCode.Substring(RawBarCode.Length - 10, 3);
                string year = RawBarCode.Substring(RawBarCode.Length - 12, 2);
                string plantCode = RawBarCode.Substring(RawBarCode.Length - 14, 2);
                string productCode = RawBarCode.Substring(0, RawBarCode.Length - 14);

                ProductCode = productCode;
                PlantCode = plantCode;
                Year = Convert.ToInt32(year);
                Day = Convert.ToInt32(day);
                LineCode = lineCode;
                SerialNumber = serialNumber;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        // public static bool operator == (Product product1, Product product2)
        // {
        //     try
        //     {
        //         if (product1 == null || product2 == null)
        //         {
        //             return false;
        //         }
        //
        //         if (product1.SerialNumber != product2.SerialNumber)
        //         {
        //             return false;
        //         }
        //
        //         if (product1.ProductCode != product2.ProductCode)
        //         {
        //             return false;
        //         }
        //
        //         if (product1.Day != product2.Day)
        //         {
        //             return false;
        //         }
        //
        //         if (product1.LineCode != product2.LineCode)
        //         {
        //             return false;
        //         }
        //
        //         if (product1.PlantCode != product2.LineCode)
        //         {
        //             return false;
        //         }
        //
        //         if (product1.Year != product2.Year)
        //         {
        //             return false;
        //         }
        //
        //         return true;
        //     }
        //     catch (Exception e)
        //     {
        //         return false;
        //     }
        // }

        // public static bool operator !=(Product product1, Product product2)
        // {
        //     if (product1 == null && product2 == null)
        //     {
        //         return false;
        //     }
        //
        //     if (product1.SerialNumber != product2.SerialNumber)
        //     {
        //         return true;
        //     }
        //
        //     if (product1.ProductCode != product2.ProductCode)
        //     {
        //         return true;
        //     }
        //
        //     if (product1.Day != product2.Day)
        //     {
        //         return true;
        //     }
        //
        //     if (product1.LineCode != product2.LineCode)
        //     {
        //         return true;
        //     }
        //
        //     if (product1.PlantCode != product2.LineCode)
        //     {
        //         return true;
        //     }
        //
        //     if (product1.Year != product2.Year)
        //     {
        //         return true;
        //     }
        //
        //     return true;
        // }
    }
}
