/// <reference path="../../scripts/angular.js" />

var app = angular.module('ResourceApp', []);
app.controller('ResourceCreate', function ($scope, $http, $window) {
    $scope.ResourceTypeList = [{ Name: 'Header Menu' }, { Name: 'Menu Item' }, { Name: 'Link' }, { Name: 'Text' }, { Name: 'Image' }, { Name: 'Video' }];
    //$scope.resource_Type = 'Header Menu';
    $scope.buttonDisable = true;
    $scope.resource_Status = "true";
    $scope.validatedForCreate = false;
    var validName = false;
    var validResourceType = false;
    var validDescription = false;
    var validParent = false;
    var validSequence = false;
    var validModule = false;
    $scope.Active = true;
    $scope.ResoruseTypeValidation_msg = false;
    $scope.ModuleValidation_msg = false;
    $scope.IsGlobal = false;
    $scope.Init = function () {
        $scope.GetAllModule();
        $scope.GetAll();
    };
    $scope.GetAllModule = function () {
        $http.get("/Module/GetAll")
            .then(function (result) {
                $scope.ModuleList = result.data;
            }), function (result) {
                console.log(result.Status);
            };
    };
    $scope.GetAll = function () {
        $http.get("/Resource/GetAll")
            .then(function (result) {
                $scope.ResourcetList = result.data;
            }), function (result) {
                console.log(result.Status);
            };
    };

    $scope.ClickGlobal = function () {
        if ($scope.IsGlobal === 'True') {
            $scope.ModuleDisable = true;
        } else {
            $scope.ModuleDisable = false;
        }
    };

    $scope.CheckValidation = function (resource) {
        if (resource.ResourceType !== undefined && resource.ResourceType !== null) {
            if (resource.ResourceType === undefined || resource.ResourceType === null || resource.ResourceType === "" || resource.ResourceType.Name === undefined || resource.ResourceType.Name === null || resource.ResourceType.Name === "") {
                validResourceType = false;
                $scope.ResoruseTypeValidation_msg = true;
            } else {
                validResourceType = true;
                $scope.ResoruseTypeValidation_msg = false;
            }
        }
        if (resource.ResourceName !== undefined && resource.ResourceName !== null) {
            if (resource.ResourceName === "") {
                validName = false;
            } else {
                validName = true;
            }
        }
        if (resource.Description !== undefined && resource.Description !== null) {
            if (resource.Description === "") {
                validDescription = false;
            } else {
                validDescription = true;
            }
        }
        //if (resource._Parent !== undefined && resource._Parent !== null) {
        //    if (resource._Parent === null || resource._Parent === "") {
        //        validParent = false;
        //    } else {
        //        validParent = true;
        //    }
        //}
        if (resource.Sequence !== undefined && resource.Sequence !== null) {
            if (resource.Sequence !== null && resource.Sequence === undefined || resource.Sequence === null || resource.Sequence === "") {
                validSequence = false;
            } else {
                validSequence = true;
            }
        }
        if ($scope.IsGlobal === 'True') {
            if (resource._ModuleName !== undefined && resource._ModuleName !== null) {
                if (resource._ModuleName === "" || resource._ModuleName.ModuleName === undefined || resource._ModuleName.ModuleName === null || resource._ModuleName.ModuleName === "") {
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


        if (validResourceType === true && validName === true && validDescription === true && validSequence === true && validModule === true) {
            $scope.buttonDisable = false;
            $scope.validatedForCreate = true;
        } else {
            $scope.buttonDisable = true;
            $scope.validatedForCreate = false;
        }
    };


    $scope.Create = function (resource) {        
        $scope.CheckValidation(resource);
        if ($scope.validatedForCreate === true) {
            resource.ResourceType = resource.ResourceType.Name;
            //resource.ParentResource = $scope.resource._Parent;   
            resource.Parent = $scope.resource._Parent === null ? "" : $scope.resource._Parent;   
            resource.ModuleId = parseInt(resource._ModuleName);
            resource.Status = $scope.resource_Status;
            if ($scope.IsGlobal === 'True') {
                resource.IsGlobal = true;
                resource.ModuleId = null;
            } else {
                resource.IsGlobal = false;
                resource.ModuleId = parseInt($scope.resource._ModuleName);
            }
            $http.post("/Resource/Create", resource)
                .then(function (result) {
                    $scope.resource = {};
                    alert(result.data);
                    $window.location.href = "/Resource/Index";
                }, function (result) {
                    alert(result.data);
                });
        }
    };
});

app.controller('ResourceList', function ($scope, $http, $window, $filter) {
    $scope.ResourceTypeList = [{ Name: 'Header Menu' }, { Name: 'Menu Item' }, { Name: 'Link' }, { Name: 'Text' }, { Name: 'Image' }, { Name: 'Video' }];
    $scope.Init = function () {
        $scope.GetAll();
        $scope.GetAllModule();
    };
    $scope.GetAll = function () {
        $http.get("/Resource/GetAll")
            .then(function (result) {
                $scope.ResourcetList = result.data;
                console.log($scope.ResourcetList);
                $scope.pagination($scope.ResourcetList);
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
    $scope.GetAllModule = function () {
        $http.get("/Module/GetAll")
            .then(function (result) {
                $scope.ModuleList = result.data;
                console.log(result.data);
            }), function (result) {
                console.log(result.Status);
            };
    };

    //Edit Part
    var validResourceType = false;
    var validName = false;
    var validDescription = false;
    var validSequence = false;
    $scope.ClickGlobal = function () {
        if ($scope.IsGlobal === 'True') {
            $scope.ModuleDisable = true;
        } else {
            $scope.ModuleDisable = false;
        }
    };
    $scope.CheckValidation = function (resource) {
        if (resource.ResourceName !== undefined && resource.ResourceName !== null) {
            if (resource.ResourceName === "") {
                validName = false;
            } else {
                validName = true;
            }
        }
        if (resource.Description !== undefined && resource.Description !== null) {
            if (resource.Description === "") {
                validDescription = false;
            } else {
                validDescription = true;
            }
        }
        if (resource.Sequence !== undefined && resource.Sequence !== null) {
            if (resource.Sequence !== null && resource.Sequence === undefined || resource.Sequence === null || resource.Sequence === "") {
                validSequence = false;
            } else {
                validSequence = true;
            }
        }

        if (validName === true && validDescription === true && validSequence === true) {
            $scope.buttonDisable = false;
            $scope.validatedForCreate = true;
        } else {
            $scope.buttonDisable = true;
            $scope.validatedForCreate = false;
        }
    };
    $scope.GetResourceEditInfo = function (resourceId, Name) {
        //$scope.ResourceTypeList = [{ Name: 'Header Menu' }, { Name: 'Menu Item' }, { Name: 'Link' }, { Name: 'Text' }, { Name: 'Image' }, { Name: 'Video' }];
        $scope.ResourceName = Name;
        $http.get("/Resource/GetById", { params: { id: resourceId } })
            .then(function (result) {
                $scope.resource = result.data;
                $scope.ModuleId = result.data.ModuleId;
                $scope.resource._Parent = result.data.ParentResource === null ? "" : result.data.ParentResource.Id;
                $scope.resource_Status = result.data.Status === true ? 'true' : 'false';
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
    $scope.Edit = function (resource) {
        $scope.CheckValidation(resource);
        if ($scope.validatedForCreate === true) {
            //console.log($scope.resource._Parent);
            resource.Parent = $scope.resource._Parent === null ? "" : $scope.resource._Parent;
            resource.Status = $scope.resource_Status;
            if ($scope.IsGlobal === 'True') {
                resource.IsGlobal = true;
            } else {
                resource.IsGlobal = false;
                $scope.ModuleId = parseInt($scope.ModuleId);
            }
            $http.post("/Resource/Edit", resource)
                .then(function (result) {                    
                    alert(result.data);
                    //$window.location.href = "/Role/Index";
                }, function (result) {
                    alert(result.data);
                });
        };
    };
});