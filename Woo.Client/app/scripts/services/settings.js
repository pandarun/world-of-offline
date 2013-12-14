'use strict';

angular.module('wooclientApp')
    .factory('settings', function () {
        // stores auth token
        var   token  = null;

        // Public API here
        return {
            token: token
        }

    });
