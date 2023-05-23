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
    public class ChatLieuController : ApiController
    {
        private DbQuanLyBanQuanAoDataContext DbQuanAo = new DbQuanLyBanQuanAoDataContext(
            "Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
            );
        //httpGet: dùng để lấy thông tin san pham
        //1. Dịch vụ lấy thông tin của toàn bộ san pham
        [HttpGet]
        public List<tblChatLieu> GetAllChatLieu()
        {
            return DbQuanAo.tblChatLieus.ToList();
        }
        //2. Dịch vụ lấy thông tin một san pham với mã nào đó
        [HttpGet]
        public tblChatLieu GetChatLieu(string id)
        {
            return DbQuanAo.tblChatLieus.FirstOrDefault(x => x.MaChatLieu == id);
        }
        //3. httpPost, dịch vụ thêm mới một san pham
        [HttpPost]
        public bool AddNewChatLieu(tblChatLieu chatLieu)
        {
            try
            {
                DbQuanAo.tblChatLieus.InsertOnSubmit(chatLieu);
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //4. httpPut để chỉnh sửa thông tin một sanPham
        [HttpPut]
        public bool UpdateNewchatLieu(tblChatLieu postData)
        {
            try
            {
                tblChatLieu chatLieu = DbQuanAo.tblChatLieus.FirstOrDefault(x => x.MaChatLieu == postData.MaChatLieu);
                if (chatLieu == null) return false;
                // gan gia tri tu post data vao sanpham tim thay trong sanpham
                chatLieu.MaChatLieu = postData.MaChatLieu;
                chatLieu.TenChatLieu = postData.TenChatLieu;
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //5.httpDelete để xóa một san pham
        [HttpDelete]
        public bool DeleteChatLieu(string id)
        {
            try
            {
                tblChatLieu chatLieu = DbQuanAo.tblChatLieus.FirstOrDefault(x => x.MaChatLieu == id);
                if (chatLieu == null) { return false; }
                tblSanPham sanPham = DbQuanAo.tblSanPhams.FirstOrDefault(x => x.MaChatLieu == id);
                if (sanPham != null) { return false; }
                DbQuanAo.tblChatLieus.DeleteOnSubmit(chatLieu);
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
