namespace Employees.Web.Results
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;

    public class CreatedAtRouteNegotiatedContentResultWithETag<T> : CreatedAtRouteNegotiatedContentResult<T>
    {
        public CreatedAtRouteNegotiatedContentResultWithETag(string routeName, IDictionary<string, object> routeValues, T content, string etag, ApiController controller)
            : base(routeName, routeValues, content, controller)
        {
            ETag = etag;
        }

        public string ETag { get; private set; }

        public override Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var task = base.ExecuteAsync(cancellationToken);

            return task.ContinueWith<HttpResponseMessage>(AddETagHeader, cancellationToken);
        }

        private HttpResponseMessage AddETagHeader(Task<HttpResponseMessage> task)
        {
            task.Result.Headers.ETag = new EntityTagHeaderValue(ETag);
            return task.Result;
        }
    }
}