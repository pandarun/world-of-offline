'use strict';

angular.module('wooclientApp')
  .controller('MainCtrl', function ($scope,meetings, user) {

    $scope.token = settings.token;


    $scope.meetings = meetings;

    $scope.user = user;
  });
