using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Bunifu.Model
{
    public class ProductItem
    {
        string id;
        string name;
        string materialID;
        float quantity;
        float importPrice;
        float exportPrice;
        string description;

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string MaterialID { get => materialID; set => materialID = value; }
        public float Quantity { get => quantity; set => quantity = value; }
        public float ImportPrice { get => importPrice; set => importPrice = value; }
        public float ExportPrice { get => exportPrice; set => exportPrice = value; }
        public string Description { get => description; set => description = value; }

        public ProductItem (DataRow dataRow)
        {
            this.id = dataRow["id_hang"].ToString();
            this.name = dataRow["tenhang"].ToString();
            this.materialID = dataRow["id_cl"].ToString();
            this.quantity = (float)dataRow["soluong"];
            this.importPrice = (float)dataRow["giamua"];
            this.exportPrice = (float)dataRow["giaban"];
            this.description = dataRow["ghichu"].ToString();
        }
    }
}
