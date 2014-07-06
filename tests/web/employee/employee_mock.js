module.exports.getEmployeeDonaldTrump = function ($httpBackend) {

    var employee = { Id: 2, Name: "Donald Trump", Email: "rich@gmail.com" };

    $httpBackend.whenGET('/api/employees/2').respond(employee);
};
