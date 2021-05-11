using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Bunifu.Model;
namespace Bunifu.DAO
{
    public class Product
    {
        private static Product instance;

        public static Product Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Product();
                }

                return instance;
            }

            set => instance = value;
        }

        internal ProductItem GetProductByID(string _productID)
        {
            string query = "select * from hanghoa where id_hang = @id_hang ";

            DataTable data = DataProvider.DataProvider.Instance.ExecuteQuery(query, new object[] { _productID });

            foreach (DataRow item in data.Rows)
            {
                return new ProductItem(item);
            }

            return null;
        }

        internal DataTable LoadProductItems()
        {
            string query = "call ShowProduce";

            return DataProvider.DataProvider.Instance.ExecuteQuery(query);
        }

        internal DataTable FillProductComboBox()
        {
            string query = "select * from chatlieu";

            return DataProvider.DataProvider.Instance.ExecuteQuery(query);
        }

        internal void UpdateProductItemInformation(string _productname,string _productMaterialID, int _productQuantity, double _productImportPrice, double _productExportPrice, string _productDescription, string _productID)
        {
            string query = "call SP_UpdateProduct( @proName , @proMatID , @proQuantily , @proPurPrice , @proSalePrice , @proDes , @proID )";

            try
            {
                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] { _productname, _productMaterialID, _productQuantity, _productImportPrice, _productExportPrice, _productDescription, _productID });
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal void InsertProductItem(string _productID, string _productName, string _productMaterialID, int _productQuantity, double _productImportPrice, double _productExportPrice, string _productDescription)
        {
            string query = "call SP_InsertProduct( @p_idHang , @p_tenHang , @p_idChatlieu , @p_soLuong , @p_giaMua , @p_giaBan , @p_ghiChu )";

            try
            {
                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] {_productID, _productName, _productMaterialID, _productQuantity, _productImportPrice, _productExportPrice, _productDescription });
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal void DeleteProductItem(string _productID)
        {
            string query =  "call SP_DeleteProduct( @_productID )";

            try
            {
                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] { _productID });
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal bool ProductExist(string _productID)
        {
            string query = "select * from hanghoa where id_hang = @id_hang ";

            DataTable result = DataProvider.DataProvider.Instance.ExecuteQuery(query, new object[] { _productID });

            return result.Rows.Count > 0;
        }
    }
}
