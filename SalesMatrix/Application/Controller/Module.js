/// <reference path="../../scripts/angular.js" />

var app = angular.module('ModuleApp', []);
app.controller('ModuleCreate', function ($scope, $http, $window, $filter) {
    $scope.buttonDisable = true;
    $scope.NameExist = false;
    $scope.module_Status = "true";
    var validName = false;    
    $scope.Active = true;
    $scope.CheckValidation = function () {
        if (validName === true) {
            $scope.buttonDisable = false;
        } else {
            $scope.buttonDisable = true;
        }
    };
    $scope.CheckName = function (ModuleName) {
        
        $http.get("/Module/ModuleExists", { params: { name: ModuleName } })
            .then(function (response) {
                if (response.data === true) {
                    $scope.NameExist = true;
                    $scope.buttonDisable = true;
                }
                else {
                    $scope.NameExist = false;
                    validName = true;
                    $scope.buttonDisable = false;
                }
            }), function (response) {
                alert(response.status);
            };
        $scope.CheckValidation();
    };

    $scope.Create = function (module) {
        module.Status = $scope.module_Status;
        $http.post("/Module/Create", module)
                .then(function (result) {
                    $scope.module = {};                    
                    alert(result.data);
                    $window.location.href = "/Module/Create";
                }, function (result) {
                    alert(result.data);
            });        
    };

    
    $scope.GetAllActive = function () {
        $http.get("/Module/GetAllActiveModule")
            .then(function (result) {
                $scope.ModuleList = result.data;
                $scope.pagination($scope.ModuleList);
            }), function (result) {
                console.log(result.Status);
            };
    };
    $scope.GetAllInactive = function () {
        $http.get("/Module/GetAllInactiveModule")
            .then(function (result) {
                $scope.ModuleList = result.data;
                $scope.pagination($scope.ModuleList);
            }), function (result) {
                console.log(result.Status);
            };
    };
    $scope.ClickActiveInactive = function (e) {
        if ($scope.Active === true) {
            $scope.GetAllInactive();
        } else {
            $scope.GetAllActive();
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
});
