var mock = require('../setupMocks/mock');
var employeePage = require('./employee_page');
var employeeMock = require('./employee_mock');

var ptor;

beforeEach(function() {
    ptor = protractor.getInstance();        
});

describe("On the 'employee' page", function () {

    it('when I navigate to an existing employee, I see the employee detail in the form', function () {

        ptor.addMockModule('httpBackEndMock', mock.build([employeeMock.getEmployeeDonaldTrump]));

        employeePage.navigateEdit("2");

        employeePage.expectNameToBe("Donald Trump");
        employeePage.expectEmailToBe("rich@gmail.com");
    });
});