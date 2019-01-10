/// <reference path="../../scripts/angular.js" />

var app = angular.module('RetailCustomerApp', []);

app.controller('RetailCustomerCreate', function ($scope, $http, $window) {
    $scope.GenderList = [{ Name: "Male" }, { Name: "Female" }, { Name: "Other" }];    
    $scope.buttonDisable = true;
    $scope.customer_Status = "true";
    $scope.validatedForCreate = false;

    var validAddress = false;
    var validRegion = false;
    var validMobileNo = false;
    var validGender = false;
    
    $scope.CheckValidation = function (customer) {        
        if (customer.Address !== undefined && customer.Address !== null) {
            if (customer.Address === "") {
                validAddress = false;
            } else {
                validAddress = true;
            }
        } else {
            validAddress = false;
        }
        
        if (customer.Region !== undefined && customer.Region !== null) {
            if (customer.Region === "") {
                validRegion = false;
            } else {
                validRegion = true;
            }
        } else {
            validRegion = false;
        }
        if (customer.MobileNo !== undefined && customer.MobileNo !== null) {
            if (customer.MobileNo === "") {
                validMobileNo = false;
            } else {
                validMobileNo = true;
            }
        } else {
            validMobileNo = false;
        }
        if (customer.Gender !== undefined && customer.Gender !== null) {
            if (customer.Gender === "" || customer.Gender === null) {
                validGender = false;
            } else {
                validGender= true;
            }
        } else {
            validGender = false;
        }
        if (validAddress === true && validRegion === true && validMobileNo === true && validGender === true) {
            $scope.buttonDisable = false;
            $scope.validatedForCreate = true;
        } else {
            $scope.buttonDisable = true;
            $scope.validatedForCreate = false;
        }
    };


    $scope.Create = function (customer) {
        $scope.CheckValidation(customer);
        if ($scope.validatedForCreate === true) {
            customer.Status = $scope.customer_Status;
            customer.DateOfBirth = new Date(customer.DateOfBirth);
            $http.post("/RetailCustomer/Create", customer)
                .then(function (result) {
                    $scope.customer = {};
                    alert(result.data);
                    $window.location.href = "/RetailCustomer/Index";
                }, function (result) {
                    alert(result.data);
                });
        }
    };
    $scope.Init = function () {
        
    };
});

app.controller('RetailCustomerList', function ($scope, $http, $filter, $window) {

    $scope.Init = function () {
        $scope.GetAll();
    };
    $scope.Active = true;

    $scope.GetAll = function () {
        $http.get("/RetailCustomer/GetAll")
            .then(function (result) {
                $scope.List = result.data;
                console.log(result.data);
                $scope.pagination($scope.List);
            }), function (response) {
                console.log(response.status);
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
    $scope.GenderList = [{ Name: "Male" }, { Name: "Female" }, { Name: "Other" }];

    var validAddress = false;
    var validRegion = false;
    var validMobileNo = false;
    var validGender = false;

    $scope.CheckValidation = function (customer) {
        if (customer.Address !== undefined && customer.Address !== null) {
            if (customer.Address === "") {
                validAddress = false;
            } else {
                validAddress = true;
            }
        } else {
            validAddress = false;
        }

        if (customer.Region !== undefined && customer.Region !== null) {
            if (customer.Region === "") {
                validRegion = false;
            } else {
                validRegion = true;
            }
        } else {
            validRegion = false;
        }
        if (customer.MobileNo !== undefined && customer.MobileNo !== null) {
            if (customer.MobileNo === "") {
                validMobileNo = false;
            } else {
                validMobileNo = true;
            }
        } else {
            validMobileNo = false;
        }
        if (customer.Gender !== undefined && customer.Gender !== null) {
            if (customer.Gender === "" || customer.Gender === null) {
                validGender = false;
            } else {
                validGender = true;
            }
        } else {
            validGender = false;
        }
        if (validAddress === true && validRegion === true && validMobileNo === true && validGender === true) {
            $scope.buttonDisable = false;
            $scope.validatedForCreate = true;
        } else {
            $scope.buttonDisable = true;
            $scope.validatedForCreate = false;
        }
    };

    $scope.GetCustomerEditInfo = function (Id, Name) {
        $scope.CustomerName = Name;
        $http.get("/RetailCustomer/GetById", { params: { id: Id } })
            .then(function (result) {
                $scope.customer = result.data;
                $scope.customer.DateOfBirth = new Date(result.data.DateOfBirth);
                $scope.customer_Status = result.data.Status === true ? 'true' : 'false';
            }), function (result) {
                console.log(result.Status);
            };
    };
    $scope.Edit = function (customer) {
        $scope.CheckValidation(customer);
        if ($scope.validatedForCreate === true) {
            customer.Status = $scope.customer_Status;
            customer.DateOfBirth = new Date(customer.DateOfBirth);
            $http.post("/RetailCustomer/Edit", customer)
                .then(function (result) {
                    $scope.customer = {};
                    alert(result.data);
                    $window.location.href = "/RetailCustomer/Index";
                }, function (result) {
                    alert(result.data);
                });
        }
    };
});