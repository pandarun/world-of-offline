'use strict';

angular.module('wooclientApp')
  .controller('MeetingDetailsCtrl', function ($scope,meeting) {

    console.log('meeting.details');
    $scope.meeting = meeting;
  });
