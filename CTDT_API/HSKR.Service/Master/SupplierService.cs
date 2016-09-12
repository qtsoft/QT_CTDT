using System.Collections.Generic;
using System.Data.Entity;
using HSKR.Model;
using HSKR.Model.ViewModels;
using System.Linq;

namespace HSKR.Service.Master
{
    public class SupplierService : EntityService<Supplier>
    {
        private readonly DbSet<Supplier> _suppliers;
        private DBContext dbContext;
        public SupplierService(IContext context)
            : base(context)
        {
            dbContext = new DBContext();
            _context = context;
            _suppliers = _context.Set<Supplier>();
        }

        public List<SupplierModel> GetByFilter(string key = "", int start = 1, int limit = 10)
        {
           
            if (start < 1)
            {
                start = 1;
            }
            if (string.IsNullOrWhiteSpace(key))
            {
               var lst = _suppliers.OrderBy(c => c.Name).Skip(start - 1).Take(limit);
                return lst.Select(c => new  SupplierModel
                {
                   Code = c.Code, 
                   Name =  c.Name, 
                   Description = c.Description
                }).ToList();
            }
            var  lstColors  = _suppliers.Where(c => c.Name.Contains(key)).OrderBy(c => c.Name).Skip(start - 1).Take(limit);
            return lstColors.Select(c => new SupplierModel
            {
                Code = c.Code,
                Name = c.Name,
                Description = c.Description
            }).ToList();

        }

        public List<SupplierModel> GetByFilter(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {

                return _suppliers.OrderBy(c => c.Name).Select(c => new SupplierModel
                {
                    Code = c.Code,
                    Name = c.Name,
                    Description = c.Description

                }).ToList();
            }
            return _suppliers.Where(c=>c.Name.Contains(key)).OrderBy(c => c.Name).Select(c => new SupplierModel
            {
                Code = c.Code,
                Name = c.Name,
                Description = c.Description

            }).ToList();
        }


        public int GetByFilterCount(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _suppliers.ToList().Count();
            }
            var count = _suppliers.Where(c => c.Name.Contains(key)).ToList().Count();
            return count;
        }

    }
}
