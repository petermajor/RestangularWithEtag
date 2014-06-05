namespace Employees.Web.Repositories
{
    using System.Collections.Generic;

    using Employees.Web.Models;

    public interface IEmployeeRepository
    {
        IEnumerable<Employee> Get();

        Employee GetById(string id);

        void Create(Employee employee);

        void Update(Employee employee);
    }
}