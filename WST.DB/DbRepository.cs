using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using WST.Core.Util;
using WST.Model;

namespace WST.DB
{
    public class DbRepository : DbContext
    {

        public DbRepository()
            : base("name=DbRepository")
        {
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="entry">entry对象</param>
        private void InitObject(DbEntityEntry entry)
        {
            if (entry.Entity is BaseEntity)
            {
                var entity = entry.Entity as BaseEntity;
                switch (entry.State)
                {
                    case EntityState.Added:
                        //初始化这些值，如果这些值为null时，自动赋值
                        if (entity.CreatedTime == new DateTime())
                            entity.CreatedTime = DateTime.Now;
                        if (entity.UpdatedTime == new DateTime())
                            entity.UpdatedTime = DateTime.Now;
                        if (string.IsNullOrEmpty(entity.ID))
                            entity.ID = Guid.NewGuid().ToString("N");
                        break;
                    case EntityState.Modified:
                        //初始化这些值，如果这些值为null时，自动赋值
                        if (entity.UpdatedTime == new DateTime())
                            entity.UpdatedTime = DateTime.Now;
                        break;

                    case EntityState.Deleted:
                        //初始化这些值，如果这些值为null时，自动赋值
                        if (entity.UpdatedTime == new DateTime())
                            entity.UpdatedTime = DateTime.Now;
                        break;
                }
            }
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Adviser> Adviser { get; set; }

        public virtual DbSet<Role> Role { get; set; }


        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserWechat> UserWechat { get; set; }



        public virtual DbSet<Operate> Operate { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }


        public virtual DbSet<Demo> Demo { get; set; }

        public virtual DbSet<DataDictionary> DataDictionary { get; set; }
        public virtual DbSet<PayOrder> PayOrder { get; set; }
        public virtual DbSet<RechargePlan> RechargePlan { get; set; }

        public virtual DbSet<ActivityCategory> ActivityCategory { get; set; }


        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<TemplateCategory> TemplateCategory { get; set; }


        public virtual DbSet<PinTuan> PinTuan { get; set; }
        public virtual DbSet<Carousel> Carousel { get; set; }
        public virtual DbSet<UserActivity> UserActivity { get; set; }
        public virtual DbSet<KanJia> KanJia { get; set; }
        
        public override int SaveChanges()
        {
            try
            {
                var entries = from e in this.ChangeTracker.Entries()
                              where e.State != EntityState.Unchanged
                              select e;   //过滤所有修改了的实体，包括：增加 / 修改 / 删除

                foreach (var entry in entries)
                {
                    InitObject(entry);
                }

                if (entries.Count() == 0)
                    return 1;
                else
                    return base.SaveChanges();

            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex);
                //并发冲突数据
                if (ex.GetType() == typeof(DbUpdateConcurrencyException))
                {
                    return -1;
                }
                return 0;
            }

        }
        
    }

}
