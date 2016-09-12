using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HSKR.Model;
using System.Text;
using System.Data.SqlClient;
using HSKR.Model.ViewModels;

namespace HSKR.Service.Master
{
    public class MaterialService : EntityService<MasterMaterial>
    {
        private readonly DbSet<MasterMaterial> _dbSetMaterial;
        private readonly DbSet<ProductPath> _prductPaths;
        private DBContext dbContext;
        public MaterialService(IContext context)
            : base(context)
        {
            dbContext = new DBContext();
            _context = context;
            _dbSetMaterial = _context.Set<MasterMaterial>();
            _prductPaths = _context.Set<ProductPath>();
        }

        /// <summary>
        /// Get Material by Key, Start , limit. Paging Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<MasterMaterialFullModel> GetByFilter(string key = "", string supplierId="", int start = 1, int limit = 10)
        {
            var sbQuery = new StringBuilder();
            //if (start < 1)
            //{
            //    start = 1;
            //}
            var query = @"Select m.Id, m.Name, m.KanjiName,m.KatakanaName,m.KanaName,
               
                m.ColorCode,
                m.ColorFromSupply,
                m.SupplierId,
                s.Name as SupplierName
			    From MasterMaterial m 
			    left join Supplier s on  m.SupplierId = s.Code Where 1=1";
            sbQuery.Append(query);
            var lstParam = new List<SqlParameter>();            
            if (!string.IsNullOrEmpty(supplierId))
            {
                sbQuery.Append(" And m.SupplierId =@SupplierId");
                lstParam.Add(new SqlParameter("SupplierId", supplierId));
            }

            

            if (!string.IsNullOrWhiteSpace(key))
            {
                sbQuery.Append(" AND m.Name like @Name ");
                lstParam.Add(new SqlParameter("Name", "%" + key + "%"));
            }

            var materials = dbContext.Database.SqlQuery<MasterMaterialFullModel>(sbQuery.ToString().Trim(), lstParam.ToArray()).ToList();

            return materials;
        }

        /// <summary>
        /// Get Material by Key
        /// </summary>
        /// <param name="key"></param>        
        /// <returns></returns>
        public List<MasterMaterial> GetByFilter(string key = "")
        {            
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbSetMaterial.OrderBy(c => c.Name).ToList();
            }
            var lst = _dbSetMaterial.Where(c => c.Name.Contains(key)).OrderBy(c => c.KanjiName).ToList();
            return lst;
        }

        /// <summary>
        /// Get Material by Key, ProductId
        /// </summary>
        /// <param name="key"></param>        
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<MasterMaterial> GetByFilter(string key = "", string productId = "")
        {
            var sbQuery = new StringBuilder();

            var query = @"select m.* from MasterMaterial m";
            sbQuery.Append(query);            
            var lstParam = new List<SqlParameter>();
            if (productId != "" && productId != "0")
            {
                sbQuery.Append(" where m.Id in(select pp.MaterialId from ProductPath pp where pp.ProductId=@ProductId)");
                lstParam.Add(new SqlParameter("ProductId", productId));
            }

            if (productId == "0")
            {
                sbQuery.Append(" where m.Id in(select pp.MaterialId from ProductPath pp)");
            }

            var materials = dbContext.Database.SqlQuery<MasterMaterial>(sbQuery.ToString().Trim(), lstParam.ToArray()).OrderBy(c => c.Name).ToList();

            return materials;
        }

        public int GetByFilterCount(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbSetMaterial.ToList().Count();
            }
            var count = _dbSetMaterial.Where(c => c.Name.Contains(key)).ToList().Count();
            return count;
        }


        /// <summary>
        /// Get Material by Key, ProductId
        /// </summary>
        /// <param name="key"></param>        
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<MasterMaterial> GetMaterialInProductPath(string productId)
        {
            var materials = _dbSetMaterial.Where(c => _prductPaths.Where(p2=>p2.ProductId == productId).All(p2=>p2.MaterialId == c.Id)).ToList();
            return materials;

        }
    }
}