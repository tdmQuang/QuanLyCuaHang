using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnNhom3.DTO
{
    public class Account
    {
        public Account(string userName, string passWord, string idStaff)
        {
            this.UserName = userName;
            this.Password = passWord;
            this.IdStaff = idStaff;
        }
        public Account(DataRow row)
        {
            this.UserName = row["userName"].ToString();
            this.Password = row["passWord"].ToString();
            this.IdStaff = row["idStaff"].ToString();
        }

        private string userName;

        public string UserName { get => userName; set => userName = value; }

        private string password;
        public string Password { get => password; set => password = value; }

        private string idStaff;
        public string IdStaff { get => idStaff; set => idStaff = value; }
    }
}
