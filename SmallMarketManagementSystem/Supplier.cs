using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chalet.SmallMarketManagementSystem
{
    public class Supplier
    {
        private string name;
        private string address;
        private string tel;

        public string Name
        {
            get { return name; }
            private set { ;}
        }

        public string Address
        {
            get { return address; }
            private set { ;}
        }

        public string Tel
        {
            get { return tel; }
            private set { ;}
        }
        public Supplier( string name, string address, string tel )
        {
            this.name = name;
            this.address = address;
            this.tel = tel;
        }
    }
}
