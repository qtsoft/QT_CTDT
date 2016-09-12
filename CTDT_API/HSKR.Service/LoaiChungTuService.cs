using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTDT.Model;
using System.Data.Entity;
using CTDT.Model.ViewModels;


namespace CTDT.Service
{
    public class LoaiChungTuService: EntityService<LoaiChungTu>
    {

        private readonly DbSet<LoaiChungTu> _dbLoaiChungTu;
        public LoaiChungTuService(IContext context)
              : base(context)
        {
            _context = context;
            _dbLoaiChungTu = _context.Set<LoaiChungTu>();
        }

        public List<LoaiChungTuModel> GetByFilter(string key = "", int start = 1, int limit = 10)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return  _dbLoaiChungTu.OrderBy(c => c.Name).Select( c=> new LoaiChungTuModel
                {
                    Name = c.Name, 
                    Description = c.Description, 
                    Id = c.Id
                }).ToList();
            }
            var lst = _dbLoaiChungTu.Where(c => c.Name.Contains(key)).OrderBy(c => c.Name);
            return lst.Select(c=> new LoaiChungTuModel
            {
                Name = c.Name,
                Description = c.Description,
                Id = c.Id
            }).ToList();
        }

     
    }
}
