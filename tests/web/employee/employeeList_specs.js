var mock = require('../setupMocks/mock');
var employeeListPage = require('./employeeList_page');
var employeeListMock = require('./employeeList_mock');

var ptor;

beforeEach(function() {
    ptor = protractor.getInstance();        
});

describe("On the 'employee list' page", function () {

    it('when I navigate to the page I see a list of the current employees sorted alphabetically', function () {

        ptor.addMockModule('httpBackEndMock', mock.build([employeeListMock.getTwoEmployees]));

        employeeListPage.navigate();

        employeeListPage.expectEmployeeCountToBe(2);

        employeeListPage.expectEmployeeNameAt(0, "Donald Trump");
        employeeListPage.expectEmployeeEmailAt(0, "rich@gmail.com");

        employeeListPage.expectEmployeeNameAt(1, "Mick Jagger");
        employeeListPage.expectEmployeeEmailAt(1, "rollingstones@gmail.com");

    });
});