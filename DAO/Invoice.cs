using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Bunifu.Model;
namespace Bunifu.DAO
{
    public class Invoice
    {
        private static Invoice instance;


        public static Invoice Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Invoice();
                }

                return instance;
            }

            set => instance = value;

        }

        internal DataTable FillInvoiceListComboBox()
        {
            string query = "select id_hd from HDban;";

            return DataProvider.DataProvider.Instance.ExecuteQuery(query);
        }

        internal Bill GetBillByBillID(string _billID)
        {
            string query = "select * from HDban where id_hd = @billID ";

            DataTable data = DataProvider.DataProvider.Instance.ExecuteQuery(query, new object[] { _billID});

            foreach (DataRow item in data.Rows)
            {
                return new Bill(item);
            }

            return null;
        }

        internal DataTable GetInvoiceItem(string _billID)
        {
            string query = "call SP_InvoiceInfo( @id_hd )";

            try
            {
                return DataProvider.DataProvider.Instance.ExecuteQuery(query, new object[] { _billID });
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }


        internal bool InvoiceExist(string _invoiceID)
        {
            string query = "Select id_hd from HDban where id_hd = @id_hd ";

            DataTable result = DataProvider.DataProvider.Instance.ExecuteQuery(query, new object[] { _invoiceID });

            return result.Rows.Count > 0;
        }

        internal bool ProductInBillDetail(string _proID, string _invoiceID)
        {
            string query = "Select id_hang from chitietHDban where id_hang = @id_hang and id_hd = @id_hd ";

            DataTable result = DataProvider.DataProvider.Instance.ExecuteQuery(query, new object[] { _proID, _invoiceID });

            return result.Rows.Count > 0;
        }

        internal void CreateNewInvoice(string invoiceID, string staffID, string date, int cusID, string tongtien)
        {
            string query = "Call SP_NewInvoice( @id_hd , @id_nv , @date , @id_kh , @tongtien )";

            try
            {
                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] { invoiceID, staffID, date, cusID, tongtien });
            }
            catch (Exception)
            {

                throw;
            }

        }

        internal void CreateNewInvoiceDetail(string invoiceID, string proID, string proQuantily, string proPrice, string proDiscount, string thanhtien)
        {
           string query = "Call SP_NewInvoiceInfo( @id_hd , @id_hang , @soluong , @donGia , @giamgia , @thanhtien )";

            try
            {
                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] { invoiceID, proID, proQuantily, proPrice, proDiscount, thanhtien });
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal void UpdateProductQuantity(double soLuongConLai, string proID)
        {
            string query = "call SP_UpdateProductQuantily( @soluong , @id_hang )";

            try
            {
                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] { soLuongConLai, proID });

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
