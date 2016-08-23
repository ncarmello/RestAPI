function ViewModel() {
    var self = this;
    $("#userInfo").hide();
    var tokenKey = 'accessToken';

    /*****************************************************************/
    self.selectedState = ko.observable();
    self.selectedCities = ko.observableArray();
    self.selectedUser = ko.observable();
    self.users = ko.observableArray();
    self.userInfo = {
        ID: ko.observable(),
        FirstName: ko.observable(),
        LastName: ko.observable(),
        DateAdded: ko.observable(),
        DateTimeAdded: ko.observable(),
        LastUpdated: ko.observable()
    }
    self.loggedInUser = ko.observable();
    self.username = ko.observable('Please select a user');
    self.error = ko.observable();
    self.cities = ko.observableArray();
    self.citiesInState = ko.observableArray();
    self.visitedStates = ko.observableArray();
    self.states = ko.observableArray();
    self.currentUser = ko.observable();

    var userObj;

    var userID = 0;
    var usersUri = "/user/";
    var statesUri = "/state/";
    var userUri = "/user/{0}/";
    var citiesUri = "/user/{0}/visits/";
    var visitedStatesUri = "/user/{0}/visits/states/";
    var citiesInStateUri = "/state/{0}/cities/";
    var deleteVisitUri = "/user/{0}/visit/{1}/";
    var visitCityUri = "/user/{0}/visit/{1}/";
    var visitCitiesUri = "/user/{0}/visits/";

    var locations = new Array();
/**********************************/
    self.result = ko.observable();
    self.user = ko.observable();

    self.registerEmail = ko.observable();
    self.registerPassword = ko.observable();
    self.registerPassword2 = ko.observable();

    self.loginEmail = ko.observable();
    self.loginPassword = ko.observable();

    function showError(jqXHR) {
        self.result(jqXHR.status + ': ' + jqXHR.statusText);
    }

    self.callApi = function () {
        self.result('');

        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'GET',
            url: '/api/hello',
            headers: headers
        }).done(function (data) {
            self.result(data);
        }).fail(showError);
    }

    self.register = function () {
        self.result('');

        var data = {
            Email: self.registerEmail(),
            Password: self.registerPassword(),
            ConfirmPassword: self.registerPassword2()
        };

        $.ajax({
            type: 'POST',
            url: '/api/Account/Register',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data)
        }).done(function (data) {
            self.result("Done!");
        }).fail(showError);
    }

    self.login = function () {
        self.result('');

        var loginData = {
            grant_type: 'password',
            username: self.loginEmail(),
            password: self.loginPassword()
        };

        $.ajax({
            type: 'POST',
            url: '/Token',
            data: loginData
        }).done(function (data) {
            self.user(data.userName);
            // Cache the access token in session storage.
            sessionStorage.setItem(tokenKey, data.access_token);
            //self.getVisits(item);
        }).success(function () {
            /*ajaxHelper("/user/login/", 'GET').done(function (data) {
                alert(ko.toJSON(data));
                self.userInfo = data;
                self.username(data['FirstName'] + " " + data['LastName']);
            });*/
            $("#login").hide();
            $("#userInfo").show();
        }).fail(showError);
    }

    self.logout = function () {
        self.user('');
        sessionStorage.removeItem(tokenKey)
    }
    /*****************************************************/
    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        return $.ajax({
            type: method,
            url: uri,
            headers: headers,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    function format(str, col) {
        col = typeof col === 'object' ? col : Array.prototype.slice.call(arguments, 1);

        return str.replace(/\{\{|\}\}|\{(\w+)\}/g, function (m, n) {
            if (m == "{{") { return "{"; }
            if (m == "}}") { return "}"; }
            return col[n];
        });
    }

    function getSessionUser() {
        var token = sessionStorage.getItem(tokenKey);
        if (token) {
            $("#login").hide();
            $("#userInfo").show();
        }
    }

    function getUsers() {
        ajaxHelper(usersUri, 'GET').done(function (data) {
            self.users(data);
        });
    }

    function getStates() {
        //alert('getstates!!!');
        ajaxHelper(statesUri, 'GET').done(function (data) {
            self.states(data);
        });
    }

    function getCitiesInit() {
        ajaxHelper(format(citiesInStateUri, 1), 'GET').done(function (data) {
            self.citiesInState(data);
        });
    }

    self.getVisits = function (item) {
        ajaxHelper(format(userUri, item.ID), 'GET').done(function (data) {
            self.userInfo.ID = data['ID'];
            self.userInfo.FirstName = data['Firstname'];
            self.userInfo.LastName = data['Lastname'];
            self.userInfo.DateAdded = data['DateAdded'];
            self.userInfo.DateTimeAdded = data['DateTimeAdded'];
            self.userInfo.LastUpdated = data['LastUpdated'];
            self.username = self.user.FirstName + " " + self.user.LastName;
        });
        ajaxHelper(format(citiesUri, item.ID), 'GET').done(function (data) {
            self.cities(data);
            clearVisits();
            for (i = 0; i < self.cities().length; i++) {
                var pt = self.cities()[i];
                locations.push(pt);
            }
            plotVisits();
        });
        ajaxHelper(format(visitedStatesUri, item.ID), 'GET').done(function (data) {
            self.visitedStates(data);
        });
        userID = item.ID;
    }

    self.getStateVisits = function (item) {
        ajaxHelper(format(visitedStatesUri, item.ID), 'GET').done(function (data) {
            self.states(data);
        });
    }

    self.getCitiesInState = function (item) {
        ajaxHelper(format(citiesInStateUri, item.ID), 'GET').done(function (data) {
            self.citiesInState(data);
        });
    }
    self.userSelected = function (data, event) {
        var userid = $('#userSelect').find(":selected").val();
        ajaxHelper(format(userUri, userid), 'GET').done(function (data) {
            self.getVisits(data);
        });
    }

    self.stateSelected = function (data, event) {
        var stateid = $('#stateSelect').find(":selected").val();
        ajaxHelper(format(citiesInStateUri, stateid), 'GET').done(function (data) {
            self.citiesInState(data);
        });
    }
    self.citiesSelected = function (data, event) {
        var selectedCities = [];
        $('#citiesSelect :selected').each(function (i, selected) {
            selectedCities[i] = $(selected).text();
        });
        var stateName = $('#stateSelect').find(":selected").text();
        var cityArray = new Array();
        for (i = 0; i < selectedCities.length; i++) {
            var city = { City: selectedCities[i], State: stateName };
            cityArray.push(city);
        }
        alert(ko.toJSON(cityArray));
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        $.ajax(format(visitCitiesUri, userID), {
            headers: headers,
            data: ko.toJSON(cityArray),
            type: "post", contentType: "application/json",
            success: function (data) {
                alert('Saved selected visits!');
                userID = data['ID'];
                var item = { ID: userID };
                self.getVisits(item);
            },
            error: function(data) {
                alert('The user is not Authorized to add visits!');
            }
        });
    }

    self.deleteVisit = function (item) {
        if (confirm(format('Are you sure you want to delete the visit to {0}, {1}?', item.City, item.Abbr))) {
            var token = sessionStorage.getItem(tokenKey);
            var headers = {};
            if (token) {
                headers.Authorization = 'Bearer ' + token;
            }
            $.ajax(format(deleteVisitUri, item.UsersID, item.VisitID), {
                headers: headers,
                type: "delete", contentType: "application/json",
                success: function (data) {
                    alert('Removed selected visit!');
                    userID = data['ID'];
                    var item = { ID: userID };
                    self.getVisits(item);
                },
                error: function (data) {
                    alert('The user is not Authorized to remove visits!');
                }
            });
            /*
            ajaxHelper(format(deleteVisitUri, item.UsersID, item.VisitID), 'DELETE').done(function (data) {
                var user = { ID: data['ID'] };
                self.getVisits(user);
            });*/
        }
    }

    self.visitCity = function (item) {
        ajaxHelper(format(visitCityUri, userID, item.ID), 'POST').done(function (data) {
            alert('Added Visit to ' + data.City + ', ' + data.Abbr);
            self.cities.push(data);
        });
    }

    // Fetch the initial data.
    getSessionUser();
    getUsers();
    getStates();
    getCitiesInit();

    /********************************************************************************************
    // In the following example, markers appear when the user clicks on the map.
    // The markers are stored in an array.
    // The user can then click an option to hide, show or delete the markers.
    var map;
    var markers = [];

    function initMap() {
        var usa = { lat: 39.909736, lng: -98.522109 };

        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 4,
            center: usa,
            mapTypeId: 'terrain'
        });

        // This event listener will call addMarker() when the map is clicked.
        map.addListener('click', function (event) {
            addMarker(event.latLng);
        });

        // Adds a marker at the center of the map.
        addMarker(usa);
    }

    function plotVisits() {
        for (i = 0; i < locations.length; i++) {
            addMarker(locations[i]);
        }
    }

    // Adds a marker to the map and push to the array.
    function addMarker(location) {
        var marker = new google.maps.Marker({
            position: new google.maps.LatLng(location.Latitude, location.Longitude),
            map: map
        });
        markers.push(marker);
    }

    // Sets the map on all markers in the array.
    function setMapOnAll(map) {
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(map);
        }
    }

    // Removes the markers from the map, but keeps them in the array.
    function clearMarkers() {
        setMapOnAll(null);
    }

    // Shows any markers currently in the array.
    function showMarkers() {
        setMapOnAll(map);
    }

    // Deletes all markers in the array by removing references to them.
    function deleteMarkers() {
        alert('deleting markers');
        clearMarkers();
        markers = [];
    }


    ********************************************************************************************/

    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 4,
        center: new google.maps.LatLng(39.909736, -98.522109),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });

    var infowindow = new google.maps.InfoWindow();

    function clearVisits() {
        locations = [];
        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 4,
            center: new google.maps.LatLng(39.909736, -98.522109),
            mapTypeId: google.maps.MapTypeId.ROADMAP
        });
    }

    function plotVisits() {
        var marker, i;
        for (i = 0; i < locations.length; i++) {
            marker = new google.maps.Marker({
                position: new google.maps.LatLng(locations[i].Latitude, locations[i].Longitude),
                map: map
            });

            google.maps.event.addListener(marker, 'click', (function (marker, i) {
                return function () {
                    infowindow.setContent(locations[i][0]);
                    infowindow.open(map, marker);
                }
            })(marker, i));
        }
    }
    /*****************************************************/
}

var app = new ViewModel();
ko.applyBindings(app);