namespace Employees.Web.Test.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Results;

    using Employees.Web.Controllers;
    using Employees.Web.Models;
    using Employees.Web.Repositories;

    using FluentAssertions;

    using NSubstitute;

    using Xunit;

    public class UpdatingAnEmployee
    {
        private const string EmployeeId = "Id1";

        private readonly EmployeeController controller;

        private readonly IEmployeeRepository repository;

        private Employee employee;

        private IHttpActionResult response;

        public UpdatingAnEmployee()
        {
            repository = Substitute.For<IEmployeeRepository>();
            controller = new EmployeeController(repository);
        }

        [Fact]
        public void GivenAnEmployeeWhenAnotherEmployeeThatDoesNotExistIsUpdatedThenNotFoundIsReturned()
        {
            GivenAnEmployee();

            WhenAnotherEmployeeThatDoesNotExistIsUpdated();

            ThenNotFoundIsReturned();
        }

        [Fact]
        public void GivenAnEmployeeWhenTheEmployeeIsUpdatedAndEtagDoesntMatchThenPreconditionFailedIsReturned()
        {
            GivenAnEmployee();

            WhenTheEmployeeIsUpdatedAndEtagDoesntMatch();

            ThenPreconditionFailedIsReturned();
        }

        [Fact]
        public void GivenAnEmployeeWhenTheEmployeeIsUpdatedAndTheIdInTheEmployeeDoesNotMatchThenBadRequestIsReturned()
        {
            GivenAnEmployee();

            WhenTheEmployeeIsUpdatedAndTheIdInTheEmployeeDoesNotMatch();

            ThenBadRequestIsReturned();
        }

        [Fact]
        public void GivenAnEmployeeWhenTheEmployeeIsUpdatedAndEtagDoesMatchThenEmployeeIsUpdatedInRepository()
        {
            GivenAnEmployee();

            WhenTheEmployeeIsUpdatedAndEtagDoesMatch();

            ThenEmployeeIsUpdatedInRepository();
        }

        private void GivenAnEmployee()
        {
            employee = new Employee
            {
                Id = EmployeeId,
                Name = "Patrick Stewart",
                Email = "pat@gmail.com"
            };

            repository.GetById(EmployeeId).Returns(employee);
        }

        private void WhenTheEmployeeIsUpdatedAndEtagDoesntMatch()
        {
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("If-Match", "\"xyz\"");
            response = controller.Update(EmployeeId, new Employee { Id = EmployeeId, Name = "Patrick Stewart", Email = "patty.s@gmail.com" });
        }

        private void WhenTheEmployeeIsUpdatedAndEtagDoesMatch()
        {
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("If-Match", employee.GenerateETag());
            response = controller.Update(EmployeeId, new Employee { Id = EmployeeId, Name = "Patrick Stewart", Email = "patty.s@gmail.com" });
        }

        private void WhenAnotherEmployeeThatDoesNotExistIsUpdated()
        {
            response = controller.Update("xyz", new Employee { Id = "xyz" });
        }

        private void WhenTheEmployeeIsUpdatedAndTheIdInTheEmployeeDoesNotMatch()
        {
            response = controller.Update(EmployeeId, new Employee { Id = "xyz", Name = "Patrick Stewart", Email = "patty.s@gmail.com" });
        }

        private void ThenNotFoundIsReturned()
        {
            response.Should().BeOfType<NotFoundResult>();
        }

        private void ThenBadRequestIsReturned()
        {
            response.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        private void ThenPreconditionFailedIsReturned()
        {
            response.Should().BeOfType<StatusCodeResult>();

            ((StatusCodeResult)response).StatusCode.Should().Be(HttpStatusCode.PreconditionFailed);
        }

        private void ThenEmployeeIsUpdatedInRepository()
        {
            repository.Received(1).Update(Arg.Is<Employee>(e => e.Id == EmployeeId && e.Name == "Patrick Stewart" && e.Email == "patty.s@gmail.com"));
            repository.Received(1).Update(Arg.Any<Employee>());
        }
    }
}
