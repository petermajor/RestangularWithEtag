function passThrough($httpBackend) {
    $httpBackend.whenGET(/^\/scripts\//).passThrough();
};

module.exports.build = function(funcs) {
	var funcStr = "angular.module('httpBackEndMock', ['ngMockE2E'])";

    if (Array.isArray(funcs)) {
    	for (var i = 0; i < funcs.length; i++) {
            verifyFunc(funcs[i]);
    		funcStr += "\r.run(" + funcs[i] + ")"
    	};
    } else {
        verifyFunc(funcs)
  		funcStr += "\r.run(" + funcs + ")"
    }

    funcStr += "\r.run(" + passThrough + ")";

    var funcTyped = Function(funcStr);

    //console.log(funcTyped.toString())
    return funcTyped;
}

function verifyFunc(func) {
    if (!func) {
        throw "function is not defined";
    }
}

module.exports.verifyNoOutstandingExpectation = function () {
    var message = browser.executeAsyncScript(verifyNoOutstandingExpectationFunction);
    expect(message).toBeNull();
};

var verifyNoOutstandingExpectationFunction = function(callback) {
    var $httpBackend = angular.element('body').injector().get('$httpBackend');
    try {
        $httpBackend.verifyNoOutstandingExpectation();
        callback(null);
    } catch (err) {
        callback(err.message);
    }
};
