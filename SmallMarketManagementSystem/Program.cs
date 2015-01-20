using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Chalet.SmallMarketManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Management.ConnectToDatabase("SmallMarketManagementSystem");
            
            Supplier newSupplier = new Supplier( "Logit", "...", "1234567" );
            var newCommodity = new Commodity("Keyborad", (Decimal)50.00, newSupplier);

            Management.StockManagement.newStock(newCommodity, 500);

            Management.SalesManagement.Sell(1, 1, new Employee(0, "Chalet", "...", "1234567"));

            var cashCounter = new Management.CashCounter(new Employee(0, "Chalet", "...", "1234567"));

            cashCounter.GenerateCheckList();

            cashCounter.AddItem(10, 1);
            cashCounter.AddItem(11, 2);
            cashCounter.AddItem(12, 3);

            cashCounter.Print();

            Management.DisconnectFromDatabase();

        }
    }
}
