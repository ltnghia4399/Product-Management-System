using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.DAO;
namespace Bunifu
{
    public partial class Form1 : Form
    {
        Account account;

        public Form1()
        {
            InitializeComponent();
            bunifuFormDock1.SubscribeControlToDragEvents(bunifuGradientPanel1);
            bunifuFormDock1.SubscribeControlToDragEvents(tabPage1);
            bunifuFormDock1.SubscribeControlToDragEvents(tabPage2);
            
        }

        private void BunifuButton1_Click(object sender, EventArgs e)
        {

        }

        private void BunifuTextBox2_TextChanged(object sender, EventArgs e)
        {

        }


        private void BunifuButton1_Click_1(object sender, EventArgs e)
        {
            BunifuPage.SetPage(0);
        }

        private void BunifuButton3_Click(object sender, EventArgs e)
        {
            BunifuPage.SetPage(1);
        }

        private void BunifuLabel5_Click(object sender, EventArgs e)
        {

        }

        private void BunifuButton2_Click(object sender, EventArgs e)
        {
            //account = new Account(usernameTxt.Text, passTxt.Text);

            if (!Account.Instance.Login(usernameTxt.Text, passTxt.Text))
            {
                MessageBox.Show("Login Fail", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                //ACCESS THE HOME PAGE

                MessageBox.Show("Login successful","Login" ,MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Hide();
                Home home = new Home(Account.Instance.GetAccountByUserName(usernameTxt.Text.Trim()));
                home.ShowDialog();
                //if (home.DialogResult != DialogResult.No) this.Hide();
                //this.Show();
            }

            usernameTxt.Text = passTxt.Text = "";
        }

        private void BunifuButton4_Click(object sender, EventArgs e)
        {
            string lastname = lastNameRegTxt.Text.Trim();
            string username = userNameRegTxt.Text.Trim();
            string password = passRegTxt.Text.Trim();
            string email = emailRegTxt.Text.Trim();

            account = new RegisterAccount(lastname, username, password, email);
            
            try
            {
                if (account.Login(userNameRegTxt.Text.Trim(), passRegTxt.Text.Trim()))
                {
                    MessageBox.Show("This user name already taken\nPlease choose another one","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    ClearRegisterForm();
                    return;
                }
                else
                {
                    account.RegisterUser();
                }
                

                ClearRegisterForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void ClearRegisterForm()
        {
            lastNameRegTxt.Text = userNameRegTxt.Text = passRegTxt.Text = emailRegTxt.Text = "";
            lastNameRegTxt.Focus();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to close this application?", "Exit", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {

                Application.ExitThread();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            usernameTxt.Focus();
        }
    }
}
