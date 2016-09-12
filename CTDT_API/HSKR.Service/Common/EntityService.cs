using CTDT.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace CTDT.Service
{
    public abstract class EntityService<T> : IEntityService<T> where T : class
    {
        public IContext _context { get; set; }
        public IDbSet<T> _dbSet { get; set; }

        public string KeyName;

        public EntityService(IContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            KeyName = typeof(T).GetProperties()[0].Name;
        }


        #region Basic CRUD Functions

        public virtual void Create(T entity)
        {
           
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            PropertyInfo pInfo = typeof(T).GetProperties()[0];
            if (pInfo.PropertyType == typeof(int))
            {
                CommonFunctions.TrySetProperty(entity, typeof(T).GetProperties()[0].Name, MaxId() + 1);
            }
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public virtual void CreateEntities(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _dbSet.Add(entity);
            _context.SaveChanges();
        }


        public virtual void CreateWithParentID(T entity, int ParentID)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            CommonFunctions.TrySetProperty(entity, typeof(T).GetProperties()[0].Name, MaxId() + 1);
            CommonFunctions.TrySetProperty(entity, typeof(T).GetProperties()[1].Name, ParentID);
            _dbSet.Add(entity);
            _context.SaveChanges();
        }


        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public virtual void Update(List<T> entityList)
        {
            foreach (T obj in entityList)
            {
                var cEntity = GetEntityById(GetIdGeneric(obj));
                if (cEntity != null) {
                    _context.Entry(obj).State = EntityState.Modified;
                _context.SaveChanges();
                }
            }
        }

        public virtual void InsertOrUpdate(T entity)
        {
            var cEntity = GetEntityById(GetIdGeneric(entity));
            if (cEntity != null)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                _dbSet.Add(entity);
            }
            _context.SaveChanges();
        }



        public virtual void UpdateWithParentID(T entity, int ParentID)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            CommonFunctions.TrySetProperty(entity, typeof(T).GetProperties()[1].Name, ParentID);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            var curentEntity = GetEntityById(GetIdGeneric(entity));
            if (curentEntity == null) throw new ArgumentNullException("entity");
            _dbSet.Remove(curentEntity);
            _context.SaveChanges();
        }

        public virtual void DeleteAll(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbSet.Where<T>(where).AsEnumerable<T>();
            foreach (T obj in objects)
                _dbSet.Remove(obj);
            _context.SaveChanges();
        }

        public virtual void DeleteAll(List<T> entityList )
        {
            foreach (T obj in entityList)
                _dbSet.Remove(obj);
            _context.SaveChanges();
        }
        public void Move(T entity)
        {
            var cEntity = GetEntityById(GetIdGeneric(entity));
            if (cEntity != null)
            {
                object cP = GetParentIdGeneric(entity);
                CommonFunctions.TrySetProperty(cEntity, typeof(T).GetProperties()[1].Name, cP);
            }
            _context.SaveChanges();
        }

        public int MaxId()
        {
            var lastEntity = _dbSet.AsEnumerable<T>().LastOrDefault();
            int result = 0;
            if (lastEntity != null)
            {
                var query = typeof(T).GetProperties()[0].GetValue(lastEntity);
                int.TryParse(query + "", out result);
            }
            return result;
        }


        #endregion

        #region Search Filter Functions

        public virtual List<T> GetAll()
        {
            return _dbSet.ToList<T>();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).ToList();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync<T>();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).ToList();
        }


        public virtual IQueryable<T> GetAllAsQueryable()
        {
            return _dbSet.AsQueryable<T>();

        }
        public virtual IEnumerable<T> GetAllAsIEnumerable()
        {
            return _dbSet.AsEnumerable<T>();

        }

        public virtual T GetEntityById(object Id)
        {
            return _dbSet.Where(typeof(T).GetProperties()[0].Name + " = @0", Id).FirstOrDefault();
        }


        #endregion

        #region Support Functions


        //Hàm lấy giá trị Id của 1 entity Generic
        private object GetIdGeneric(T entity)
        {
            var id = typeof(T).GetProperties()[0].GetValue(entity);
            //int result = CommonFunctions.TryParseId(id + "");
            return id;
        }

        private object GetId1Generic(T entity)
        {
            var id = typeof(T).GetProperties()[1].GetValue(entity);
            //int result = CommonFunctions.TryParseId(id + "");
            return id;
        }

        //Hàm lấy giá trị ParentId của 1 entity Generic
        private object GetParentIdGeneric(T entity)
        {
            var id = typeof(T).GetProperties()[1].GetValue(entity);
            // int? result = CommonFunctions.TryParseParentId(id + "");
            return id;
        }


        #endregion
    }

    public interface IEntityService<T> where T : class 
    {
        IContext _context { get; set; }
        IDbSet<T> _dbSet { get; set; }

        void Create(T entity);
        void CreateWithParentID(T entity, int ParentID);
        void Delete(T entity);

        void DeleteAll(Expression<Func<T, bool>> where);
        void Update(T entity);

        void InsertOrUpdate(T entity);

        void Move(T entity);

        int MaxId();

        T GetEntityById(object Id);

        List<T> GetAll();
        Task<List<T>> GetAllAsync();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        IQueryable<T> GetAllAsQueryable();
        IEnumerable<T> GetAllAsIEnumerable();
        IEnumerable<T> Find(Expression<Func<T, bool>> where);
    }
}
