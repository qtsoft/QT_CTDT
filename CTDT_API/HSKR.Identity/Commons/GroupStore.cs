using HSKR.Identity.IdentityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSKR.Identity.Commons
{
    public class GroupStore: IDisposable
    {
        private bool _disposed;
        private GroupStoreBase _groupStore;


        public GroupStore(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.Context = context;
            this._groupStore = new GroupStoreBase(context);
        }


        public IQueryable<MyGroup> Groups
        {
            get
            {
                return this._groupStore.EntitySet;
            }
        }

        public DbContext Context
        {
            get;
            private set;
        }


        public virtual void Create(MyGroup group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException("role");
            }
            this._groupStore.Create(group);
            this.Context.SaveChanges();
        }


        public virtual async Task CreateAsync(MyGroup group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException("role");
            }
            this._groupStore.Create(group);
            await this.Context.SaveChangesAsync();
        }


        public virtual async Task DeleteAsync(MyGroup group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            this._groupStore.Delete(group);
            await this.Context.SaveChangesAsync();
        }


        public virtual void Delete(MyGroup group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            this._groupStore.Delete(group);
            this.Context.SaveChanges();
        }



        public Task<MyGroup> FindByIdAsync(long roleId)
        {
            this.ThrowIfDisposed();
            return this._groupStore.GetByIdAsync(roleId);
        }


        public MyGroup FindById(long roleId)
        {
            this.ThrowIfDisposed();
            return this._groupStore.GetById(roleId);
        }

        public Task<MyGroup> FindByNameAsync(string groupName)
        {
            this.ThrowIfDisposed();
            return QueryableExtensions
                .FirstOrDefaultAsync<MyGroup>(this._groupStore.EntitySet,
                    (MyGroup u) => u.Name.ToUpper() == groupName.ToUpper());
        }


        public virtual async Task UpdateAsync(MyGroup group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            this._groupStore.Update(group);
            await this.Context.SaveChangesAsync();
        }


        public virtual void Update(MyGroup group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            this._groupStore.Update(group);
            this.Context.SaveChanges();
        }


        // DISPOSE STUFF: ===============================================

        public bool DisposeContext
        {
            get;
            set;
        }


        private void ThrowIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (this.DisposeContext && disposing && this.Context != null)
            {
                this.Context.Dispose();
            }
            this._disposed = true;
            this.Context = null;
            this._groupStore = null;
        }

    }
}
