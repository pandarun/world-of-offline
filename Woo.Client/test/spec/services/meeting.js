'use strict';

describe('Service: Meeting', function () {

  // load the service's module
  beforeEach(module('wooclientApp'));

  // instantiate service
  var Meeting;
  beforeEach(inject(function (_Meeting_) {
    Meeting = _Meeting_;
  }));

  it('should do something', function () {
    expect(!!Meeting).toBe(true);
  });

});
