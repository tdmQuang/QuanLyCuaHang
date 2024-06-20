using System;
using System.Collections.Generic;
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
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace DoAnNhom3.Client
{
    public partial class NhanVien : Form
    {
        // Khai báo
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private NetworkStream stream;
        private string idStaff;
        private string fullName;
        public NhanVien(string idStaff, string fullName)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            this.idStaff = idStaff;
            this.fullName = fullName + ": ";


            try
            {
                // Kết nối đến server localhost sẽ thay đổi thành ip của server khi kết nối LAN
                client = new TcpClient("localhost", 8888);
                stream = client.GetStream();
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
            while (true)
            {
                try
                {
                    byte[] data = new byte[1024 * 5000];
                    int bytesRec = stream.Read(data, 0, data.Length);

                    if (bytesRec > 0)
                    {
                        string message = (string)Deserialize(data);
                        if (message.Equals("Ok!Load"))
                        {
                            Invoke(new Action(() =>
                            {
                                LoadTable();
                            }));
                        }
                        else if (message.StartsWith("CHAT|"))
                        {
                            string chatMessage = message.Substring(5); // Bỏ "CHAT|" để lấy nội dung tin nhắn
                            Invoke(new Action(() =>
                            {
                                AddOtherChatMessage(chatMessage);
                            }));
                        }
                        else
                        {
                            int idTable = Convert.ToInt32(message);
                            Invoke(new Action(() =>
                            {
                                LoadTable();
                                ShowBill(idTable);
                            }));
                        }
                    }
                }
                catch
                {
                }
            }
        }

        void LoadTable()
        {
            Console.WriteLine("check LoadTable() 0000");
            flowPanel_Table.Controls.Clear();
            //        flowPanel_Table.
            List<Table> tableList = new List<Table>();
            DataTable data = null;
            try
            {
                data = DataClient.Instance.ExecuteQuery("SELECT * FROM dbo.TableFood");

            }
            catch (Exception ex)
            {
                Console.WriteLine("get data table " + ex.Message);
            }
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
            Console.WriteLine("check LoadTable() 1111");
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
            Console.WriteLine("check ShowBill(table.ID) 0000");
            lvBill.Items.Clear();
            List<DTO.Menu> listBillInfo = new List<DTO.Menu>();

            string query = "SELECT f.name, bi.count, f.price, f.price*bi.count AS totalPrice FROM dbo.BillInfo AS bi, dbo.Bill AS b, dbo.Food AS f WHERE bi.idBill = b.idBill AND bi.idFood = f.idFood AND b.status = 0 AND b.idTable = " + id;
            DataTable data = null;
            try
            {
                data = DataClient.Instance.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine("get data bill " + ex.Message);
            }


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
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);
        }

        private void NhanVien_FormClosing(object sender, FormClosingEventArgs e)
        {
            DangNhap lg = new DangNhap();
            lg.Show();
        }
        // chưa sửa code này

        private void btnThemMon_Click(object sender, EventArgs e)
        {
            Table table = lvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Vui lòng chọn bàn");
                return;
            }
            int idBill = BillDAO.Instance.GetUncheckedBill_ByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmThemMon.Value;
            if (idBill == -1)   //Bill mới 
            {
                if (writer != null && client.Connected)
                {
                    writer.WriteLine($"ORDER1|{table.ID}|{foodID}|{count}");
                }
            }
            else
            {
                if (writer != null && client.Connected)
                {
                    writer.WriteLine($"ORDER2|{table.ID}|{idBill}|{foodID}|{count}");

                }
            }
            Invoke(new Action(() =>
            {
                LoadTable();
                ShowBill(table.ID);
            }));
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            Table table = lvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUncheckedBill_ByTableID(table.ID);
            int discount = (int)nmGiamGia.Value;
            string price = textBox_totalPrice.Text.Split(',')[0].Replace(".", "");
            int priceI = int.Parse(price);
            double totalPrice = Convert.ToDouble(priceI);
            double finalTotalPrice = (totalPrice - (totalPrice / 100) * discount) * 1000;
            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Xác nhận thanh toán hoá đơn cho {0}\nTổng tiền sau khi giảm: {1}đ", table.Name, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    // Sau khi xác nhận thanh toán thì sẽ gửi request đến server để thanh toán bill
                    writer.WriteLine($"PAY|{table.ID}|{idBill}|{discount}|{finalTotalPrice}");
                    /*ListView.ListViewItemCollection items = lvBill.Items;
                    string totalPriceinhoadon = textBox_totalPrice.Text;*/

                    // Hiển thị Form hóa đơn và chuyển dữ liệu sang

                    // Khởi tạo form hoadon và truyền giá trị discountValue
                    hoadon formHoaDon = new hoadon(lvBill, (int)nmGiamGia.Value);

                    formHoaDon.Owner = this; // Đặt form NhanVien làm Owner

                    // Hiển thị form hoadon và chờ cho đến khi form hoadon đóng
                    formHoaDon.ShowDialog();

                    // Xóa toàn bộ item trong lvBill
                    lvBill.Items.Clear();

                }
            }

            Invoke(new Action(() =>
            {
                ShowBill(table.ID);
                LoadTable();
            }));
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


        // Hàm để thêm tin nhắn chat vào RichTextBox
        private void AddMyChatMessage(string message)
        {
            // Đặt phông chữ và kích thước
            rtbChatHistory.SelectionFont = new Font("Arial", 20, FontStyle.Regular);
            // Căn chỉnh văn bản về bên phải
            rtbChatHistory.SelectionAlignment = HorizontalAlignment.Right;

            // Thêm khoảng trống để tạo padding
            rtbChatHistory.SelectionBackColor = Color.LightBlue;
            rtbChatHistory.AppendText("  ");
            rtbChatHistory.SelectionBackColor = Color.Transparent;

            // Thêm tin nhắn với màu nền xanh dương
            rtbChatHistory.SelectionBackColor = Color.LightBlue;
            rtbChatHistory.AppendText($"{message} ");
            rtbChatHistory.SelectionBackColor = Color.Transparent;

            // Thêm khoảng trống để tạo padding
            rtbChatHistory.SelectionBackColor = Color.LightBlue;
            rtbChatHistory.AppendText(" ");
            rtbChatHistory.SelectionBackColor = Color.Transparent;

            // Thêm thời gian gửi tin nhắn
            rtbChatHistory.SelectionBackColor = Color.Transparent;
            rtbChatHistory.AppendText($"{DateTime.Now.ToString("HH:mm")}");

            // Xuống dòng
            rtbChatHistory.AppendText(Environment.NewLine);

            // Cuộn đến vị trí hiện tại của caret để đảm bảo tin nhắn mới luôn hiển thị
            rtbChatHistory.ScrollToCaret();
        }

        private void AddOtherChatMessage(string message)
        {
            // Đặt phông chữ và kích thước
            rtbChatHistory.SelectionFont = new Font("Arial", 20, FontStyle.Regular);
            // Căn chỉnh văn bản về bên trái
            rtbChatHistory.SelectionAlignment = HorizontalAlignment.Left;

            // Thêm khoảng trống để tạo padding
            rtbChatHistory.SelectionBackColor = Color.LightGreen;
            rtbChatHistory.AppendText("  ");
            rtbChatHistory.SelectionBackColor = Color.Transparent;

            // Thêm tin nhắn với màu nền xanh lá cây
            rtbChatHistory.SelectionBackColor = Color.LightGreen;
            rtbChatHistory.AppendText($"{message} ");
            rtbChatHistory.SelectionBackColor = Color.Transparent;

            // Thêm khoảng trống để tạo padding
            rtbChatHistory.SelectionBackColor = Color.LightGreen;
            rtbChatHistory.AppendText(" ");
            rtbChatHistory.SelectionBackColor = Color.Transparent;

            // Thêm thời gian gửi tin nhắn
            rtbChatHistory.SelectionBackColor = Color.Transparent;
            rtbChatHistory.AppendText($"{DateTime.Now.ToString("HH:mm")}");

            // Xuống dòng
            rtbChatHistory.AppendText(Environment.NewLine);

            // Cuộn đến vị trí hiện tại của caret để đảm bảo tin nhắn mới luôn hiển thị
            rtbChatHistory.ScrollToCaret();
        }

        // Hàm để gửi tin nhắn chat
        private void btnSendChat_Click(object sender, EventArgs e)
        {
            if (client != null && client.Connected && !string.IsNullOrEmpty(txtChatMessage.Text))
            {
                string message = $"CHAT|{fullName}{txtChatMessage.Text}";
                //byte[] data = Serialize(message);
                //stream.Write(data, 0, data.Length);
                writer.WriteLine(message);
                AddMyChatMessage(txtChatMessage.Text);
                txtChatMessage.Clear();
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





        private static readonly HttpClient HttpClient = new HttpClient();

        private async void btnTranscribeAudio_Click(object sender, EventArgs e)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Lấy API key từ biến môi trường
                    var apiKey = Environment.GetEnvironmentVariable("ASSEMBLYAI_API_KEY");
                    if (string.IsNullOrEmpty(apiKey))
                    {
                        MessageBox.Show("API key không được thiết lập. Vui lòng kiểm tra biến môi trường ASSEMBLYAI_API_KEY.");
                        return;
                    }
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                    // Đường dẫn đến file âm thanh cần chuyển đổi
                    var path = "D:\\QuanLyCuaHang(BanHoanChinh)\\HelloAndSeeYouAgainBoss.m4a"; // Thay đổi đường dẫn này thành file của bạn
                    var uploadedFileUrl = await UploadFileAsync(path, httpClient);
                    var transcript = await CreateTranscriptAsync(uploadedFileUrl, httpClient);
                    transcript = await WaitForTranscriptToProcess(transcript, httpClient);

                    // Hiển thị kết quả trong txtChatMessage
                    txtChatMessage.Text = transcript.Text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private static async Task<string> UploadFileAsync(string filePath, HttpClient httpClient)
        {
            using (var fileStream = File.OpenRead(filePath))
            using (var fileContent = new StreamContent(fileStream))
            {
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                using (var response = await httpClient.PostAsync("https://api.assemblyai.com/v2/upload", fileContent))
                {
                    response.EnsureSuccessStatusCode();
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var jsonDoc = JsonDocument.Parse(jsonString);
                    return jsonDoc.RootElement.GetProperty("upload_url").GetString();
                }
            }
        }

        private static async Task<Transcript> CreateTranscriptAsync(string audioUrl, HttpClient httpClient)
        {
            var data = new { audio_url = audioUrl };
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync("https://api.assemblyai.com/v2/transcript", content))
            {
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Transcript>(jsonString);
            }
        }

        private static async Task<Transcript> WaitForTranscriptToProcess(Transcript transcript, HttpClient httpClient)
        {
            var pollingEndpoint = $"https://api.assemblyai.com/v2/transcript/{transcript.Id}";

            while (true)
            {
                var pollingResponse = await httpClient.GetAsync(pollingEndpoint);
                var pollingJson = await pollingResponse.Content.ReadAsStringAsync();
                transcript = JsonSerializer.Deserialize<Transcript>(pollingJson);

                switch (transcript.Status)
                {
                    case "processing":
                    case "queued":
                        await Task.Delay(TimeSpan.FromSeconds(3));
                        break;
                    case "completed":
                        return transcript;
                    case "error":
                        throw new Exception($"Transcription failed: {transcript.Error}");
                    default:
                        throw new Exception("This code shouldn't be reachable.");
                }
            }
        }


        public class Transcript
        {
            public string Id { get; set; }
            public string Status { get; set; }
            public string Text { get; set; }

            [JsonPropertyName("language_code")]
            public string LanguageCode { get; set; }

            public string Error { get; set; }
        }


    }
}
