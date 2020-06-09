// ==UserScript==
// @name         YouTubeMusic Transmitter
// @version      0.2
// @description  Stream your YouTube Music name & position to Discord
// @author       Radolyn
// @license      Apache-2.0
// @match        https://music.youtube.com/*
// @supportURL   https://github.com/Radolyn/YTMusicDSPresence
// @updateURL    https://github.com/Radolyn/YTMusicDSPresence/raw/master/youtube_music.user.js
// @downloadURL  https://github.com/Radolyn/YTMusicDSPresence/raw/master/youtube_music.user.js
// @homepage     https://github.com/Radolyn/YTMusicDSPresence
// @namespace    https://github.com/Radolyn/YTMusicDSPresence
// @require      https://momentjs.com/downloads/moment.js
// @run-at       document-end
// @grant        GM_xmlhttpRequest
// ==/UserScript==

(function () {
  "use strict";
  function sendActivity(data2) {
    GM_xmlhttpRequest({
      method: "POST",
      url: "http://localhost:1339/presence/",
      data: JSON.stringify(data2),
      headers: {
        "Content-Type": "application/json",
      },
    });
  }
  function getSong() {
    return document.getElementsByClassName(
      "title style-scope ytmusic-player-bar"
    )[0].innerText;
  }
  function getDuration() {
    return document
      .getElementsByClassName("time-info style-scope ytmusic-player-bar")[0]
      .innerText.split("/");
  }
  function getEnd() {
    var array = getDuration();
    var diff1;
    var diff2;
    var time = Math.round(new Date() / 1);
    if (array[0].split(":").length === 2) {
      diff1 = moment(array[0], "mm:ss").unix();
    } else {
      diff1 = moment(array[0], "HH:mm:ss").unix();
    }
    if (array[1].split(":").length === 2) {
      diff2 = moment(array[1], "mm:ss").unix();
    } else {
      diff2 = moment(array[1], "HH:mm:ss").unix();
    }
    var end = diff2 * 1000 - diff1 * 1000 + time;
    return [time, end];
  }
  function getArtist() {
    return document
      .getElementsByClassName("byline style-scope ytmusic-player-bar")[0]
      .title.replace(" • ", "");
  }
  function getState() {
    var aria = document
      .getElementsByClassName(
        "play-pause-button style-scope ytmusic-player-bar"
      )[0]
      .ariaLabel.toLowerCase();
    return "play" === aria;
  }
  function getId() {
    var link = window.location.href;
    var sliceBefore = function (str, pattern) {
      return str.slice(str.indexOf(pattern) + pattern.length);
    };
    var sliceAfter = function (str, pattern) {
      return str.slice(0, str.indexOf(pattern));
    };
    return sliceAfter(sliceBefore(link, "watch?v="), "&list=");
  }
  setInterval(function () {
    var song = getSong();
    var times = getEnd();
    var artist = getArtist();
    var state = getState();
    var id = getId();
    var array = {
      song: song,
      id: id,
      artist: artist,
      start: times[0],
      end: times[1],
      paused: state,
    };
    sendActivity(array);
  }, 1000);
})();
