# YTMusicDSPresence

Discord presence for YouTube Music

## Installation

You need to install TamperMonkey ([Chrome](https://chrome.google.com/webstore/detail/tampermonkey/dhdgffkkebhmkfjojejmpbldmpobfkfo)\\[Opera](https://addons.opera.com/ru/extensions/details/tampermonkey-beta)\\[Firefox](https://addons.mozilla.org/ru/firefox/addon/tampermonkey/))

Click to the link and install script:

[youtube_music.user.js](https://github.com/Radolyn/YTMusicDSPresence/raw/master/youtube_music.user.js)

Download proxy from [Releases](https://github.com/Radolyn/YTMusicDSPresence/releases) tab

Double click on it & start listening your favourite music from YouTube Music

## Screenshot

![Screenshot](https://radolyn.com/shared/youtube_music.png)

## Configuration

```conf
applicationId=718862357282947144 # Application id
port=1339 # Port
timeout=5000 # Browser timeout (if we can't get data >timeout, then we're cleaning rich presence)
largeImageKey=1200px-youtube_music_logo_svg # Big icon
largeImageText=YouTube Music # Big icon text
paused=pause_v2 # Paused icon
playing=play # Playing icon
pausedText=Простаивает # Paused text
playingText=Слушает # Playing text
enableJoinRequestSounds=true # Enable join request sounds
enableAutoUrlOpen=true # Open YouTube Music on join
enableInviteFeature=true # Enable invite feature (your friends need to download this app too)
hideConsole=false # Hide app console
```

## Bugs

- Works on English only
