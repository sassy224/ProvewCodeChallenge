/*
 * The main controller for this application
 */
//Recommended way to declare
proview.app.controller("ProviewController", ProviewController);
//Inject dependency
ProviewController.$inject = ['ProviewFactory', 'MessageService', '$confirm'];

function ProviewController(ProviewFactory, MessageService, $confirm) {
    //vm: ViewModel, use this approach instead of $scope
    var vm = this;
    vm.model = {};
    vm.search = {};
    vm.model.pageSizeList = ProviewFactory.getPageSizeList();
    vm.model.userInputs = [];
    vm.model.totalUserInputs = 0;
    vm.model.userEpxression = "";
    vm.model.userResult = "";

    //Default paging and sorting paramters. Private variables
    var pCurrentPage = 0; //PageIndex
    var pPageItems = 5;   //PageSize  
    var pOrderBy = "";    //SortBy
    var pOrderByReverse = false;  //Reversed

    //Declare functions here
    vm.onServerSideItemsRequested = onServerSideItemsRequested;
    vm.initiateSearchItems = initiateSearchItems;
    vm.search = search;
    vm.deleteConfirm = deleteConfirm;
    vm.deleteItem = deleteItem;
    vm.evalExpression = evalExpression;
    vm.addItem = addItem;
    //Call function
    vm.initiateSearchItems();

    //Declare function imlementation here
    //Init params
    function initiateSearchItems() {
        vm.model.pPageSizeObj = { selected: 10 };

        //Search parameter.
        vm.search.pSearchText = "";
    }

    //Called from on-data-required directive.
    function onServerSideItemsRequested(currentPage, pageItems, filterBy, filterByFields, orderBy, orderByReverse) {
        loadItems(currentPage, pageItems, orderBy, orderByReverse);
    }

    //Ajax call for list data.
    function loadItems(currentPage, pageItems, orderBy, orderByReverse) {
        //Bypass on initial page loading.
        if (currentPage != undefined) {
            pCurrentPage = currentPage;
            pPageItems = pageItems;
            if (orderBy == null) orderBy = "";
            pOrderBy = orderBy;
            pOrderByReverse = orderByReverse;

            //Call factory to load product list from server
            var data = ProviewFactory.loadItems(currentPage, pageItems, orderBy, orderByReverse, vm.search.pSearchText).then(function (data) {
                if (data != null) {
                    vm.model.userInputs = data.userInputs;
                    vm.model.totalUserInputs = data.total;
                } else {
                    //alert("Error getting list product.");
                    MessageService.add('danger', 'Error', 'Error getting list items.');
                }
            });
        }
    }

    //Global search 
    function search() {
        //Call function to load items based on the new filter condition
        loadItems(pCurrentPage, pPageItems, pOrderBy, pOrderByReverse);
    }

    //Add item to database when user press Enter
    function addItem() {
        //Only add when input is not empty
        if (vm.model.userEpxression.length == 0) {
            return;
        }

        //Call factory to handle the business
        var data = ProviewFactory.addItem(vm.model.userEpxression, vm.model.userResult).then(function (data) {
            if (data != null && data == true) {
                //Clear input
                vm.model.userEpxression = "";
                vm.model.userResult = "";
                //Show message
                MessageService.add('success', 'Success', "Item was added successfully");
                //Reload items
                loadItems(pCurrentPage, pPageItems, pOrderBy, pOrderByReverse);
            } else {
                //Show message
                MessageService.add('danger', 'Error', "Error occured when adding item");
            }
        });
    }

    //Show confirm dialog when user wants to delete item from database
    function deleteConfirm(gridItem) {
        $confirm({ text: 'Are you sure you want to delete?', title: 'Confirm', ok: 'Yes', cancel: 'No' })
        .then(function () {
            //Call function when ok is selected
            deleteItem(gridItem);
        });
    }

    //Delete item from database
    function deleteItem(gridItem) {
        //Call factory to handle the business
        var data = ProviewFactory.deleteItem(gridItem.Id).then(function (data) {
            if (data != null && data == true) {
                //Show message
                MessageService.add('success', 'Success', "Item was deleted successfully");
                //Reload items
                loadItems(pCurrentPage, pPageItems, pOrderBy, pOrderByReverse);
            } else {
                //Show message
                MessageService.add('danger', 'Error', "Error occured when deleting item");
            }
        });
    }

    //Evaluate the input expression
    function evalExpression() {
        'use strict';
        //Only eval when there's data
        if (vm.model.userEpxression == null || vm.model.userEpxression == "") {
            vm.model.userResult = "";
            return;
        }

        try {
            //Use mathjs lib to eval the expression
            vm.model.userResult = math.eval(vm.model.userEpxression).toString();
        } catch (e) {
            //Display the error
            vm.model.userResult = e.toString();
        }
        
    }
};

