﻿// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in cordova-simulate or on Android devices/emulators: launch your app, set breakpoints, 
// and then run "window.location.reload()" in the JavaScript Console.
(function () {
    "use strict";

    document.addEventListener('deviceready', onDeviceReady.bind(this), false);

    function onDeviceReady() {
        // Handle the Cordova pause and resume events
        document.addEventListener( 'pause', onPause.bind( this ), false );
        document.addEventListener('resume', onResume.bind(this), false);
        document.getElementById("cameraTakePicture").addEventListener("click", cameraTakePicture); 
        
        // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
        var parentElement = document.getElementById('deviceready');
        var listeningElement = parentElement.querySelector('.listening');
        var receivedElement = parentElement.querySelector('.received');
        listeningElement.setAttribute('style', 'display:none;');
        receivedElement.setAttribute('style', 'display:block;');
    };

    function cameraTakePicture() {
        console.log("triggered");
        navigator.camera.getPicture(onSuccess, onFail, {
            encodingType: Camera.EncodingType.JPEG,
            quality: 50,
            targetWidth: 896,
            targetHeight: 504,
            correctOrientation: false,
            destinationType: Camera.DestinationType.DATA_URL
        });

        function onSuccess(imageData) {
            var image = document.getElementById('myImage');
            image.src = "data:image/jpeg;base64," + imageData;
            console.log(imageData)
            var url = "http://192.168.137.54:5000/circuit/image/mobile";

            var obj = {};
            obj["image"] = image.src;

            var req = new XMLHttpRequest();
            req.open("POST", url, true);
            req.setRequestHeader("Content-Type", "application/json");
            req.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 201) {
                    console.log(req.responseText);
                }
            };
            req.send(JSON.stringify(obj));
        }

        function onFail(message) {
            alert('Failed because: ' + message);
        }
    }

    function onPause() {
        // TODO: This application has been suspended. Save application state here.
    };

    function onResume() {
        // TODO: This application has been reactivated. Restore application state here.
    };
} )();