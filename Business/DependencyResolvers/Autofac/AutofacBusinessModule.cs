using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            // builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();
            builder.RegisterType<ProductManager>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<EfProductDal>().As<IProductDal>().InstancePerLifetimeScope();
            builder.RegisterType<FileServiceManager>().As<IFileService>().InstancePerLifetimeScope();

            builder.RegisterType<PaymentManager>().As<IPaymentService>().InstancePerLifetimeScope();
            builder.RegisterType<EfPaymentDal>().As<IPaymentDal>().InstancePerLifetimeScope();

            builder.RegisterType<OrderManager>().As<IOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<EfOrderDal>().As<IOrderDal>().InstancePerLifetimeScope();

            builder.RegisterType<IngredientManager>().As<IIngredientService>().InstancePerLifetimeScope();
            builder.RegisterType<EfIngredientDal>().As<IIngredientDal>().InstancePerLifetimeScope();

            builder.RegisterType<TableManager>().As<ITableService>().InstancePerLifetimeScope();
            builder.RegisterType<EfTableDal>().As<ITableDal>().InstancePerLifetimeScope();

            builder.RegisterType<OrderItemManager>().As<IOrderItemService>().InstancePerLifetimeScope();
            builder.RegisterType<EfOrderItemDal>().As<IOrderItemDal>().InstancePerLifetimeScope();

            builder.RegisterType<ProductIngredientManager>().As<IProductIngredientService>().InstancePerLifetimeScope();
            builder.RegisterType<EfProductIngredientDal>().As<IProductIngredientDal>().InstancePerLifetimeScope();

            builder.RegisterType<EfProductionHistoryDal>().As<IProductionHistoryDal>().InstancePerLifetimeScope();



            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

        }
    }
}
