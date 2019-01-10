/// <reference path="../../scripts/angular.js" />

var app = angular.module('RoleApp', []);
app.controller('RoleCreate', function ($scope, $http, $window) {
    $scope.buttonDisable = true;
    $scope.NameExist = false;
    $scope.role_Status = "true";
    var validName = false;
    var validStatus = false;

    $scope.CheckName = function (RoleName) {
        $http.get("/Role/RoleExists", { params: { name: RoleName } })
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
    };
    $scope.Create = function (role) {
        role.Status = $scope.role_Status;
        if (role.RoleName === undefined || role.RoleName === null || role.RoleName === "") {
            validName = false;
        } else {
            validName = true;
        }
        if (role.Status === undefined || role.Status === null || role.Status === "") {
            validStatus = false;
        } else {
            validStatus = true;
        }

        if (validName === true && validStatus === true) {
            $http.post("/Role/Create", role)
                .then(function (result) {
                    alert(result.data);
                    $scope.role = {};
                    $window.location.href = "/Role/Index";
                }, function (result) {
                    alert(result.data);
                });
        }
    };
});

app.controller('RoleEdit', function ($scope, $http, $window) {
    $scope.buttonDisable = true;
    $scope.NameExist = false;
    var validName = false;
    var validStatus = false;

    $scope.CheckNameWithId = function (RoleName, RoleId) {
        $http.get("/Role/RoleExistsForEdit", { params: { name: RoleName, id: RoleId } })
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
                alert(response.data);
            };
    };

    $scope.GetById = function (id) {
        $http.get("/Role/GetById", { params: { id: id } })
            .then(function (result) {
                $scope.role = result.data;
                $scope.CheckNameWithId(result.data.RoleName, id);
                if (result.data.Status === true) {
                    $scope.role_Status = 'true';
                } else {
                    $scope.role_Status = 'false';
                }
            }), function (result) {
                console.log(result.Status);
            };
    };
    $scope.Edit = function (role) {
        role.Status = $scope.role_Status;
        if (role.RoleName === undefined || role.RoleName === null || role.RoleName === "") {
            validName = false;
        } else {
            validName = true;
        }
        if (role.Status === undefined || role.Status === null || role.Status === "") {
            validStatus = false;
        } else {
            validStatus = true;
        }

        if (validName === true && validStatus === true) {
            $http.post("/Role/Edit", role)
                .then(function (result) {
                    $scope.role = {};
                    alert(result.data);
                    $window.location.href = "/Role/Index";
                }, function (result) {
                    alert(result.data);
                });
        }
    };
});

app.controller('RoleList', function ($scope, $http, $filter) {

    $scope.Init = function () {
        $scope.GetAllActive();
    };
    $scope.Active = true;
    $scope.GetAllActive = function () {
        $http.get("/Role/GetAllActiveRole")
            .then(function (result) {
                if (result.data === 'false') {
                    alert('Something went wrong');
                } else {
                    $scope.RoleList = result.data;
                    $scope.pagination($scope.RoleList);
                }
            }), function (result) {
                alert('Something went wrong');
                console.log(result.Status);
            };
    };
    $scope.GetAllInactive = function () {
        $http.get("/Role/GetAllInactiveRole")
            .then(function (result) {
                $scope.RoleList = result.data;
                $scope.pagination($scope.RoleList);
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

app.controller('RoleDetails', function ($scope, $http) {

    $scope.GetAllUserByRoleId = function (RoleId) {
        $http.get("/Role/GetAllUserByRoleId", { params: { roleId: RoleId } })
            .then(function (result) {
                $scope.UserList = result.data;
            }), function (result) {
                console.log(result.Status);
            };
    };
});