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

namespace MediaWatcherLib {
    public class MediaPoller {

        private Timer _timer;

        private GlobalSystemMediaTransportControlsSessionPlaybackStatus _currentStatus = GlobalSystemMediaTransportControlsSessionPlaybackStatus.Closed;

        public event EventHandler<MediaEventArgs> MediaChanged;

        protected virtual void OnMediaEventArgs(MediaEventArgs e) {
            MediaChanged?.Invoke(this, e);
        }

        public void Start() {
            _timer = new Timer(Poll, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
        }

        public void Shutdown() {
            _timer.Dispose();
        }

        private void Poll(object sender) {
            var data = GetMediaData();
            if (_currentStatus != data.Status) {
                MediaChanged.Invoke(this, data.CreateEvent());
            }
            _currentStatus = data.Status;
        }

        private PlaybackInfo GetMediaData() {
            var task = Task.Run(async () => await GetMediaDataAsync());
            return task.Result;
        }

        private async Task<PlaybackInfo> GetMediaDataAsync() {
            var sessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            var currentSession = sessionManager.GetCurrentSession();
            if (currentSession != null) {
                var mediaProperties = await currentSession.TryGetMediaPropertiesAsync();
                return new PlaybackInfo() {
                    Status = currentSession.GetPlaybackInfo().PlaybackStatus,
                    Artist = mediaProperties?.Artist,
                    Title = mediaProperties?.Title
                };
            }
            return new PlaybackInfo() { Status = GlobalSystemMediaTransportControlsSessionPlaybackStatus.Closed };
        }
    }
}
