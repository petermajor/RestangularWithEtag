namespace Employees.Web.Repositories
{
    using System.Collections.Generic;
    using System.Threading;

    using Employees.Web.Models;

    public class EmployeeRepository : IEmployeeRepository
    {
        private static readonly Dictionary<string, Employee> Employees;

        private static int nextId;

        static EmployeeRepository()
        {
            Employees = new Dictionary<string, Employee>
                            {
                                { "1", new Employee { Id = "1", Name = "Robert Smith", Email = "Robert.Smith@gmail.com" } },
                                { "2", new Employee { Id = "2", Name = "Tom Jones", Email = "Tom.Jones@hotmail.com" } },
                            };
            nextId = 3;
        }

        public IEnumerable<Employee> Get()
        {
            return Employees.Values;
        }

        public Employee GetById(string id)
        {
            Employee employee;
            Employees.TryGetValue(id, out employee);
            return employee;
        }

        public void Create(Employee employee)
        {
            var id = Interlocked.Increment(ref nextId);

            employee.Id = id.ToString();

            Employees.Add(employee.Id, employee);
        }

        public void Update(Employee employee)
        {
            Employees[employee.Id] = employee;
        }
    }
}