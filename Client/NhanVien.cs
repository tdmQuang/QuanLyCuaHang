using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using DoAnNhom3.DTO;
using DoAnNhom3.DAO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Drawing.Printing;

namespace DoAnNhom3.Client
{
    public partial class NhanVien : Form
    {
        // Khai báo 
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private string idStaff;

        public NhanVien(string idStaff)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            this.idStaff = idStaff;

            try
            {
                // Kết nối đến server localhost sẽ thay đổi thành ip của server khi kết nối LAN
                client = new TcpClient("localhost", 8888);
                reader = new StreamReader(client.GetStream());
                writer = new StreamWriter(client.GetStream());
                writer.AutoFlush = true;

                // Gửi tên người dùng đến server để đăng ký
                writer.WriteLine(idStaff);

                // Bắt đầu nhận tin nhắn từ server
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessages));
                receiveThread.Start();

                LoadTable();
                LoadCategory();
                btnThanhToan.Enabled = false;
            }
            catch (SocketException)
            {
                MessageBox.Show("Không thể kết nối đến server. Vui lòng kiểm tra lại kết nối và thử lại.", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void ReceiveMessages()
        {

        }

        void LoadTable()
        {
            
        }

        void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
            btnThanhToan.Enabled = true;
        }

        void ShowBill(int id)
        {
           
        }

        void LoadCategory()
        {
           
        }

        void LoadFoodListByCategoryID(int id)
        {
            
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void NhanVien_FormClosing(object sender, FormClosingEventArgs e)
        {
            DangNhap lg = new DangNhap();
            lg.Show();
        }
        // chưa sửa code này

        private void btnThemMon_Click(object sender, EventArgs e)
        {
            
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            
        }

        private void thongTinCaNhan_Click(object sender, EventArgs e)
        {
            ThongTinCaNhan profile = new ThongTinCaNhan(idStaff, writer);
            profile.ShowDialog();
        }

        private void dangXuat_Click(object sender, EventArgs e)
        {
            this.Close();
            DangNhap lg = new DangNhap();
            lg.Show();
            return;
        }

        private void NhanVien_Load(object sender, EventArgs e)
        {

        }
    }
}
