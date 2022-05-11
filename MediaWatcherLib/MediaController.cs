
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
using System.Threading.Tasks;
using MediaWatcherLib.OSC;

namespace MediaWatcherLib {
    public class MediaController {

        private UDPListener oscListener;

        public void Start() {
            oscListener = new UDPListener(9001, OnOscPacket);
        }

        public void Shutdown() {
            oscListener.Close();
        }

        private void OnOscPacket(OscPacket packet) {
            if (packet == null) {
                return;
            }
            var message = packet as OscMessage;
            if (message == null || message.Arguments == null || message.Arguments.Count == 0) {
                return;
            }

            var value = message.Arguments[0];
            if (value == null || !(value is bool boolean) || !boolean) {
                return;
            }

            Task.Run(async () => {
                switch (message.Address) {
                    case "/avatar/parameters/MediaSkipNext":
                        await MediaAccessor.TrySkipNextAsync();
                        break;
                    case "/avatar/parameters/MediaSkipPrevious":
                        await MediaAccessor.TrySkipPreviousAsync();
                        break;
                    case "/avatar/parameters/MediaPause":
                        await MediaAccessor.TryPauseAsync();
                        break;
                    case "/avatar/parameters/MediaStop":
                        await MediaAccessor.TryStopAsync();
                        break;
                    case "/avatar/parameters/MediaPlay":
                        await MediaAccessor.TryPlayAsync();
                        break;
                    case "/avatar/parameters/MediaTogglePlayPause":
                        await MediaAccessor.TryTogglePlayPauseAsync();
                        break;
                }
            });
        }
    }
}
