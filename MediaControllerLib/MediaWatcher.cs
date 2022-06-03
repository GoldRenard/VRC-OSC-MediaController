
/*
MIT License

Copyright (c) 2022 Renard Gold

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
*/
using System;
using MediaControllerLib.OSC;

namespace MediaControllerLib {
    public class MediaWatcher {

        public event EventHandler<MediaEventArgs> MediaChanged;

        private MediaPoller _poller;
        private UDPSender _sender;

        private readonly string _oscAddress;
        private readonly int _oscPort;
        private readonly string _targetParameter;

        public MediaWatcher(string oscAddress, int oscPort, string targetParameter) {
            _oscAddress = oscAddress;
            _oscPort = oscPort;
            _targetParameter = targetParameter;
        }

        public void Start() {
            _sender = new UDPSender(_oscAddress, _oscPort);
            _poller = new MediaPoller();
            _poller.MediaChanged += OnMediaChanged;
            _poller.Start();
        }

        public void Shutdown() {
            _poller?.Shutdown();
            _sender?.Close();
            _poller = null;
            _sender = null;
        }

        private void OnMediaChanged(object sender, MediaEventArgs e) {
            _sender.Send(new OscMessage(_targetParameter, e.IsPlaying ? 1 : 0));
            MediaChanged.Invoke(sender, e);
        }

        public void SendMessage(string parameterName, float value) {
            _sender.Send(new OscMessage(parameterName, value));
        }
    }
}
