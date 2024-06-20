using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnNhom3.DTO
{
    [Serializable]
    public class Table
    {
        public Table(int idTable, string nameTable, int status)
        {
            this.ID = idTable;
            this.Name = nameTable;
            this.Status = status;
        }

        public Table(DataRow row)
        {
            this.ID = (int)row["idTable"];
            this.Name = row["nameTable"].ToString();
            this.Status = (int)row["status"];
        }

        private int idTable;

        private string nameTable;

        private int status;
        public int ID { get => idTable; set => idTable = value; }
        public string Name { get => nameTable; set => nameTable = value; }
        public int Status { get => status; set => status = value; }
    }
}
