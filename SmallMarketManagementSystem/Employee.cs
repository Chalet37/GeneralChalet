using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chalet.SmallMarketManagementSystem
{
    public class Employee
    {
        private int id;
        private string name;
        private string address;
        private string tel;

        public int Id
        {
            get { return id; }
            private set { ;}
        }

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

        public Employee( int id, string name, string address, string tel )
        {
            this.id = id;
            this.name = name;
            this.address = address;
            this.tel = tel;
        }
    }
}
