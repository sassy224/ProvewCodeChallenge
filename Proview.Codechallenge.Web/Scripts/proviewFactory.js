/*
 * The main factory for communicating with server
 */
//Recommended way to declare
proview.app.factory("ProviewFactory", ProviewFactory);
//Inject
ProviewFactory.$inject = ['$http', '$q'];

function ProviewFactory($http, $q) {
    //Declare functions here
    var factory = {
        loadItems: loadItems,
        addItem: addItem,
        deleteItem: deleteItem,
        getPageSizeList: getPageSizeList
    };
    return factory;

    function getPageSizeList() {
        return [
          { value: 10, text: "10" },
          { value: 25, text: "25" },
          { value: 50, text: "50" },
          { value: 100, text: "100" }
        ];
    }

    //Declare function implementation here
    function loadItems(currentPage, pageItems, orderBy, orderByReverse, searchText) {
        var deferred = $q.defer();

        $http({
            method: 'POST',
            url: 'Home/GetUserInputs',
            data: $.param({ currentPage: currentPage, pageItems: pageItems, orderBy: orderBy, orderByReverse: orderByReverse, searchText: searchText }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).
            success(function (data, status, headers, config) {
                // this callback will be called asynchronously
                // when the response is available
                //return data;
                deferred.resolve(data);
            }).
            error(function (data, status, headers, config) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
                //data["err"] = "Error getting product list data.";
                //return data;
                deferred.resolve(data);
            });

        return deferred.promise;
    };

    function addItem(exp, result) {
        var deferred = $q.defer();

        $http({
            method: 'POST',
            url: 'Home/AddUserInput',
            data: $.param({ expression: exp, expessionResult: result }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).
            success(function (data, status, headers, config) {
                // this callback will be called asynchronously
                // when the response is available
                //return data;
                deferred.resolve(data);
            }).
            error(function (data, status, headers, config) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
                //data["err"] = "Error getting product list data.";
                //return data;
                deferred.resolve(data);
            });

        return deferred.promise;

    };

    function deleteItem(uId) {
        var deferred = $q.defer();

        $http({
            method: 'POST',
            url: 'Home/DeleteUserInput',
            data: $.param({ uId: uId }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).
            success(function (data, status, headers, config) {
                // this callback will be called asynchronously
                // when the response is available
                //return data;
                deferred.resolve(data);
            }).
            error(function (data, status, headers, config) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
                //data["err"] = "Error getting product list data.";
                //return data;
                deferred.resolve(data);
            });

        return deferred.promise;

    };
};