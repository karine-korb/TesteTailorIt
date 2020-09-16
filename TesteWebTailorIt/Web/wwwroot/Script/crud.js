angular.module('myApp').controller('employee', function ($scope, $http) {

    id = localStorage.idSearch;
    $scope.get = function(){
        $http.get("https://localhost:44330/api/Employees/Get/"+id)
            .then(function (response) {
                if (response) {
                    $scope.employee = response.data;
                    $scope.employee.birthDate = new Date(response.data.birthDate);
                }
            });
    }
    
    $scope.get();

    $scope.save = function () {
        $http.post("https://localhost:44330/api/Employees/Insert/", $scope.employee)
            .then(function (response) {
                if(response)
                    console.log("ok")
            })
    }

    //$scope.employee = {
    //    id: 1,
    //    firstName: "Karine",
    //    lastName: "Fecury ParoschiKorb",
    //    birthDate: "1995-08-09",
    //    email: "karine.paroschi@hotmail.com",
    //    gender: "F",
    //    habilities: "C#, HTML, CSS, AngularJS, SQL"
    //};
});