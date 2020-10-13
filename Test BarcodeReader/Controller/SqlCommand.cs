using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_BarcodeReader.Model;

namespace Test_BarcodeReader.Controller
{
    public class SqlCommand
    {
        public static bool SendProduct(DataSql data, DbConnection dbConnection)
        {
            int TestResult = data.TestResult ? 1 : 0;
            string query =
                $"insert into {Constants.TableName} (TestID, SerialNo, TestResult, TestTime, TestMachine) value ({DataSql.TestID}, '{data.SerialNo}', {TestResult}, '{data.TestTime}', '{DataSql.TestMachine}')";
            return dbConnection.Insert(query);
        }
    }
}
