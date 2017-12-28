using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using WST.IService;
using WST.Service;

namespace WST.Web.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            container.RegisterType<IActivityCategoryService, ActivityCategoryService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IAdviserService, AdviserService>();
            
            container.RegisterType<IAdminService, AdminService>();
            
            container.RegisterType<IMenuService, MenuService>();
            container.RegisterType<IOperateService, OperateService>();
            container.RegisterType<IDemoService, DemoService>();

            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IDataDictionaryService, DataDictionaryService>();
            container.RegisterType<IPayOrderService, PayOrderService>();
            container.RegisterType<IRechargePlanService, RechargePlanService>();

            
            container.RegisterType<ITemplateService, TemplateService>();
            container.RegisterType<ITemplateCategoryService, TemplateCategoryService>();
            container.RegisterType<IUserWechatService, UserWechatService>();

            container.RegisterType<IPinTuanService, PinTuanService>();
            container.RegisterType<ICarouselService, CarouselService>();

            container.RegisterType<IUserActivityService, UserActivityService>();

            container.RegisterType<IKanJiaService, KanJiaService>();
            container.RegisterType<IMusicService, MusicService>();
            container.RegisterType<IMiaoShaService, MiaoShaService>();
            container.RegisterType<IMpAutoReplyService, MpAutoReplyService>();
            container.RegisterType<IMpMenuService, MpMenuService>();

        }
        
    }
}
