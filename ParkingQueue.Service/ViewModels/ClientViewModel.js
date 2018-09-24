
// State:

var clientViewModel = {
    clients: ko.observableArray()
}

// Behavior:

// Заменяет содержимое ViewModel целиком, 
// без реакции на каждое изменение:
function UpdateViewModel(queue) {
    //debugger;

    var temp = clientViewModel.clients;

    clientViewModel.clients.valueWillMutate();

    temp.removeAll();
    ko.utils.arrayPushAll(temp(), queue);

    /*
    $.each(queue, function(index, item) {
        clients.push(ko.observable(item));
    });
    */

    clientViewModel.clients.valueHasMutated();
}

ko.applyBindings(clientViewModel);