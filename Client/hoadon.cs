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
           
        }



        private void hoadon_Load(object sender, EventArgs e)
        {

        }


    }
}
