using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_BarcodeReader.Model
{
    public class ConfigurationInfo
    {
        public DbConnection MySqlConnection { get; set; }
        public List<string> ProductCode { get; set; }
        public int TestID { get; set; }
        public string TestMachine { get; set; }
    }
}
