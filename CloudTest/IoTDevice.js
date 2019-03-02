var id = "";

var firebase = require("firebase");
  var config = {
    apiKey: "AIzaSyCnuNoVDI1yew7A9bWogQoj-xpo5e3ZTpk",
    authDomain: "sh-cloud-test.firebaseapp.com",
    databaseURL: "https://sh-cloud-test.firebaseio.com",
    projectId: "sh-cloud-test",
    storageBucket: "sh-cloud-test.appspot.com",
    messagingSenderId: "1071401105941"
  };

firebase.initializeApp(config);
firebase.database().ref('7N9MC7MQ').set({
    name: "Bert's MacBook Air",
    age: 100,
    last_use: Date.now(),
    battery: 90
  });

var database = firebase.database();
database.ref('7N9MC7MQ').once('value').then(function(snapshot) {
	console.log(snapshot.val());});