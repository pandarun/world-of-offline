'use strict';

angular.module('wooclientApp')
    .controller('MeetingCtrl', function ($scope, resourceFactory) {

        $scope.data = {};

        $scope.submit = function(){

            var newMeeting = {};

            newMeeting.since = $scope.data.startAt;
            newMeeting.to = $scope.data.endAt;
            newMeeting.summary = $scope.data.description;

            resourceFactory('event').save(newMeeting)

        }
    });
