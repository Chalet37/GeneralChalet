using System;
using System.Collections.Generic;

namespace Chalet.SmallMarketManagementSystem
{
    public struct CashList
    {
        private int commodityID;
        private string commodityName;
        private Decimal unitPrice;
        private int amount;
        private Decimal total;

        public int CommodityID
        {
            get { return commodityID; }
            private set { ;}
        }

        public string CommodityName
        {
            get { return commodityName; }
            private set { ;}
        }

        public Decimal UnitPrice
        {
            get { return unitPrice; }
            private set { ;}
        }

        public int Amount
        {
            get { return amount; }
            private set { ;}
        }

        public Decimal Total
        {
            get { return total; }
            private set { ;}
        }

        public CashList(int id, string name, Decimal unitPrice, int amount )
        {
            this.commodityID = id;
            this.commodityName = name;
            this.amount = amount;
            this.unitPrice = unitPrice;
            this.total = amount * unitPrice;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3} {4}", commodityID, commodityName, unitPrice, amount, total);
        }
    }
}