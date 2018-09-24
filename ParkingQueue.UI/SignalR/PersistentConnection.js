//debugger;

// адрес хаба:
var connection = $.hubConnection();
    connection.url = "/qms.service/signalr";
    //connection.url = "/ParkingQueue.Service/signalr";

// подключение к хабу:
connection.start({ jsonp: true, xdomain: true })
    //.done(function () { $("#connectionStatus").html("Connection succeeded"); })
    .fail(function () {
        $("#connectionStatus").html("Не удалось установить соединение с сервисом.");
    });

// создание прокси:
var proxy = connection.createHubProxy('queueHub');

// реакция на входящее событие прокси:
proxy.on('queueChanged', function (queue, reason, rotation) {
    //debugger;

    UpdateViewModel($.parseJSON(queue));

    timerSeconds = rotation;

    $("#player")[0].play();
});

var timerSeconds = 0;
var timerId;

// запуск таймера обратного отсчёта:
$(function () {
    timerId = setInterval(function() {
        //debugger;

        $("connectionStatus").html($.now());

        if (timerSeconds > 0) {
            //debugger;
            timerSeconds = timerSeconds - 1;

            if (timerSeconds === 0) {
                //debugger;
                $("#timer").html("");
                return;
            }

            var m = Math.floor(timerSeconds / 60);
            var s = timerSeconds % 60;

            var mString = ("0" + m).substring(0, 2);
            var sString = ("0" + s).substring(0, 2);

            var timerString = mString + ":" + sString;

            $("#timer").html(timerString);
        }
    }, 1000);
});
