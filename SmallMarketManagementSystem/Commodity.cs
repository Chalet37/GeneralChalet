using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Chalet.SmallMarketManagementSystem
{
    public class Commodity
    {
        private string name;
        private Decimal unitPrice;
        private Supplier supplierInfo;

        public string Name
        {
            get { return name; }
            private set { ;}
        }

        public Decimal UnitPrice
        {
            get { return unitPrice; }
            private set { ;}
        }

        public Supplier SupplierInfo
        {
            get { return supplierInfo; }
            private set { ;}
        }

        public Commodity( string name, Decimal unitPrice, Supplier supplierInfo)
        {
            this.name = name;
            this.unitPrice = unitPrice;
            this.supplierInfo = supplierInfo;
        }
    }
}
