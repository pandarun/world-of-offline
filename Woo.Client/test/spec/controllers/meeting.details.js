'use strict';

describe('Controller: MeetingDetailsCtrl', function () {

  // load the controller's module
  beforeEach(module('wooclientApp'));

  var MeetingDetailsCtrl,
    scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    MeetingDetailsCtrl = $controller('MeetingDetailsCtrl', {
      $scope: scope
    });
  }));

  it('should attach a list of awesomeThings to the scope', function () {
    expect(scope.awesomeThings.length).toBe(3);
  });
});
