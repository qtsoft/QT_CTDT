using HSKR.Identity.IdentityModels;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HSKR.Identity.Commons
{
    public class GroupStoreBase
    {
        public DbContext Context
        {
            get;
            private set;
        }

        public DbSet<MyGroup> DbEntitySet
        {
            get;
            private set;
        }

        public IQueryable<MyGroup> EntitySet
        {
            get
            {
                return this.DbEntitySet;
            }
        }

        public GroupStoreBase(DbContext context)
        {
            this.Context = context;
            this.DbEntitySet = context.Set<MyGroup>();
        }

        public void Create(MyGroup entity)
        {
            this.DbEntitySet.Add(entity);
        }

        public void Delete(MyGroup entity)
        {
            this.DbEntitySet.Remove(entity);
        }

        public virtual Task<MyGroup> GetByIdAsync(object id)
        {
            return this.DbEntitySet.FindAsync(new object[] { id });
        }

        public virtual MyGroup GetById(object id)
        {
            return this.DbEntitySet.Find(new object[] { id });
        }

        public virtual void Update(MyGroup entity)
        {
            if (entity != null)
            {
                this.Context.Entry<MyGroup>(entity).State = EntityState.Modified;
            }
        }
    }
}
