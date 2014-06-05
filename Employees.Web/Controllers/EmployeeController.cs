namespace Employees.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Routing;

    using Employees.Web.Models;
    using Employees.Web.Repositories;
    using Employees.Web.Results;

    [RoutePrefix("api/employees")]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeRepository repository;

        public EmployeeController()
            : this(new EmployeeRepository())
        {
        }

        public EmployeeController(IEmployeeRepository repository)
        {
            this.repository = repository;
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

            return OkWithETag(employee);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(Employee employee)
        {
            repository.Create(employee);

            return CreatedAtRouteWithETag("GetById", new { id = employee.Id }, employee);
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(string id, Employee employee)
        {
            if (!string.Equals(employee.Id, id, StringComparison.Ordinal))
            {
                return BadRequest("Employee Id in resource and URL must match.");
            }

            var existingEmployee = repository.GetById(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            var etag = GetETagFromRequest(Request);
            var existingETag = existingEmployee.GenerateETag();
            if (!string.Equals(existingETag, etag, StringComparison.Ordinal))
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }

            repository.Update(employee);

            return OkWithETag(employee);
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

        private IHttpActionResult CreatedAtRouteWithETag<T>(string routeName, object routeValues, T content)
        {
            return new CreatedAtRouteNegotiatedContentResultWithETag<T>(routeName, new HttpRouteValueDictionary(routeValues), content, content.GenerateETag(), this);
        }

        private IHttpActionResult OkWithETag<T>(T content)
        {
            return new OkNegotiatedContentWithETagResult<T>(content, content.GenerateETag(), this);
        }
    }
}
