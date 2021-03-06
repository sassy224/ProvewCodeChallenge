﻿/*
 * This controller is used to display messages that are passed from other controllers to MessageService
 */
proview.app.controller("MessageController", MessageController);
//Inject dependency
MessageController.$inject = ['MessageService'];

function MessageController(MessageService) {
    var vm = this;
    vm.messages = MessageService.get();

    vm.closeMsg = function (index) {
        MessageService.closeMessageIdx(index);
    };
}
