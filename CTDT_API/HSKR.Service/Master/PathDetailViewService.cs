using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HSKR.Model;
using System.Text;
using System.Data.SqlClient;
using HSKR.Model.ViewModels;

namespace HSKR.Service.Master
{
    public class PathDetailViewService : EntityService<PathDetail>
    {
        private readonly DbSet<ViewPathDetail> _dbSetPathDetailView;
        private DBContext dbContext;
        public PathDetailViewService(IContext context)
             : base(context)
        {
            dbContext = new DBContext();
            _context = context;
            _dbSetPathDetailView = _context.Set<ViewPathDetail>();
        }


        /// <summary>
        /// get PathDetailView by productId
        /// </summary>
        /// <param name="productId"></param>       
        /// <returns></returns>
        public List<PathDetaiViewFullModel> GetByFilter(string productId = "")
        {
            var pathDetails = _dbSetPathDetailView.Where(c => c.ProductId.Equals(productId)).ToList();

            var pathDetailViews = pathDetails.Select(p => new PathDetaiViewFullModel
            {
                Id = p.Id,
                Name = p.Name,
                PathId = p.PathId,
                TimeWork = p.TimeWork,
                Step = p.Step,
                YarnColor = p.YarnColor,
                PathName = p.PathName,
                ProductId = p.ProductId,
                ProductName = p.ProductName
            }).ToList();
            return pathDetailViews;
        }       


        /// <summary>
        /// get count PathDetailView
        /// </summary>        
        /// <returns></returns>
        public int GetByFilterCount()
        {           
            var count = _dbSetPathDetailView.ToList().Count();
            return count;
        }
    }
}
