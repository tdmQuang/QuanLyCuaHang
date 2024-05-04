using DoAnNhom3.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnNhom3
{
    public partial class ChuNhaHang : Form
    {
        public ChuNhaHang()
        {
            InitializeComponent();

            LoadAccountList();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        void LoadAccountList()
        {
            

            string query = "exec dbo.USP_GetAccountByUsername @userName = N'abc'";
            dgv_taikhoan.DataSource = DataProvider.Instance.ExecuteQuery(query, new object[] {"staff"}); 

        }
    }
}
