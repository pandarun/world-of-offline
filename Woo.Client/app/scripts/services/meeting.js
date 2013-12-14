'use strict';

angular.module('wooclientApp')
    .factory('Meetings', function () {


        var meetings = [
            {
                id : 1,
                description: "Тематическая вечеринка в Парке Горького, приглашаются все желающие, приводите своих друзей и знакомых! Будет весело!",
                isVisited : true,
                participants:[{
                        userId : 1,
                        name : "Алена",
                        imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/hcCQUHyLT9OiUpuczDfT_Young_Peul_girl_in_Mali.jpg",
                        info : "Пляж, бутик, салон красоты, пляж .."
                    },
                    {
                        userId : 2,
                        name : "Кристи",
                        imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/Zn2Y2EK1QZmc5QagBGbf_Lara_and_the_mystery_of_the_vanishing_hair",
                        info : "Яркая, целеустремленная девушка"
                    },
                    {
                        userId : 4,
                        name : "Макс",
                        imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/NMQQhMLwSmOQr7F2l6Sv_Milton_Neves_na_Cidade_do_Galo_30.04.2010",
                        info : "Любит кататься на роликах"
                    },
                    {
                        userId : 5,
                        name : "Дима",
                        imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/YgX3bL6lRiGSKrSWMO14_Ivan",
                        info : "Бокс. Мне нет равных"
                    }]
            },
            {
                id : 2,
                description: "Схожу в кино с девушкой, желательно на комедию.",
                isApproved : true,
                participants:[{
                    userId : 1,
                    name : "Алена",
                    imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/hcCQUHyLT9OiUpuczDfT_Young_Peul_girl_in_Mali.jpg",
                    info : "Пляж, бутик, салон красоты, пляж .."
                },
                    {
                        userId : 2,
                        name : "Кристи",
                        imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/Zn2Y2EK1QZmc5QagBGbf_Lara_and_the_mystery_of_the_vanishing_hair",
                        info : "Яркая, целеустремленная девушка"
                    },
                    {
                        userId : 4,
                        name : "Макс",
                        imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/NMQQhMLwSmOQr7F2l6Sv_Milton_Neves_na_Cidade_do_Galo_30.04.2010",
                        info : "Любит кататься на роликах"
                    },
                    {
                        userId : 5,
                        name : "Дима",
                        imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/YgX3bL6lRiGSKrSWMO14_Ivan",
                        info : "Бокс. Мне нет равных"
                    }]

            },
            {
                id : 3,
                description: "Хоккей в Сокольниках, приглашается команда в соперники. Не забудьте взять шайбу, а то будет как в прошлый раз :)).",
                isRequest : true,
                participants:[{
                    userId : 1,
                    name : "Алена",
                    imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/hcCQUHyLT9OiUpuczDfT_Young_Peul_girl_in_Mali.jpg",
                    info : "Пляж, бутик, салон красоты, пляж .."
                },
                    {
                        userId : 2,
                        name : "Кристи",
                        imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/Zn2Y2EK1QZmc5QagBGbf_Lara_and_the_mystery_of_the_vanishing_hair",
                        info : "Яркая, целеустремленная девушка"
                    },
                    {
                        userId : 4,
                        name : "Макс",
                        imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/NMQQhMLwSmOQr7F2l6Sv_Milton_Neves_na_Cidade_do_Galo_30.04.2010",
                        info : "Любит кататься на роликах"
                    },
                    {
                        userId : 5,
                        name : "Дима",
                        imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/YgX3bL6lRiGSKrSWMO14_Ivan",
                        info : "Бокс. Мне нет равных"
                    }]
            },
            {
                id : 4,
                description: "Хоккей в Сокольниках, приглашается команда в соперники. Не забудьте взять шайбу, а то будет как в прошлый раз :)).",
                isApproved : true,
                participants:[{
                    userId : 1,
                    name : "Алена",
                    imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/hcCQUHyLT9OiUpuczDfT_Young_Peul_girl_in_Mali.jpg",
                    info : "Пляж, бутик, салон красоты, пляж .."
                },
                    {
                        userId : 2,
                        name : "Кристи",
                        imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/Zn2Y2EK1QZmc5QagBGbf_Lara_and_the_mystery_of_the_vanishing_hair",
                        info : "Яркая, целеустремленная девушка"
                    },
                    {
                        userId : 4,
                        name : "Макс",
                        imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/NMQQhMLwSmOQr7F2l6Sv_Milton_Neves_na_Cidade_do_Galo_30.04.2010",
                        info : "Любит кататься на роликах"
                    },
                    {
                        userId : 5,
                        name : "Дима",
                        imageSrc:"https://s3.amazonaws.com/media.jetstrap.com/YgX3bL6lRiGSKrSWMO14_Ivan",
                        info : "Бокс. Мне нет равных"
                    }]
            }
        ];

        function getMeetings(){
            return meetings;
        }

        function getMeetingById(id){
            return getMeetings()[id];
        }


        // Public API here
        return {
            getMeetings:getMeetings,
            getMeetingById : getMeetingById
        };
    });
