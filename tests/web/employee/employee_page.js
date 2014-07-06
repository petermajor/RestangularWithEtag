EmployeePage = function () { 

    this.navigateNew = function() {
        browser.get('/');
    };

    this.navigateEdit = function(id) {
        browser.get('/employee?id=' + id);
    };

    this.nameLocator = by.css('#nameInput');
    this.emailLocator = by.css('#emailInput');

    this.expectNameToBe = function(exectedName) {
        var input = element(this.nameLocator);
        var name = input.getAttribute("value");
        expect(name).toEqual(exectedName);
    }

    this.expectEmailToBe = function(exectedEmail) {
        var input = element(this.emailLocator);
        var email = input.getAttribute("value");
        expect(email).toEqual(exectedEmail);
    }
};

module.exports = new EmployeePage();