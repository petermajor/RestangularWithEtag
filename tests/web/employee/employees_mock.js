module.exports.getTwoEmployees = function ($httpBackend) {

    var employees = [
        { Id: 1, Name: "Mick Jagger", Email: "rollingstones@gmail.com" },
        { Id: 2, Name: "Donald Trump", Email: "rich@gmail.com" },
    ];
    $httpBackend.whenGET('/api/employees').respond(employees);
};
