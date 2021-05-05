﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunifu.DataProvider;
using System.Windows.Forms;

namespace Bunifu.DAO
{
    class RegisterAccount : Account
    {
        string lastname;
        string username;
        string password;
        string email;
        public RegisterAccount(string _lastname, string _username, string _password,  string _email)
        {
            this.lastname = _lastname;
            this.username = _username;
            this.password = _password;
            this.email = _email;
        }

        public override void RegisterUser()
        {
            string query = "call AddUser( @lastName , @userName , @password , @email )";
            //string query = "Insert into RegisterUser(lastName, userName, passWord, email)"
            //+ "values ( @lastName , @userName , @passWord , @email )";

            try
            {
                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] {this.lastname, this.username, this.password, this.email});
                string regInfo = string.Format("You can login with UserName: {0}", this.username);
                MessageBox.Show(regInfo, "Register Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Register Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
