using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Bunifu.DataProvider;

namespace Bunifu.DAO
{
    public class Customer
    {
        private static Customer instance;

        public static Customer Instance
        {
            get 
            {
                if(instance == null)
                {
                    instance = new Customer();
                }

                return instance;
            }

            set => instance = value;
        }

        internal DataTable LoadCustomerItems()
        {
            string query = "call ShowCustomer()";

            return DataProvider.DataProvider.Instance.ExecuteQuery(query);
        }

        internal bool CustomerExist(string _customerID)
        {
            string query = "select * from khach where id_kh = @id_kh ";

            DataTable result = DataProvider.DataProvider.Instance.ExecuteQuery(query, new object[] { _customerID });

            return result.Rows.Count > 0;
        }

        internal void InsertCustomerItem(string _customerID, string _customerName, string _customerAdress, int _customerTel)
        {
            string query = "Call SP_NewCustomer( @p_ID , @p_Name , @p_Address , @p_Tel );";

            try
            {
                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] { _customerID, _customerName, _customerAdress, _customerTel });
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal void UpdateCustomerInformation(string _customerID, string _customerName, string _customerAddress, long _customerTel)
        {
            string query = "call SP_UpdateCustomer( @p_Name , @p_Address , @p_Tel , @p_id );";

            try
            {
                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] { _customerName, _customerAddress, _customerTel, _customerID });
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal void DeleteCustomerItem(string _customerID)
        {
            string query = "call SP_DeleteCustomer( @p_id )";

            try
            {
                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] { _customerID });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
