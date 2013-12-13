'use strict';

angular.module('wooclientApp')
  .controller('UserCtrl', function ($scope,meetings) {
    $scope.meetings = meetings;
  });
