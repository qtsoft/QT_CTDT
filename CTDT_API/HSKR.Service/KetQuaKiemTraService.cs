using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CTDT.Model;
using CTDT.Model.ViewModels;
namespace CTDT.Service
{
    public class KetQuaKiemTraService: EntityService<KetQuaKiemTra>
    {
        private readonly DbSet<KetQuaKiemTra> _dbKetQua;
        public KetQuaKiemTraService(IContext context)
              : base(context)
        {
            _context = context;
            _dbKetQua = _context.Set<KetQuaKiemTra>();
        }

        public List<KetQuaKiemTraModel> GetByFilter(string key = "", int start = 1, int limit = 10)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbKetQua.OrderBy(c => c.MaChungTu).Select(c => new KetQuaKiemTraModel
                {
                    DiaDiem = c.DiaDiemKT,
                    KetQua = c.KetQuaKT,
                    DonViKiemTra = c.DonViKT, 
                    MaChungTu = c.MaChungTu, 
                    NgayKiemTra = c.NgayKT, 
                    NguoiKiemTra = c.NguoiKiemTra, 
                    NoiDungKiemTra = c.NoiDungKT
                 
                }).ToList();
            }
            var lst = _dbKetQua.Where(c => c.MaChungTu.Contains(key)).OrderBy(c => c.MaChungTu);
            return lst.Select(c => new KetQuaKiemTraModel
            {
                DiaDiem = c.DiaDiemKT,
                KetQua = c.KetQuaKT,
                DonViKiemTra = c.DonViKT,
                MaChungTu = c.MaChungTu,
                NgayKiemTra = c.NgayKT,
                NguoiKiemTra = c.NguoiKiemTra,
                NoiDungKiemTra = c.NoiDungKT
            }).ToList();
        }
    }
}
