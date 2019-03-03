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
const database = firebase.database();
let importedFromDatabase = null;

// TEMPP
// var list = ['KK9KGXMU', 'L9TFNUDZ', 'DYEUQU5H','TREGQYGT', '825DYMN4', 'BAVQX362', 'B6HXZVDB', 'QF2R8N3Z'];
// list.forEach(function(key) {
//   database.ref(key).set({
//     name: "",
//     battery: 100,
//     age: 100,
//     notificiation: {
//       mail: null,
//       social: null,
//       app: null,
//       system: null
//     },
//     last_use: Date.now()
//   });
// });
// TEMPP


function pullInDataFromFirebase() {
  database.ref().once('value').then(async function(snapshot) {
    importedFromDatabase = await snapshot.val();
  });
}
pullInDataFromFirebase();
mainloop();

function mainloop(){
  if (!importedFromDatabase) {
    setTimeout(arguments.callee, 600);
  }
  else {
    timerBasedEvents();
  }
}


function popNotification() {
  let differentNotifications = [
    'New message from Mom',
    'Important Email',
    'New Friend Request',
    'System Update',
    'Dinner with Kait 30 minutes',
    'Malware Detected',
    'Recent Purchase from Amazon',
  ];
  let randomIndex = [];
  let result = [];
  let randomCount = Math.random() * 3;
  while (randomIndex.length < randomCount) {
    let tempRand =   Math.random() * Math.floor(differentNotifications.length);
    if (!randomIndex.includes(tempRand)) {
      randomIndex.push(tempRand);
    }
  }
  for (let i = 0; i < differentNotifications.length; i++) {
    if (randomIndex.includes(i)) {
      result.push(differentNotifications[i]);
    }
  }
  return result;
}


function timerBasedEvents() {
  let timeSinceStart = 0;
  for (let id in importedFromDatabase) {
    let object = importedFromDatabase[id];
    if (object.hasOwnProperty('battery') && object.hasOwnProperty('power')) {
      if (object['power'] == 'true' && object['battery'] > 0) {
        object['battery']-= 1;
        if (object['battery'] <= 15) {
          // object['note'] = 'Please plug in your device';
        }
      }
      // if (timeSinceStart % 12 == 0) {
      //   let notifications = popNotification();
      //   object['notification'] = notifications;
      //
      // }
    }
    // if (object.hasOwnProperty('downloading')) {
    //   object['downloading'] += 1;
    // }
    // if (object.hasOwnProperty('last_use')) {
    //   object['last_use'] = Date.now();
    // }
    // if (timeSinceStart % 30000 == 0) {
    //   object['age'] += 1;
    // }
    if(timeSinceStart % 30 == 0 && object.hasOwnProperty('power')){
      if(object['power'] == 'true'){
        object['power'] = 'false';
      }else{
        object['power'] = 'true';
      }
    }

    /////////////
    database.ref(id).update(object);
  }

  // database.ref('7N9MC7MQ').once('value').then(function(snapshot) {
  //     var dict = snapshot.val();
  //      currBattery = dict['battery'];
  // });
  timeSinceStart += 6;
  setTimeout(arguments.callee, 6000); // every six seconds
}

//Anything timer-based goes below:
// (function() {
//   for (let id in localData) {
//     console.log(localData.id);
//   }
//   setTimeout(arguments.callee, 6000); // every six seconds
// })();

// (function(){
//   if(currBattery) {
//     timeSinceStart += 6
//     database.ref('7N9MC7MQ').update({
//       age: 100 + Math.floor(timeSinceStart/3), //only adds to age if a ''whole day'' passed
//       last_use: Date.now(),
//       battery: currBattery - 1
//     });
//   }
//
//   database.ref('7N9MC7MQ').once('value').then(function(snapshot) {
//     console.log(snapshot.val());
//   });
//   //updates battery
//   database.ref('7N9MC7MQ').once('value').then(function(snapshot) {
//       var dict = snapshot.val();
//        currBattery = dict['battery'];
//   });
//   setTimeout(arguments.callee, 6000);
//
// })();
