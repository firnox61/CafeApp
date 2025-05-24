using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.CrossCuttingCorcerns.Logger;
using Core.CrossCuttingCorcerns.Logging;


namespace Core.Aspects.Autofac.Logging
{
    public class LogAspect : MethodInterception
    {
        private readonly ILogger _loggerService;

        public LogAspect()
        {
            _loggerService = new FileLogger(); // ServiceTool kullanmıyorsan doğrudan çözüm
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var methodName = $"{invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}";
            var arguments = string.Join(", ", invocation.Arguments.Select(a => a?.ToString() ?? "null"));
            _loggerService.LogInfo($"[BEFORE] {methodName} args: {arguments}");
        }

        protected override void OnException(IInvocation invocation, Exception e)
        {
            var methodName = $"{invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}";
            _loggerService.LogError($"[EXCEPTION] {methodName} - {e.Message}");
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            var methodName = $"{invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}";
            _loggerService.LogInfo($"[SUCCESS] {methodName}");
        }
    }
}