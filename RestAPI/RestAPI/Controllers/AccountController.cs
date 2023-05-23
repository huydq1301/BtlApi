using RestAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestAPI.Controllers
{
    public class AccountController : ApiController
    {
        private DbQuanLyBanQuanAoDataContext DbQuanAo = new DbQuanLyBanQuanAoDataContext(
            "Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
            );
        //httpGet: dùng để lấy thông tin san pham
        //2. Dịch vụ lấy thông tin một san pham với mã nào đó
        [HttpGet]
        public tblAccount GetAccount(string id)
        {
            tblAccount Account = DbQuanAo.tblAccounts.FirstOrDefault( x => x.MaKH==id);
            if(Account != null)
            if (Account.IsDeleted == 0)
                   return Account;
                return null;
        }
        [HttpGet]
        public tblAccount CheckLogin(string user, string pass)
        {
            tblAccount Accountname = DbQuanAo.tblAccounts.FirstOrDefault(x => x.Username == user && x.Password == pass);
            if( Accountname != null ) {
                if (Accountname.IsDeleted == 0)
                    return Accountname;
            }
            
            return null;
        }
        [HttpPost]
        public bool AddNewAccount(tblAccount Account)
        {
            try
            {
                Account.IsDeleted = 0;
                DbQuanAo.tblAccounts.InsertOnSubmit(Account);
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //4. httpPut để chỉnh sửa thông tin một Account
        [HttpPut]
        public bool UpdateNewAccount(tblAccount postData)
        {
            try
            {
                tblAccount Account = DbQuanAo.tblAccounts.FirstOrDefault(x => x.MaKH == postData.MaKH);
                if (Account == null) return false;
                // gan gia tri tu post data vao Account tim thay trong Account
                Account.MaKH = postData.MaKH;
                Account.Username = postData.Username;
                Account.Password = postData.Password;
                Account.Email = postData.Email;
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        [HttpPut]
        public bool UpDateIsDeleted(string id)
        {
            try
            {
                tblAccount Account = DbQuanAo.tblAccounts.FirstOrDefault(x => x.MaKH == id);
                if (Account == null) return false;
                // gan gia tri tu post data vao Account tim thay trong Account
                Account.IsDeleted = 1;
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        [HttpDelete]
        public bool DeleteAccount(string id)
        {
            try
            {
                tblAccount NV = DbQuanAo.tblAccounts.FirstOrDefault(x => x.MaKH == id);
                if (NV == null) { return false; }
                DbQuanAo.tblAccounts.DeleteOnSubmit(NV);
                // submit changed
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
