using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace Chalet.SmallMarketManagementSystem
{
    public static class Management
    { 
        /// <summary>
        ///     定义在Chalet.SmallMarketManagementSystem.Management中的静态类
        ///     提供与入库相关的方法
        /// </summary>
        public static class StockManagement
        {
            /// <summary>
            ///     将数据库中未曾记录过的新商品入库
            /// </summary>
            /// <param name="newCommodity">新商品的实例</param>
            /// <param name="amount">入库数量</param>
            /// <returns>新商品在StorageList中的编号</returns>
            public static int newStock(Commodity newCommodity, int amount)
            {
                var SupplierID = Search<int>("SupplierID",
                                             "SuppliersInfo",
                                             String.Format("SupplierName = '{0}'", newCommodity.SupplierInfo.Name));
                if (SupplierID == default(int))
                    SupplierID = NewSupplier(newCommodity.SupplierInfo);

                SqlCommand cmd = new SqlCommand("NewStock", Connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@CommodityName",
                                                    SqlDbType.NChar,
                                                    50,
                                                    "CommodityName"));
                cmd.Parameters.Add(new SqlParameter("@UnitPrice",
                                                    SqlDbType.Money,
                                                    0,
                                                    "UnitPrie"));
                cmd.Parameters.Add(new SqlParameter("@Amount",
                                                     SqlDbType.Int,
                                                     0,
                                                     "StorageAmount"));
                cmd.Parameters.Add(new SqlParameter("@SupplierID",
                                                    SqlDbType.NChar,
                                                    50,
                                                    "SupplierID"));
                cmd.Parameters.Add(new SqlParameter("@StockDate",
                                                    SqlDbType.DateTime,
                                                    0,
                                                    "StockDate"));
                cmd.Parameters.Add(new SqlParameter("@CommodityID",
                                                        SqlDbType.Int,
                                                        0,
                                                        ParameterDirection.Output,
                                                        false,
                                                        0,
                                                        0,
                                                        "CommodityID",
                                                        DataRowVersion.Default,
                                                        null));
                cmd.UpdatedRowSource = UpdateRowSource.OutputParameters;

                cmd.Parameters["@CommodityName"].Value = newCommodity.Name;
                cmd.Parameters["@UnitPrice"].Value = newCommodity.UnitPrice;
                cmd.Parameters["@Amount"].Value = amount;
                cmd.Parameters["@SupplierID"].Value = SupplierID;
                cmd.Parameters["@StockDate"].Value = DateTime.Now;
                cmd.ExecuteNonQuery();
                int newID = (int)cmd.Parameters["@CommodityID"].Value;

                return newID;
            }
            /// <summary>
            ///     将数据库中已经记录过的新商品入库
            /// </summary>
            /// <param name="CommodityID">商品在StorageList中的编号</param>
            /// <param name="amount">入库数量</param>
            public static void newStock(int CommodityID, int amount)
            {
                SqlCommand cmd = new SqlCommand("StockUpdate", Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CommodityID", CommodityID);
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@StockDate", DateTime.Now);
                cmd.ExecuteNonQuery();
            }

            /// <summary>
            ///     将库存清单输出到线性表中
            /// </summary>
            /// <returns>存有库存中所有物品的线性表</returns>
            public static List<StorageList> StorageListOuput()
            {
                try
                {
                    var OutputList = new List<StorageList>();
                    string select = "SELECT CommodityID, CommodityName, UnitPrice, StorageAmount, SupplierID" +
                        "FROM StorageList";
                    SqlCommand cmd = new SqlCommand(select, Connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var supplierInfo = Search("SupplierName, SupplierAddress, SupplierTel", "SuppliersInfo", "SupplierID = " + (string)reader[4]);
                        var supplier = new Supplier((string)supplierInfo[0], (string)supplierInfo[1], (string)supplierInfo[2]);
                        OutputList.Add(new StorageList((int)reader[0], (string)reader[1], (Decimal)reader[2], (int)reader[3], supplier));
                    }

                    reader.Close();
                    return OutputList;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Source + ": " + ex.Message);
                    return null;
                }
            }
        }
        /// <summary>
        ///     定义在Chalet.SmallMarketManagementSystem.Management中的静态类
        ///     提供与销售相关的方法
        /// </summary>
        public static class SalesManagement
        {
            /// <summary>
            ///     销售商品，并将其记录在SalesRecord表中
            /// </summary>
            /// <param name="commodityID">所销售商品在StorageList中的编号</param>
            /// <param name="amount">销售的数量</param>
            /// <param name="user">处理销售的员工</param>
            /// <returns>销售操作是否成功</returns>
            public static bool Sell( int commodityID, int amount, Employee user )
            {
                try
                {
                    int previousAmount = Search<int>("StorageAmount",
                                                 "StorageList",
                                                 String.Format("CommodityID = {0}", commodityID));
                    if( previousAmount <= 0 )
                        throw new Exception( "No Storage for commodity " + commodityID.ToString() );

                    SqlCommand cmd = new SqlCommand("Sell", Connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CommodityID", commodityID);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@SalesDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@EmployeeID", user.Id);
                    cmd.ExecuteNonQuery();
                }
                catch( Exception ex )
                {
                    Console.WriteLine(ex.Source + ": " + ex.Message);
                    return false;
                }
                return true;   
            }
        }
        /// <summary>
        ///     定义在Chalet.SmallMarketManagementSystem.Management中的类
        ///     提供与收银有关的字段与方法
        /// </summary>
        public class CashCounter
        {
            /// <summary>
            ///     进行收银操作的员工
            /// </summary>
            private Employee user;
            /// <summary>
            ///     收银操作所建立的清单
            /// </summary>
            private List<CashList> checkList;

            /// <summary>
            ///     进行收银操作的员工
            /// </summary>
            public Employee User
            {
                get { return user; }
            }

            /// <summary>
            ///     构造函数
            /// </summary>
            /// <param name="user">进行收银操作的员工</param>
            public CashCounter( Employee user )
            {
                this.user = user;
            }
            /// <summary>
            ///     生成清单
            /// </summary>
            public void GenerateCheckList()
            {
                checkList = new List<CashList>();
            }

            /// <summary>
            ///     在清单中添加项目
            /// </summary>
            /// <param name="id">商品在StorageList中的编号</param>
            /// <param name="amount">顾客购买的数量</param>
            /// <returns>项目是否添加成功</returns>
            public bool AddItem( int id, int amount )
            {
                var result = Search("CommodityName, UnitPrice, StorageAmount",
                            "StorageList",
                            String.Format("CommodityID = {0}", id));
                try
                {
                    if (result == null)
                        throw new Exception("Commodity No Found!");
                    if ((int)result[2] <= 0)
                        throw new Exception("No storage!");

                    var newItem = new CashList(id, (string)result[0], (Decimal)result[1], amount);
                    checkList.Add(newItem);
                    return true;
                }
                catch( Exception ex )
                {
                    Console.WriteLine(ex.Source + ": " + ex.Message);
                    return false;
                }
            }

            /// <summary>
            ///     打印清单中的项目
            /// </summary>
            public void Print()
            {
                foreach( CashList item in checkList )
                {
                    Console.WriteLine(item.ToString());
                }
            }

            /// <summary>
            ///     本次收银结算
            /// </summary>
            /// <param name="actualIncome">本次收银实收</param>
            /// <returns>本次收银找零金额</returns>
            public Decimal Settle( Decimal actualIncome)
            {
                Decimal ShouldCash = 0;
                foreach( CashList item in checkList )
                {
                    ShouldCash += item.Total;
                }

                return actualIncome - ShouldCash;
            }
        }

        public static SqlConnection Connection;

        /// <summary>
        ///     连接到指定数据库
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <param name="integratedSecurity">身份验证模式</param>
        /// <param name="server">数据库服务器</param>
        public static void ConnectToDatabase(string databaseName,
                                             string integratedSecurity = "SSPI",
                                             string server = "local")
        {
            try
            {
                string source = String.Format("server=({0});integrated security={1};database={2}",
                                               server, integratedSecurity, databaseName);
                SqlConnection conn = new SqlConnection(source);
                conn.Open();
                Connection = conn;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
                Connection = null;
            }

        }

        /// <summary>
        ///     与数据库断开连接
        /// </summary>
        public static void DisconnectFromDatabase()
        {
            try
            {
                Connection.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }
        }

        /// <summary>
        ///     查找（指定类型）的数据（泛型版本）
        /// </summary>
        /// <typeparam name="ResultType">指定的类型</typeparam>
        /// <param name="wanted">SELECT语句后的表达式（要查找的列名）</param>
        /// <param name="table">FROM语句后的表达式（要查找的表的名称）</param>
        /// <param name="key">WHERE语句后的条件表达式（查找的依据）</param>
        /// <returns>指定类型的查找结果</returns>
        public static ResultType Search<ResultType>( string wanted, string table, string key )
        {
            string select = String.Format("SELECT {0} FROM {1} WHERE {2}", wanted, table, key);
            SqlCommand cmd = new SqlCommand(select, Connection);
            SqlDataReader reader = cmd.ExecuteReader();

            reader.Read();
            if (reader.HasRows == false || !(reader[0] is ResultType))
            {
                reader.Close();
                return default(ResultType);
            }

            
            var result = (ResultType)reader[wanted];
            reader.Close();
            return result;
        }

            /// <summary>
            ///     查找一系列数据（结果有多行）
            /// </summary>
            /// <param name="wanted">SELECT语句后的表达式（要查找的列名）</param>
            /// <param name="table">FROM语句后的表达式（要查找的表的名称）</param>
            /// <param name="key">WHERE语句后的条件表达式（查找的依据）</param>
            /// <param name="nColumn">要查找的列数</param>
            /// <returns>查询结果的List表</returns>
        public static List<object[]> Search( string wanted, string table, string key, int nColumn )
        {
            string select = String.Format("SELECT {0} FROM {1} WHERE {2}", wanted, table, key);
            SqlCommand cmd = new SqlCommand(select, Connection);
            SqlDataReader reader = cmd.ExecuteReader();

            var result = new List<object[]>();

            int i = 0;
            while( reader.Read() )
            {
                var row = new object[nColumn];
                for( int j = 0; j < nColumn; j ++ )
                    row[j] = reader[j];

                result.Add(row);
                i ++;
            }

            reader.Close();

            return result;
        }

        /// <summary>
        ///     查找一系列数据
        /// </summary>
        /// <param name="wanted">SELECT语句后的表达式（要查找的列名）</param>
        /// <param name="table">FROM语句后的表达式（要查找的表的名称）</param>
        /// <param name="key">WHERE语句后的条件表达式（查找的依据）</param>
        /// <returns>查找结果的object数组</returns>
        public static object[] Search( string wanted, string table, string key )
        {
            try
            {
                string select = String.Format("SELECT {0} FROM {1} WHERE {2}", wanted, table, key);
                SqlCommand cmd = new SqlCommand(select, Connection);
                SqlDataReader reader = cmd.ExecuteReader();

                var result = new object[reader.FieldCount];

                reader.Read();
                for (int i = 0; i < reader.FieldCount; i ++ )
                {
                    result[i] = reader[i];
                }
                reader.Close();

                return result;
            }
            catch( Exception ex )
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
                return null;
            }

            
        }

        /// <summary>
        ///     在EmployeeInfo表中添加新的员工信息
        /// </summary>
        /// <param name="newEmployee">新员工的实例</param>
        /// <returns>新员工在EmployeeInfo表中的编号</returns>
        public static int NewEmployee(Employee newEmployee)
        {
            SqlCommand cmd = new SqlCommand("NewEmployee", Connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@EmployeeName",
                                                SqlDbType.NChar,
                                                50,
                                                "EmployeeName"));
            cmd.Parameters.Add(new SqlParameter("@EmployeeAddress",
                                                SqlDbType.NChar,
                                                50,
                                                "EmployeeAddress"));
            cmd.Parameters.Add(new SqlParameter("@EmployeeTel",
                                                 SqlDbType.Int,
                                                 0,
                                                 "EmployeeTel"));
            cmd.Parameters.Add(new SqlParameter("@EmployeeID",
                                                    SqlDbType.Int,
                                                    0,
                                                    ParameterDirection.Output,
                                                    false,
                                                    0,
                                                    0,
                                                    "EmployeeID",
                                                    DataRowVersion.Default,
                                                    null));
            cmd.UpdatedRowSource = UpdateRowSource.OutputParameters;

            cmd.Parameters["@EmployeeName"].Value = newEmployee.Name;
            cmd.Parameters["@EmployeeAddress"].Value = newEmployee.Address;
            cmd.Parameters["@EmployeeTel"].Value = newEmployee.Tel;
            cmd.ExecuteNonQuery();
            int newID = (int)cmd.Parameters["@EmployeeID"].Value;

            return newID;
        }

        /// <summary>
        ///     在SupplierInfo表中添加新的供应商信息
        /// </summary>
        /// <param name="newSupplier">新的供应商的实例</param>
        /// <returns>新的供应商在SupplierInfo表中的编号</returns>
        public static int NewSupplier(Supplier newSupplier)
        {
            SqlCommand cmd = new SqlCommand("NewSupplier", Connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@SupplierName",
                                                SqlDbType.NChar,
                                                50,
                                                "SupplierName"));
            cmd.Parameters.Add(new SqlParameter("@SupplierAddress",
                                                SqlDbType.NChar,
                                                50,
                                                "SupplierAddress"));
            cmd.Parameters.Add(new SqlParameter("@SupplierTel",
                                                 SqlDbType.Int,
                                                 0,
                                                 "SupplierTel"));
            cmd.Parameters.Add(new SqlParameter("@SupplierID",
                                                    SqlDbType.Int,
                                                    0,
                                                    ParameterDirection.Output,
                                                    false,
                                                    0,
                                                    0,
                                                    "SupplierID",
                                                    DataRowVersion.Default,
                                                    null));
            cmd.UpdatedRowSource = UpdateRowSource.OutputParameters;

            cmd.Parameters["@SupplierName"].Value = newSupplier.Name;
            cmd.Parameters["@SupplierAddress"].Value = newSupplier.Address;
            cmd.Parameters["@SupplierTel"].Value = newSupplier.Tel;
            cmd.ExecuteNonQuery();
            int newID = (int)cmd.Parameters["@SupplierID"].Value;

            return newID;
        }
    }
}
