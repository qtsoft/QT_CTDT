using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTDT.Model.ViewModels
{
    public class MobileChungTuModel
    {

        public string MaChungTu { get; set; }
        public string LoaiChungTu { get; set; }
        public string Barcode { get; set; }
        public string DonViBanHanh { get; set; }
        public DateTime? NgayPheDuyet { get; set; }
        public string TrangThai { get; set; }
        public string Files { get; set; }
    }

    public class HoSoChungTu
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
