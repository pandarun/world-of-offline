'use strict';

angular.module('wooclientApp')
  .controller('MainCtrl', function ($scope,meetings) {
    $scope.meetings = meetings;
  });
