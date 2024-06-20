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
        public void UpdateAccount(string idStaff, string fullName, string phoneNumber, string userName, string passWord, string newPassWord)
        {
            //Update họ tên, sđt hoặc mật khẩu theo mã nhân viên và tên tài khoản
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_UpdateAccount @idStaff , @fullName , @phoneNumber , @userName , @passWord , @newPassWord", new object[] { idStaff, fullName, phoneNumber, userName, passWord, newPassWord });
        }

        public void DeleteAccount(string idStaff)
        {
            //Xóa tất cả thông tin liên quan đến tài khoản (các thông tin về bill, account, thông tin cá nhân)
            DataProvider.Instance.ExecuteQuery("DELETE FROM BillInfo WHERE idBill IN (SELECT idBill FROM Bill WHERE idStaff = @idStaff )", new object[] { idStaff });
            DataProvider.Instance.ExecuteQuery("DELETE FROM Bill WHERE idStaff = @idStaff", new object[] { idStaff });
            DataProvider.Instance.ExecuteQuery("DELETE FROM Account WHERE idStaff = @idStaff", new object[] { idStaff });
            DataProvider.Instance.ExecuteQuery("DELETE FROM Staff WHERE idStaff = @idStaff", new object[] { idStaff });
        }
        public void InsertAccount(string userName, string idStaff, string fullName, string phoneNumber, string firstDoW, string position)
        {
            //Khi thêm 1 tài khoản thì phải tạo thông tin cá nhân rồi thì sẽ tạo 1 tài khoản để đăng nhập
            DataProvider.Instance.ExecuteQuery("INSERT Staff( idStaff , fullName , phoneNumber , firstDoW , position ) VALUES( @idStaff , @fullName , @phoneNumber , @firstDoW , @position )", new object[] { idStaff, fullName, phoneNumber, firstDoW, position });
            DataProvider.Instance.ExecuteQuery("INSERT Account( userName , passWord , idStaff ) VALUES( @userName , @password , @idStaff )", new object[] { userName, "123", idStaff });
        }
    }
}
