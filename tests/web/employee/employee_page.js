EmployeePage = function () { 

    this.navigateNew = function() {
        browser.get('/');
    };

    this.navigateEdit = function(id) {
        browser.get('/employee?id=' + id);
    };

    this.nameLocator = by.css('#nameInput');
    this.emailLocator = by.css('#emailInput');
    this.saveLocator = by.css('#save');
    this.modalLocator = by.css('.modal');
    this.modalOkLocator = by.css('#modalOk');

    this.setName = function(name) {
        var input = element(this.nameLocator);
        input.clear();
        input.sendKeys(name)
    }

    this.clickSaveButton = function() {
        var button = element(this.saveLocator);
        button.click();
    }

    this.clickModalOkButton = function() {
        var button = element(this.modalOkLocator);
        button.click();
    }

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

    this.expectModalIsShowing = function() {
        var modal = element(this.modalLocator);
        expect(modal.isPresent()).toBe(true);
    }

    this.expectModalIsNotShowing = function() {
        var modal = element(this.modalLocator);
        expect(modal.isPresent()).toBe(false);
    }
};

module.exports = new EmployeePage();