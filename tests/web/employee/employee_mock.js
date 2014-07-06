module.exports.getEmployeeDonaldTrump = function ($httpBackend) {

    var employee = { Id: 2, Name: "Donald Trump", Email: "rich@gmail.com" };

    $httpBackend.whenGET('/api/employees/2').respond(employee);
};

module.exports.expectPutDonnieTrump = function ($httpBackend) {
    
    $httpBackend.whenPUT('/api/employees/2').respond(
    	function(method, url, data, headers) {
            return [200, data, {}];
        });

    $httpBackend.expectPUT('/api/employees/2', { Id : 2, Name : "Donnie Trump", Email : "rich@gmail.com" });

};

module.exports.expectPutConcurrencyError = function ($httpBackend) {
    
    $httpBackend.whenPUT('/api/employees/2').respond(412);

};
