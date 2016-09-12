using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HSKR.Model;

namespace HSKR.Service.Master
{
    public class TeamService : EntityService<MasterTeam>
    {
         private  readonly  DbSet<MasterTeam> _dbSetTeam;
         public TeamService(IContext context)
              : base(context)
        {
            _context = context;
            _dbSetTeam = _context.Set<MasterTeam>();
        }         

        /// <summary>
         /// get MasterTeam by Key, Start , limit. Paging Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
         public List<MasterTeam> GetByFilter(string key = "", int start = 1, int limit = 10)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbSetTeam.OrderBy(c => c.Name).ToList();
            }
            var lst = _dbSetTeam.Where(c => c.Name.Contains(key)).OrderBy(c => c.Name).ToList();
            return lst;
        }


         public List<MasterTeam> GetByFilter(string key = "")
         {
             if (string.IsNullOrWhiteSpace(key))
             {
                 return _dbSetTeam.OrderBy(c => c.KanjiName).ToList();
             }
             var lst = _dbSetTeam.Where(c => c.KanjiName.Contains(key)).OrderBy(c => c.KanjiName).ToList();
             return lst;
         }


        public int GetByFilterCount(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbSetTeam.ToList().Count();
            }
            var count = _dbSetTeam.Where(c => c.KanjiName.Contains(key)).ToList().Count();
            return count;
        }
    }
}
