using DoAnNhom3.DAO;
using DoAnNhom3.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnNhom3
{
    public partial class ThongTinCaNhan : Form
    {
        private Account loginAccount;
        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }

        public ThongTinCaNhan(Account acc)
        {
            InitializeComponent();
            loginAccount = acc;
            
        }

        void ChangeAccount(Account acc)
        {
            txt_username.Text = LoginAccount.Username;
            txt_displayname.Text = LoginAccount.DisplayName;

        }

        void UpdateAcount()
        {
            string username = txt_username.Text;
            string displayname = txt_displayname.Text;
            string password = txt_password.Text;
            string newpass = txt_newPass.Text;
            string conformpass = txt_confirmPass.Text;

            if (!newpass.Equals(conformpass))
            {
                MessageBox.Show("vui long nhap lai mat khau dung voi mat khau moi");
            }
            else
            {
                if(AccountDAO.Instance.UpdateAccount(username, displayname, password, newpass))
                {
                    MessageBox.Show("cap nhat thanh cong");
                }
                else
                {
                    MessageBox.Show("vui long dien dung mat kkau");
                }
            }
        }

        private void btn_capNhat_Click(object sender, EventArgs e)
        {
            UpdateAcount();
        }
    }
}
