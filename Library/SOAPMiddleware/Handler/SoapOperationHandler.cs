using Microsoft.AspNetCore.Http;
using SOAPMiddleware.Reader;
using SOAPMiddleware.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SOAPMiddleware.Handler
{
    public class SoapOperationHandler
    {
        private HttpContext _httpContext;
        private IServiceProvider _serviceProvider;
        private readonly MessageEncoder _msgEncoder;
        private readonly int _maxSizeOfHeader = 0x10000;
        private readonly ServiceDescription _service;
        private OperationDescription _operationDescription;
        private object _responseObject;

        public SoapOperationHandler(HttpContext httpContext, IServiceProvider serviceProvider, Type serviceType, MessageEncoder encoder)
        {
            _httpContext = httpContext;
            _msgEncoder = encoder;
            _serviceProvider = serviceProvider;
            _service = new ServiceDescription(serviceType);

        }

        public void HandleSOAPOperation()
        {
            HandleSOAPRequest();
            HandleSOAPResponse();
        }

        private void HandleSOAPRequest()
        {
            // Read request message
            Message requestMessage =
                    _msgEncoder.ReadMessage(
                                _httpContext.Request.Body,
                                _maxSizeOfHeader,
                                _httpContext.Request.ContentType);

            string soapAction = _httpContext.Request.Headers["SOAPAction"].ToString().Trim('\"');

            if (!string.IsNullOrEmpty(soapAction))
            {
                requestMessage.Headers.Action = soapAction;
            }

            _operationDescription =
                _service.Operations.Where(o => o.SoapAction
                .Equals(requestMessage.Headers.Action, StringComparison.Ordinal))
                .FirstOrDefault();

            if (_operationDescription == null)
            {
                throw new InvalidOperationException($"No operation found for specified action: {requestMessage.Headers.Action}");
            }

            // Get service type
            object serviceInstance = _serviceProvider.GetService(_service.ServiceType);

            // Get operation arguments from message
            List<object> arguments =
                new SOAPObjectReader(requestMessage, _operationDescription)
                        .GetRequestArguments();

            // Invoke Operation method
            _responseObject = _operationDescription.DispatchMethod.Invoke(serviceInstance, arguments.ToArray());

        }

        private void HandleSOAPResponse()
        {
            // Create response message
            string resultName = _operationDescription.DispatchMethod.ReturnParameter
                .GetCustomAttribute<MessageParameterAttribute>()?.Name ?? $"{_operationDescription.Name}Result";

            ServiceBodyWriter bodyWriter = new ServiceBodyWriter(
                _operationDescription.Contract.Namespace,
                $"{_operationDescription.Name}Response",
                resultName,
                _responseObject);

            Message responseMsg =
                Message.CreateMessage(
                    _msgEncoder.MessageVersion,
                    _operationDescription.ReplyAction,
                    bodyWriter);

            _httpContext.Response.ContentType = _httpContext.Request.ContentType;
            _httpContext.Response.Headers["SOAPAction"] = responseMsg.Headers.Action;

            _msgEncoder.WriteMessage(responseMsg, _httpContext.Response.Body);
        }
    }
}
