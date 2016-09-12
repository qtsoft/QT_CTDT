using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HSKR.Model;
using HSKR.Model.ViewModels;
using System.Text;
using System.Data.SqlClient;

namespace HSKR.Service.Master
{
    public class ProductPathService : EntityService<ProductPath>
    {
        private  readonly DbSet<ProductPath> _dbSetProductPath;
        private readonly DbSet<Product> _dbSetProduct;
        private readonly DbSet<MasterPath> _dbSetMasterPath;
        private DBContext dbContext;
        public ProductPathService(IContext context)
            : base(context)
        {
            _context = context;
            dbContext = new DBContext();
            _dbSetProductPath = _context.Set<ProductPath>();
            _dbSetProduct = _context.Set<Product>();
            _dbSetMasterPath = _context.Set<MasterPath>();
        }

        /// <summary>
        /// get ProductPath by materialId, ProductId, Start, limit. Paging Data
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="productId"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<ProductPathFullModel> GetByFilter(string materialId="",string productId = "", int start = 1, int limit = 10)
        {
            var sbQuery = new StringBuilder();

            var query = @"Select pp.Id, pp.PathId, pp.ProductId,pp.CreateDate,
                pp.ModifyDate,
                pp.Quantity,
                pp.TotalTime,
                pp.UrlImageSource as UrlSource,
                pp.MaterialId,
                pp.TimePerPath,
                pp.Step,
                mp.Name as PathName,
                p.Name as ProductName,                              
                ISNULL((select sum(t.Quantity) from task t
		        inner join ProductPath pp1 on pp1.Id=t.ProductPathId
		        where t.ProductPathId=pp.Id),0) as CreateQuantity

			    From ProductPath pp 
			    inner join Product p on  pp.ProductId = p.Id                
			    inner join MasterMaterial mate on pp.MaterialId = mate.Id
                inner join MasterPath mp on  pp.PathId = mp.Id";
            sbQuery.Append(query);
            sbQuery.Append(" Where 1=1 ");                   

            var lstParam = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(productId) && productId != "0")
            {
                sbQuery.Append(" AND p.Id =@ProductId");
                lstParam.Add(new SqlParameter("ProductId", productId));
            }
            if (!string.IsNullOrEmpty(materialId) && materialId != "0")
            {
                sbQuery.Append(" AND mate.Id =@MaterialId");
                lstParam.Add(new SqlParameter("MaterialId", materialId));
            }
            sbQuery.Append(" Order by CONVERT(INT, mp.Name) ");
            var productPaths = dbContext.Database.SqlQuery<ProductPathFullModel>(sbQuery.ToString().Trim(), lstParam.ToArray()).ToList();
            
            return productPaths;
        }

        /// <summary>
        /// Get ProductPath by material, productId, orderId
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="productId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<ProductPathFullModel> GetByFilter(string materialId = "", string productId = "", string orderId = "")
        {
            var sbQuery = new StringBuilder();

            var query = @"Select pp.Id, pp.PathId, pp.ProductId,pp.CreateDate,
                pp.ModifyDate,
                pp.Quantity,
                pp.TotalTime,
                pp.UrlImageSource as UrlSource,
                pp.MaterialId,
                pp.TimePerPath,
                pp.Step,
                mp.Name as PathName,
                p.Name as ProductName,                              
                ISNULL((select sum(t.Quantity) from task t
		        inner join ProductPath pp1 on pp1.Id=t.ProductPathId
		        where t.ProductPathId=pp.Id),0) as CreateQuantity

			    From ProductPath pp 
			    inner join Product p on  pp.ProductId = p.Id                
			    inner join MasterMaterial mate on pp.MaterialId = mate.Id
                inner join MasterPath mp on  pp.PathId = mp.Id";
            sbQuery.Append(query);
            sbQuery.Append(" Where 1=1 ");

            var lstParam = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(productId) && productId != "0")
            {
                sbQuery.Append(" AND p.Id =@ProductId");
                lstParam.Add(new SqlParameter("ProductId", productId));
            }
            if (!string.IsNullOrEmpty(materialId) && materialId != "0")
            {
                sbQuery.Append(" AND mate.Id =@MaterialId");
                lstParam.Add(new SqlParameter("MaterialId", materialId));
            }

            if (!string.IsNullOrEmpty(orderId) && orderId != "0")
            {
                sbQuery.Append(" AND p.OrderId =@OrderId");
                lstParam.Add(new SqlParameter("OrderId", orderId));
            }
            sbQuery.Append(" Order by CONVERT(INT, mp.Name) ");
            var productPaths = dbContext.Database.SqlQuery<ProductPathFullModel>(sbQuery.ToString().Trim(), lstParam.ToArray()).ToList();

            return productPaths;
        }

        /// <summary>
        /// get count ProductPath
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetByFilterCount(string productId ="", string materialId = "")
        {
            var count = _dbSetProductPath.Where(c => c.ProductId.Equals(productId) && c.MaterialId.Equals(materialId)).ToList().Count();
            return count;
        }

        public List<MasterPathModel> GetPathNotInProductPath(string styleId = "", string productId = "")
        {
            var paths = _dbSetMasterPath.Where(p => (p.StyleId==styleId && !_dbSetProductPath.Any(pp => pp.PathId == p.Id && pp.ProductId==productId))).ToList();

            var masterPaths = paths.Select(p => new MasterPathModel
            {
                Id = p.Id, 
                Name = p.Name, 
                CreateDate = p.CreateDate, 
                StyleId = p.StyleId, 
                KanaName = p.KanaName,
                KanjiName = p.KanjiName, 
                KatakanaName = p.KatakanaName, 
                ModifyDate = p.ModifyDate, 
                Step =  p.Step, 
                TimePerPath = p.TimePerPath, 
                UrlSource = p.UrlSource
            }).ToList();

            if(masterPaths.Count>0)
            {
                for (int i = 0; i < masterPaths.Count - 1; i++)
                {
                    for (int j = i + 1; j < masterPaths.Count; j++)
                    {
                        if (int.Parse(masterPaths[j].Name)< int.Parse(masterPaths[i].Name))
                        {
                            var masterPathTemp = masterPaths[j];
                            masterPaths[j] = masterPaths[i];
                            masterPaths[i] = masterPathTemp;
                        }
                }
                }

            }
            

            return masterPaths;
        }
        
    }
}
