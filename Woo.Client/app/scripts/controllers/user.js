'use strict';

angular.module('wooclientApp')
  .controller('UserCtrl', function ($scope,meetings, user) {
    $scope.meetings = meetings;
        $scope.user = user;

    });
