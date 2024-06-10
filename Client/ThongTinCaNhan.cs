using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnNhom3.Client
{
    public partial class ThongTinCaNhan : Form
    {
        private string idStaff;
        private string fullName;
        private string phoneNumber;
        private string position;
        private string userName;
        private string passWord;
        private string newPassWord;
        private StreamWriter writer;

        public ThongTinCaNhan(string idStaff, StreamWriter writer)
        {
            InitializeComponent();
            this.idStaff = idStaff;
            LoadProfile(idStaff);

            maNhanVien.Text = idStaff;
            maNhanVien.Enabled = false;

            hoTen.Text = fullName;
            soDienThoai.Text = phoneNumber;

            chucVu.Text = position;
            chucVu.Enabled = false;

            tenDangNhap.Text = userName;
            tenDangNhap.Enabled = false;

            this.writer = writer;
        }

        void LoadProfile(string idStaff)
        {
            try
            {
                string connectionString = "Data Source=QUANG;Initial Catalog=QLNH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Mở kết nối
                    connection.Open();

                    // Tạo đối tượng command để thực hiện truy vấn
                    string query = "SELECT fullName, phoneNumber, position, userName, passWord FROM Staff, Account WHERE Staff.idStaff = @idStaff AND Staff.idStaff = Account.idStaff";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Truyền tham số vào truy vấn
                        command.Parameters.AddWithValue("@idStaff", idStaff);

                        // Thực hiện truy vấn và đọc dữ liệu
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Lấy dữ liệu từ cột tương ứng và gán vào các biến
                                fullName = reader.GetString(0);
                                phoneNumber = reader.GetString(1);
                                position = reader.GetString(2);
                                userName = reader.GetString(3);
                                passWord = reader.GetString(4);
                            }
                        }
                    }

                    // Đóng kết nối
                    connection.Close();
                }
            }
            catch
            {
            }
        }
        private string Hashing(string password)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);

            SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hash;
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {

        }
    }
}
