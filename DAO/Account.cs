using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunifu.Connection;
using Bunifu.DataProvider;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace Bunifu.DAO
{
    public class Account
    {
        private static Account instance;

        public static Account Instance
        {
            get {

                if(instance == null)
                {
                    instance = new Account();
                }

                return instance;
            }
            set => instance = value;
        }


        public bool Login(string _username, string _password)
        {
            string query = "Select * from RegisterUser where userName = '" + _username.Trim() + "' and passWord = '" + _password.Trim() + "'";

            DataTable result = DataProvider.DataProvider.Instance.ExecuteQuery(query);

            return result.Rows.Count > 0;
        }

        public virtual void RegisterUser() { }

        public User GetAccountByUserName(string username)
        {
            DataTable data = DataProvider.DataProvider.Instance.ExecuteQuery("Select *from RegisterUser where userName = '" + username + "'");

            foreach (DataRow item in data.Rows)
            {
                return new User(item);
            }

            return null;
        }

        public void UpdateAccountInfomation(int _id, string _displayname, string _password, string _email, int _role, string _description)
        {
            string query = "call UpdateUserInformation( @id , @lastName , @password , @email , @role , @description )";

            try
            {
                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] {_id, _displayname, _password, _email, _role, _description });

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        internal void DeleteAccountByID(int _id)
        {
            string query = "call SP_DeleteUser( @_id )";

            try
            {
                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] { _id });
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal DataTable LoadEmployeeAccount()
        {
            string query = "call ShowUser()";

            return DataProvider.DataProvider.Instance.ExecuteQuery(query);
        }

        internal string GetUserByID(int _ID)
        {
            string query = "select registerUser.lastName from registerUser,hdban where registerUser.id = '" + _ID + "'";
            return DataProvider.DataProvider.Instance.GetFieldValues(query);
        }

    }
}
