using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnNhom3.DTO
{
    public class BillInfo
    {
        public BillInfo(int idBill, int idFood, int count)
        {
            this.IDBill = idBill;
            this.IDFood = idFood;
            this.Count = count;
        }
        private int count; //food count

        public BillInfo(DataRow row)
        {
            this.IDBill = (int)row["idBill"];
            this.IDFood = (int)row["idFood"];
            this.Count = (int)row["count"];
        }
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        private int idFood;

        public int IDFood
        {
            get { return idFood; }
            set { idFood = value; }
        }

        private int idBill;
        public int IDBill
        {
            get { return idBill; }
            set { idBill = value; }
        }
    }
}
