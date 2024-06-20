using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using DoAnNhom3.DAO;

namespace DoAnNhom3
{
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
        }
       
        private string Hashing(string password)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);

            SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hash;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void showPass_CheckedChanged(object sender, EventArgs e)
        {
            if (txtPassword.UseSystemPasswordChar)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            // Đóng form đăng nhập
            this.Close();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string connectionString = SystemConstants.MainConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Select idStaff: nếu nhập tài khoản, mật khẩu đúng thì sẽ có kết quả trả về
                string query = "SELECT fullName, Account.idStaff FROM Account, Staff Where Account.idStaff = Staff.idStaff AND userName = @username AND passWord = @password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", txtUsername.Text);
                command.Parameters.AddWithValue("@password", Hashing(txtPassword.Text));

                // Thực hiện truy vấn SQL
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string idStaff = reader["idStaff"].ToString();
                    string fullName = reader["fullName"].ToString();
                    reader.Close();

                    // Tạo câu lệnh SQL để lấy position của staff dựa trên idStaff
                    query = "SELECT position FROM Staff WHERE idStaff = @idStaff";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idStaff", idStaff);

                    // Thực hiện truy vấn SQL
                    object positionObj = command.ExecuteScalar();
                    if (positionObj != null)
                    {
                        string position = positionObj.ToString();

                        // Kiểm tra giá trị của position và hiển thị form tương ứng
                        if (position == "Nhân Viên")
                        {
                            this.Hide();
                            // Hiển thị form nhân viên
                            Client.NhanVien nhanVien = new Client.NhanVien(idStaff, fullName);
                            nhanVien.Show();
                        }
                        else if (position == "Quản Lý")
                        {
                            this.Hide();
                            // Hiển thị form quản lý
                            Server.ChuCuaHang quanLy = new Server.ChuCuaHang();
                            quanLy.Start();
                            quanLy.Show();
                        }
                    }
                    else
                    {
                        // Hiển thị thông báo lỗi: không tìm thấy vị trí của nhân viên
                        MessageBox.Show("Không tìm thấy vị trí của nhân viên", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Hiển thị thông báo lỗi: sai tên đăng nhập hoặc mật khẩu
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                // Đóng kết nối
                connection.Close();
            }
        }

        private void DangNhap_Load(object sender, EventArgs e)
        {

        }
    }
}
