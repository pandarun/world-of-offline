'use strict';

angular.module('wooclientApp')
  .factory('user', function () {
    // Service logic
    // ...

    var user = {
        name : "Stanislav Chernykh",
        imageSrc : "https://s3.amazonaws.com/media.jetstrap.com/sw33HcufSAC2LtOmMHMI_photo.jpg"
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
