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
using System.Runtime.Serialization.Formatters.Binary;
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
                catch
                {


                }
            }
        }

        private void ReceiveMessages(TcpClient client)
        {
            try
            {
                // Nhận tên người dùng đăng ký từ client
                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.AutoFlush = true;

                string idStaff = reader.ReadLine();

                while (true)
                {
                    string message = reader.ReadLine();

                    if (message.StartsWith("CHAT|"))
                    {
                        string chatMessage = message.Substring(5);
                        Invoke(new Action(() =>
                        {
                            AddOtherChatMessage(chatMessage);
                        }));
                        BroadcastMessage(message, client); // Gửi tin nhắn tới các client khác, ngoại trừ client gửi tin nhắn
                    }
                    else if (message.StartsWith("ORDER1|"))
                    {
                        // Xử lý ORDER1
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
                        // Xử lý ORDER2
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
                        // Xử lý UPDATE_Account
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

        private void BroadcastMessage(string message, TcpClient senderClient)
        {
            foreach (TcpClient otherClient in clients)
            {
                if (otherClient != senderClient)
                {
                    NetworkStream stream = otherClient.GetStream();
                    byte[] data = Serialize(message);
                    stream.Write(data, 0, data.Length);
                }
            }
        }

        // Phân mảnh
        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, obj);

            return stream.ToArray();
        }

        // Gom mảnh
        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter.Deserialize(stream);
        }

        private void AddMyChatMessage(string message)
        {
            // Đặt phông chữ và kích thước
            chatView.SelectionFont = new Font("Arial", 20, FontStyle.Regular);
            // Căn chỉnh văn bản về bên phải
            chatView.SelectionAlignment = HorizontalAlignment.Right;

            // Thêm khoảng trống để tạo padding
            chatView.SelectionBackColor = Color.LightBlue;
            chatView.AppendText("  ");
            chatView.SelectionBackColor = Color.Transparent;

            // Thêm tin nhắn với màu nền xanh dương
            chatView.SelectionBackColor = Color.LightBlue;
            chatView.AppendText($"{message} ");
            chatView.SelectionBackColor = Color.Transparent;

            // Thêm khoảng trống để tạo padding
            chatView.SelectionBackColor = Color.LightBlue;
            chatView.AppendText(" ");
            chatView.SelectionBackColor = Color.Transparent;

            // Thêm thời gian gửi tin nhắn
            chatView.SelectionBackColor = Color.Transparent;
            chatView.AppendText($"{DateTime.Now.ToString("HH:mm")}");

            // Xuống dòng
            chatView.AppendText(Environment.NewLine);

            // Cuộn đến vị trí hiện tại của caret để đảm bảo tin nhắn mới luôn hiển thị
            chatView.ScrollToCaret();
        }

        private void AddOtherChatMessage(string message)
        {
            // Đặt phông chữ và kích thước
            chatView.SelectionFont = new Font("Arial", 20, FontStyle.Regular);
            // Căn chỉnh văn bản về bên trái
            chatView.SelectionAlignment = HorizontalAlignment.Left;

            // Thêm khoảng trống để tạo padding
            chatView.SelectionBackColor = Color.LightGreen;
            chatView.AppendText("  ");
            chatView.SelectionBackColor = Color.Transparent;

            // Thêm tin nhắn với màu nền xanh lá cây
            chatView.SelectionBackColor = Color.LightGreen;
            chatView.AppendText($"{message} ");
            chatView.SelectionBackColor = Color.Transparent;

            // Thêm khoảng trống để tạo padding
            chatView.SelectionBackColor = Color.LightGreen;
            chatView.AppendText(" ");
            chatView.SelectionBackColor = Color.Transparent;

            // Thêm thời gian gửi tin nhắn
            chatView.SelectionBackColor = Color.Transparent;
            chatView.AppendText($"{DateTime.Now.ToString("HH:mm")}");

            // Xuống dòng
            chatView.AppendText(Environment.NewLine);

            // Cuộn đến vị trí hiện tại của caret để đảm bảo tin nhắn mới luôn hiển thị
            chatView.ScrollToCaret();
        }

        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(inputMessageTextbox.Text))
            {
                string message = $"CHAT|Quản lý: {inputMessageTextbox.Text}";
                BroadcastMessage(message, null); // Gửi tin nhắn tới tất cả các client
                AddMyChatMessage($"{inputMessageTextbox.Text}");
                inputMessageTextbox.Clear();
            }
        }

        #endregion



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
            if (!checkFood()) return;
            DataProvider.Instance.ExecuteQuery("INSERT dbo.Food(name, idCategory, price) VALUES( @name , @idCategory , @price )", new object[] { textBoxNameFood.Text, cbbCategory.SelectedValue, textBoxFoodPrice.Text });
            dataGridViewFood.DataSource = DataProvider.Instance.ExecuteQuery("SELECT idFood AS [Mã món], name AS [Tên món], idCategory AS [Mã danh mục], price AS [Giá tiền] FROM Food");
            ClearFoodInfo();
            buttonViewFood.Enabled = true;
            btnClear.Enabled = true;
        }

        private void buttonDelFood_Click(object sender, EventArgs e)
        {
            if (textBoxIdFood.Text == "")
            {
                MessageBox.Show("Vui lòng chọn món ăn cần xóa!");
                return;
            }
            DataProvider.Instance.ExecuteQuery("DELETE FROM Food WHERE idFood = @idFood", new object[] { textBoxIdFood.Text });
            dataGridViewFood.DataSource = DataProvider.Instance.ExecuteQuery("SELECT idFood AS [Mã món], name AS [Tên món], idCategory AS [Mã danh mục], price AS [Giá tiền] FROM Food");
            ClearFoodInfo();
            buttonViewFood.Enabled = true;
            btnClear.Enabled = true;
        }

        private void buttonModFood_Click(object sender, EventArgs e)
        {
            if (!checkFood()) return;
            DataProvider.Instance.ExecuteQuery("UPDATE Food SET name = @name , idCategory = @idCategory , price = @price WHERE idFood = @idFood", new object[] { textBoxNameFood.Text, cbbCategory.SelectedValue, textBoxFoodPrice.Text, textBoxIdFood.Text });
            dataGridViewFood.DataSource = DataProvider.Instance.ExecuteQuery("SELECT idFood AS [Mã món], name AS [Tên món], idCategory AS [Mã danh mục], price AS [Giá tiền] FROM Food");
            ClearFoodInfo();
            buttonViewFood.Enabled = true;
            btnClear.Enabled = true;
        }
        private void buttonViewFood_Click(object sender, EventArgs e)
        {
            dataGridViewFood.DataSource = DataProvider.Instance.ExecuteQuery("SELECT idFood AS [Mã món], Food.name AS [Tên món], Food.idCategory AS [Mã danh mục], price AS [Giá tiền], FoodCategory.name AS [Tên danh mục] \r\nFROM Food\r\njoin FoodCategory on Food.idCategory = FoodCategory.idCategory");
        }


        private void dataGridViewFood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0) return;
            // Kiểm tra xem hàng đang được chọn có giá trị hay không
            if (e.RowIndex >= 0 && dataGridViewFood.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                // Lấy hàng đang được chọn trong DataGridView
                DataGridViewRow row = dataGridViewFood.Rows[e.RowIndex];

                // Lấy các giá trị của ô trong hàng đó và gán vào các ô textbox tương ứng
                textBoxIdFood.Text = row.Cells["Mã món"].Value.ToString();
                textBoxNameFood.Text = row.Cells["Tên món"].Value.ToString();
                cbbCategory.SelectedValue = row.Cells["Mã danh mục"].Value.ToString();
                textBoxFoodPrice.Text = row.Cells["Giá tiền"].Value.ToString();

                buttonDelFood.Enabled = true;
                buttonModFood.Enabled = true;
                buttonAddFood.Enabled = false;
            }
            else
            {
                // Nếu hàng được chọn không có giá trị, đặt giá trị của các ô textbox tương ứng về trống
                ClearFoodInfo();
            }
        }


        void ClearFoodInfo()
        {
            textBoxIdFood.Clear();
            textBoxNameFood.Clear();
            cbbCategory.SelectedIndex = -1;
            textBoxFoodPrice.Clear();
        }
        void EnableInput(bool enable)
        {
            //textBoxIdFood.Enabled = enable;
            textBoxNameFood.Enabled = enable;
            cbbCategory.Enabled = enable;
            textBoxFoodPrice.Enabled = enable;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFoodInfo();
            EnableInput(true);
            buttonAddFood.Enabled = true;
            buttonModFood.Enabled = false;
            buttonDelFood.Enabled = false;

            //clear dataGridViewFood selected row
            dataGridViewFood.ClearSelection();
        }
        private bool checkFood()
        {
            if (textBoxNameFood.Text == "" || cbbCategory.SelectedIndex == -1 || textBoxFoodPrice.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin món ăn!");
                return false;
            }
            return true;
        }

        #endregion

        #region category
        private void buttonAddCategory_Click(object sender, EventArgs e)
        {
            if (textBoxCategoryName.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!");
                return;
            }
            DataProvider.Instance.ExecuteQuery("INSERT FoodCategory( name ) VALUES( @name )", new object[] { textBoxCategoryName.Text });
            dataGridViewCategory.DataSource = DataProvider.Instance.ExecuteQuery("SELECT idCategory AS [Mã danh mục], name AS [Tên danh mục] FROM FoodCategory");
            textBoxCategoryId.Clear();
            textBoxCategoryName.Clear();
            buttonViewCategory.Enabled = true;
            btb_ClearCategory.Enabled = true;
        }

        private void buttonDelCategory_Click(object sender, EventArgs e)
        {
            if (textBoxCategoryId.Text == "")
            {
                MessageBox.Show("Vui lòng chọn danh mục cần xóa!");
                return;
            }
            DataProvider.Instance.ExecuteQuery("DELETE FROM Food WHERE idCategory = @idCategory", new object[] { textBoxCategoryId.Text });
            DataProvider.Instance.ExecuteQuery("DELETE FROM FoodCategory WHERE idCategory = @idCategory", new object[] { textBoxCategoryId.Text });
            dataGridViewCategory.DataSource = DataProvider.Instance.ExecuteQuery("SELECT idCategory AS [Mã danh mục], name AS [Tên danh mục] FROM FoodCategory");
            textBoxCategoryId.Clear();
            textBoxCategoryName.Clear();
            buttonViewCategory.Enabled = true;
            btb_ClearCategory.Enabled = true;
        }

        private void buttonModCategory_Click(object sender, EventArgs e)
        {
            if (textBoxCategoryId.Text == "")
            {
                MessageBox.Show("Vui lòng chọn danh mục cần sửa!");
                return;
            }
            if (textBoxCategoryName.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!");
                return;
            }
            DataProvider.Instance.ExecuteQuery("UPDATE FoodCategory SET name = @name WHERE idCategory = @idCategory", new object[] { textBoxCategoryName.Text, textBoxCategoryId.Text });
            dataGridViewCategory.DataSource = DataProvider.Instance.ExecuteQuery("SELECT idCategory AS [Mã danh mục], name AS [Tên danh mục] FROM FoodCategory");
            textBoxCategoryId.Clear();
            textBoxCategoryName.Clear();
            buttonViewCategory.Enabled = true;
            btb_ClearCategory.Enabled = true;
        }

        private void buttonViewCategory_Click(object sender, EventArgs e)
        {
            dataGridViewCategory.DataSource = DataProvider.Instance.ExecuteQuery("SELECT idCategory AS [Mã danh mục], name AS [Tên danh mục] FROM FoodCategory");
        }

        private void dataGridViewCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem hàng đang được chọn có giá trị hay không
            if (e.RowIndex >= 0 && dataGridViewCategory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                // Lấy hàng đang được chọn trong DataGridView
                DataGridViewRow row = dataGridViewCategory.Rows[e.RowIndex];

                // Lấy các giá trị của ô trong hàng đó và gán vào các ô textbox tương ứng
                textBoxCategoryId.Text = row.Cells["Mã danh mục"].Value.ToString();
                textBoxCategoryName.Text = row.Cells["Tên danh mục"].Value.ToString();

                buttonDelCategory.Enabled = true;
                buttonModCategory.Enabled = true;
                buttonAddCategory.Enabled = false;
            }
            else
            {
                // Nếu hàng được chọn không có giá trị, đặt giá trị của các ô textbox tương ứng về trống
                textBoxCategoryId.Clear();
                textBoxCategoryName.Clear();
            }
        }
        private void btb_ClearCategory_Click(object sender, EventArgs e)
        {
            ClearCaterogy();
            EnableInputCaterogy(true);
            buttonAddCategory.Enabled = true;
            buttonModCategory.Enabled = false;
            buttonDelCategory.Enabled = false;

            //clear dataGridViewFood selected row
            dataGridViewFood.ClearSelection();
        }
        void ClearCaterogy()
        {
            textBoxCategoryId.Clear();
            textBoxCategoryName.Clear();
        }
        void EnableInputCaterogy(bool enable)
        {
            //textBoxCategoryId.Enabled = enable;
            textBoxCategoryName.Enabled = enable;
        }
        #endregion

        #region infoStaff
        private void buttonAddAccount_Click(object sender, EventArgs e)
        {
            string dateWork = textBoxDow.Value.Year + "/" + textBoxDow.Value.Month + "/" + textBoxDow.Value.Day;
            DAO.AccountDAO.Instance.InsertAccount(textBoxUsername.Text, textBoxIdStaff.Text, textBoxFullName.Text, textBoxSDT.Text, dateWork, textBoxPosition.Text);
            dataGridViewInfoStaff.DataSource = DataProvider.Instance.ExecuteQuery("SELECT userName AS [Tài khoản], A.idStaff AS [Mã nhân viên], S.fullName AS [Họ và tên], S.phoneNumber AS [SĐT], S.firstDoW AS [Ngày vào làm], S.position AS [Chức vụ]  FROM Account AS A , Staff AS S WHERE A.idStaff = S.idStaff");
            ClearInfoAccount();
        }

        private void buttonDelAccount_Click(object sender, EventArgs e)
        {
            DAO.AccountDAO.Instance.DeleteAccount(textBoxIdStaff.Text);
            dataGridViewInfoStaff.DataSource = DataProvider.Instance.ExecuteQuery("SELECT userName AS [Tài khoản], A.idStaff AS [Mã nhân viên], S.fullName AS [Họ và tên], S.phoneNumber AS [SĐT], S.firstDoW AS [Ngày vào làm], S.position AS [Chức vụ]  FROM Account AS A , Staff AS S WHERE A.idStaff = S.idStaff");
            ClearInfoAccount();
        }

        private void buttonViewAccount_Click(object sender, EventArgs e)
        {
            dataGridViewInfoStaff.DataSource = DataProvider.Instance.ExecuteQuery("SELECT userName AS [Tài khoản], A.idStaff AS [Mã nhân viên], S.fullName AS [Họ và tên], S.phoneNumber AS [SĐT], S.firstDoW AS [Ngày vào làm], S.position AS [Chức vụ]  FROM Account AS A , Staff AS S WHERE A.idStaff = S.idStaff");
        }

        private void buttonResetPassword_Click(object sender, EventArgs e)
        {
            string defaultPassword = Hashing("123");
            DataProvider.Instance.ExecuteQuery("UPDATE Account SET passWord = @passWord WHERE userName = @userName AND idStaff = @idStaff", new object[] { defaultPassword, textBoxUsername.Text, textBoxIdStaff.Text });
            MessageBox.Show("Đặt lại mật khẩu thành công!\nMật khẩu mặc định là 123.");
        }

        private void dataGridViewInfoStaff_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Lấy hàng đang được chọn trong DataGridView
            DataGridViewRow row = dataGridViewInfoStaff.Rows[e.RowIndex];

            // Lấy các giá trị của ô trong hàng đó và gán vào các ô textbox tương ứng
            textBoxUsername.Text = row.Cells["Tài khoản"].Value.ToString();
            textBoxIdStaff.Text = row.Cells["Mã nhân viên"].Value.ToString();
            textBoxFullName.Text = row.Cells["Họ và tên"].Value.ToString();
            textBoxSDT.Text = row.Cells["SĐT"].Value.ToString();
            textBoxDow.Text = row.Cells["Ngày vào làm"].Value.ToString();
            textBoxPosition.Text = row.Cells["Chức vụ"].Value.ToString();

            if (textBoxIdStaff.Text == "")
            {
                textBoxUsername.Enabled = true;
                textBoxIdStaff.Enabled = true;
                textBoxFullName.Enabled = true;
                textBoxSDT.Enabled = true;
                textBoxDow.Enabled = true;
                textBoxPosition.Enabled = true;
                buttonResetPassword.Enabled = false;
                buttonDelAccount.Enabled = false;
                buttonAddAccount.Enabled = true;
            }
            else
            {
                buttonResetPassword.Enabled = true;
                buttonDelAccount.Enabled = true;
                buttonAddAccount.Enabled = false;
                textBoxUsername.Enabled = false;
                textBoxIdStaff.Enabled = false;
                textBoxFullName.Enabled = false;
                textBoxSDT.Enabled = false;
                textBoxDow.Enabled = false;
                textBoxPosition.Enabled = false;
            }
        }

        void ClearInfoAccount()
        {
            textBoxUsername.Clear();
            textBoxIdStaff.Clear();
            textBoxFullName.Clear();
            textBoxSDT.Clear();
            textBoxDow.Refresh();
            textBoxPosition.Clear();
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

        private void tabAdmin_Selected(object sender, TabControlEventArgs e)
        {
            // Load danh sách món ăn
            cbbCategory.DataSource = DataProvider.Instance.ExecuteQuery("select * from FoodCategory");
            cbbCategory.DisplayMember = "name";
            cbbCategory.ValueMember = "idCategory";
            cbbCategory.SelectedIndex = -1;
        }

        private void btn_ClearAccount_Click(object sender, EventArgs e)
        {
            ClearInfoAccount();
            textBoxUsername.Enabled = true;
            textBoxIdStaff.Enabled = true;
            textBoxFullName.Enabled = true;
            textBoxSDT.Enabled = true;
            textBoxDow.Enabled = true;
            textBoxPosition.Enabled = true;
            buttonResetPassword.Enabled = false;
            buttonDelAccount.Enabled = false;
            buttonAddAccount.Enabled = true;

        }


        private void btnSaveHistory_Click(object sender, EventArgs e)
        {

        }

        private void ChuCuaHang_Load(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }
    }
}

