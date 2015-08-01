/*
 * This service is used by other controllers to store messages that will be handled by MessageController. 
 */
proview.app.factory('MessageService', MessageService);

MessageService.$inject = ['$timeout'];

function MessageService($timeout) {

    var service = {
        add: add,
        closeMessage: closeMessage,
        closeMessageIdx: closeMessageIdx,
        clear: clear,
        get: get
    },
    messages = [];

    return service;

    function add(type, title, msg) {
        messages.push({
            type: type,
            title: title,
            msg: msg,            
            close: function () {
                return closeMessage(this);
            }
        });
        $timeout(function () {
            // remove the message after 2000 ms
            closeMessage(this);
        }, 2000);
    }

    function closeMessage(message) {
        return closeMessageIdx(messages.indexOf(message));
    }

    function closeMessageIdx(index) {
        return messages.splice(index, 1);
    }

    function clear() {
        messages = [];
    }

    function get() {
        return messages;
    }

}
