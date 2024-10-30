using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Airoport
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void RegButton_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Users (login, password, role) " +
            "VALUES (@login, @password, @role)";
            if (PasswordBox.Text.Equals(ConfirmPasswordBox.Text))
            {
                SQLiteCommand command = new SQLiteCommand(query, ConnectionDataBaseClass.Connection);
                if (LoginValidation.Validation(LoginBox.Text) && PasswordValidtaion.Validation(PasswordBox.Text))
                {
                    command.Parameters.AddWithValue("@login", LoginBox.Text);
                    command.Parameters.AddWithValue("@password", Hashing.Hash(PasswordBox.Text));
                    command.Parameters.AddWithValue("@role", "administrator");
                    command.ExecuteNonQuery();
                    MessageBox.Show("Вы успешно зарегестрировались!");
                }
            }
            else
            {
                MessageBox.Show("Пароли не совпадают");
            }
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            OpenForms.OpenFormInPanel(new SignUp(), panel1);
        }

        private void Register_Leave(object sender, EventArgs e)
        {
            ConnectionDataBaseClass.Disconnect();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            ConnectionDataBaseClass.Connect();
        }
    }
}
