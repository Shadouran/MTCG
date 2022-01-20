using MTCG.Core.Request;
using MTCG.Core.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MTCG
{
    class RouteParser : IRouteParser
    {
        public bool IsMatch(RequestContext request, HttpMethod method, string routePattern)
        {
            if (method != request.Method)
                return false;

            Regex regex = new Regex(@"{.*}");
            var pattern = "^" + regex.Replace(routePattern, ".*").Replace("/", "\\/");
            if (request.ResourcePath.Count(c => c == '/') >= 2)
                pattern += '$';
            return Regex.IsMatch(request.ResourcePath, pattern);
        }

        public Dictionary<string, string> ParseParameters(RequestContext request, string routePattern)
        {
            var parameters = new Dictionary<string, string>();
            var pattern = "^" + routePattern.Replace("{id}", "(?<_id>.*)").Replace("/", "\\/") + "$";

            var result = Regex.Match(request.ResourcePath, pattern);
            if (result.Groups["_id"].Success)
                parameters["_id"] = result.Groups["_id"].Captures[0].Value;

            var UrlParams = request.ResourcePath.Split(new char[]{ '?', '&'}).ToList();
            UrlParams.RemoveAt(0);
            if(UrlParams.Count > 0)
                foreach (var param in UrlParams)
                {
                    var keyValue = param.Split('=');
                    parameters[keyValue[0]] = keyValue[1];
                }

            return parameters;
        }
    }
}
