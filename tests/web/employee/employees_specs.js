var mock = require('../setupMocks/mock');
var employeesPage = require('./employees_page');
var employeesMock = require('./employees_mock');

var ptor;

beforeEach(function() {
    ptor = protractor.getInstance();        
});

describe("On the 'employees list' page", function () {

    it('when I navigate to the page I see a list of the current employees sorted alphabetically', function () {

        ptor.addMockModule('httpBackEndMock', mock.build([employeesMock.getTwoEmployees]));

        employeesPage.navigate();

        employeesPage.expectEmployeeCountToBe(2);

        employeesPage.expectEmployeeNameAt(0, "Donald Trump");
        employeesPage.expectEmployeeEmailAt(0, "rich@gmail.com");

        employeesPage.expectEmployeeNameAt(1, "Mick Jagger");
        employeesPage.expectEmployeeEmailAt(1, "rollingstones@gmail.com");

    });
});