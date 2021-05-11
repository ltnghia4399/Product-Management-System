using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace Bunifu.Model
{
    public class Client
    {
        int id;
        string name;
        string address;
        string tel;

        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public string Tel { get => tel; set => tel = value; }
        public int Id { get => id; set => id = value; }

        public Client (DataRow dataRow)
        {
            this.id = (int)dataRow["id_kh"];
            this.name = dataRow["tenkhach"].ToString();
            this.address = dataRow["diachi"].ToString();
            this.tel = dataRow["sdt"].ToString();
        }
    }
}
