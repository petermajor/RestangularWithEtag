namespace Employees.Web.Test.Controllers
{
    using System.Web.Http;
    using System.Web.Http.Results;

    using Employees.Web.Controllers;
    using Employees.Web.Models;
    using Employees.Web.Repositories;
    using Employees.Web.Results;

    using FluentAssertions;

    using NSubstitute;

    using Xunit;

    public class GettingAnEmployee
    {
        private const string EmployeeId = "Id1";

        private readonly EmployeeController controller;

        private readonly IEmployeeRepository repository;

        private Employee employee;

        private IHttpActionResult response;

        public GettingAnEmployee()
        {
            repository = Substitute.For<IEmployeeRepository>();
            controller = new EmployeeController(repository);
        }

        [Fact]
        public void GivenAnEmployeeWhenGetByIdIsCalledThenTheEmployeeIsReturned()
        {
            GivenAnEmployee();

            WhenGetByIdIsCalled();

            ThenTheEmployeeIsReturned();
        }

        [Fact]
        public void GivenAnEmployeeWhenGetByIdIsCalledForADifferentEmployeeThenNotFoundIsReturned()
        {
            GivenAnEmployee();

            WhenGetByIdIsCalledForADifferentEmployee();

            ThenNotFoundIsReturned();
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

        private void WhenGetByIdIsCalled()
        {
            response = controller.GetById(EmployeeId);
        }

        private void WhenGetByIdIsCalledForADifferentEmployee()
        {
            response = controller.GetById("xyz");
        }

        private void ThenTheEmployeeIsReturned()
        {
            response.Should().BeOfType<OkNegotiatedContentWithETagResult<Employee>>();

            var expectedEmployee = new Employee
            {
                Id = EmployeeId,
                Name = "Patrick Stewart",
                Email = "pat@gmail.com"
            };

            var result = (OkNegotiatedContentWithETagResult<Employee>)response;
            result.ETag.Should().Be(expectedEmployee.GenerateETag());

            var actualEmployee = result.Content;
            actualEmployee.ShouldBeEquivalentTo(expectedEmployee);
        }

        private void ThenNotFoundIsReturned()
        {
            response.Should().BeOfType<NotFoundResult>();
        }
    }
}