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
        public Account(string userName, string displayName, int type, string password = null) 
        {
            this.Username = userName;
            this.DisplayName = displayName;
            this.Type = type;
            this.Password = password;
        }

        public Account (DataRow row)
        {
            this.Username = row["userName"].ToString();
            this.DisplayName = row["displayName"].ToString();
            this.Type = (int)row["type"];
            this.Password = row["password"].ToString();
        }

        private string userName;
        public string Username {  get { return userName; } set {  userName = value; } }

        private string displayName;
        public string DisplayName { get { return displayName; } set { displayName = value; } }

        private int type;
        public int Type { get { return type; } set { type = value; } }

        private string password;
        public string Password { get {  return password; } set {  password = value; } }


    }
}
