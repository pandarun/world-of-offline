'use strict';

angular.module('wooclientApp')
  .controller('MeetingDetailsCtrl', function ($scope,meeting) {

    $scope.meeting = meeting;
  });
