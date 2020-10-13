using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_BarcodeReader.Controller;

namespace Test_BarcodeReader.Model
{
    public class DataSql
    {
        public static int TestID { get; set; }
        public string SerialNo { get; set; }
        public bool TestResult { get; set; }
        public string TestValue { get; set; }
        public string TestTime { get; set; }
        public static string TestMachine { get; set; }
        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string Add3 { get; set; }
        public string Add4 { get; set; }
        public bool IsSent { get; set; }

        public DataSql(string serialNo)
        {
            DateTime theDate = DateTime.Now;
            string time = theDate.ToString("yyyy-MM-dd H:mm:ss");
            SerialNo = serialNo;
            TestTime = time;
        }

        public DataSql(Product product)
        {
            SerialNo = product.SerialNumber;
            TestResult = product.IsGoodProduct;
            DateTime theDate = DateTime.Now;
            string time = theDate.ToString("yyyy-MM-dd H:mm:ss");
            TestTime = time;
        }
    }
}
