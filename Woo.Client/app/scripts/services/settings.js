'use strict';

angular.module('wooclientApp')
    .factory('settings', function () {
        // stores auth token
        var   token  = {};

        // Public API here
        return {
            token: token
        }

    });
