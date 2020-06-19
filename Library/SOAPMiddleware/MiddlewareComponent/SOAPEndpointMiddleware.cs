using Microsoft.AspNetCore.Http;
using SOAPMiddleware.Handler;
using System;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace SOAPMiddleware
{
    public class SOAPEndpointMiddleware
    {
        // The middleware delegate to call after this one finishes processing
        private readonly RequestDelegate _next;
        private readonly Type _serviceType;
        private readonly string _endpointPath;
        private readonly MessageEncoder _msgEncoder;

        public SOAPEndpointMiddleware(RequestDelegate next, Type serviceType, string path, MessageEncoder encoder)
        {
            _next = next;
            _serviceType = serviceType;
            _endpointPath = path;
            _msgEncoder = encoder;
            _serviceType = serviceType;
        }

        public async Task Invoke(HttpContext httpContext, IServiceProvider serviceProvider)
        {
            if (httpContext.Request.Path.Equals(_endpointPath, StringComparison.Ordinal))
            {
                new SoapOperationHandler(httpContext, serviceProvider,_serviceType, _msgEncoder)
                    .HandleSOAPOperation();
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}
