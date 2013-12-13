'use strict';

describe('Controller: MeetingCtrl', function () {

  // load the controller's module
  beforeEach(module('wooclientApp'));

  var MeetingCtrl,
    scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    MeetingCtrl = $controller('MeetingCtrl', {
      $scope: scope
    });
  }));

  it('should attach a list of awesomeThings to the scope', function () {
    expect(scope.awesomeThings.length).toBe(3);
  });
});
