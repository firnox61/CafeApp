using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Aspects.Autofac.Transaction
{
    public class TransactionScopeAspect : MethodInterception
    {
        public override void Intercept(IInvocation invocation)
        {
            using (var transactionScope = new TransactionScope(
                TransactionScopeAsyncFlowOption.Enabled)) // 🔥 async destekli
            {
                try
                {
                    invocation.Proceed(); // metodu çalıştır
                    transactionScope.Complete(); // başarılıysa commit et
                }
                catch (Exception)
                {
                    // rollback otomatik, dispose yeterli
                    throw;
                }
            }
        }
    }
}