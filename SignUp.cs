using System;
using System.Data.Entity;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Airoport
{
    public partial class SignUp : Form
    {
        public static string roleDb { private set; get; }
        public static int userId { private set; get; }

        public SignUp()
        {
            InitializeComponent();
        }

        public void ValidateSignUp()
        { 
            string query = "SELECT * FROM Users";
            bool isUserValid = false;

            using (SQLiteCommand command = new SQLiteCommand(query, ConnectionDataBaseClass.Connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userId = reader.GetInt32(0);
                        string loginDb = reader.GetString(1);
                        string passwordDb = reader.GetString(2);
                        roleDb = reader.GetString(3);

                        if (loginDb.Equals(LoginBox.Text) && passwordDb.Equals(Hashing.Hash(PasswordBox.Text))
                             && (roleDb.Equals("administrator") || roleDb.Equals("mechanic") || roleDb.Equals("manager")))
                        {
                            isUserValid = true;
                            break;
                        }
                    }
                }
            }

            if (isUserValid)
            {
                switch (roleDb)
                {
                    case "administrator":
                        MessageBox.Show("Добро пожаловать!!!");
                        OpenForms.OpenForm(this, new Main());
                        break;
                }
            }
            else
            {
                MessageBox.Show("Перепроверьте введенные данные!!!");
            }
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            ValidateSignUp();
        }

        private void RegButton_Click(object sender, EventArgs e)
        {
            OpenForms.OpenFormInPanel(new Register(), panel1);
        }

        private void SignUp_Load(object sender, EventArgs e)
        {
            ConnectionDataBaseClass.Connect();
        }

        private void SignUp_Leave(object sender, EventArgs e)
        {
            ConnectionDataBaseClass.Disconnect();
        }
    }
}
