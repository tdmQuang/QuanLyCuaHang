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

            this.idStaff = idStaff;

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

        private void ReceiveMessages()
        {
            while (true)
            {
                try
                {
                    // Nhận tin nhắn từ server để cập nhật bàn và các món khi 1 client thêm món hoặc thanh toán
                    string message = reader.ReadLine();

                    if (message.Equals("Ok!Load"))
                    {
                        Invoke(new Action(() => {
                            LoadTable();
                        }));
                    }
                    else
                    {
                        int idTable = Convert.ToInt32(message);
                        Invoke(new Action(() => {
                            LoadTable();
                            ShowBill(idTable);
                        }));
                    }
                }
                catch
                {
                }
            }
        }

        void LoadTable()
        {
            flowPanel_Table.Controls.Clear();
            List<Table> tableList = new List<Table>();
            DataTable data = DataClient.Instance.ExecuteQuery("SELECT * FROM dbo.TableFood");
            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = 100, Height = 100 };
                btn.Text = item.Name + Environment.NewLine;
                btn.Click += btn_Click;
                btn.Tag = item;
                switch (item.Status)
                {
                    case 0:
                        btn.BackColor = Color.Green;
                        btn.Text += "Bàn trống";
                        break;
                    default:
                        btn.BackColor = Color.Red;
                        btn.Text += "Bàn đã có người";
                        break;
                }

                flowPanel_Table.Controls.Add(btn);
            }
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
            lvBill.Items.Clear();
            List<DTO.Menu> listBillInfo = new List<DTO.Menu>();

            string query = "SELECT f.name, bi.count, f.price, f.price*bi.count AS totalPrice FROM dbo.BillInfo AS bi, dbo.Bill AS b, dbo.Food AS f WHERE bi.idBill = b.idBill AND bi.idFood = f.idFood AND b.status = 0 AND b.idTable = " + id;
            DataTable data = DataClient.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                DTO.Menu menu = new DTO.Menu(item);
                listBillInfo.Add(menu);
            }

            float totalPrice = 0;

            foreach (DTO.Menu item in listBillInfo)
            {
                ListViewItem lvItem = new ListViewItem(item.FoodName.ToString());
                lvItem.SubItems.Add(item.Count.ToString());
                lvItem.SubItems.Add(item.Price.ToString());
                lvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lvBill.Items.Add(lvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN"); //don vi tien te cua VN

            textBox_totalPrice.Text = totalPrice.ToString("c", culture); //c = currency
        }

        void LoadCategory()
        {
            List<Category> listCategory = new List<Category>();

            string query = "SELECT * FROM FoodCategory";

            DataTable data = DataClient.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                listCategory.Add(category);
            }
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = new List<Food>();

            string query = "SELECT * FROM dbo.Food WHERE idCategory = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listFood.Add(food);
            }

            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
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
    }
}
