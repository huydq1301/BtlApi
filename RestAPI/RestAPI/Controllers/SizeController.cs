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
    public class SizeController : ApiController
    {
        private DbQuanLyBanQuanAoDataContext DbQuanAo = new DbQuanLyBanQuanAoDataContext(
            "Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
            );
        //httpGet: dùng để lấy thông tin san pham
        //1. Dịch vụ lấy thông tin của toàn bộ san pham
        [HttpGet]
        public List<tblSize> GetAllSize()
        {
            return DbQuanAo.tblSizes.ToList();
        }
        //2. Dịch vụ lấy thông tin một san pham với mã nào đó
        [HttpGet]
        public tblSize GetSize(string id)
        {
            return DbQuanAo.tblSizes.FirstOrDefault(x => x.MaSize == id);
        }
        //3. httpPost, dịch vụ thêm mới một san pham
        [HttpPost]
        public bool AddNewSize(tblSize size)
        {
            try
            {
                DbQuanAo.tblSizes.InsertOnSubmit(size);
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
        public bool UpdateNewsize(tblSize postData)
        {
            try
            {
                tblSize size = DbQuanAo.tblSizes.FirstOrDefault(x => x.MaSize == postData.MaSize);
                if (size == null) return false;
                // gan gia tri tu post data vao sanpham tim thay trong sanpham
                size.MaSize = postData.MaSize;
                size.TenSize = postData.TenSize;
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
        public bool DeleteSize(string id)
        {
            try
            {
                tblSize size = DbQuanAo.tblSizes.FirstOrDefault(x => x.MaSize == id);
                if (size == null) { return false; }
                tblSanPham sanPham = DbQuanAo.tblSanPhams.FirstOrDefault(x => x.MaSize == id);
                if (sanPham != null) { return false; }
                DbQuanAo.tblSizes.DeleteOnSubmit(size);
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
