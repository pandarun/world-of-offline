'use strict';

angular.module('wooclientApp')
    .controller('MeetingCtrl', function ($scope, $resource) {

        console.log('MeetingCtrl');

        $scope.submit = function(){
            var meeting = $resource('meetings')

            var Meeting = $resource('/meeting/:id',
                { id:'@id'});

            var newMeeting = new Meeting({number:'0123'});
            newMeeting.description = data.description;
            newMeeting.startAt = data.startAt;
            newMeeting.endAt = data.endAt;
            newMeeting.$save()

        }
    });
