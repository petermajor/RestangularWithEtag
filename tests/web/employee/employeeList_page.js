EmployeesPage = function () { 

    this.navigate = function() {
        browser.get('/');
    };

    this.employeesLocator = by.repeater('employee in employees');

    this.getEmployeeAt = function(index) {
		return this.employeesLocator.row(index);
    }

    this.getEmployeeName = function(employee) {
		var e = element(employee.column('employee.Name'));
		return e.getText();
    }

    this.getEmployeeEmail = function(employee) {
		var e = element(employee.column('employee.Email'));
		return e.getText();
    }

    this.expectEmployeeCountToBe = function(expectedCount) {
    	var employees = element.all(this.employeesLocator);
    	expect(employees.count()).toBe(expectedCount);
    }

    this.expectEmployeeNameAt = function(index, exectedName) {
    	var employee = this.getEmployeeAt(index);
    	var name = this.getEmployeeName(employee);
    	expect(name).toEqual(exectedName);
    }

    this.expectEmployeeEmailAt = function(index, exectedEmail) {
    	var employee = this.getEmployeeAt(index);
    	var email = this.getEmployeeEmail(employee);
    	expect(email).toEqual(exectedEmail);
    }
};

module.exports = new EmployeesPage();