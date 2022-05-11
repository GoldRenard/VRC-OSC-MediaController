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