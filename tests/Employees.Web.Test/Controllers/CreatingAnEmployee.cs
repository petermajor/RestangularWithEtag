namespace Employees.Web.Test.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;

    using Employees.Web.Controllers;
    using Employees.Web.Models;
    using Employees.Web.Repositories;
    using Employees.Web.Results;

    using FluentAssertions;

    using NSubstitute;

    using Xunit;

    public class CreatingAnEmployee
    {
        private const string EmployeeId = "Id1";

        private readonly EmployeeController controller;

        private readonly IEmployeeRepository repository;

        private IHttpActionResult response;

        public CreatingAnEmployee()
        {
            repository = Substitute.For<IEmployeeRepository>();
            repository.When(r => r.Create(Arg.Any<Employee>())).Do(
                a =>
                {
                    var e = (Employee)a[0];
                    e.Id = EmployeeId;
                });

            controller = new EmployeeController(repository);
        }

        [Fact]
        public void WhenAnEmployeeIsCreatedThenTheEmployeeIsAddedToTheRepository()
        {
            WhenAnEmployeeIsCreated();

            ThenTheEmployeeIsAddedToTheRepository();
        }

        [Fact]
        public void WhenAnEmployeeIsCreatedThenTheEmployeeIsReturned()
        {
            WhenAnEmployeeIsCreated();

            ThenTheEmployeeIsReturned();
        }

        private void WhenAnEmployeeIsCreated()
        {
            response = controller.Create(new Employee { Name = "Patrick Stewart", Email = "pat@gmail.com" });
        }

        private void ThenTheEmployeeIsAddedToTheRepository()
        {
            repository.Received(1).Create(Arg.Is<Employee>(e => e.Id == EmployeeId && e.Name == "Patrick Stewart" && e.Email == "pat@gmail.com"));
            repository.Received(1).Create(Arg.Any<Employee>());
        }

        private void ThenTheEmployeeIsReturned()
        {
            response.Should().BeOfType<CreatedAtRouteNegotiatedContentResultWithETag<Employee>>();

            var expectedEmployee = new Employee
            {
                Id = EmployeeId,
                Name = "Patrick Stewart",
                Email = "pat@gmail.com"
            };

            var result = (CreatedAtRouteNegotiatedContentResultWithETag<Employee>)response;
            
            result.ETag.Should().Be(expectedEmployee.GenerateETag());

            result.RouteName.Should().Be("GetById");
            result.RouteValues.ShouldAllBeEquivalentTo(new Dictionary<string, object> { { "id", EmployeeId } });

            var actualEmployee = result.Content;
            actualEmployee.ShouldBeEquivalentTo(expectedEmployee);
        }
    }
}
