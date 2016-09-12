using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HSKR.Model;
using HSKR.Model.ViewModels;
using System.Text;
using System.Data.SqlClient;

namespace HSKR.Service.Master
{
    public class IssuesService : EntityService<MasterIssue>
    {
        private  readonly  DbSet<MasterIssue> _dbSetIssues;
        private DBContext dbContext;
        public IssuesService(IContext context)
              : base(context)
        {
            dbContext = new DBContext();
            _context = context;
            _dbSetIssues = _context.Set<MasterIssue>();
        }


        public List<MasterIssue> GetByFilter(string key = "", int start = 1, int limit = 10)
        {
            var sbQuery = new StringBuilder();
            if (start < 1)
            {
                start = 1;
            }
            var query = @"Select i.Id, i.Name, i.KanjiName,i.KatakanaName,i.KanaName,               
                i.CountryCode               
			    From MasterIssues i";
            sbQuery.Append(query);
            var lstParam = new List<SqlParameter>();
            //if (!string.IsNullOrWhiteSpace(countryCode))
            //{
            //    sbQuery.Append(" And i.CountryCode =@CountryCode");
            //    lstParam.Add(new SqlParameter("CountryCode", countryCode));
            //}



            if (!string.IsNullOrWhiteSpace(key))
            {
                sbQuery.Append(" And i.Name like @Name ");
                lstParam.Add(new SqlParameter("Name", "%" + key + "%"));
            }

            var issues = dbContext.Database.SqlQuery<MasterIssue>(sbQuery.ToString().Trim(), lstParam.ToArray()).ToList();

            return issues;
        }

        public int GetByFilterCount(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbSet.ToList().Count();
            }
            var count = _dbSet.Where(c => c.KanjiName.Contains(key)).ToList().Count();
            return count;
        }

        //public List<MasterIssueModel> GetByCountry(string countryCode = "")
        //{
        //    var issues = _dbSetIssues.Where(c => c.CountryCode.Equals(countryCode.Trim()));

        //    var issuesModellst = new List<MasterIssueModel>();
        //    foreach (MasterIssue obj in issues)
        //    {
        //        var issuesModel = new MasterIssueModel();
        //        string[] name=obj.Name.Split(';');
        //        var nameList = new List<IssueNameInfo>();
        //        for (int i = 0; i < name.Length; i++)
        //        {
        //            string[] values = name[i].Split(':');
        //            var keyValue = new IssueNameInfo();
        //            keyValue.Code = values[0];
        //            keyValue.Value = values[1];
        //            nameList.Add(keyValue);
        //        }
                
        //        issuesModel.Id = obj.Id;
        //        issuesModel.CountryCode = obj.CountryCode;
        //        issuesModel.KanaName = obj.KanaName;
        //        issuesModel.KanjiName = obj.KanjiName;
        //        issuesModel.KatakanaName = obj.KatakanaName;
        //        issuesModel.Name = nameList;
        //        issuesModellst.Add(issuesModel);
        //    }

        //    //var issuesModel = issues.Select(c => new MasterIssueModel
        //    //{
        //    //    Name = c.Name, 
        //    //    CountryCode = c.CountryCode, 
        //    //    KanaName = c.KanaName, 
        //    //    KanjiName = c.KanjiName, 
        //    //    KatakanaName = c.KatakanaName, 
        //    //    Id = c.Id
        //    //}).ToList();
        //    return issuesModellst;
        //} 
    }
}
