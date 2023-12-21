using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Reflection;
using System.Xml.XPath;

namespace SecretGiftExchange.API
{
    public class IncludeXmlCommentsWithExceptionsOperationFilter : IOperationFilter
    {
        private readonly XPathNavigator _xmlNavigator;
        
        public IncludeXmlCommentsWithExceptionsOperationFilter(string xmlPath)
        {
            var xmlDocument = new XPathDocument(xmlPath);
            _xmlNavigator = xmlDocument.CreateNavigator();
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;
            var method = apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo : null;

            if (method == null) return;

            var memberName = $"M:{method.DeclaringType.FullName}.{method.Name}";
            var methodNode = _xmlNavigator.SelectSingleNode($"/doc/members/member[@name='{memberName}']");

            if (methodNode != null)
            {
                var summaryNode = methodNode.SelectSingleNode("summary");
                var returnsNode = methodNode.SelectSingleNode("returns");
                var exceptionNodes = methodNode.Select("exception");

                if (summaryNode != null)
                {
                    operation.Summary = summaryNode.Value.Trim();
                }

                if (returnsNode != null)
                {
                    operation.Description += "<br/><b>Returns:</b> " + returnsNode.Value.Trim();
                }

                while (exceptionNodes.MoveNext())
                {
                    var exceptionNode = exceptionNodes.Current;
                    var exceptionType = exceptionNode.GetAttribute("cref", "");
                    operation.Description += $"<br/><b>Throws:</b> {exceptionType} - {exceptionNode.Value.Trim()}";
                }
            }
        }
    }

}
