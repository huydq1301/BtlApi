using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class ChiTietHoaDonBanController : ApiController
    {
        private DbQuanLyBanQuanAoDataContext DbQuanAo = new DbQuanLyBanQuanAoDataContext(
            "Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
            );
        //httpGet: dùng để lấy thông tin san pham
        //1. Dịch vụ lấy thông tin của toàn bộ san pham
        [HttpGet]
        public List<tblChiTietHDB> GetAllChiTietHDN(string id)
        {
            List<tblChiTietHDB> listSP = DbQuanAo.tblChiTietHDBs.Where(x => x.MaHDB == id).ToList();
            List<tblChiTietHDB> listSPCurrent = new List<tblChiTietHDB>();
            foreach (var sp in listSP)
            {
                if (sp.IsDeleted == 0)
                    listSPCurrent.Add(sp);
            }
            return listSPCurrent;
        }

        [HttpGet]
        public tblChiTietHDB GetID(string MaHDB)
        {
            tblChiTietHDB tChitiet = DbQuanAo.tblChiTietHDBs.FirstOrDefault(x => x.MaHDB == MaHDB);
            if (tChitiet != null)

                if (tChitiet.IsDeleted == 0)
                return tChitiet;
            return null;
        }
        [HttpGet]
        public tblChiTietHDB getAll(string MaSP)
        {
            tblChiTietHDB tChitiet = DbQuanAo.tblChiTietHDBs.FirstOrDefault(x => x.MaSP == MaSP);
            if (tChitiet != null)

                if (tChitiet.IsDeleted == 0)
                return tChitiet;
            return null;
        }
        //2. Dịch vụ lấy thông tin một san pham với mã nào đó
        [HttpGet]
        public tblChiTietHDB GetChiTietHDN(string hdn, string sp)
        {
            tblChiTietHDB tChitiet = DbQuanAo.tblChiTietHDBs.FirstOrDefault(x => x.MaHDB == hdn && x.MaSP == sp);
            if (tChitiet != null)

                if (tChitiet.IsDeleted == 0)
                return tChitiet;
            return null;
        }
        //3. httpPost, dịch vụ thêm mới một san pham
        [HttpPost]
        public bool AddNewCTHDB(tblChiTietHDB hdb)
        {
            try
            {
                SqlConnection conn = new SqlConnection("Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO tblChiTietHDB (MaHDB, MaSP, SLBan, ThanhTien, IsDeleted) VALUES (@MaHDB, @MaSP, @SLBan, @ThanhTien, 0)";
                cmd.Parameters.AddWithValue("@MaHDB", hdb.MaHDB);
                cmd.Parameters.AddWithValue("@MaSP", hdb.MaSP);
                cmd.Parameters.AddWithValue("@SLBan", hdb.SLBan);
                cmd.Parameters.AddWithValue("@ThanhTien", hdb.ThanhTien);
                conn.Open();
                int result = cmd.ExecuteNonQuery();
                conn.Close();
                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        //4. httpPut để chỉnh sửa thông tin một ChiTietHDN
        [HttpPut]
        public bool UpdateNewChiTietHDN(tblChiTietHDB postData)
        {
            try
            {
                tblChiTietHDB ChiTietHDN = DbQuanAo.tblChiTietHDBs.FirstOrDefault(x => x.MaHDB == postData.MaHDB && x.MaSP == postData.MaSP);
                if (ChiTietHDN == null) return false;
                // gan gia tri tu post data vao ChiTietHDN tim thay trong ChiTietHDN
                ChiTietHDN.MaHDB = postData.MaHDB;
                ChiTietHDN.MaSP = postData.MaSP;
                ChiTietHDN.SLBan = postData.SLBan;
                ChiTietHDN.ThanhTien = postData.ThanhTien;
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        [HttpPut]
        public bool UpDateIsDeleted(string id , string masp)
        {
            try
            {
                tblChiTietHDB sanPham = DbQuanAo.tblChiTietHDBs.FirstOrDefault(x => x.MaHDB == id && x.MaSP == masp);
                if (sanPham == null) return false;
                // gan gia tri tu post data vao sanpham tim thay trong sanpham
                sanPham.IsDeleted = 1;
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
        public bool DeleteCT(string hdn, string masp)
        {
            tblChiTietHDB HoaDon = DbQuanAo.tblChiTietHDBs.FirstOrDefault(x => x.MaHDB == hdn & x.MaSP == masp);
            if (HoaDon == null) { return false; }
            DbQuanAo.tblChiTietHDBs.DeleteOnSubmit(HoaDon);
            // submit changed
            DbQuanAo.SubmitChanges();
            return true;
        }
    }
}
