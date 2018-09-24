
// State:

var operatorViewModel = {
    clients: ko.observableArray()
}

var serviceUrl = "http://localhost/qms.service/api/";

// Behavior:

$(function () {
    $.ajax(serviceUrl + "queue?count=20", {
        type: "GET",
        success: function (data) {
            //debugger;
            UpdateViewModel(data);
            //ko.applyBindings(operatorViewModel);
        },
        error: function () {
            //debugger;
            $("#connectionStatus").html("Не удалось установить соединение с сервисом.");
        },
        dataType: "json",
        crossDomain: true
    });
});

// Заменяет содержимое ViewModel целиком, 
// без реакции на каждое изменение:
function UpdateViewModel(queue) {
    //debugger;

    var temp = operatorViewModel.clients;

    operatorViewModel.clients.valueWillMutate();

    temp.removeAll();
    ko.utils.arrayPushAll(temp(), queue);

    /*
    $.each(queue, function(index, item) {
        clients.push(ko.observable(item));
    });
    */

    operatorViewModel.clients.valueHasMutated();
}

ko.applyBindings(operatorViewModel);