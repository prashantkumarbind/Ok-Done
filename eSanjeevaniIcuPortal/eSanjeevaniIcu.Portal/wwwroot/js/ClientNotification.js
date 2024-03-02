"use strict"
var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();
connection.start();
connection.on("ReceiveMessage", function (msg) {
    var li = document.createElement("li");
    li.textContent = msg;
    document.getElementById("msgList").appendChild(li);
    $(document).ready(function () {
        $('#divBell').show();
        $('#myAudio')[0].load();
        $('#myAudio')[0].play();
    })
})
connection.on("ClientMessage", function (msg) {
    var lii = document.createElement("lii");
    lii.textContent = msg;
    document.getElementById("ClimsgList").appendChild(lii);
    //$(document).ready(function () {
    //    $('#divBell').show();
    //    $('#myAudio')[0].load();
    //    $('#myAudio')[0].play();
    //})
})

connection.on("SenderMessage", function (FirstP, SecondP) {
    var li = document.createElement("li");
    li.textContent = "Calling " + FirstP + "...";
    document.getElementById("CallerName").appendChild(li);
    $(document).ready(function () {
        $('#divBell').show();
        $('#myAudio')[0].load();
        $('#myAudio')[0].play();
        $('#hdnId').val(SecondP);
        //$('#hdnauthToken').val(ThirdP);
    })
})