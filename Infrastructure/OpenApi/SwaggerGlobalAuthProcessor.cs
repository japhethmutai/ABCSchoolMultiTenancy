using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Namotion.Reflection;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System.Reflection;

namespace Infrastructure.OpenApi
{
    public class SwaggerGlobalAuthProcessor(string scheme) : IOperationProcessor
    {
        private readonly string _scheme = scheme;

        public SwaggerGlobalAuthProcessor() : this(JwtBearerDefaults.AuthenticationScheme)
        {
        }

        public bool Process(OperationProcessorContext context)
        {
            IList<object> list = ((AspNetCoreOperationProcessorContext)context)
                .ApiDescription.ActionDescriptor.TryGetPropertyValue<IList<object>>("Endpointmetadata");

            if (list is not null)
            {
                if (list.OfType<AllowAnonymousAttribute>().Any())
                {
                    return true;
                }

                if (context.OperationDescription.Operation.Security.Count == 0)
                {
                    (context.OperationDescription.Operation.Security ??= new List<OpenApiSecurityRequirement>())
                        .Add(new OpenApiSecurityRequirement
                        {
                            {
                                _scheme,
                                Array.Empty<string>()
                            }
                        });
                }
            }

            return true;
        }
    }

    internal static class ObjectExtensions
    {
        public static T TryGetPropertyValue<T>(this object obj, string propertyName, T defaultValue = default) =>
            obj.GetType().GetRuntimeProperty(propertyName) is PropertyInfo propertyInfo ? (T)propertyInfo.GetValue(obj) : defaultValue;
    }
}
