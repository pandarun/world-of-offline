'use strict';

angular.module('wooclientApp')
  .factory('user', function () {
    // Service logic
    // ...

    var user = {
        name : "DimaN",
        imageSrc : "data:image/jpeg;base64,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"
    }

    function authorize(){

            var authorized_user = user;
            return authorized_user;

    }

    // Public API here
    return {
      authorize:authorize
    };
  });
