"use strict";

// Start the main app logic.
requirejs([
    'hft/commonui',
    'hft/gameclient',
    'hft/misc/input',
    'hft/misc/misc',
    'hft/misc/mobilehacks',
    'hft/misc/touch',
  ], function(
    CommonUI,
    GameClient,
    Input,
    Misc,
    MobileHacks,
    Touch) {

  var globals = {
    debug: false,
  };
  Misc.applyUrlSettings(globals);
  MobileHacks.fixHeightHack();

  var score = 0;
  var statusElem = document.getElementById("gamestatus");
  var inputElem = document.getElementById("inputarea");
  var colorElem = document.getElementById("display");
  var client = new GameClient();
  
  var button1 = document.getElementById("button1");
  var button2 = document.getElementById("button2");
  var button3 = document.getElementById("button3");
  var button4 = document.getElementById("button4");
  var button5 = document.getElementById("button5");
  var button6 = document.getElementById("button6");
  var button7 = document.getElementById("button7");
  var button8 = document.getElementById("button8");
  var button9 = document.getElementById("button9");

             
         
         button1.onclick = function()
       {
           console.log("workin'"); //chrome console
           client.sendCmd('consolePrint', {//will go to unity, when set up correctly
            thingToPrint: "1",
                buttonNumber : 1
            });
              }
        
        button2.onclick = function()
       {
           console.log("workin'"); //chrome console
           client.sendCmd('consolePrint', {//will go to unity, when set up correctly
            thingToPrint: "2",
                buttonNumber : 2
            });
              }
        
        button3.onclick = function()
       {
           console.log("workin'"); //chrome console
           client.sendCmd('consolePrint', {//will go to unity, when set up correctly
            thingToPrint: "3",
                buttonNumber : 3
            });
              }
              
         button4.onclick = function()
       {
           console.log("workin'"); //chrome console
           client.sendCmd('consolePrint', {//will go to unity, when set up correctly
            thingToPrint: "4",
                buttonNumber : 4
            });
           
       };
              
              button5.onclick = function()
       {
           console.log("workin'"); //chrome console
           client.sendCmd('consolePrint', {//will go to unity, when set up correctly
            thingToPrint: "5",
                buttonNumber : 5
            });
           
       };
              
                button6.onclick = function()
       {
           console.log("workin'"); //chrome console
           client.sendCmd('consolePrint', {//will go to unity, when set up correctly
            thingToPrint: "6",
                buttonNumber : 6
            });
           
       };
        
                button7.onclick = function()
       {
           console.log("workin'"); //chrome console
           client.sendCmd('consolePrint', {//will go to unity, when set up correctly
            thingToPrint: "7",
                buttonNumber : 7
            });
           
       };
           
                button8.onclick = function()
       {
           console.log("workin'"); //chrome console
           client.sendCmd('consolePrint', {//will go to unity, when set up correctly
            thingToPrint: "8",
                buttonNumber : 8
            });
           
       };
              
                button9.onclick = function()
       {
           console.log("workin'"); //chrome console
           client.sendCmd('consolePrint', {//will go to unity, when set up correctly
            thingToPrint: "9",
                buttonNumber : 9
            });
           
       };
   

  // Note: CommonUI handles these events for almost all the samples.
  var onConnect = function() {
    statusElem.innerHTML = "you've connected to the relayserver";
  };

  var onDisconnect = function() {
    statusElem.innerHTML = "you were disconnected from the relayserver";
  }

  // If I was going to handle this without CommonUI this is what I'd do
  //client.addEventListener('connect', onConnect);
  //client.addEventListener('disconnect', onDisconnect);

  // Because I want the CommonUI to work
  globals.disconnectFn = onDisconnect;
  globals.connectFn = onConnect;

  CommonUI.setupStandardControllerUI(client, globals);

  var randInt = function(range) {
    return Math.floor(Math.random() * range);
  };

  // Sends a move command to the game.
  //
  // This will generate a 'move' event in the corresponding
  // NetPlayer object in the game.
  var sendMoveCmd = function(position, target) {
    client.sendCmd('move', {
      x: position.x / target.clientWidth,
      y: position.y / target.clientHeight,
    });
  };

  // Pick a random color
  var color =  'rgb(' + randInt(256) + "," + randInt(256) + "," + randInt(256) + ")";
  // Send the color to the game.
  //
  // This will generate a 'color' event in the corresponding
  // NetPlayer object in the game.
  client.sendCmd('color', {
    color: color,
  });
  

          // receive a message from unity
  client.addEventListener('messageToPhone', function(cmd) {
   console.log(cmd.message);
  });
        
          // Update our score when the game tells us.
  client.addEventListener('shape', function(cmd) {
  	console.log("shape result:" + cmd.shape);
  	statusElem.innerHTML = "Got a " + cmd.shape;
    //score += cmd.points;
    //statusElem.innerHTML = "You scored: " + cmd.points + " total: " + score;
  });
        
  // Update our score when the game tells us.
  client.addEventListener('scored', function(cmd) {
    score += cmd.points;
    statusElem.innerHTML = "You scored: " + cmd.points + " total: " + score;
  });
});
