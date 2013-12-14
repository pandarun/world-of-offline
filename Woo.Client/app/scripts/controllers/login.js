'use strict';

angular.module('wooclientApp')
  .controller('LoginCtrl', function ($scope,settings,$http,$rootScope) {

        // auth events
        $rootScope.$on('$stateChangeStart',
            function(event, toState, toParams, fromState, fromParams){
                event.preventDefault();
                // transitionTo() promise will be rejected with
                // a 'transition prevented' error

                var fb_url = 'http://woohoo.azurewebsites.net/api/Account/ExternalLogin?provider=Facebook&response_type=token&client_id=self&redirect_uri=http%3A%2F%2Fwoohoo.azurewebsites.net%2F&state=MbAsq3dbxNE6I';

                if(settings && !settings.token){
                    $http.get(fb_url)
                        .then(function(results){

                            var relative = results[0].url;
                            //$rootScope.url = "http://woohoo.azurewebsites.net" + relative;
                            $rootScope.url = fb_url;

                        });
                }

            })



        $rootScope.$on("url", function(item){
            if(item){
                var parsed = '/http://woohoo.azurewebsites.net/Token/Index?token=(*)[#_=_]*/'.exec(item);
                settings.token =  parsed[1];
            }
        });
  });
