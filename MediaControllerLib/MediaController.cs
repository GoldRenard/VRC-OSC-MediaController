
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
using System.Threading.Tasks;
using MediaControllerLib.OSC;

namespace MediaControllerLib {
    public class MediaController {

        private UDPListener _oscListener;
        private readonly int _oscListenPort;
        private readonly string _oscListenSkipNextParameter;
        private readonly string _oscListenSkipPreviousParameter;
        private readonly string _oscListenPauseParameter;
        private readonly string _oscListenPlayParameter;
        private readonly string _oscListenStopParameter;
        private readonly string _oscListenTogglePlayPauseParameter;

        public event EventHandler<OscMessage> OnOscMessage;

        public MediaController(
            int oscListenPort,
            string oscListenSkipNextParameter,
            string oscListenSkipPreviousParameter,
            string oscListenPauseParameter,
            string oscListenPlayParameter,
            string oscListenStopParameter,
            string oscListenTogglePlayPauseParameter) {
            _oscListenPort = oscListenPort;
            _oscListenSkipNextParameter = oscListenSkipNextParameter;
            _oscListenSkipPreviousParameter = oscListenSkipPreviousParameter;
            _oscListenPauseParameter = oscListenPauseParameter;
            _oscListenPlayParameter = oscListenPlayParameter;
            _oscListenStopParameter = oscListenStopParameter;
            _oscListenTogglePlayPauseParameter = oscListenTogglePlayPauseParameter;
        }

        public void Start() {
            _oscListener = new UDPListener(_oscListenPort, OnOscPacket);
        }

        public void Shutdown() {
            _oscListener?.Close();
        }

        private void OnOscPacket(OscPacket packet) {
            if (packet == null) {
                return;
            }
            var message = packet as OscMessage;
            if (message == null
                || message.Address == null
                || message.Arguments == null
                || message.Arguments.Count == 0) {
                return;
            }

            if (OnOscMessage != null) {
                try {
                    OnOscMessage(this, message);
                } catch {
                    // ignore, we don't care
                }
            }

            var value = message.Arguments[0];
            if (value == null || !(value is bool boolean) || !boolean) {
                return;
            }

            Task.Run(async () => {
                var address = message.Address;

                if (address.Equals(_oscListenSkipNextParameter)) {
                    await MediaAccessor.TrySkipNextAsync();
                } else if (address.Equals(_oscListenSkipPreviousParameter)) {
                    await MediaAccessor.TrySkipPreviousAsync();
                } else if (address.Equals(_oscListenPauseParameter)) {
                    await MediaAccessor.TryPauseAsync();
                } else if (address.Equals(_oscListenPlayParameter)) {
                    await MediaAccessor.TryPlayAsync();
                } else if (address.Equals(_oscListenStopParameter)) {
                    await MediaAccessor.TryStopAsync();
                } else if (address.Equals(_oscListenTogglePlayPauseParameter)) {
                    await MediaAccessor.TryTogglePlayPauseAsync();
                }
            });
        }
    }
}
