using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace SOAPMiddleware.MiddlewareComponent
{
    public class ArgumentParser
    {
        private object GetArgumentsForNonClass(XmlDictionaryReader xmlReader, OperationDescription operation, ParameterInfo param)
        {
            // Find the element for the operation's data
            xmlReader.ReadStartElement(operation.Name, operation.Contract.Namespace);

            string paramName = param.GetCustomAttribute<MessageParameterAttribute>()?.Name ?? param.Name;

            xmlReader.MoveToStartElement(paramName, operation.Contract.Namespace);

            if (!xmlReader.IsStartElement(paramName, operation.Contract.Namespace))
            {
                return null;
            }

            DataContractSerializer serializer =
                  new DataContractSerializer(
                      param.ParameterType,
                      paramName,
                      operation.Contract.Namespace);

            return serializer.ReadObject(xmlReader, verifyObjectName: true);
        }

        private object GetArgumentsForClass(XmlDictionaryReader xmlReader, OperationDescription operation, ParameterInfo param)
        {
            DataContractSerializer serializer =
                new DataContractSerializer(
                    param.ParameterType,
                    operation.Name,
                    operation.Contract.Namespace);

            return serializer.ReadObject(xmlReader, verifyObjectName: true);
        }

        public object[] GetRequestArguments(Message requestMsg, OperationDescription operationDescription)
        {
            ParameterInfo[] parameters = operationDescription.DispatchMethod.GetParameters();
            List<object> arguments = new List<object>();

            // Deserialize request wrapper and object
            using (XmlDictionaryReader xmlReader = requestMsg.GetReaderAtBodyContents())
            {
                if (!xmlReader.IsStartElement(
                    operationDescription.Name,
                    operationDescription.Contract.Namespace)
                    )
                {
                    return null;
                }

                foreach (var param in parameters)
                {
                    if (param.ParameterType.IsClass)
                    {
                        arguments.Add(
                            GetArgumentsForClass(
                                xmlReader, operationDescription, param));
                    }
                    else
                    {
                        arguments.Add(
                            GetArgumentsForNonClass(
                                xmlReader, operationDescription, param));
                    }
                }
            }

            return arguments.ToArray();
        }
    }
}
