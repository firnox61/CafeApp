using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.CrossCuttingCorcerns.Logger;


namespace Core.Aspects.Autofac.Logging
{
    public class LogAspect : MethodInterception
    {
        private readonly ILogger _logger;

        public LogAspect(Type loggerType)
        {
            if (!typeof(ILogger).IsAssignableFrom(loggerType))
                throw new Exception("Geçersiz logger tipi");

            _logger = (ILogger)Activator.CreateInstance(loggerType)!;
        }

        protected override void OnBefore(IInvocation invocation)
        {
            Console.WriteLine("🔥 LogAspect devrede"); // TEST
            _logger.Log($"{invocation.Method.Name} çağrıldı.");
            /*var message = $"{invocation.Method.DeclaringType?.FullName}.{invocation.Method.Name} çağrıldı.";
            _logger.Log(message);*/
        }

        protected override void OnException(IInvocation invocation, Exception e)
        {
            var message = $"HATA: {invocation.Method.Name} -- {e.Message}";
            _logger.Log(message);
        }
    }

}
