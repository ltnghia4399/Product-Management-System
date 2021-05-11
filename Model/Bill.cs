using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Bunifu.Model
{
    public class Bill
    {
        string billID;
        int staffID;
        string date;
        int customerID;
        float total;

        public string BillID { get => billID; set => billID = value; }
        public int StaffID { get => staffID; set => staffID = value; }
        public string Date { get => date; set => date = value; }
        public int CustomerID { get => customerID; set => customerID = value; }
        public float Total { get => total; set => total = value; }

        public Bill (DataRow dataRow)
        {
            this.billID = dataRow["id_hd"].ToString();
            this.staffID = (int)dataRow["id_nv"];
            this.date = dataRow["ngayban"].ToString();
            this.customerID = (int)dataRow["id_kh"];
            this.total = (float)dataRow["tongtien"];
        }
    }
}
