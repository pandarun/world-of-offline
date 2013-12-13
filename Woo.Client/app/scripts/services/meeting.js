'use strict';

angular.module('wooclientApp')
  .factory('Meetings', function () {





    var meetings = [
            {
            description: "Тематическая вечеринка в Парке Горького, приглашаются все желающие, приводите своих друзей и знакомых! Будет весело!",

            participants:[{
                userId : 1,
                name : "Вася",
                imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/hcCQUHyLT9OiUpuczDfT_Young_Peul_girl_in_Mali.jpg"
            },
            {
                userId : 2,
                name : "Петя",
                imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/Zn2Y2EK1QZmc5QagBGbf_Lara_and_the_mystery_of_the_vanishing_hair"

            },
            {
                userId : 4,
                name : "Маша",
                imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/NMQQhMLwSmOQr7F2l6Sv_Milton_Neves_na_Cidade_do_Galo_30.04.2010"

            },
            {
                userId : 5,
                name : "Даша",     
                imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/YgX3bL6lRiGSKrSWMO14_Ivan"
            }]
        },
        {
            description: "Схожу в кино с девушкой, желательно на комедию.",
            isApproved : true,
            participants:[{
                userId : 1,
                name : "Вася",
                imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/hcCQUHyLT9OiUpuczDfT_Young_Peul_girl_in_Mali.jpg"
            },
                {
                    userId : 2,
                    name : "Петя",
                    imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/Zn2Y2EK1QZmc5QagBGbf_Lara_and_the_mystery_of_the_vanishing_hair"

                },
                {
                    userId : 4,
                    name : "Маша",
                    imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/NMQQhMLwSmOQr7F2l6Sv_Milton_Neves_na_Cidade_do_Galo_30.04.2010"

                },
                {
                    userId : 5,
                    name : "Даша",
                    imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/YgX3bL6lRiGSKrSWMO14_Ivan"
                }]
            
        },
        {
            description: "Хоккей в Сокольниках, приглашается команда в соперники. Не забудьте взять шайбу, а то будет как в прошлый раз :)).",
            participants:[{
                userId : 1,
                name : "Вася",
                imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/hcCQUHyLT9OiUpuczDfT_Young_Peul_girl_in_Mali.jpg"
            },
                {
                    userId : 2,
                    name : "Петя",
                    imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/Zn2Y2EK1QZmc5QagBGbf_Lara_and_the_mystery_of_the_vanishing_hair"

                },
                {
                    userId : 4,
                    name : "Маша",
                    imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/NMQQhMLwSmOQr7F2l6Sv_Milton_Neves_na_Cidade_do_Galo_30.04.2010"

                },
                {
                    userId : 5,
                    name : "Даша",
                    imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/YgX3bL6lRiGSKrSWMO14_Ivan"
                }]
        },
        {
            description: "Хоккей в Сокольниках, приглашается команда в соперники. Не забудьте взять шайбу, а то будет как в прошлый раз :)).",
            isApproved : true,
            participants:[{
                userId : 1,
                name : "Вася",
                imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/hcCQUHyLT9OiUpuczDfT_Young_Peul_girl_in_Mali.jpg"
            },
                {
                    userId : 2,
                    name : "Петя",
                    imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/Zn2Y2EK1QZmc5QagBGbf_Lara_and_the_mystery_of_the_vanishing_hair"

                },
                {
                    userId : 4,
                    name : "Маша",
                    imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/NMQQhMLwSmOQr7F2l6Sv_Milton_Neves_na_Cidade_do_Galo_30.04.2010"

                },
                {
                    userId : 5,
                    name : "Даша",
                    imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/YgX3bL6lRiGSKrSWMO14_Ivan"
                }]
        }
    ];

    function getMeetings(){
        return meetings;
    }


    // Public API here
    return {
      getMeetings:getMeetings
    };
  });
