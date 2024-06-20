using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnNhom3.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }

        private BillDAO() { }
        /// <summary>
        /// Thanh cong: bill ID
        /// that bai: -1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetUncheckedBill_ByTableID(int id)
        {
            //Lấy idBill chưa thanh toán theo số bàn 
            DataTable data = DataClient.Instance.ExecuteQuery("SELECT * FROM dbo.Bill WHERE idTable = " + id + " AND status = 0");

            if (data.Rows.Count > 0)
            {
                DTO.Bill bill = new DTO.Bill(data.Rows[0]);
                return bill.IDBill;
            }

            //Nếu bill mới thì sẽ trả về -1
            return -1;
        }

        public int CheckExistFoodIDInBillInfo(int idBill, int idFood)
        {
            //Kiểm tra xem món ăn đã có trong bill chưa
            DataTable data = DataClient.Instance.ExecuteQuery("SELECT top(1)count FROM dbo.BillInfo WHERE idBill = " + idBill + " AND idFood = " + idFood);

            //check if food exist in bill
            if (data.Rows.Count > 0)
            {
                return (int)data.Rows[0]["count"];
            }
            return -1;
        }

        public void CheckOut(int idBill, int discount, decimal total)
        {
            //Chuyển trạng thái của Bill("Chưa thanh toán" thành "Đã thanh toán") và Bàn("Đang có người" thành "Bàn trống") khi bấm thanh toán, tổng giá trị bill, thời điểm thanh toán, giảm giá
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_CheckOut @idBill , @distcount , @total", new object[] { idBill, discount, total });
        }
        public void InsertBill(int idTable, string idStaff)
        {
            //Khi tạo một Bill mới thì sẽ thêm thông tin idStaff, ngày tạo, số bàn, trạng thái("Chưa thanh toán") của Bill và set trạng thái Bàn hiện tại thành "Đã có người"
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBill @idTable , @idStaff", new object[] { idTable, idStaff });
        }

        public DataTable GetBillListByDates(DateTime checkIn, DateTime checkOut)
        {
            //Lấy thông tin về mã bill, mã nhân viên, tên bàn, tổng tiền, thời gian tạo và thanh toán, giảm giá trong khoảng thời gian (checkIn, checkOut) đã thanh toán
            return DataProvider.Instance.ExecuteQuery("EXEC USP_GetListBillByDates @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }

        public int GetMax_IDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("SELECT Max(idBill) FROM dbo.Bill");
            }
            catch
            {
                return 1;
            }
        }
    }
}
