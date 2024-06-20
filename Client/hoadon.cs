using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnNhom3.Client
{
    public partial class hoadon : Form
    {
        private ListView lvbill;
        private double totalAmount = 0;
        private int discountValue;



        public hoadon(ListView lvBill, int discount)
        {
            InitializeComponent();
            this.lvbill = lvBill;
            this.discountValue = discount;
            ShowBillInfo();

        }
        public void ShowBillInfo()
        {
            richTextBox1.Clear();
            string l = "Team cafe";
            Font originalFont = richTextBox1.SelectionFont;
            Font largerFont = new Font(originalFont.FontFamily, originalFont.Size + 3, FontStyle.Bold);
            richTextBox1.SelectionFont = largerFont;
            richTextBox1.AppendText(l + Environment.NewLine);
            richTextBox1.AppendText(Environment.NewLine);
            string h = "\t\t\t HÓA ĐƠN BÁN HÀNG";

            richTextBox1.SelectionFont = new Font("MS PMincho", 16, FontStyle.Regular);
            richTextBox1.AppendText(h + Environment.NewLine);
            richTextBox1.AppendText(Environment.NewLine);
            richTextBox1.AppendText(Environment.NewLine);

            richTextBox1.AppendText("------------------------------------------------------------------" + Environment.NewLine);
            richTextBox1.AppendText("Tên món\t Số lượng\t Đơn giá\t Thành Tiền" + Environment.NewLine);
            richTextBox1.AppendText("------------------------------------------------------------------" + Environment.NewLine);
            foreach (ListViewItem item in lvbill.Items)
            {
                string foodName = item.SubItems[0].Text;
                string quantity = item.SubItems[1].Text;
                string unitPrice = item.SubItems[2].Text;
                string totalPrice = item.SubItems[3].Text;

                string line = string.Format("{0,-20}{1,-10}{2,-10}{3,-10}", foodName, quantity, unitPrice, totalPrice);

                richTextBox1.AppendText(line + Environment.NewLine);

                totalAmount += Convert.ToDouble(totalPrice);
            }
            richTextBox1.AppendText("---------------------------------------------------" + Environment.NewLine);
            // double discountedTotal = totalAmount - (totalAmount * discountValue / 100);
            richTextBox1.AppendText(Environment.NewLine);
            richTextBox1.AppendText(Environment.NewLine);
            string lineTotal1 = string.Format("{0,-30}{1,-15}đ", "Tổng tiền:", totalAmount.ToString());
            double discountedTotal = totalAmount - (totalAmount * discountValue / 100);
            string lineTotal = string.Format("{0,-30}{1,-15}%", "Giảm giá:", discountValue.ToString());
            string lineTotal2 = string.Format("{0,-25}{1,-15}đ", "Tổng cộng:", discountedTotal.ToString());
            richTextBox1.AppendText(lineTotal1 + Environment.NewLine);
            richTextBox1.AppendText(lineTotal + Environment.NewLine);
            richTextBox1.AppendText(lineTotal2 + Environment.NewLine);
        }



        private void hoadon_Load(object sender, EventArgs e)
        {

        }


    }
}
