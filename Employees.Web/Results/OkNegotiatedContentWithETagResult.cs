namespace Employees.Web.Results
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;

    public class OkNegotiatedContentWithETagResult<T> : OkNegotiatedContentResult<T>
    {
        public OkNegotiatedContentWithETagResult(T content, string etag, ApiController controller)
            : base(content, controller)
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