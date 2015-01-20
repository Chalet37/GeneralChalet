using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chalet.SmallMarketManagementSystem
{
    public class StorageList
    {
        private int id;
        private string name;
        private Decimal unitPrice;
        private int storageAmount;
        private Supplier supplierInfo;

        public int ID
        {
            get { return id; }
            private set { ;}
        }

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

        public int StorageAmount
        {
            get { return storageAmount; }
            private set { ;}
        }

        public Supplier SupplierInfo
        {
            get { return supplierInfo; }
            private set { ;}
        }

        public StorageList( int id, string name, Decimal unitPrice, int storageAmount, Supplier supplierInfo )
        {
            this.id = id;
            this.name = name;
            this.unitPrice = unitPrice;
            this.storageAmount = storageAmount;
            this.supplierInfo = supplierInfo; 
        }
    }
}
