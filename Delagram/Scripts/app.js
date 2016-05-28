var app = angular.module("PostApp", []);

app.controller("PostsController", function ($http) {

    var self = this;

    self.posts = [];

    self.getPosts = function () {
        $http.get("/api/Post")
            .then(function (response) {
                self.posts.push(response.data);
            })
    }
    setTimeout(self.getPosts(), 10000);


})