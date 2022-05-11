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
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Control;

namespace MediaControllerLib {
    public class MediaPoller {

        private GlobalSystemMediaTransportControlsSessionPlaybackStatus? _currentStatus = null;

        public event EventHandler<MediaEventArgs> MediaChanged;

        private CancellationTokenSource cancellationToken = null;

        protected virtual void OnMediaEventArgs(MediaEventArgs e) {
            MediaChanged?.Invoke(this, e);
        }

        public void Start() {
            cancellationToken = new CancellationTokenSource();
            CancellationToken token = cancellationToken.Token;

            var task = Task.Run(async () => {
                token.ThrowIfCancellationRequested();

                while (true) {
                    var data = await MediaAccessor.GetPlaybackInfoAsync();
                    if (_currentStatus == null || _currentStatus != data.Status) {
                        MediaChanged.Invoke(this, data.CreateEvent());
                        _currentStatus = data.Status;
                    }
                    await Task.Delay(1000);
                    GC.Collect();
                    if (token.IsCancellationRequested) {
                        token.ThrowIfCancellationRequested();
                    }
                }
            }, token);
        }

        public void Shutdown() {
            cancellationToken?.Cancel();
            cancellationToken = null;
        }
    }
}
