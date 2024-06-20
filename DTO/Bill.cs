using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnNhom3.DTO
{
    public class Bill
    {
        public Bill(int idBill, string idStaff, DateTime? dateCheckIn, DateTime? dateCheckOut, int status, int discount = 0)
        {
            this.IDBill = idBill;
            this.IDStaff = idStaff;
            this.DateCheckIn = dateCheckIn;
            this.DateCheckOut = dateCheckOut;
            this.Status = status;
            this.Discount = discount;
        }

        public Bill(DataRow row)
        {
            this.IDBill = (int)row["idBill"];
            this.IDStaff = row["IDStaff"].ToString();
            this.DateCheckIn = (DateTime?)row["dateCheckIn"];

            var dateCheckOutTemp = row["dateCheckOut"];
            if (dateCheckOutTemp.ToString() != "")
            {
                this.DateCheckOut = (DateTime?)dateCheckOutTemp;
            }
            this.Status = (int)row["status"];

            if (row["discount"].ToString() != "")
                this.Discount = (int)row["discount"];
        }

        private string idStaff;
        public string IDStaff { get => idStaff; set => idStaff = value; }

        private int total;
        public int Total { get => total; set => total = value; }

        private int status;
        public int Status { get => status; set => status = value; }


        private int discount;
        public int Discount { get => discount; set => discount = value; }


        private DateTime? dateCheckOut;
        public DateTime? DateCheckOut { get => dateCheckOut; set => dateCheckOut = value; }

        private DateTime? dateCheckIn;
        public DateTime? DateCheckIn { get => dateCheckIn; set => dateCheckIn = value; }

        private int idBill;
        public int IDBill { get => idBill; set => idBill = value; }

    }
}
