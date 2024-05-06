using DoAnNhom3.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnNhom3.DAO
{
    internal class AccountDAO
    {
        private static AccountDAO instance;
        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }

        private AccountDAO() { }
        public bool Login(string username, string password)
        {
            string query = "USP_Login @userName , @passWord";

            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[]{username, password});

            return result.Rows.Count > 0;
        }

        public Account GetAccountByUserName(string username)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from account where userName = '" + username + "'");
            foreach (DataRow item in data.Rows) 
            {
                return new Account(item);
            }
            return null;
        }

        public bool UpdateAccount(string username, string displayname, string password, string newpass) 
        {
            int result = DataProvider.Instance.ExecuteNonQuery("exce USP_UpdateAccount @userName, @displayName, @passWord, @newPassword", 
                new object[] {username, displayname, password, newpass});
            return result > 0;

        }


    }
}
