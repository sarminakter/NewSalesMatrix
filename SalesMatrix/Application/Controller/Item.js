/// <reference path="../../scripts/angular.js" />

var app = angular.module('ItemApp', ['ngFileUpload']);

app.controller('ItemCreate', function ($scope, $http, $window, Upload) {
    $scope.buttonDisable = true;
    $scope.item_Status = "true";

    $scope.validatedForCreate = false;
    var validName = false;
    var validParentItem = false;
    
    $scope.GetAllNotActualItem = function () {
        $http.get("/Item/GetAllNotActualItem")
            .then(function (result) {
                $scope.ParentItemList = result.data;
            }), function (result) {
                console.log(result.Status);
            };
    };

    $scope.CheckValidation = function (item) {
        if (item.ItemName !== undefined && item.ItemName !== null) {
            if (item.ItemName === "") {
                validName = false;
            } else {
                validName = true;
            }
        }
        //if (item.ParentItemId !== undefined && item.ParentItemId !== null) {
        //    if (item.ParentItemId === "") {
        //        validParentItem = false;
        //    } else {
        //        validParentItem = true;
        //    }
        //}

        if (validName === true ) {
            $scope.buttonDisable = false;
            $scope.validatedForCreate = true;
        } else {
            $scope.buttonDisable = true;
            $scope.validatedForCreate = false;
        }
    };
    $scope.UploadFiles = function (files) {
        $scope.SelectedFiles = files;
    };

    $scope.Create = function (item) {
        $scope.CheckValidation(item);
        if ($scope.validatedForCreate === true) {
            item.Status = $scope.item_Status;

            if ($scope.SelectedFiles && $scope.SelectedFiles.length) {
                Upload.upload({
                    url: '/Item/Upload/',
                    data: {
                        files: $scope.SelectedFiles
                    }
                });
            }

            $http.post("/Item/Create", item)
                .then(function (result) {
                    alert(result.data);
                    $scope.item = {};
                    $window.location.href = "/Item/Index";
                }, function (result) {
                    alert(result.status);
                });
        }
    };
    $scope.Init = function () {
        $scope.GetAllNotActualItem();
    };
});

app.controller('ItemList', function ($scope, $http, $filter, $window) {

    $scope.Init = function () {
        $scope.GetAllActiveItem();
    };
    $scope.Active = true;

    $scope.GetAllActiveItem = function () {
        $http.get("/Item/GetAllActiveItem")
            .then(function (result) {
                $scope.ItemList = result.data;
                $scope.pagination($scope.ItemList);
            }), function (response) {
                console.log(response.status);
            };
    };

    $scope.GetAllInactiveItem = function () {
        $http.get("/Item/GetAllInactiveItem")
            .then(function (result) {
                $scope.ItemList = result.data;
                $scope.pagination($scope.ItemList);
            }), function (response) {
                console.log(response.status);
            };
    };


    $scope.ClickActiveInactive = function (e) {
        if ($scope.Active === true) {
            $scope.GetAllInactiveItem();
        } else {
            $scope.GetAllActiveItem();
        }
    };

    $scope.pagination = function (list) {
        $scope.sort = {
            sortingOrder: 'id',
            reverse: false
        };
        $scope.filteredItems = [];
        $scope.groupedItems = [];
        $scope.itemsPerPage = 10;
        $scope.pagedItems = [];
        $scope.currentPage = 0;
        $scope.items = list;
        if (($scope.items.length % $scope.itemsPerPage) === 0) {
            $scope.gap = parseInt(($scope.items.length / $scope.itemsPerPage));
        } else {
            $scope.gap = parseInt(($scope.items.length / $scope.itemsPerPage) + 1);
        }

        var searchMatch = function (haystack, needle) {
            if (!needle) {
                return true;
            }
            return haystack.toLowerCase().indexOf(needle.toLowerCase()) !== -1;
        };

        // init the filtered items
        $scope.search = function () {
            $scope.filteredItems = $filter('filter')($scope.items, function (item) {
                for (var attr in item) {
                    if (searchMatch(item[attr], $scope.query))
                        return true;
                }
                return false;
            });
            // take care of the sorting order
            if ($scope.sort.sortingOrder !== '') {
                $scope.filteredItems = $filter('orderBy')($scope.filteredItems, $scope.sort.sortingOrder, $scope.sort.reverse);
            }
            $scope.currentPage = 0;
            // now group by pages
            $scope.groupToPages();
        };


        // calculate page in place
        $scope.groupToPages = function () {
            $scope.pagedItems = [];

            for (var i = 0; i < $scope.filteredItems.length; i++) {
                if (i % $scope.itemsPerPage === 0) {
                    $scope.pagedItems[Math.floor(i / $scope.itemsPerPage)] = [$scope.filteredItems[i]];
                } else {
                    $scope.pagedItems[Math.floor(i / $scope.itemsPerPage)].push($scope.filteredItems[i]);
                }
            }
        };

        $scope.range = function (size, start, end) {
            var ret = [];
            if (size < end) {
                end = size;
                start = size - $scope.gap;
            }
            for (var i = start; i < end; i++) {
                ret.push(i);
            }
            return ret;
        };

        $scope.prevPage = function () {
            if ($scope.currentPage > 0) {
                $scope.currentPage--;
            }
        };

        $scope.nextPage = function () {
            if ($scope.currentPage < $scope.pagedItems.length - 1) {
                $scope.currentPage++;
            }
        };

        $scope.setPage = function () {
            $scope.currentPage = this.n;
        };

        // functions have been describe process the data for display
        $scope.search();
    };

    //Edit Part
    $scope.GetAllNotActualItem = function () {
        $http.get("/Item/GetAllNotActualItem")
            .then(function (result) {
                $scope.ParentItemList = result.data;
            }), function (result) {
                console.log(result.Status);
            };
    };
    $scope.buttonDisable = true;
    $scope.item_Status = "true";

    $scope.validatedForCreate = false;
    var validName = false;
    var validParentItem = false;
   
    $scope.CheckValidation = function (item) {
        if (item.ItemName !== undefined && item.ItemName !== null) {
            if (item.ItemName === "") {
                validName = false;
            } else {
                validName = true;
            }
        }
        //if (item.ParentItemId !== undefined && item.ParentItemId !== null) {
        //    if (item.ParentItemId === "") {
        //        validParentItem = false;
        //    } else {
        //        validParentItem = true;
        //    }
        //}

        if (validName === true) {
            $scope.buttonDisable = false;
            $scope.validatedForCreate = true;
        } else {
            $scope.buttonDisable = true;
            $scope.validatedForCreate = false;
        }
    };

    $scope.GetItemEditInfo = function (ItemId, Name) {
        $scope.ItemName = Name;
        $http.get("/Item/GetById", { params: { id: ItemId } })
            .then(function (result) {
                $scope.item = result.data;
                $scope.item_Status = result.data.Status === true ? 'true' : 'false';                
                $scope.item.ParentItemId = result.data.ParentItemId === null ? "" : result.data.ParentItemId;                
            }), function (result) {
                console.log(result.Status);
            };
    };
    $scope.Edit = function (item) {
        $scope.CheckValidation(item);
        if ($scope.validatedForCreate === true) {
            item.Status = $scope.item_Status;
            $http.post("/Item/Edit", item)
                .then(function (result) {
                    alert(result.data);
                    $scope.item = {};
                    $window.location.href = "/Item/Index";
                }, function (result) {
                    alert(result.status);
                });
        };
    };
});