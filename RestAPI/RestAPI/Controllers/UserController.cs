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
    public class UserController : ApiController
    {
        private DbQuanLyBanQuanAoDataContext DbQuanAo = new DbQuanLyBanQuanAoDataContext(
            "Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
            );
        //httpGet: dùng để lấy thông tin san pham
        //2. Dịch vụ lấy thông tin một san pham với mã nào đó
        [HttpGet]
        public tblUser GetUser(string id)
        {
            tblUser User = DbQuanAo.tblUsers.FirstOrDefault( x => x.MaNV==id);
            if(User != null)
            if (User.IsDeleted == 0)
                   return User;
                return null;
        }
        [HttpGet]
        public tblUser CheckLogin(string user, string pass)
        {
            tblUser username = DbQuanAo.tblUsers.FirstOrDefault(x => x.Username == user && x.Password == pass);
            if(username != null)
            {
                if (username.IsDeleted == 0)
                    return username;
            }
            
            return null;
        }
        [HttpPost]
        public bool AddNewUser(tblUser user)
        {
            try
            {
                user.IsDeleted = 0;
                DbQuanAo.tblUsers.InsertOnSubmit(user);
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //4. httpPut để chỉnh sửa thông tin một User
        [HttpPut]
        public bool UpdateNewUser(tblUser postData)
        {
            try
            {
                tblUser user = DbQuanAo.tblUsers.FirstOrDefault(x => x.MaNV == postData.MaNV);
                if (user == null) return false;
                // gan gia tri tu post data vao User tim thay trong User
                user.MaNV = postData.MaNV;
                user.Username = postData.Username;
                user.Password = postData.Password;
                user.Email = postData.Email;
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
                tblUser User = DbQuanAo.tblUsers.FirstOrDefault(x => x.MaNV == id);
                if (User == null) return false;
                // gan gia tri tu post data vao User tim thay trong User
                User.IsDeleted = 1;
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        [HttpDelete]
        public bool DeleteUser(string id)
        {
            try
            {
                tblUser NV = DbQuanAo.tblUsers.FirstOrDefault(x => x.MaNV == id);
                if (NV == null) { return false; }
                DbQuanAo.tblUsers.DeleteOnSubmit(NV);
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
