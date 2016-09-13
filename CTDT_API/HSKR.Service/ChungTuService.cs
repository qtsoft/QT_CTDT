using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTDT.Model;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Security.Policy;
using CTDT.Model.ViewModels;

namespace CTDT.Service
{
    public class ChungTuService :EntityService<ChungTu>
    {
        private readonly DbSet<ChungTu> _dbChungTu;
        private readonly DbSet<ViewChungTu> _viewChungTus;
        public ChungTuService(IContext context)
              : base(context)
        {
            _context = context;
            _dbChungTu = _context.Set<ChungTu>();
            _viewChungTus = _context.Set<ViewChungTu>();
        }

        public List<ViewChungTu> GetByFilter(string key = "", string maChungTu="", int maLoaiChungTu=0, string donViBanHanh="", int trangThai=0, int start = 1, int limit = 10)
        {

         
            Expression<Func<ViewChungTu, bool>> lambda;
            if (maLoaiChungTu > 0)
            {
                lambda = c => c.MaLoaiChungTu == maLoaiChungTu;
            }
            if (!string.IsNullOrWhiteSpace(key))
            {
                lambda = c => (c.Ten.Contains(key)&& c.MaLoaiChungTu == maLoaiChungTu);
            }
            if (!string.IsNullOrWhiteSpace(maChungTu))
            {
                lambda = c => (c.Ten.Contains(key)&& c.MaLoaiChungTu == maLoaiChungTu);
            }
            if(trangThai == 0)
            {
                return _viewChungTus.OrderBy(c => c.MaChungTu).ToList();
            }
            
            var lst = _viewChungTus.Where(c=>(c.TrangThai==trangThai) ).OrderBy(c => c.MaChungTu);
            return lst.ToList();
            
        }
    }
}
