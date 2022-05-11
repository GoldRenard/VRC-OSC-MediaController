# Windows Media OSC Monitor for VRChat

## What is this?
This simple console app allows you to send OSC message representing current state of system-wide media playback to your VRChat Avatar.
It sends single `ToggleMusic` parameter that can be treated as bool showing if anything is playing at the moment. 
This way you can toggle something on your avatar showing that you're listening to music at the moment.

## How it works?
It polls WinRT (UWP) Windows API to get current playing media state with some short interval. 
In other words, it gets the same system-wide media state available in Windows 10 Volume Flyout and Windows 11 Quick Settings media controls. 
This application uses parts of [SharpOSC](https://github.com/ValdemarOrn/SharpOSC) project to send OSC actual messages.

## Requirements 
- Windows 10 1809 (October 2018 Update/Build 17763) or higher, Windows 11
- .NET Framework 4.8 (only 1809 lacks this though)
- Any media application that reports playback state OS-wide.