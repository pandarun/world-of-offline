'use strict';

angular.module('wooclientApp')
  .controller('MainCtrl', function ($scope,meetings, user) {
    $scope.meetings = meetings;

    $scope.user = user;
  });
