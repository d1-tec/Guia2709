using System;
using System.Windows.Forms;
using SocialBoard.Domain;
using SocialBoard.Exceptions;
using SocialBoard.LogicInstance;

namespace SocialBoard.UI
{
    public partial class CreateUser : UserControl
    {
        private User userAdmin;

        public CreateUser(User userAdmin)
        {
            InitializeComponent();
            InitializeAtributes(userAdmin);
            DisableAllFieldsButMail();
        }

        private void InitializeAtributes(User aUserAdmin)
        {
            userAdmin = aUserAdmin;
            txtPassword.PasswordChar = '*';
        }

        private void ResetFields()
        {
            txtName.Text = "";
            txtLastName.Text = "";
            txtMail.Text = "";
            txtPassword.Text = "";
            lblEmptyName.Text = "";
            lblEmptyLastName.Text = "";
            lblEmptyPassword.Text = "";
            lblErrorMail.Text = "";
            lblWrongBirthDate.Text = "";
        }

        private void DisableAllFieldsButMail()
        {
            txtLastName.Enabled = false;
            txtName.Enabled = false;
            dateTimeBirth.Enabled = false;
            txtPassword.Enabled = false;
            chboxAdministrator.Enabled = false;
        }

        private User CreateAUser()
        {
            string name = txtName.Text;
            string lastName = txtLastName.Text;
            string password = txtPassword.Text;
            string mail = txtMail.Text;
            DateTime birth = dateTimeBirth.Value;
            User newUser = new User() { Name = name, LastName = lastName, BirthDate = birth, Password = password, Mail = mail };
            return newUser;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                User newUser = CreateAUser();
                LogicInstances.UserLogic().Create(userAdmin, newUser);
                if (chboxAdministrator.Checked)
                {
                    LogicInstances.UserLogic().AddAdminRole(newUser.Id);
                }
                Message.ShowSuccessMessage("User created with success");
                ResetFields();

            }
            catch (UserException ex)
            {
                if (ex.Message.Contains("Name") && !ex.Message.Contains("Last"))
                {
                    lblEmptyName.Text = ex.Message;
                }
                else
                {
                    lblEmptyName.Text = "";
                }
                if (ex.Message.Contains("Last"))
                {
                    lblEmptyLastName.Text = ex.Message;
                }
                else
                {
                    lblEmptyLastName.Text = "";
                }
                if (ex.Message.Contains("Invalid"))
                {
                    lblErrorMail.Text = ex.Message;
                }
                else if (ex.Message.Contains("exists"))
                {
                    lblErrorMail.Text = ex.Message;
                }
                else
                {
                    lblErrorMail.Text = "";
                }
                if (ex.Message.Contains("Password"))
                {
                    lblEmptyPassword.Text = ex.Message;
                }
                else
                {
                    lblEmptyPassword.Text = "";
                }
                if (ex.Message.Contains("birth date"))
                {
                    lblWrongBirthDate.Text = ex.Message;
                }
                else
                {
                    lblWrongBirthDate.Text = "";
                }
            }

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (txtName.Text.Equals(""))
            {
                lblEmptyName.Text = "Name cannot be null";
                txtLastName.Enabled = false;
                txtPassword.Enabled = false;
                dateTimeBirth.Enabled = false;
            }
            else
            {
                lblEmptyName.Text = "";
                txtLastName.Enabled = true;
                txtPassword.Enabled = true;
                dateTimeBirth.Enabled = true;
            }
        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {
            if (txtLastName.Text.Equals(""))
            {
                lblEmptyLastName.Text = "Last name cannot be null";
                txtPassword.Enabled = false;
                dateTimeBirth.Enabled = false;
            }
            else
            {
                lblEmptyLastName.Text = "";
                txtPassword.Enabled = true;
                dateTimeBirth.Enabled = true;
            }
        }

        private void txtMail_TextChanged(object sender, EventArgs e)
        {
            if (txtMail.Text.Equals(""))
            {
                lblErrorMail.Text = "Mail cannot be null";
                txtName.Enabled = false;
                txtLastName.Enabled = false;
                txtPassword.Enabled = false;
                dateTimeBirth.Enabled = false;
            }
            else
            {
                lblEmptyName.Text = "";
                txtName.Enabled = true;
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text.Equals(""))
            {
                lblEmptyPassword.Text = "Password cannot be null";
                chboxAdministrator.Enabled = false;
            }
            else
            {
                lblEmptyName.Text = "";
                chboxAdministrator.Enabled = true;
            }
        }
    }
}
