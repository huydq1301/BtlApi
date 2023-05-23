using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Remoting.Contexts;
using Microsoft.SqlServer.Server;

namespace RestAPI.Controllers
{
    public class DoanhThuController : ApiController
    {
        private DbQuanLyBanQuanAoDataContext DbQuanAo = new DbQuanLyBanQuanAoDataContext(
            "Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
            );
        //httpGet: dùng để lấy thông tin san pham
        //1. Dịch vụ lấy thông tin của toàn bộ san pham
        [HttpGet]
        public IQueryable DoanhThuNam(int nam)
        {
            return DbQuanAo.TienBanNam(nam);
            
        }
        [HttpGet]
        public IQueryable DoanhThuNamTheoSP(int namtheosp)
        {
            return DbQuanAo.SPBanBDTron(namtheosp);

        }
        [HttpGet]
        public IQueryable NhapNam(int namnhap)
        {
            return DbQuanAo.TienNhapNam(namnhap);

        }
        [HttpGet]
        public IQueryable NhapNamTheoSP(int namnhaptheosp)
        {
            return DbQuanAo.SPNhapBDTron(namnhaptheosp);

        }
    }
}
