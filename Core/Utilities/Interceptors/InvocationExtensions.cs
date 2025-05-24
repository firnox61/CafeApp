using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Interceptors
{
    public static class InvocationExtensions
    {
        public static async Task ProceedAsync(this IInvocation invocation)
        {
            invocation.Proceed();
            if (invocation.Method.ReturnType == typeof(Task))
            {
                await (Task)invocation.ReturnValue;
            }
            else if (typeof(Task).IsAssignableFrom(invocation.Method.ReturnType))
            {
                await (Task)invocation.ReturnValue;
            }
        }
    }
}