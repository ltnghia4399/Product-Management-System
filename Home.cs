using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Bunifu.Connection;
using Bunifu.DAO;
namespace Bunifu
{
    public enum TabPagesIndex
    {
        CHECKOUT = 0,
        USER = 1,
        ADMIN = 2,
    }

    public enum ProductState
    {
        NEW,
        UPDATE,
        DELETE
    }

    public partial class Home : Form
    {
        bool isAdmin = false;

        int id;

        string username;
        string email;
        string displayname;
        string password;
        string description;

        ProductState state = ProductState.UPDATE;

        public Home(User _user)
        {
            InitializeComponent();

            UpdateForm(_user);

            LoadProductItems();
            
        }

        void UpdateForm(User _user)
        {
            lbUserIdUserPage.Text = string.Format("Account ID: {0}", _user.Id);

            lbEmailUserPage.Text = lbEmail.Text = string.Format(_user.Email);

            lbName.Text = string.Format("Welcome back {0} ", _user.Lastname);

            cbRoleUserPage.SelectedIndex = _user.Type;

            lbDescriptionUserPage.Text = _user.Description;

            id = _user.Id;
            username = _user.Username;
            email = _user.Email;
            displayname = _user.Lastname;
            password = _user.Password;
            description = _user.Description;

            if (AdminCheck(_user.Type))
            {
                lbRoleUserPage.Text = lbRole.Text = string.Format("ADMIN");
                cbRoleUserPage.Enabled = true;
                isAdmin = true;
            }
            else
            {
                lbRoleUserPage.Text = lbRole.Text = string.Format("STAFF");
                cbRoleUserPage.Enabled = false;
                isAdmin = false;
            }
        }

        bool AdminCheck(int _userType) => _userType == 1 ? true : false;

        void LoadProductItems()
        {
            dgvProduct.DataSource = Product.Instance.LoadProductItems();
            dgvProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            cbProductMaterialID.ValueMember = "id_cl";
            cbProductMaterialID.DisplayMember = "ten_cl";
            cbProductMaterialID.DataSource = Product.Instance.FillProductComboBox();
        }

        private void BtnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to log out?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(dialogResult == DialogResult.Yes)
            {

                this.Hide();
                Form1 loginForm = new Form1();
                loginForm.Show();
            }
            else
            {
                return;
            }
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                // back to login;

                this.Hide();
                Form1 loginForm = new Form1();
                loginForm.Show();
            }
        }

        void SetUserTextBoxValue()
        {
            txtUsernameUserPage.Text = username;
            txtEmailUserPage.Text = email;
            txtDisplaynameUserPage.Text = displayname;
            txtPasswordUserPage.Text = password;
            txtDescriptionUserPage.Text = description;
        }

        private void BtnAdmin_Click(object sender, EventArgs e)
        {
            if (!isAdmin)
            {
                MessageBox.Show("You must be an Administrator to access this page","Access denied",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }

            Main_Page.SetPage((int)TabPagesIndex.ADMIN);
            lbTitle.Text = "ADMIN";
        }

        private void BtnEditAccount_Click(object sender, EventArgs e)
        {
            Main_Page.SetPage((int)TabPagesIndex.USER);
            lbTitle.Text = "USER";
            SetUserTextBoxValue();
        }

        private void BtnInvoice_Click(object sender, EventArgs e)
        {
            Main_Page.SetPage((int)TabPagesIndex.CHECKOUT);
            lbTitle.Text = "CHECKOUT";
        }

        private void DgvProduct_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtProductID.Text = dgvProduct.CurrentRow.Cells["ID Hang"].Value.ToString();
            txtProductName.Text = dgvProduct.CurrentRow.Cells["Ten mat hang"].Value.ToString();
            cbProductMaterialID.SelectedValue = dgvProduct.CurrentRow.Cells["ID chat lieu"].Value.ToString();
            txtProductQuantity.Text = dgvProduct.CurrentRow.Cells["So luong"].Value.ToString();
            txtProductImportPrice.Text = dgvProduct.CurrentRow.Cells["Gia nhap hang"].Value.ToString();
            txtProductExportPrice.Text = dgvProduct.CurrentRow.Cells["Gia ban ra"].Value.ToString();
            txtProductDescription.Text = dgvProduct.CurrentRow.Cells["Mo ta mat hang"].Value.ToString();
        }

        private void btnApplyProduct_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case ProductState.NEW:
                    InsertProduct();
                    break;
                case ProductState.UPDATE:
                    UpdateProduct();
                    break;
                case ProductState.DELETE:
                    DeleteProduct();
                    break;
                default:
                    break;
            }

        }

        private void BtnNewProduct_Click(object sender, EventArgs e)
        {
            state = ProductState.NEW;

            DialogResult dialogResult = MessageBox.Show("Fill all information on the left panel to add more product", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if(dialogResult == DialogResult.OK)
            {
                ClearProductInput();

                btnApplyProduct.Text = "APPLY NEW PRODUCT";
                lbStateProduct.Text = string.Format("{0} Product", state.ToString());

                btnApplyProduct.Enabled = true;
                btnCancelProductTask.Enabled = true;

                btnUpdateProduct.Enabled = false;
                btnNewProduct.Enabled = false;
                btnDeleteProduct.Enabled = false;

                txtProductID.ReadOnly = false;
                txtProductID.Enabled = true;
                txtProductID.Focus();
            }
        }

        private void BtnDeleteProduct_Click(object sender, EventArgs e)
        {
            state = ProductState.DELETE;

            DialogResult dialogResult = MessageBox.Show("Double click to the row of product you want to delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.OK)
            {
                ClearProductInput();

                btnApplyProduct.Text = "APPLY DELETE PRODUCT";
                lbStateProduct.Text = string.Format("{0} Product", state.ToString());

                btnApplyProduct.Enabled = true;
                btnCancelProductTask.Enabled = true;

                btnUpdateProduct.Enabled = false;
                btnNewProduct.Enabled = false;
                btnDeleteProduct.Enabled = false;
            }
        }

        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            state = ProductState.UPDATE;

            DialogResult dialogResult = MessageBox.Show("Select row to update product item", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.OK)
            {
                ClearProductInput();

                btnApplyProduct.Text = "APPLY UPDATE PRODUCT";
                lbStateProduct.Text = string.Format("{0} Product", state.ToString());

                btnApplyProduct.Enabled = true;
                btnCancelProductTask.Enabled = true;

                btnUpdateProduct.Enabled = false;
                btnNewProduct.Enabled = false;
                btnDeleteProduct.Enabled = false;
            }
        }

        private void btnUpdateAccount_Click(object sender, EventArgs e)
        {
            try
            {
                Account.Instance.UpdateAccountInfomation(id,
                                                        txtDisplaynameUserPage.Text,
                                                        txtPasswordUserPage.Text,
                                                        txtEmailUserPage.Text,
                                                        cbRoleUserPage.SelectedIndex,
                                                        txtDescriptionUserPage.Text);

                string updateInfo = string.Format("Update your information successful");
                DialogResult dr = MessageBox.Show(updateInfo, "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (dr == DialogResult.OK)
                {
                    //Update form user info
                    UpdateForm(Account.Instance.GetAccountByUserName(username));
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        

        void UpdateProduct()
        {
            if (txtProductID.Text == "")
            {
                MessageBox.Show("Please double click to the row of product you want to update ", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string productID = txtProductID.Text.Trim();
            string productName = txtProductName.Text.Trim();
            string productMaterialID = cbProductMaterialID.SelectedValue.ToString();
            int productQuantity = Convert.ToInt32(txtProductQuantity.Text.Trim());
            double productImportPrice = Convert.ToDouble(txtProductImportPrice.Text.Trim());
            double productExportPrice = Convert.ToDouble(txtProductExportPrice.Text.Trim());
            string productDescription = txtProductDescription.Text.Trim();

            try
            {
                Product.Instance.UpdateProductItemInformation(productName, productMaterialID, productQuantity, productImportPrice, productExportPrice, productDescription, productID);
                string updateProductInfo = string.Format("Update product information successful");
                DialogResult dr = MessageBox.Show(updateProductInfo, "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    //Refresh product data grid view 
                    LoadProductItems();
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        void InsertProduct()
        {
            if (txtProductID.Text == "")
            {
                MessageBox.Show("Please fill product information on the left panel ", "Insert Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string productID = txtProductID.Text.Trim();
            string productName = txtProductName.Text.Trim();
            string productMaterialID = cbProductMaterialID.SelectedValue.ToString();
            int productQuantity = Convert.ToInt32(txtProductQuantity.Text.Trim());
            double productImportPrice = Convert.ToDouble(txtProductImportPrice.Text.Trim());
            double productExportPrice = Convert.ToDouble(txtProductExportPrice.Text.Trim());
            string productDescription = txtProductDescription.Text.Trim();

            try
            {
                Product.Instance.InsertProductItem(productID, productName, productMaterialID, productQuantity, productImportPrice, productExportPrice, productDescription);
                string insertProductInfo = string.Format("Insert product {0} successful", productName);
                DialogResult dr = MessageBox.Show(insertProductInfo, "Insert Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    //Refresh product data grid view 
                    ClearProductInput();
                    LoadProductItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        void DeleteProduct()
        {
            if(txtProductID.Text == "")
            {
                MessageBox.Show("Please double click to the row of product you want to delete ", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string productID = txtProductID.Text.Trim();
            string productName = txtProductName.Text.Trim();
            try
            {
                Product.Instance.DeleteProductItem(productID);
                string deleteProductInfo = string.Format("Delete product {0} successful", productName);
                DialogResult dr = MessageBox.Show(deleteProductInfo, "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    //Clear all product input textbox
                    ClearProductInput();
                    //Refresh product data grid view 
                    LoadProductItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        void ClearProductInput()
        {
            txtProductID.Text 
                = txtProductName.Text 
                = txtProductQuantity.Text 
                = txtProductImportPrice.Text
                = txtProductExportPrice.Text 
                = txtProductDescription.Text = "";

            cbProductMaterialID.SelectedIndex = -1;


        }

        private void btnCancelProductTask_Click(object sender, EventArgs e)
        {
            CancelActionInProductPage();
        }

        void CancelActionInProductPage()
        {
            ClearProductInput();
            state = ProductState.NEW;
            lbStateProduct.Text = string.Format("Product");
            btnApplyProduct.Enabled = false;
            btnCancelProductTask.Enabled = false;
            btnUpdateProduct.Enabled = true;
            btnNewProduct.Enabled = true;
            btnDeleteProduct.Enabled = true;
            txtProductID.Enabled = false;
            txtProductID.ReadOnly = true;
        }
    }
}
