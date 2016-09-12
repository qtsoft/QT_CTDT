using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HSKR.Model;

namespace HSKR.Service.Master
{
    public class CountryService : EntityService<MasterCountry>
    {
        private readonly DbSet<MasterCountry> _dbSetCountries;

        public CountryService(IContext context)
            : base(context)
        {
            _context = context;
            _dbSetCountries = _context.Set<MasterCountry>();
        }


        /// <summary>
        ///     get Material by Key, Start , limit. Paging Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="limmit"></param>
        /// <returns></returns>
        public List<MasterCountry> GetByFilter(string key = "", int start = 1, int limit = 10)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbSetCountries.OrderBy(c => c.Name).ToList();
            }
            var lst = _dbSetCountries.Where(c => c.Name.Contains(key)).OrderBy(c => c.Name).ToList();
            return lst;
        }


        /// <returns></returns>
        public List<MasterCountry> GetByFilter(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbSetCountries.OrderBy(c => c.Name).ToList();
            }
            var lst = _dbSetCountries.Where(c => c.Name.Contains(key)).OrderBy(c => c.Name).ToList();
            return lst;
        }
        /// <summary>
        /// get count Path
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetByFilterCount(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbSetCountries.ToList().Count();
            }
            var count = _dbSetCountries.Where(c => c.Name.Contains(key)).ToList().Count();
            return count;
        }
    }
}
