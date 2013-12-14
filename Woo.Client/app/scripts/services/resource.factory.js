'use strict';

angular.module('wooclientApp')
  .factory('resourceFactory', function ($resource) {



    // Public API here
    return function(id) {

          switch(id){
              case 'event':
              {

                  return $resource('/api/event', {})
              }
          }

      }
  });
