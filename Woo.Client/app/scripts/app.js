'use strict';

angular.module('wooclientApp', [
  'ui.router'
])
  .config(function ($stateProvider, $urlRouterProvider,$provide,$httpProvider) {
        // For any unmatched url, redirect to /main
        $urlRouterProvider.otherwise("/main");
        //
        // Now set up the states
        $stateProvider
            .state('main', {
                abstract : true,
                url: "/main",
                templateUrl: "views/main.html",
                controller: 'MainCtrl',
                resolve : {
                    meetings: ['$q','Meetings',function($q,Meetings){

                        var deferred = $q.defer();

                        var meetings = Meetings.getMeetings();

                        deferred.resolve(meetings);


                        return deferred.promise;
                    }],

                    user : ['$q','user',function($q,user){

                        var deferred = $q.defer();

                        var authorized_user = user.authorize();

                        deferred.resolve(authorized_user);


                        return deferred.promise;
                    }]

                }
            })
            .state('main.list', {
                url: "",
                templateUrl: "views/meeting.list.html"
            })
            .state('user', {
                url: "/user",
                abstract : true,
                templateUrl: "views/user.html",
                controller: 'UserCtrl',
                resolve : {
                    meetings: ['$q','Meetings',function($q,Meetings){

                        var deferred = $q.defer();

                        var meetings = Meetings.getMeetings();

                        deferred.resolve(meetings);


                        return deferred.promise;
                    }],

                    user : ['$q','user',function($q,user){

                        var deferred = $q.defer();

                        var authorized_user = user.authorize();

                        deferred.resolve(authorized_user);


                        return deferred.promise;
                    }]

                }
            })
            .state('user.list',{
                url : '',
                templateUrl: "views/meeting.list.html"
            })
            .state('meeting', {
                url: "/meetings/:id",
                templateUrl: "views/meeting.details.html",
                controller: 'MeetingCtrl'
            });

        // auth interceptor config

        $provide.factory('httpErrorInterceptor', ['$q', 'settings', function($q, settings,logger) {
            return {
                // optional method
                'request': function(config) {


                    // do something on success
                    if (settings.token) {
                        config.headers['X-Auth-Token'] = settings.token;
                    }

                    return config || $q.when(config);
                },

                'requestError': function(rejection) {
                    logger.Error(JSON.stringify(rejection));
                    // do something on error
                    return $q.reject(rejection);
                },
                'responseError': function(rejection) {
                    logger.Error(JSON.stringify(rejection));
                    // do something on error
                    return $q.reject(rejection);
                }
            };
        }]);

        $httpProvider.interceptors.push('httpErrorInterceptor');
        $httpProvider.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';

  });
