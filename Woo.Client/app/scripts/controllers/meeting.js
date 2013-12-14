'use strict';

angular.module('wooclientApp')
    .controller('MeetingCtrl', function ($scope, resourceFactory) {

        $scope.data = {};

        $scope.submit = function(){

            var newMeeting = {
                since : $scope.data.startAt,
                to : $scope.data.endAt,
                summary : $scope.data.description
            };

            resourceFactory('event').save(newMeeting)

        }
    });
