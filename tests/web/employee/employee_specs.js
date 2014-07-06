var mock = require('../setupMocks/mock');
var employeePage = require('./employee_page');
var employeeMock = require('./employee_mock');

var ptor;

beforeEach(function() {
    ptor = protractor.getInstance();        
});

describe("On the edit 'employee' page", function () {

    it('when I navigate to an employee then I see the employee details in the page', function () {

        ptor.addMockModule('httpBackEndMock', mock.build([employeeMock.getEmployeeDonaldTrump]));

        employeePage.navigateEdit("2");

        employeePage.expectNameToBe("Donald Trump");
        employeePage.expectEmailToBe("rich@gmail.com");
    });

    it("when I change the name and click 'save' then the changes are saved", function () {

        ptor.addMockModule('httpBackEndMock', mock.build([employeeMock.getEmployeeDonaldTrump, employeeMock.expectPutDonnieTrump]));

        employeePage.navigateEdit("2");

        employeePage.setName("Donnie Trump");

        employeePage.clickSaveButton();

        mock.verifyNoOutstandingExpectation();
    });

    it("when I attempt to change an employee that has been changed by someone else then I see a error message and the changes are not saved", function () {

        ptor.addMockModule('httpBackEndMock', mock.build([employeeMock.getEmployeeDonaldTrump, employeeMock.expectPutConcurrencyError]));

        employeePage.navigateEdit("2");

        employeePage.setName("Donnie Trump");

        employeePage.clickSaveButton();

        employeePage.expectModalIsShowing();

        employeePage.clickModalOkButton();

        employeePage.expectModalIsNotShowing();

        mock.verifyNoOutstandingExpectation();
    });
});