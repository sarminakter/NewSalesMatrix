/// <reference path="../../scripts/angular.js" />

var app = angular.module('OperationApp', []);

app.controller('OperationCreate', function ($scope, $http, $window) {
    $scope.buttonDisable = true;
    $scope.operation_Status = "true";
    $scope.validatedForCreate = false;
    $scope.IsGlobal = false;
    var validName = false;
    var validDescription = false;
    var validModule = false;

    $scope.GetAllModule = function () {
        $http.get("/Module/GetAll")
            .then(function (result) {
                $scope.ModuleList = result.data;
            }), function (result) {
                console.log(result.Status);
            };
    };

    $scope.CheckValidation = function (operation) {
        if (operation.OperationName !== undefined && operation.OperationName !== null) {
            if (operation.OperationName === "") {
                validName = false;
            } else {
                validName = true;
            }
        }
        if (operation.Description !== undefined && operation.Description !== null) {
            if (operation.Description === "") {
                validDescription = false;
            } else {
                validDescription = true;
            }
        }
        if ($scope.IsGlobal === 'True') {
            if (operation._Module !== undefined && operation._Module !== null) {
                if (operation._Module === "" || operation._Module.ModuleName === undefined || operation._Module.ModuleName === null || operation._Module.ModuleName === "") {
                    validModule = false;
                    $scope.ModuleValidation_msg = true;
                } else {
                    validModule = true;
                    $scope.ModuleValidation_msg = false;
                }
            }
        } else {
            validModule = true;
            $scope.ModuleValidation_msg = false;
        }

        if (validName === true && validDescription === true && validModule === true && ($scope.IsGlobal === 'True' || ($scope.operation._Module !== undefined && $scope.operation._Module !== null))) {
            $scope.buttonDisable = false;
            $scope.validatedForCreate = true;
        } else {
            $scope.buttonDisable = true;
            $scope.validatedForCreate = false;
        }
    };
    $scope.ClickGlobal = function () {
        if ($scope.IsGlobal === 'True') {
            $scope.ModuleDisable = true;
        } else {
            $scope.ModuleDisable = false;
        }
    };
    $scope.Create = function (operation) {
        $scope.CheckValidation(operation);
        if ($scope.validatedForCreate === true) {
            operation.Status = $scope.operation_Status;
            if ($scope.IsGlobal === 'True') {
                operation.IsGlobal = true;
                operation.ModuleId = null;
            } else {
                operation.IsGlobal = false;
                operation.ModuleId = parseInt($scope.operation._Module);
            }
            $http.post("/Operation/Create", operation)
                .then(function (result) {
                    $scope.operation = {};
                    alert(result.data);
                    $window.location.href = "/Operation/Index";
                }, function (result) {
                    alert(result.data);
                });
        }
    };
    $scope.Init = function () {
        $scope.GetAllModule();
    };
});

app.controller('OperationList', function ($scope, $http, $window, $filter) {
    $scope.Init = function () {
        $scope.GetAll();
        $scope.GetAllModule();
    };
    $scope.GetAll = function () {
        $http.get("/Operation/GetAll")
            .then(function (result) {
                console.log(result.data);
                $scope.OperationList = result.data;
                $scope.pagination($scope.OperationList);
            }), function (result) {
                console.log(result.Status);
            };
    };
    $scope.GetAllModule = function () {
        $http.get("/Module/GetAll")
            .then(function (result) {
                $scope.ModuleList = result.data;
            }), function (result) {
                console.log(result.Status);
            };
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
    $scope.validatedForCreate = false;
    $scope.IsGlobal = false;
    var validName = false;
    var validDescription = false;
    var validModule = false;
    $scope.ClickGlobal = function () {
        if ($scope.IsGlobal === 'True') {
            $scope.ModuleDisable = true;
        } else {
            $scope.ModuleDisable = false;
        }
    };
    $scope.CheckValidation = function (operation) {
        if (operation.OperationName !== undefined && operation.OperationName !== null) {
            if (operation.OperationName === "") {
                validName = false;
            } else {
                validName = true;
            }
        }
        if (operation.Description !== undefined && operation.Description !== null) {
            if (operation.Description === "") {
                validDescription = false;
            } else {
                validDescription = true;
            }
        }
        if ($scope.IsGlobal === 'True') {
            if (operation._Module !== undefined && operation._Module !== null) {
                if (operation._Module === "" || operation._Module.ModuleName === undefined || operation._Module.ModuleName === null || operation._Module.ModuleName === "") {
                    validModule = false;
                    $scope.ModuleValidation_msg = true;
                } else {
                    validModule = true;
                    $scope.ModuleValidation_msg = false;
                }
            }
        } else {
            validModule = true;
            $scope.ModuleValidation_msg = false;
        }

        if (validName === true && validDescription === true && validModule === true && ($scope.IsGlobal === 'True' || ($scope.operation._Module !== undefined && $scope.operation._Module !== null))) {
            $scope.buttonDisable = false;
            $scope.validatedForCreate = true;
        } else {
            $scope.buttonDisable = true;
            $scope.validatedForCreate = false;
        }
    };
    $scope.GetOperationEditInfo = function (operationId, Name) {
        //$scope.ResourceTypeList = [{ Name: 'Header Menu' }, { Name: 'Menu Item' }, { Name: 'Link' }, { Name: 'Text' }, { Name: 'Image' }, { Name: 'Video' }];
        $scope.OperationName = Name;
        $http.get("/Operation/GetById", { params: { id: operationId } })
            .then(function (result) {
                $scope.operation = result.data;
                $scope.operation._Module = result.data.ModuleId;
                console.log(result.data);
                $scope.operation_Status = result.data.Status === true ? 'true' : 'false';
                if (result.data.IsGlobal === true) {
                    $scope.IsGlobal = 'True';
                    $scope.ModuleDisable = true;
                } else {
                    $scope.IsGlobal = 'False';
                    $scope.ModuleDisable = false;
                }
            }), function (result) {
                console.log(result.Status);
            };
    };
    $scope.Edit = function (operation) {
        $scope.CheckValidation(operation);
        if ($scope.validatedForCreate === true) {
            operation.Status = $scope.operation_Status;
            if ($scope.IsGlobal === 'True') {
                operation.IsGlobal = true;
                operation.ModuleId = null;
            } else {
                operation.IsGlobal = false;
                operation.ModuleId = parseInt($scope.operation._Module);
            }
            $http.post("/Operation/Edit", operation)
                .then(function (result) {
                    $scope.operation = {};
                    alert(result.data);
                    $window.location.href = "/Operation/Index";
                }, function (result) {
                    alert(result.data);
                });
        }
    };
});