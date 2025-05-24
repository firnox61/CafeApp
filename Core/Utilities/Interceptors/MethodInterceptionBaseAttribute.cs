using Castle.DynamicProxy;

namespace Core.Utilities.Interceptors
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor, IAsyncInterceptor
    {
        public int Priority { get; set; }//öncelik hangisiçalışsın önce loglama veya authorize

        public virtual void Intercept(IInvocation invocation)
        {

        }
        public virtual async Task InterceptAsync(IInvocation invocation)
        {
            Intercept(invocation); // fallback
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            InterceptAsync(invocation).Wait();
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            InterceptAsync(invocation).Wait();
        }

        public void InterceptSynchronous(IInvocation invocation)
        {
            Intercept(invocation);
        }
    }
}
