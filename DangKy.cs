//using PacketStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class DangKy : Form
    {
        public DangKy()
        {
            InitializeComponent();
        }

        private bool InputIsLogical()
        {
            string sHoTen = txt_fullname.Text;
            string sUsername = txt_username.Text;
            string sPassword = txt_password.Text;
            string sConfirmPasswd = txtConfirmPass.Text;

            if (sHoTen == "" || sUsername == "" || sPassword == "" || sConfirmPasswd == "")
            {
                MessageBox.Show("Vui lòng điền đẩy đủ thông tin!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (sUsername.Length > 20)
            {
                MessageBox.Show("Tên đăng nhập không quá 20 ký tự!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (sPassword.Length > 20)
            {
                MessageBox.Show("Mật khẩu không quá 20 ký tự!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (sUsername.Where(x => x == ' ').Any())
            {
                MessageBox.Show("Tên đăng nhập không có khoảng trống!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (sPassword.Where(x => x == ' ').Any())
            {
                MessageBox.Show("Mật khẩu không có khoảng trống!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (sConfirmPasswd != sPassword)
            {
                MessageBox.Show("Mật khẩu không trùng khớp! Vui lòng xác nhận lại mật khẩu!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void btn_register_Click(object sender, EventArgs e)
        {/*
            if (InputIsLogical())
            {
                Packet sendData = new Packet()
                {
                    User = User.Both,
                    Header = Header.TaiKhoan,
                    Option = Options.SignUp,
                    TaiKhoan = new TaiKhoan("", txt_username.Text, txt_fullname.Text.Trim(), txt_password.Text, "Nhân viên")
                };
                byte[] data = sendData.GetDataStream();
                NetworkStream net_stream = frmDangNhap.tcpClient.GetStream();
                net_stream.Write(data, 0, data.Length);
                net_stream.Flush();
            }*/
        }

        private void cbShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowPass.Checked)
            {
                txt_password.PasswordChar = '\0';
                txtConfirmPass.PasswordChar = '\0';
            }
            else
            {
                txt_password.PasswordChar = '*';
                txtConfirmPass.PasswordChar = '*';
            }
        }

        private void lb_backtologin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void DangKy_Load(object sender, EventArgs e)
        {

        }
    }
}