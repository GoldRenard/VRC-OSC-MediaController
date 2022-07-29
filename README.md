# Windows Media OSC Controller and Gestures Indicator for VRChat

## What is this?

This simple tray application allows you to automatically send OSC message to your VRChat Avatar representing current state of system-wide media playback and control the playback as well.

It sends single `ToggleMusic` boolean parameter showing if anything is playing at the moment and updates it when state has changed. 
This way you can toggle something on your avatar showing that you're listening to music at the moment.

This application can also listen for incoming OSC Messages (expects boolean parameters set to true) to control the playback:
- `MediaSkipNext` will skip to next track;
- `MediaSkipPrevious` will skip to previous track;
- `MediaPause` will pause playback;
- `MediaStop` will stop playback;
- `MediaPlay` will play/unpause playback;
- `MediaTogglePlayPause` will toggle play/pause.

Parameters, OSC addresses and ports are fully configurable (check `.config` file).

## How it works?
It uses WinRT (UWP) Windows API to get control over media playback and retrieve its status.
In other words, it uses the same system-wide media interface available in Windows 10 Volume Flyout and Windows 11 Quick Settings media controls. 
This application uses parts of [SharpOSC](https://github.com/ValdemarOrn/SharpOSC) project to send/receive OSC messages.

## Additional Features

This application were made for personal use mostly so it also includes some additional features I would like to keep as single application.

### Gestures Indicator

This application includes a small OpenVR Overlay displaying VRChat gestures you're making. Think about it as simple mod-free [GesturesIndicator](https://github.com/ImTiara/VRCMods) implementation (icons credits to this mod creator).

### Movement Forwarder
This application can listen for two float OSC parameters that it will forward back to VRChat as movement input.
It allows to control avatar movement based on its user-defined parameter values that you can change any way you want. See `App.config`:
```
    <add key="OSCListenInputV" value="/avatar/parameters/MoveInputV" />
    <add key="OSCListenInputH" value="/avatar/parameters/MoveInputH" />
```

## Requirements 
- Windows 10 1809 (October 2018 Update/Build 17763) or higher, Windows 11
- .NET Framework 4.8 (only 1809 lacks this though)
- Any media application that reports playback state OS-wide.

## License

MIT License

Copyright (c) 2022 Renard Gold / 2012 Valdemar Örn Erlingsson

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.