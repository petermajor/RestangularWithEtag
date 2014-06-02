namespace Employees.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Web.Http;
    using System.Web.Http.Routing;

    using Employees.Web.Models;
    using Employees.Web.Repositories;
    using Employees.Web.Results;

    using Newtonsoft.Json;

    [RoutePrefix("api/employees")]
    public class EmployeeController : ApiController
    {
        private readonly EmployeeRepository repository;

        public EmployeeController()
        {
            repository = new EmployeeRepository();
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var employees = repository.Get();

            return Ok(employees);
        }

        [Route("{id}", Name = "GetById")]
        public IHttpActionResult GetById(string id)
        {
            var employee = repository.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            return OkWithETag(employee, GenerateETag(employee));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(Employee employee)
        {
            repository.Create(employee);

            return CreatedAtRouteWithETag("GetById", new { id = employee.Id }, employee, GenerateETag(employee));
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(string id, Employee employee)
        {
            if (!string.Equals(employee.Id, id, StringComparison.Ordinal))
            {
                return BadRequest("Transaction Id in resource and URL must match.");
            }

            var existingEmployee = repository.GetById(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            var etag = GetETagFromRequest(Request);
            var existingETag = GenerateETag(existingEmployee);
            if (!string.Equals(existingETag, etag, StringComparison.Ordinal))
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }

            repository.Update(employee);

            return OkWithETag(employee, GenerateETag(employee));
        }

        public string GetETagFromRequest(HttpRequestMessage request)
        {
            var ifMatchHeaders = request.Headers.IfMatch;
            if (ifMatchHeaders == null)
            {
                return null;
            }

            var ifMatchHeader = ifMatchHeaders.SingleOrDefault();
            if (ifMatchHeader == null)
            {
                return null;
            }

            return ifMatchHeader.Tag;
        }

        private static string GenerateETag(Employee obj)
        {
            var objJson = JsonConvert.SerializeObject(obj);
            var objJsonBytes = System.Text.Encoding.ASCII.GetBytes(objJson);

            var hashProvider = new MD5CryptoServiceProvider();
            var hash = hashProvider.ComputeHash(objJsonBytes);

            var etagString = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

            return string.Format("\"{0}\"", etagString);
        }

        private IHttpActionResult CreatedAtRouteWithETag<T>(string routeName, object routeValues, T content, string etag)
        {
            return new CreatedAtRouteNegotiatedContentResultWithETag<T>(routeName, new HttpRouteValueDictionary(routeValues), content, etag, this);
        }

        private IHttpActionResult OkWithETag<T>(T content, string etag)
        {
            return new OkNegotiatedContentWithETagResult<T>(content, etag, this);
        }
    }
}
