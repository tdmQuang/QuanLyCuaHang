using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnNhom3.DTO
{
    public class Food
    {
        public Food(int idFood, string name, int categoryID, float price)
        {
            this.ID = idFood;
            this.Name = name;
            this.CategoryID = categoryID;
            this.Price = price;
        }
        public Food(DataRow row)
        {
            this.ID = (int)row["idFood"];
            this.Name = row["name"].ToString();
            this.CategoryID = (int)row["idcategory"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
        }

        private float price;
        private int categoryID;

        public int CategoryID { get => categoryID; set => categoryID = value; }


        private string name;

        private int idFood;
        public string Name { get => name; set => name = value; }

        public int ID { get => idFood; set => idFood = value; }
        public float Price { get => price; set => price = value; }
    }
}
