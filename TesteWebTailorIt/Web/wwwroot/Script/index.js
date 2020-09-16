'use strict';

angular.module('myApp', ['ngStorage']);
angular.module('myApp').controller('relat', function ($scope, $http, $window, $localStorage) {

    $scope.employees = [];
    $scope.filterDomains = {
        age: [],
        gende:[]
    };
    $scope.filter = {
        age: "",
        gender: ""
    };

    $scope.get = function () {
        $scope.employees = [];
        $http.get("https://localhost:44330/api/Employees/GetAll/")
            .then(function (response) {
                if (response) {

                    for (var i = 0; i < response.data.length; i++) { 

                        let date = new Date() - new Date(response.data[i].birthDate);
                        let ageDate = new Date(date); // miliseconds from epoch
                        //$scope.employees[i].age = Math.abs(ageDate.getUTCFullYear() - 1970); 

                        $scope.employees.push({
                            id: response.data[i].id,
                            name: response.data[i].firstName + ' ' + response.data[i].lastName,
                            email: response.data[i].email,
                            birthDate: new Date(response.data[i].birthDate).toLocaleDateString(),
                            age: Math.abs(ageDate.getUTCFullYear() - 1970),
                            gender: (response.data[i].gender === 'F') ? 'Feminino' : 'Masculino',
                            habilities: response.data[i].habilities.name
                        })

                        $scope.getDomains(response.data);
                        
                    }
                }
            });
    }
    
    $scope.get();

    $scope.getDomains = function (employees) {
        $http.post("https://localhost:44330/api/Employees/GetDomains/", employees)
            .then(function (response) {
                if (response)
                    $scope.filterDomains = response.data;
            })
    }

    $scope.filterEmployees = function () {
        $scope.employees = [];
        $http.post("https://localhost:44330/api/Employees/FilterEmployees", $scope.filter)
            .then(function (response) {
                if (response)
                    for (var i = 0; i < response.data.length; i++) {

                        let date = new Date() - new Date(response.data[i].birthDate);
                        let ageDate = new Date(date); 

                        $scope.employees.push({
                            id: response.data[i].id,
                            name: response.data[i].firstName + ' ' + response.data[i].lastName,
                            email: response.data[i].email,
                            birthDate: new Date(response.data[i].birthDate).toLocaleDateString(),
                            age: Math.abs(ageDate.getUTCFullYear() - 1970),
                            gender: (response.data[i].gender === 'F') ? 'Feminino' : 'Masculino',
                            habilities: response.data[i].habilities.name
                        })
                                                
                    }
            })
    }

    $scope.edit = function (employee) {
        $localStorage.idSearch = employee.id;
        $window.location.href = "Crud.html";
    }

    $scope.insert = function () {
        $localStorage.idSearch = 0;
        $window.location.href = "Crud.html";
    }

    $scope.delete = function (employee) {
        $http.delete("https://localhost:44330/api/Employees/Delete/" + employee.id)
            .then(function (response) {
                if (response)
                    $scope.get();
            })
    }

});

angular.module('myApp').controller('employee', function ($scope, $http, $window, $localStorage, $httpParamSerializer) {

    let id = $localStorage.idSearch;
    $scope.get = function () {

        $http.get("https://localhost:44330/api/Habilities/GetAll/")
            .then(function (response) {
                if (response) {
                    $scope.habilitiesDomain = response.data;
                }
            });

        if (id != 0) {
            $http.get("https://localhost:44330/api/Employees/Get/" + id)
                .then(function (response) {
                    if (response) {
                        $scope.employee = response.data;
                        $scope.employee.birthDate = new Date(response.data.birthDate);
                    }
                });
        }
    }

    $scope.get();

    $scope.save = function () {

        
        $http.post("https://localhost:44330/api/Employees/Save/", JSON.stringify($scope.employee))
            .then(function (response) {
                if (response)
                    $scope.cancel();
            })
    };

    $scope.cancel = function () {
        $localStorage.idSearch = null;
        $window.location.href = "Index.html";
    };
    
});