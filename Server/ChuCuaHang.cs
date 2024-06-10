using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using DoAnNhom3.DAO;
using DoAnNhom3.DTO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DoAnNhom3.Server
{
    public partial class ChuCuaHang : Form
    {
        public ChuCuaHang()
        {
            InitializeComponent();
            LoadDateTimePickerBill();
        }

        #region ClientServer
        private TcpListener listener;
        private List<TcpClient> clients = new List<TcpClient>();
        public void Start()
        {
            // Bắt đầu lắng nghe kết nối từ client
            listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();
            Thread acceptThread = new Thread(AcceptClients);
            acceptThread.Start();


        }
        private void AcceptClients()
        {
            while (true)
            {
                try
                {
                    // Chấp nhận kết nối từ client mới
                    TcpClient client = listener.AcceptTcpClient();
                    clients.Add(client);

                    // Bắt đầu thread để xử lý tin nhắn từ client
                    Thread receiveThread = new Thread(() => ReceiveMessages(client));
                    receiveThread.Start();
                }
                catch { }
            }
        }

        private void ReceiveMessages(TcpClient client)
        {
            try
            {
                // Nhận tên người dùng đăng ký từ client
                StreamReader reader = new StreamReader(client.GetStream());
                string idStaff = reader.ReadLine();

                while (true)
                {
                    // Nhận tin nhắn từ client
                    string message = reader.ReadLine();

                    if (message.StartsWith("ORDER1|"))
                    {
                        // Nếu là đơn hàng mới, thêm hóa đơn vào cơ sở dữ liệu
                        string[] parts = message.Split('|');
                        int idTable = Convert.ToInt32(parts[1]);
                        int idFood = Convert.ToInt32(parts[2]);
                        int count = Convert.ToInt32(parts[3]);

                        BillDAO.Instance.InsertBill(idTable, idStaff);
                        BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMax_IDBill(), idFood, count);
                        Invoke(new Action(() => {
                            Reponse(idTable, client);
                        }));

                    }
                    else if (message.StartsWith("ORDER2|"))
                    {
                        // Nếu là đơn hàng đã có sẵn, thêm thông tin món mới
                        string[] parts = message.Split('|');
                        int idTable = Convert.ToInt32(parts[1]);
                        int idBill = Convert.ToInt32(parts[2]);
                        int idFood = Convert.ToInt32(parts[3]);
                        int count = Convert.ToInt32(parts[4]);

                        BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count);
                        Invoke(new Action(() => {
                            Reponse(idTable, client);
                        }));
                    }
                    else if (message.StartsWith("PAY|"))
                    {
                        // Thanh toán
                        string[] parts = message.Split('|');
                        int idTable = Convert.ToInt32(parts[1]);
                        int idBill = Convert.ToInt32(parts[2]);
                        int discount = Convert.ToInt32(parts[3]);
                        decimal total = Convert.ToDecimal(parts[4]);

                        BillDAO.Instance.CheckOut(idBill, discount, total);
                        Invoke(new Action(() => {
                            Reponse(idTable);
                        }));
                    }
                    else if (message.StartsWith("UPDATE_Account|"))
                    {
                        // Cập nhật tài khoản
                        string[] parts = message.Split('|');
                        string IdStaff = Convert.ToString(parts[1]);
                        string fullName = Convert.ToString(parts[2]);
                        string phoneNumber = Convert.ToString(parts[3]);
                        string userName = Convert.ToString(parts[4]);
                        string passWord = Convert.ToString(parts[5]);
                        string newPassWord = Convert.ToString(parts[6]);

                        AccountDAO.Instance.UpdateAccount(IdStaff, fullName, phoneNumber, userName, passWord, newPassWord);
                    }
                }
            }
            catch { }
        }
        private void Reponse(int idTable)
        {
            string responseMessage = idTable.ToString();
            foreach (TcpClient otherClient in clients)
            {
                StreamWriter writer = new StreamWriter(otherClient.GetStream());
                writer.WriteLine(responseMessage);
                writer.Flush();
            }
        }

        private void Reponse(int idTable, TcpClient client)
        {
            string reponseMessageForAll = "Ok!Load";
            string responseMessage = idTable.ToString();
            foreach (TcpClient otherClient in clients)
            {
                StreamWriter writer = new StreamWriter(otherClient.GetStream());
                writer.WriteLine(reponseMessageForAll);
                writer.Flush();
            }
            // Gửi tin nhắn đến một client cụ thể
            if (client.Connected)
            {
                try
                {
                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(responseMessage);
                    writer.Flush();
                }
                catch
                {
                }
            }
        }
        #endregion

        private void QuanLy_FormClosing(object sender, FormClosingEventArgs e)
        {
            DangNhap lg = new DangNhap();
            lg.Show();
        }

        #region revenue
        void LoadDateTimePickerBill() //thong ke se show checkIn = ngay dau thang, checkOut = ngay cuoi thang
        {
            DateTime today = DateTime.Now;
            dateTime_FromDate.Value = new DateTime(today.Year, today.Month, 1); //dua ve dau thang
            dateTime_ToDate.Value = dateTime_FromDate.Value.AddMonths(1).AddDays(-1); //dau thang sau -1 = ngay cuoi cua thang truoc
        }

        void LoadListBillByDates(DateTime checkIn, DateTime checkOut)
        {
            //Load danh sách các hóa đơn "từ ngày .. đến ngày .."
            dataGridView_Bill.DataSource = DAO.BillDAO.Instance.GetBillListByDates(checkIn, checkOut);
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            LoadListBillByDates(dateTime_FromDate.Value, dateTime_ToDate.Value);
        }

        #endregion

        #region food
        private void buttonAddFood_Click(object sender, EventArgs e)
        {
           
        }

        private void buttonDelFood_Click(object sender, EventArgs e)
        {
          
        }

        private void buttonModFood_Click(object sender, EventArgs e)
        {
        }
        private void buttonViewFood_Click(object sender, EventArgs e)
        {
           
        }


        private void dataGridViewFood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }


        void ClearFoodInfo()
        {
           
        }

        #endregion

        #region category
        private void buttonAddCategory_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonDelCategory_Click(object sender, EventArgs e)
        {
           
        }

        private void buttonModCategory_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonViewCategory_Click(object sender, EventArgs e)
        {
           
        }

        private void dataGridViewCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }


        #endregion

        #region infoStaff
        private void buttonAddAccount_Click(object sender, EventArgs e)
        {
         
        }

        private void buttonDelAccount_Click(object sender, EventArgs e)
        {
        }

        private void buttonViewAccount_Click(object sender, EventArgs e)
        {
          
        }

        private void buttonResetPassword_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridViewInfoStaff_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        void ClearInfoAccount()
        {
            
        }

        private string Hashing(string password)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);

            SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hash;
        }

        #endregion

     
    }
}

