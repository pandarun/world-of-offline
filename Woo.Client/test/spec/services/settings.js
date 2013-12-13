'use strict';

describe('Service: Settings', function () {

  // load the service's module
  beforeEach(module('wooclientApp'));

  // instantiate service
  var Settings;
  beforeEach(inject(function (_Settings_) {
    Settings = _Settings_;
  }));

  it('should do something', function () {
    expect(!!Settings).toBe(true);
  });

});
