using Client;
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
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLb_DK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DangKy dangKy = new DangKy();
            this.Hide();
            dangKy.ShowDialog();
            this.Show();
        }

        private void btn_dangNhap_Click(object sender, EventArgs e)
        {
            string username= txt_username.Text;
            string password=txt_Pass.Text;

            if (Login(username, password))
            {
                Account loginAccount = AccountDAO.Instance.GetAccountByUserName(username);
                NhanVien nv= new NhanVien(loginAccount);
                this.Hide();
                nv.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Sai ten tai khoan");
            }

        }

        bool Login(string username, string password)
        {
            return AccountDAO.Instance.Login(username, password);
        }
    }
}
