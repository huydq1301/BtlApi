using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;

namespace RestAPI.Controllers
{
    public class KhachHangController : ApiController
    {
        private DbQuanLyBanQuanAoDataContext DbQuanAo = new DbQuanLyBanQuanAoDataContext(
            "Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
            );
        //httpGet: dùng để lấy thông tin san pham
        //1. Dịch vụ lấy thông tin của toàn bộ san pham
        [HttpGet]
        public List<tblKhachHang> GetAllkh()
        {
            List<tblKhachHang> listSP = DbQuanAo.tblKhachHangs.ToList();
            List<tblKhachHang> listSPCurrent = new List<tblKhachHang>();
            foreach (var sp in listSP)
            {
                if (sp.IsDeleted == 0)
                    listSPCurrent.Add(sp);
            }
            return listSPCurrent;
        }
        [HttpGet]
        public List<tblKhachHang> getAll(string name)
        {
            if (name == null || name == "")
            {
                GetAllkh();
            }
            List<tblKhachHang> tkh = new List<tblKhachHang>();
            tkh = DbQuanAo.tblKhachHangs.Where(x => x.TenKH.Contains(name)).ToList();
            List<tblKhachHang> listSPCurrent = new List<tblKhachHang>();
            foreach (var sp in tkh)
            {
                if (sp.IsDeleted == 0)
                    listSPCurrent.Add(sp);
            }
            return listSPCurrent;
        }
        //2. Dịch vụ lấy thông tin một san pham với mã nào đó
        [HttpGet]
        public tblKhachHang GetKH(string id)
        {
            tblKhachHang sp = DbQuanAo.tblKhachHangs.FirstOrDefault(x => x.MaKH == id);
            if (sp != null)
                if (sp.IsDeleted == 0)
                    return sp;
            return null;
        }
        //3. httpPost, dịch vụ thêm mới một san pham
        [HttpPost]
        public bool AddNewKH(tblKhachHang kh)
        {
            try
            {
                kh.IsDeleted = 0;
                DbQuanAo.tblKhachHangs.InsertOnSubmit(kh);
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //4. httpPut để chỉnh sửa thông tin một kh
       
    }
}
