using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnNhom3.DTO
{
    public class Category
    {
        public Category(int idCategory, string name)
        {
            this.ID = idCategory;
            this.Name = name;
        }

        public Category(DataRow row)
        {
            this.ID = (int)row["idCategory"];
            this.Name = row["name"].ToString();
        }

        private string name;
        public string Name { get => name; set => name = value; }

        private int idCategory;

        public int ID { get => idCategory; set => idCategory = value; }
    }
}
