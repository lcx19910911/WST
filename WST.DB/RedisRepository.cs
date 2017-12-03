using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq.Expressions;
using ServiceStack.Redis.Pipeline;
using ServiceStack.Redis.Support;
using System.Data.SqlClient;

namespace WST.DB
{

    /// <summary>
    /// 操作redis与entity framework
    /// </summary>
    public class RedisRepository<TEntity> :
        IDisposable
        where TEntity : class
    {
        public DbContext context;
        IRedisClient redisDB;
        IRedisTypedClient<TEntity> redisTypedClient;
        IRedisList<TEntity> table;

        public RedisRepository(DbContext context)
        {
            this.context = context;
            redisDB = new RedisClient("192.168.2.47", 6379);//redis服务IP和端口
            redisTypedClient = redisDB.As<TEntity>();
            table = redisTypedClient.Lists[typeof(TEntity).Name];
        }



        #region Repository<TEntity>成员

        /// <summary>
        /// 添加一条数据，该操作会同时插入到mssql和redis
        /// </summary>
        /// <param name="item">数据模型</param>
        /// <returns>是否成功，成功返回1，失败返回0</returns>
        public int Insert(TEntity item)
        {
            int result = 0;
            if (item != null)
            {
                context.Set<TEntity>().Add(item);
                result = context.SaveChanges();
                if (result > 0)
                {
                    Task.Run(async () => await AsyncAddEntity(item));//异步向redis插入数据。
                }
            }
            return result;
        }

        /// <summary>
        /// 删除单条数据操作，同时删除redis和SqlServer
        /// </summary>
        /// <param name="keysValue">删除条件</param>
        /// <returns>是否成功，成功返回1，失败返回0</returns>
        public int Delete(params object[] keysValue)
        {
            var entity = context.Set<TEntity>().Find(keysValue);
            context.Set<TEntity>().Remove(entity);
            int result = context.SaveChanges();
            if (result > 0)
            {
                Task.Run(async () => await AsyncDelEntity(entity));//异步删除一条数据
            }
            return result;
        }

        /// <summary>
        /// 修改操作，同时修改SqlServer和redis
        /// </summary>
        /// <param name="itemOld">旧数据</param>
        /// <param name="item">新数据</param>
        /// <returns>是否成功，成功返回1，失败返回0</returns>
        public int Update(Func<TEntity, bool> func, TEntity item)
        {
            context.Entry<TEntity>(item).State = EntityState.Modified;
            int result = context.SaveChanges();
            if (result > 0)
            {
                Task.Run(async () => await AsyncEditEntity(func, item));//异步修改redis数据。
            }
            return result;
        }

        /// <summary>
        /// 批量删除数据 ，同时删除redis和SqlServer
        /// </summary>
        /// <param name="func">删除条件</param>
        /// <returns>返回删除成功的行数</returns>
        public int DeleteSelect(Func<TEntity, bool> func)
        {
            var entities = context.Set<TEntity>().Where(func);
            context.Set<TEntity>().RemoveRange(entities);
            int result = context.SaveChanges();
            if (result > 0)
            {
                Task.Run(async () => await AsyncDelEntity(func));//异步删除redis数据。
            }
            return result;
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetModel()
        {
            return table.GetAll().AsQueryable();
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="func">查询条件</param>
        /// <param name="keySelector">排序字段</param>
        /// <param name="pageIndex">获取页面的页数</param>
        /// <param name="pageSize">页面行数</param>
        /// <param name="totalPage">返回数据行总数</param>
        /// <returns></returns>
        public IList<TEntity> GetModel(Func<TEntity, bool> func, Func<TEntity, object> keySelector, int pageIndex, int pageSize, out int totalPage)
        {
            int startRow = (pageIndex - 1) * pageSize;
            totalPage = table.Count();
            //判断缓存中数据是否为空并且数据库内数据行数是否与缓存中行数一致，如果为空或者不一致  从数据库查询数据 异步插入到缓存中。
            int dbCount = context.Set<TEntity>().Count();
            if (dbCount != totalPage || totalPage == 0)
            {
                totalPage = dbCount;
                List<TEntity> listDB = context.Set<TEntity>().AsQueryable().ToList();
                Task.Run(async () => await AsyncAddEntity(listDB));//异步向redis插入数据。
                return context.Set<TEntity>().AsQueryable().Where(func).OrderBy(keySelector).Skip(startRow).Take(pageSize).ToList();
            }
            return table.GetAll().AsQueryable().Where(func).OrderBy(keySelector).Skip(startRow).Take(pageSize).ToList();
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="func">查询条件</param>
        /// <returns></returns>
        public TEntity Find(Func<TEntity, bool> func)
        {
            return table.Where(func).FirstOrDefault();
        }

        /// <summary>
        /// 异步插入到redis列表
        /// </summary>
        /// <param name="listDB"></param>
        /// <returns></returns>
        private Task AsyncAddEntity(List<TEntity> listDB)
        {
            return Task.Factory.StartNew(() =>
            {
                table.RemoveAll();
                listDB.ForEach(m => redisTypedClient.AddItemToList(table, m));
                redisDB.Save();
            });
        }

        /// <summary>
        ///  异步插入到redis单条
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Task AsyncAddEntity(TEntity entity)
        {
            return Task.Factory.StartNew(() =>
            {
                redisTypedClient.AddItemToList(table, entity);
                redisDB.Save();
            });
        }

        /// <summary>
        /// 异步删除一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Task AsyncDelEntity(TEntity entity)
        {
            return Task.Factory.StartNew(() =>
            {
                redisTypedClient.RemoveItemFromList(table, entity);
                redisDB.Save();
            });
        }
        private Task AsyncDelEntity(Func<TEntity, bool> func)
        {
            return Task.Factory.StartNew(() =>
            {
                table.GetAll().AsQueryable().Where(func).ToList().ForEach(m => redisTypedClient.RemoveItemFromList(table, m));
                redisDB.Save();
            });
        }
        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="func"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private Task AsyncEditEntity(Func<TEntity, bool> func, TEntity item)
        {
            return Task.Factory.StartNew(() =>
            {
                redisTypedClient.RemoveItemFromList(table, Find(func));
                redisTypedClient.AddItemToList(table, item);
                redisDB.Save();
            });
        }


        #endregion

        #region IDisposable成员
        public void Dispose()
        {
            this.ExplicitDispose();
        }
        #endregion

        #region Protected Methods


        /// <summary>
        /// 垃圾回收
        /// </summary>
        protected void ExplicitDispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)//清除非托管资源
            {
                table = null;
                redisTypedClient = null;
                redisDB.Dispose();
            }
        }
        #endregion

        #region Finalization Constructs
        /// <summary>
        /// Finalizes the object.
        /// </summary>
        ~RedisRepository()
        {
            this.Dispose(false);
        }
        #endregion
    }
}