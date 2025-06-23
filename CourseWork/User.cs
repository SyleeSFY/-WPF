using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Runtime.Remoting.Contexts;
using System.Windows.Controls;
namespace Course_work
{
    public abstract class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public abstract void Add();
        public abstract void Update(int ID);
        public abstract void Delete(int ID);
    }
}


