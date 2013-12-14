'use strict';

describe('Service: ResourceFactory', function () {

  // load the service's module
  beforeEach(module('wooclientApp'));

  // instantiate service
  var ResourceFactory;
  beforeEach(inject(function (_ResourceFactory_) {
    ResourceFactory = _ResourceFactory_;
  }));

  it('should do something', function () {
    expect(!!ResourceFactory).toBe(true);
  });

});
