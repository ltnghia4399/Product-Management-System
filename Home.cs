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
using Bunifu.Model;

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

        ProductState state = ProductState.NEW;

        public Home(User _user)
        {
            InitializeComponent();

            UpdateForm(_user);

            LoadProductItems();

            LoadMaterialItems();

            LoadCustomerItems();

            LoadEmployeeItems();

            LoadInvoiceList();
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
            lbTitle.Text = "USER PROFILE";
            SetUserTextBoxValue();
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

        private void BtnInvoice_Click(object sender, EventArgs e)
        {
            Main_Page.SetPage((int)TabPagesIndex.CHECKOUT);
            lbTitle.Text = "CHECKOUT";
        }


        //========================================== PRODUCT
        void LoadProductItems()
        {
            dgvProduct.DataSource = Product.Instance.LoadProductItems();
            dgvProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            cbProductMaterialID.ValueMember = "id_cl";
            cbProductMaterialID.DisplayMember = "ten_cl";
            cbProductMaterialID.DataSource = Product.Instance.FillProductComboBox();
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
                btnCancelProduct.Enabled = true;

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
                btnCancelProduct.Enabled = true;

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
                btnCancelProduct.Enabled = true;

                btnUpdateProduct.Enabled = false;
                btnNewProduct.Enabled = false;
                btnDeleteProduct.Enabled = false;
            }
        }


        void UpdateProduct()
        {
            if (txtProductName.Text == "" || txtProductImportPrice.Text == "" || txtProductExportPrice.Text == "" || txtProductQuantity.Text == "")
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

            string deleteCustomerWarning = string.Format("Do you want to update product {0} ?", productName);
            DialogResult warn = MessageBox.Show(deleteCustomerWarning, "Update Product", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (warn == DialogResult.No)
            {
                return;
            }

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
            if (txtProductID.Text == "" || txtProductName.Text == "" || txtProductImportPrice.Text == "" || txtProductExportPrice.Text == "" || txtProductQuantity.Text == "")
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

            string deleteCustomerWarning = string.Format("Do you want to insert product {0} ?", productName);
            DialogResult warn = MessageBox.Show(deleteCustomerWarning, "Delete Product", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (warn == DialogResult.No)
            {
                return;
            }

            if (Product.Instance.ProductExist(productID))
            {
                string result = string.Format("Product {0} has been exist\nInsert Failed", productID);
                MessageBox.Show(result, "Insert Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

            string deleteCustomerWarning = string.Format("Do you want to delete product {0} ?", productName);
            DialogResult warn = MessageBox.Show(deleteCustomerWarning, "Delete Product", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (warn == DialogResult.No)
            {
                return;
            }

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

        void btnCancelProductTask_Click(object sender, EventArgs e)
        {
            CancelActionInProductPage();
        }

        void CancelActionInProductPage()
        {
            ClearProductInput();
            state = ProductState.NEW;
            lbStateProduct.Text = string.Format("Product");
            btnApplyProduct.Enabled = false;
            btnCancelProduct.Enabled = false;
            btnUpdateProduct.Enabled = true;
            btnNewProduct.Enabled = true;
            btnDeleteProduct.Enabled = true;
            txtProductID.Enabled = false;
            txtProductID.ReadOnly = true;
        }

        //========================================== MATERIAL
        void LoadMaterialItems()
        {
            dgvMaterial.DataSource = Material.Instance.LoadMaterialItems();
            dgvMaterial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void DgvMaterial_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaterialID.Text = dgvMaterial.CurrentRow.Cells["Ma chat lieu"].Value.ToString();
            txtMaterialName.Text = dgvMaterial.CurrentRow.Cells["Ten chat lieu"].Value.ToString();
        }

        private void ClearMaterialInput()
        {
            txtMaterialID.Text
                = txtMaterialName.Text = "";


            btnApplyMaterial.Enabled = true;
            btnCancelMaterial.Enabled = true;

            btnUpdateMaterial.Enabled = false;
            btnNewMaterial.Enabled = false;
            btnDeleteMaterial.Enabled = false;

        }

        private void BtnNewMaterial_Click(object sender, EventArgs e)
        {
            state = ProductState.NEW;

            DialogResult dialogResult = MessageBox.Show("Fill all information on the left panel to add more material", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.OK)
            {
                ClearMaterialInput();

                btnApplyMaterial.Text = string.Format("APPLY {0} MATERIAL",state.ToString());
                lbStateMaterial.Text = string.Format("{0} Material", state.ToString());

                txtMaterialID.ReadOnly = false;
                txtMaterialID.Enabled = true;
                txtMaterialID.Focus();
            }
        }

        private void BtnUpdateMaterial_Click(object sender, EventArgs e)
        {
            state = ProductState.UPDATE;

            DialogResult dialogResult = MessageBox.Show("Select row to update material item", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.OK)
            {
                ClearMaterialInput();

                btnApplyMaterial.Text = string.Format("APPLY {0} MATERIAL", state.ToString());
                lbStateMaterial.Text = string.Format("{0} Material", state.ToString());

                txtMaterialName.Focus();
            }
        }

        private void BtnDeleteMaterial_Click(object sender, EventArgs e)
        {
            state = ProductState.DELETE;

            DialogResult dialogResult = MessageBox.Show("Double click to the row of material you want to delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.OK)
            {
                ClearProductInput();

                btnApplyMaterial.Text = string.Format("APPLY {0} MATERIAL", state.ToString());
                lbStateMaterial.Text = string.Format("{0} Material", state.ToString());

            }
        }

        private void BtnCancelMaterial_Click(object sender, EventArgs e)
        {
            CancelActionInMaterialPage();
        }

        private void CancelActionInMaterialPage()
        {
            ClearMaterialInput();
            state = ProductState.NEW;
            lbStateMaterial.Text = string.Format("Material");
            btnApplyMaterial.Enabled = false;
            btnCancelMaterial.Enabled = false;
            btnUpdateMaterial.Enabled = true;
            btnNewMaterial.Enabled = true;
            btnDeleteMaterial.Enabled = true;
            txtMaterialID.Enabled = false;
            txtMaterialID.ReadOnly = true;
        }

        private void BtnApplyMaterial_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case ProductState.NEW:
                    InsertMaterial();
                    break;
                case ProductState.UPDATE:
                    UpdateMaterial();
                    break;
                case ProductState.DELETE:
                    DeleteMaterial();
                    break;
                default:
                    break;
            }
        }

        private void InsertMaterial()
        {
            if (txtMaterialID.Text == "" || txtMaterialName.Text == "")
            {
                MessageBox.Show("Please fill material information on the left panel ", "Insert Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string materialID = txtMaterialID.Text.Trim();
            string materialName = txtMaterialName.Text.Trim();

            string deleteCustomerWarning = string.Format("Do you want to insert material {0} ?", materialName);
            DialogResult warn = MessageBox.Show(deleteCustomerWarning, "Insert Material", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (warn == DialogResult.No)
            {
                return;
            }

            if (Material.Instance.MaterialExist(materialID))
            {
                string result = string.Format("Material {0} has been exist\nInsert Failed",materialName);
                MessageBox.Show(result, "Insert Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Material.Instance.InsertProductItem(materialID, materialName);
                string insertMaterialInfo = string.Format("Insert material {0} successful", materialName);
                DialogResult dr = MessageBox.Show(insertMaterialInfo, "Insert Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    //Refresh product data grid view 
                    ClearMaterialInput();
                    LoadMaterialItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void UpdateMaterial()
        {
            if (txtMaterialName.Text == "")
            {
                MessageBox.Show("Please fill the name of the material you want to update ", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string materialID = txtMaterialID.Text.Trim();
            string materialName = txtMaterialName.Text.Trim();

            string deleteCustomerWarning = string.Format("Do you want to update material {0} ?", materialName);
            DialogResult warn = MessageBox.Show(deleteCustomerWarning, "Update Material", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (warn == DialogResult.No)
            {
                return;
            }

            try
            {
                Material.Instance.UpdateProductItemInformation(materialID, materialName);
                string updateMaterialInfo = string.Format("Update material information successful");
                DialogResult dr = MessageBox.Show(updateMaterialInfo, "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    //Refresh material data grid view 
                    LoadMaterialItems();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void DeleteMaterial()
        {
            if (txtMaterialID.Text == "")
            {
                MessageBox.Show("Please double click to the row of product you want to delete ", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string materialID = txtMaterialID.Text.Trim();
            string materialName = txtMaterialName.Text.Trim();

            string deleteMaterialWarning = string.Format("Do you want to delete material {0} ?", materialName);
            DialogResult warn = MessageBox.Show(deleteMaterialWarning, "Delete Material", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (warn == DialogResult.No)
            {
                return;
            }

            try
            {
                Material.Instance.DeleteMaterialItem(materialID);
                string deleteMaterialInfo = string.Format("Delete material {0} successful", materialName);
                DialogResult dr = MessageBox.Show(deleteMaterialInfo, "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    //Clear all product input textbox
                    ClearMaterialInput();
                    //Refresh product data grid view 
                    LoadMaterialItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }


        //========================================== CUSTOMER

        void LoadCustomerItems()
        {
            dgvCustomer.DataSource = Customer.Instance.LoadCustomerItems();
            dgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ClearCustomerInput()
        {
            txtCustomerAddress.Text = txtCustomerID.Text = txtCustomerName.Text = txtCustomerTel.Text = "";

            btnApplyCustomer.Enabled = true;
            btnCancelCustomer.Enabled = true;

            btnUpdateCustomer.Enabled = false;
            btnNewCustomer.Enabled = false;
            btnDeleteCustomer.Enabled = false;

        }

        private void BtnNewCustomer_Click(object sender, EventArgs e)
        {
            state = ProductState.NEW;

            DialogResult dialogResult = MessageBox.Show("Fill all information on the left panel to add more customer", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.OK)
            {
                ClearCustomerInput();

                btnApplyCustomer.Text = string.Format("APPLY {0} Customer", state.ToString());
                lbStateCustomer.Text = string.Format("{0} Customer", state.ToString());

                
                txtCustomerID.ReadOnly = false;
                txtCustomerID.Enabled = true;
                txtCustomerID.Focus();
            }
        }

        private void BtnUpdateCustomer_Click(object sender, EventArgs e)
        {
            state = ProductState.UPDATE;

            DialogResult dialogResult = MessageBox.Show("Select row to update customer information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.OK)
            {
                ClearCustomerInput();

                btnApplyCustomer.Text = string.Format("APPLY {0} CUSTOMER", state.ToString());
                lbStateCustomer.Text = string.Format("{0} Customer", state.ToString());

                txtCustomerName.Focus();
            }
        }

        private void BtnDeleteCustomer_Click(object sender, EventArgs e)
        {
            state = ProductState.DELETE;

            DialogResult dialogResult = MessageBox.Show("Double click to the row of customer you want to delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.OK)
            {
                ClearCustomerInput();

                btnApplyCustomer.Text = string.Format("APPLY {0} CUSTOMER", state.ToString());
                lbStateCustomer.Text = string.Format("{0} Customer", state.ToString());

            }
        }

        private void BtnCancelCustomer_Click(object sender, EventArgs e)
        {
            CancelActionInCustomerPage();
        }

        void CancelActionInCustomerPage()
        {
            ClearCustomerInput();
            state = ProductState.NEW;
            lbStateCustomer.Text = string.Format("Customer");
            btnApplyCustomer.Enabled = false;
            btnCancelCustomer.Enabled = false;
            btnUpdateCustomer.Enabled = true;
            btnNewCustomer.Enabled = true;
            btnDeleteCustomer.Enabled = true;
            txtCustomerID.Enabled = false;
            txtCustomerID.ReadOnly = true;
        }

        private void BtnApplyCustomer_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case ProductState.NEW:
                    InsertCustomer();
                    break;
                case ProductState.UPDATE:
                    UpdateCustomer();
                    break;
                case ProductState.DELETE:
                    DeleteCustomer();
                    break;
                default:
                    break;
            }
        }

        void InsertCustomer()
        {
            if (txtCustomerID.Text == "" || txtCustomerName.Text == "" || txtCustomerAddress.Text == "" || txtCustomerTel.Text == "")
            {
                MessageBox.Show("Please fill customer information on the left panel ", "Insert Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string customerID = txtCustomerID.Text.Trim();
            string customerName = txtCustomerName.Text.Trim();
            string customerAddress = txtCustomerAddress.Text.Trim();
            int customerTel = Convert.ToInt32(txtCustomerTel.Text.Trim());

            string deleteCustomerWarning = string.Format("Do you want to insert customer {0} ?", customerName);
            DialogResult warn = MessageBox.Show(deleteCustomerWarning, "Insert Customer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (warn == DialogResult.No)
            {
                return;
            }

            if (Customer.Instance.CustomerExist(customerID))
            {
                string result = string.Format("Customer {0} has been exist\nInsert Failed", customerName);
                MessageBox.Show(result, "Insert Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Customer.Instance.InsertCustomerItem(customerName, customerAddress, customerTel);
                string insertCustomerInfo = string.Format("Insert material {0} successful", customerName);
                DialogResult dr = MessageBox.Show(insertCustomerInfo, "Insert Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    //Refresh product data grid view 
                    ClearCustomerInput();
                    LoadCustomerItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        void UpdateCustomer()
        {
            if (txtCustomerID.Text == "" || txtCustomerName.Text == "" || txtCustomerAddress.Text == "" || txtCustomerTel.Text == "")
            {
                MessageBox.Show("Select row to update customer information", "Insert Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string customerID = txtCustomerID.Text.Trim();
            string customerName = txtCustomerName.Text.Trim();
            string customerAddress = txtCustomerAddress.Text.Trim();
            long customerTel = Convert.ToInt64(txtCustomerTel.Text.Trim());

            string deleteCustomerWarning = string.Format("Do you want to update customer {0} ?", customerName);
            DialogResult warn = MessageBox.Show(deleteCustomerWarning, "Update Customer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (warn == DialogResult.No)
            {
                return;
            }

            try
            {
                Customer.Instance.UpdateCustomerInformation(customerID, customerName, customerAddress, customerTel);
                string updateCustomerInfo = string.Format("Update material information successful");
                DialogResult dr = MessageBox.Show(updateCustomerInfo, "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    //Refresh material data grid view 
                    LoadCustomerItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        void DeleteCustomer()
        {
            if (txtCustomerID.Text == "" || txtCustomerName.Text == "" || txtCustomerAddress.Text == "" || txtCustomerTel.Text == "")
            {
                MessageBox.Show("Please double click to the row of customer you want to delete ", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string customerID = txtCustomerID.Text.Trim();
            string customerName = txtCustomerName.Text.Trim();
            
            string deleteCustomerWarning = string.Format("Do you want to delete customer {0} ?", customerName);
            DialogResult warn = MessageBox.Show(deleteCustomerWarning, "Delete Customer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (warn == DialogResult.No)
            {
                return;
            }

            try
            {
                Customer.Instance.DeleteCustomerItem(customerID);
                string deleteMaterialInfo = string.Format("Delete customer {0} successful", customerName);
                DialogResult dr = MessageBox.Show(deleteMaterialInfo, "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    //Clear all product input textbox
                    ClearCustomerInput();
                    //Refresh product data grid view 
                    LoadCustomerItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void DgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCustomerID.Text = dgvCustomer.CurrentRow.Cells["ID khach hang"].Value.ToString();
            txtCustomerName.Text = dgvCustomer.CurrentRow.Cells["Ten khach hang"].Value.ToString();
            txtCustomerAddress.Text = dgvCustomer.CurrentRow.Cells["Dia chi"].Value.ToString();
            txtCustomerTel.Text = dgvCustomer.CurrentRow.Cells["So dien thoai"].Value.ToString();
        }

        //========================================== EMPLOYEE

        void LoadEmployeeItems()
        {
            dgvEmployeeAccount.DataSource = Account.Instance.LoadEmployeeAccount();
            dgvEmployeeAccount.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void BtnNewEmp_Click(object sender, EventArgs e)
        {
            state = ProductState.NEW;

            DialogResult dialogResult = MessageBox.Show("Fill all information on the left panel to add more employee", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.OK)
            {
                ClearEmployeeInput();

                btnApplyEmp.Text = string.Format("APPLY {0} EMPLOYEE", state.ToString());
                lbStateEmp.Text = string.Format("{0} Employee", state.ToString());

                txtEmpUserName.ReadOnly = false;
                txtEmpUserName.Enabled = true;
                txtEmpUserName.Focus();
            }
        }

        private void BtnUpdateEmp_Click(object sender, EventArgs e)
        {
            state = ProductState.UPDATE;

            DialogResult dialogResult = MessageBox.Show("Select row to update employee information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.OK)
            {
                ClearEmployeeInput();

                btnApplyEmp.Text = string.Format("APPLY {0} EMPLOYEE", state.ToString());
                lbStateEmp.Text = string.Format("{0} Employee", state.ToString());

                txtEmpDisplayName.Focus();
            }
        }

        private void BtnDeleteEmp_Click(object sender, EventArgs e)
        {
            state = ProductState.DELETE;

            DialogResult dialogResult = MessageBox.Show("Double click to the row of employee you want to delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.OK)
            {
                ClearEmployeeInput();

                btnApplyEmp.Text = string.Format("APPLY {0} EMPLOYEE", state.ToString());
                lbStateEmp.Text = string.Format("{0} Employee", state.ToString());

            }
        }

        private void BtnCancelEmp_Click(object sender, EventArgs e)
        {
            ClearEmployeeInput();
            state = ProductState.NEW;
            lbStateEmp.Text = string.Format("Employee Account");
            btnApplyEmp.Enabled = false;
            btnCancelEmp.Enabled = false;
            btnUpdateEmp.Enabled = true;
            btnNewEmp.Enabled = true;
            btnDeleteEmp.Enabled = true;
            txtEmpUserName.Enabled = false;
            txtEmpUserName.ReadOnly = true;
        }

        void ClearEmployeeInput()
        {
            txtEmpUserName.Text = txtEmpDisplayName.Text = txtEmpEmail.Text = txtEmpPassword.Text = txtEmpDescription.Text = "";

            cbEmpAccountRole.SelectedIndex = 0;

            btnApplyEmp.Enabled = true;
            btnCancelEmp.Enabled = true;

            btnUpdateEmp.Enabled = false;
            btnNewEmp.Enabled = false;
            btnDeleteEmp.Enabled = false;
        }

        private void BtnApplyEmp_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case ProductState.NEW:
                    InsertEmployee();
                    break;
                case ProductState.UPDATE:
                    UpdateEmployee();
                    break;
                case ProductState.DELETE:
                    DeleteEmployee();
                    break;
                default:
                    break;
            }
        }

        void InsertEmployee()
        {
            if (txtEmpUserName.Text == "" || txtEmpDisplayName.Text == "" || txtEmpEmail.Text == "" || txtEmpPassword.Text == "")
            {
                MessageBox.Show("Please fill employee information on the left panel ", "Insert Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string empUserName = txtEmpUserName.Text.Trim();
            string empDisplayName = txtEmpDisplayName.Text.Trim();
            string empEmail = txtEmpEmail.Text.Trim();
            string empPassword = txtEmpPassword.Text.Trim();
            int empRole = cbEmpAccountRole.SelectedIndex;
            string empDescription = txtEmpDescription.Text.Trim();

            string insertEmpWarning = string.Format("Do you want to insert employee {0} ?", empDisplayName);
            DialogResult warn = MessageBox.Show(insertEmpWarning, "Insert Customer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (warn == DialogResult.No)
            {
                return;
            }

            if (Account.Instance.GetAccountByUserName(empUserName) != null)
            {
                string result = string.Format("Your username {0} has been exist\nInsert Failed", empDisplayName);
                MessageBox.Show(result, "Insert Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                //Account.Instance.RegisterUser(empUserName, empDisplayName, empEmail, empRole);

                Account newAccount = new RegisterAccount(empDisplayName, empUserName, empPassword, empEmail, empRole, empDescription);

                newAccount.RegisterUser();

                string insertCustomerInfo = string.Format("Insert employee {0} successful", empDisplayName);
                DialogResult dr = MessageBox.Show(insertCustomerInfo, "Insert Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    //Refresh product data grid view 
                    ClearEmployeeInput();
                    LoadEmployeeItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        int empID;
        void UpdateEmployee()
        {
            if (txtEmpDisplayName.Text == "" || txtEmpEmail.Text == "" || txtEmpPassword.Text == "")
            {
                MessageBox.Show("Select row to update employee information ", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int empUserID = empID;
            string empDisplayName = txtEmpDisplayName.Text.Trim();
            string empEmail = txtEmpEmail.Text.Trim();
            string empPassword = txtEmpPassword.Text.Trim();
            int empRole = cbEmpAccountRole.SelectedIndex;
            string empDescription = txtEmpDescription.Text.Trim();

            string deleteCustomerWarning = string.Format("Do you want to update customer {0} ?", empDisplayName);
            DialogResult warn = MessageBox.Show(deleteCustomerWarning, "Update Customer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (warn == DialogResult.No)
            {
                return;
            }

            try
            {
                Account.Instance.UpdateAccountInfomation(empUserID, empDisplayName, empPassword, empEmail,empRole, empDescription);
                string updateCustomerInfo = string.Format("Update employee information successful");
                DialogResult dr = MessageBox.Show(updateCustomerInfo, "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    //Refresh material data grid view 
                    LoadEmployeeItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        void DeleteEmployee()
        {
            if (txtEmpUserName.Text == "" || txtEmpDisplayName.Text == "" || txtEmpEmail.Text == "" || txtEmpPassword.Text == "")
            {
                MessageBox.Show("Please double click to the row of employee you want to delete ", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            int empUserID = empID;
            string empUserName = txtEmpUserName.Text.Trim();

            string deleteCustomerWarning = string.Format("Do you want to delete employee {0} ?", empUserName);
            DialogResult warn = MessageBox.Show(deleteCustomerWarning, "Delete Employee", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (warn == DialogResult.No)
            {
                return;
            }

            try
            {
                Account.Instance.DeleteAccountByID(empUserID);
                string deleteMaterialInfo = string.Format("Delete employee {0} successful", empUserName);
                DialogResult dr = MessageBox.Show(deleteMaterialInfo, "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    //Clear all product input textbox
                    ClearEmployeeInput();
                    //Refresh product data grid view 
                    LoadEmployeeItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }


        private void DgvEmployeeAccount_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            empID = Convert.ToInt32(dgvEmployeeAccount.CurrentRow.Cells["ID Nhan vien"].Value.ToString());

            txtEmpUserName.Text = dgvEmployeeAccount.CurrentRow.Cells["Ten dang nhap"].Value.ToString();
            txtEmpDisplayName.Text = dgvEmployeeAccount.CurrentRow.Cells["Ten hien thi"].Value.ToString();
            txtEmpEmail.Text = dgvEmployeeAccount.CurrentRow.Cells["Email"].Value.ToString();
            txtEmpPassword.Text = dgvEmployeeAccount.CurrentRow.Cells["Mat khau"].Value.ToString();

            txtEmpDescription.Text = dgvEmployeeAccount.CurrentRow.Cells["Mo ta ban than"].Value.ToString();

            cbEmpAccountRole.SelectedIndex = Convert.ToInt32(dgvEmployeeAccount.CurrentRow.Cells["Loai tai khoan"].Value.ToString());
        }


        //=================================================== CHECK OUT

        void LoadInvoiceList()
        {
            cboInvoiceList.DataSource = Invoice.Instance.FillInvoiceListComboBox();
            cboInvoiceList.ValueMember = "id_hd";
            cboInvoiceList.DisplayMember = "id_hd";
            cboInvoiceList.SelectedIndex = -1;

            cboInvoiceIDCustomer.DataSource = Customer.Instance.LoadCustomerItems();
            cboInvoiceIDCustomer.ValueMember = "ID khach hang";
            cboInvoiceIDCustomer.SelectedIndex = -1;

            cboInvoiceProduct.DataSource = Product.Instance.LoadProductItems();
            cboInvoiceProduct.ValueMember = "ID Hang";
            cboInvoiceProduct.DisplayMember = "Ten mat hang";
            cboInvoiceProduct.SelectedIndex = -1;
        }

        void Numbervalidation(KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("Number input only","Validation",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                e.Handled = true;
            }
        }

        string CreateInvoiceKey(string key)
        {
            try
            {
                string d = DateTime.Now.ToString("ddMMyyyy");
                string t = DateTime.Now.ToString("h:mm-tt");

                key = string.Format("{0}-{1}-{2}",key,d,t);

                return key;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        string GetDateTime(string _dateFormat, string _timeFormat)
        {
            try
            {
                string d = DateTime.Now.ToString(_dateFormat);
                string t = DateTime.Now.ToString(_timeFormat);

                string r = string.Format("{0} {1}", d, t);

                return r;

            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lbInvoiceID.Text = CreateInvoiceKey("HD");
            lbInvoiceDate.Text = GetDateTime("dd/MMM/yyyy", "h:mm tt");
            lbInvoiceStaffID.Text = id.ToString();
            lbInvoiceStaffName.Text = displayname;

            //Cusomter
            cboInvoiceIDCustomer.Enabled = true;
            cboInvoiceIDCustomer.Focus();
            cboInvoiceIDCustomer.SelectedIndex = -1;
            //Product
            cboInvoiceProduct.Enabled = true;
            txtInvoiceDiscount.Enabled = true;
            txtInvoiceDiscount.ReadOnly = false;
            txtInvoiceQuantity.Enabled = true;
            txtInvoiceQuantity.ReadOnly = false;

            btnInvoiceAddProduct.Enabled = true;
        }

        void LoadInvoiceInformation(Bill _bill, Client _client)
        {
            lbInvoiceStaffID.Text = _bill.StaffID.ToString();
            lbInvoiceTotal.Text = _bill.Total.ToString();
            lbInvoiceID.Text = _bill.BillID;
            lbInvoiceDate.Text = _bill.Date;
            lbInvoiceStaffName.Text = Account.Instance.GetUserByID(_bill.StaffID);

            txtInvoiceCustomerName.Text = _client.Name;
            txtInvoiceCustomerAddress.Text = _client.Address;
            txtInvoiceTel.Text = _client.Tel;
            cboInvoiceIDCustomer.SelectedIndex = _client.Id - 1;

            dgvInvoiceItems.DataSource = Invoice.Instance.GetInvoiceItem(_bill.BillID);
            dgvInvoiceItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void BtnFindInvoice_Click(object sender, EventArgs e)
        {
            if (cboInvoiceList.SelectedIndex == -1)
            {
                MessageBox.Show("Choose a receipt to find", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboInvoiceList.Focus();
                return;
            }

            try
            {
                
                Bill bill = Invoice.Instance.GetBillByBillID(cboInvoiceList.SelectedValue.ToString());

                Client client = Customer.Instance.GetCustomerByID(bill.CustomerID);

                LoadInvoiceInformation(bill,client);
                //LoadInvoiceGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtInvoiceQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            Numbervalidation(e);
        }

        private void TxtInvoiceDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            Numbervalidation(e);
        }

        
        private void TxtInvoiceTel_KeyPress(object sender, KeyPressEventArgs e)
        {
            Numbervalidation(e);
        }

        private void NewCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtInvoiceCustomerName.Enabled = true;
            txtInvoiceCustomerAddress.Enabled = true;
            txtInvoiceTel.Enabled = true;

            txtInvoiceCustomerName.ReadOnly = false;
            txtInvoiceCustomerAddress.ReadOnly = false;
            txtInvoiceTel.ReadOnly = false;

            cboInvoiceIDCustomer.SelectedIndex = -1;

            bunifuButton1.Enabled = true;
        }

        private void CboInvoiceIDCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            Client client = Customer.Instance.GetCustomerByID(cboInvoiceIDCustomer.SelectedIndex + 1);

            if(client == null)
            {
                txtInvoiceCustomerName.Text = "";
                txtInvoiceCustomerAddress.Text = "";
                txtInvoiceTel.Text = "";
                return;
            }

            txtInvoiceCustomerName.Text = client.Name;
            txtInvoiceCustomerAddress.Text = client.Address;
            txtInvoiceTel.Text = client.Tel;
        }

        float exportPrice;

        private void CboInvoiceProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ProductItem productItem = Product.Instance.GetProductByID(cboInvoiceProduct.SelectedValue.ToString());

            if(cboInvoiceProduct.SelectedValue == null)
            {
                return;
            }

            ProductItem productItem = Product.Instance.GetProductByID(cboInvoiceProduct.SelectedValue.ToString());

            if (productItem == null)
            {
                return;
            }

            exportPrice = productItem.ExportPrice;

            lbInvoiceUnitPrice.Text = string.Format("{0} VND", exportPrice.ToString());
        }

        private void btnInvoiceAddProduct_Click(object sender, EventArgs e)
        {
            

            string invoiceID = lbInvoiceID.Text;
            string invoiceDate = lbInvoiceDate.Text;
            string invoiceStaffID = lbInvoiceStaffID.Text;
            string invoiceStaffName = lbInvoiceStaffName.Text;


            int invoiceCustomerID = cboInvoiceIDCustomer.SelectedIndex + 1;

            string invoiceTotalPrice = lbInvoiceTotal.Text;

            string invoiceProductID = cboInvoiceProduct.SelectedValue.ToString();

            int invoiceProductQuantity = Convert.ToInt32( txtInvoiceQuantity.Text.Trim());

            try
            {
                if (!Invoice.Instance.InvoiceExist(invoiceID))
                {
                    Invoice.Instance.CreateNewInvoice(invoiceID,invoiceStaffID, invoiceDate, invoiceCustomerID,invoiceTotalPrice);
                }

                if (txtInvoiceQuantity.Text.Length == 0 || (txtInvoiceQuantity.Text == "0"))
                {
                    MessageBox.Show("You must enter Product Quantily", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtInvoiceQuantity.Focus();
                    txtInvoiceQuantity.Text = "";
                    return;
                }

                if (txtInvoiceDiscount.Text.Length == 0)
                {
                    MessageBox.Show("You must enter Product Discount", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtInvoiceDiscount.Focus();
                    txtInvoiceDiscount.Text = "";
                    return;
                }

                if (Invoice.Instance.ProductInBillDetail(invoiceProductID, invoiceID))
                {
                    MessageBox.Show("Product already have, Enter another product type", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboInvoiceProduct.Focus();
                    return;
                }




                float quantity;

                string productID;

                string invoiceDiscount = txtInvoiceDiscount.Text.Trim();

                ProductItem productItem = Product.Instance.GetProductByID(cboInvoiceProduct.SelectedValue.ToString());

                quantity = productItem.Quantity;
                productID = productItem.Id;


                if (Convert.ToDouble(invoiceProductQuantity) > quantity)
                {
                    MessageBox.Show("This product has remained " + quantity, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtInvoiceQuantity.Text = "";
                    txtInvoiceQuantity.Focus();
                    return;
                }
                else
                {
                    try
                    {
                        //DataProvider.Instance.ExecuteNonQuery(query, new object[] { invoiceID, proID, proQuantily, proPrice, proDiscount, thanhtien });

                        Invoice.Instance.CreateNewInvoiceDetail(invoiceID, productID, invoiceProductQuantity.ToString(), exportPrice.ToString(), invoiceDiscount, GetSubTotal().ToString());

                        //DataProvider.Instance.ExecuteNonQuery(queryInsert, new object[] { invoiceID, proID, proQuantily, proPrice, proDiscount, thanhtien });

                        //LoadInvoiceGridView();

                        dgvInvoiceItems.DataSource = Invoice.Instance.GetInvoiceItem(lbInvoiceID.Text);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Error: " + error, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                // Update Product quantily to Database table hanghoa

                double soLuongConLai, tong, Tongmoi;



                soLuongConLai = quantity - Convert.ToDouble(invoiceProductQuantity);

                Invoice.Instance.UpdateProductQuantity(soLuongConLai, productID);

                //Update Total price of Invoice

                string queryTongtien = "Select tongtien from HDban where id_hd = '" + invoiceID + "'";

                tong = Convert.ToDouble(DataProvider.DataProvider.Instance.GetFieldValues(queryTongtien));



                Tongmoi = tong + Convert.ToDouble(GetSubTotal());

                string query = "call SP_UpdatePriceInvoice( @tongmoi , @id_hd )";

                DataProvider.DataProvider.Instance.ExecuteNonQuery(query, new object[] { Tongmoi, invoiceID });

                lbInvoiceTotal.Text = Tongmoi.ToString();

                dgvInvoiceItems.DataSource = Invoice.Instance.GetInvoiceItem(lbInvoiceID.Text);
                cboInvoiceList.DataSource = Invoice.Instance.FillInvoiceListComboBox();
                //ResetValuesPro();
                //btnSave.Focus();
                //btnPrintBill.Enabled = true;
                //btnNew.Enabled = true;

            }
            catch (Exception)
            {

                throw;
            }


        }

        private void TxtInvoiceQuantity_TextChanged(object sender, EventArgs e)
        {
            lbInvoiceSubTotal.Text = string.Format("{0} VND", GetSubTotal());
        }

        private void TxtInvoiceDiscount_TextChanged(object sender, EventArgs e)
        {
            lbInvoiceSubTotal.Text = string.Format("{0} VND", GetSubTotal());
        }

        double GetSubTotal()
        {
            double tongTien, soLuong, donGia, giamGia;

            if (txtInvoiceQuantity.Text == "")
            {
                soLuong = 0;
            }
            else
            {
                soLuong = Convert.ToDouble(txtInvoiceQuantity.Text);
            }

            if (txtInvoiceDiscount.Text == "")
            {
                giamGia = 0;
            }
            else
            {
                giamGia = Convert.ToDouble(txtInvoiceDiscount.Text);
            }

                donGia = Convert.ToDouble(exportPrice);

            tongTien = (soLuong * donGia) - (soLuong * donGia * giamGia / 100);

            return tongTien;
        }

        private void BunifuButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Customer.Instance.InsertCustomerItem(txtInvoiceCustomerName.Text.Trim(), txtInvoiceCustomerAddress.Text.Trim(), Convert.ToInt32(txtInvoiceTel.Text.Trim()));
                string insertCustomerInfo = string.Format("Insert customer {0} successful. Do you want to add more customer ?", txtInvoiceCustomerName.Text.Trim());
                DialogResult dr = MessageBox.Show(insertCustomerInfo, "Insert Successful", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    //Refresh product data grid view 
                    txtInvoiceCustomerName.Text = "";
                    txtInvoiceCustomerAddress.Text = "";
                    txtInvoiceTel.Text = "";


                    cboInvoiceIDCustomer.DataSource = Customer.Instance.LoadCustomerItems();
                    cboInvoiceIDCustomer.ValueMember = "ID khach hang";
                    cboInvoiceIDCustomer.SelectedIndex = -1;
                }
                else
                {
                    bunifuButton1.Enabled = false;

                    txtInvoiceCustomerName.Enabled = false;
                    txtInvoiceCustomerAddress.Enabled = false;
                    txtInvoiceTel.Enabled = false;

                    txtInvoiceCustomerName.ReadOnly = true;
                    txtInvoiceCustomerAddress.ReadOnly = true;
                    txtInvoiceTel.ReadOnly = true;

                    txtInvoiceCustomerName.Text = "";
                    txtInvoiceCustomerAddress.Text = "";
                    txtInvoiceTel.Text = "";


                    cboInvoiceIDCustomer.DataSource = Customer.Instance.LoadCustomerItems();
                    cboInvoiceIDCustomer.ValueMember = "ID khach hang";
                    cboInvoiceIDCustomer.SelectedIndex = -1;

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bunifuButton1.Enabled = false;
            cboInvoiceProduct.SelectedIndex = -1;
            cboInvoiceIDCustomer.SelectedIndex = -1;
            cboInvoiceIDCustomer.Enabled = false;
            cboInvoiceProduct.Enabled = false;
            btnInvoiceAddProduct.Enabled = false;
            txtInvoiceDiscount.Enabled = false;
            txtInvoiceQuantity.Enabled = false;
            txtInvoiceQuantity.ReadOnly = true;
            txtInvoiceDiscount.ReadOnly = true;
        }
    }
}
