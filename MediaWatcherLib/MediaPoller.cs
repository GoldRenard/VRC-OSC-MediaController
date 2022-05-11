using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Control;

namespace MediaWatcherLib {
    public class MediaPoller {

        public class MediaEventArgs : EventArgs {
            public bool IsPlaying {
                get; set;
            }
            public string Artist {
                get; set;
            }
            public string Title {
                get; set;
            }
        }

        public class MediaData {
            public GlobalSystemMediaTransportControlsSessionPlaybackStatus Status {
                get; set;
            }
            public string Artist {
                get; set;
            }
            public string Title {
                get; set;
            }
        }

        private Timer _timer;

        public event EventHandler<MediaEventArgs> MediaChanged;

        private GlobalSystemMediaTransportControlsSessionPlaybackStatus _currentStatus = GlobalSystemMediaTransportControlsSessionPlaybackStatus.Closed;

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
                MediaChanged.Invoke(this, new MediaEventArgs() {
                    IsPlaying = data.Status == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing,
                    Artist = data.Artist,
                    Title = data.Title
                });
            }
            _currentStatus = data.Status;
        }

        private MediaData GetMediaData() {
            var task = Task.Run(async () => await GetMediaDataAsync());
            return task.Result;
        }

        private async Task<MediaData> GetMediaDataAsync() {
            var sessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            var currentSession = sessionManager.GetCurrentSession();
            if (currentSession != null) {
                var mediaProperties = await currentSession.TryGetMediaPropertiesAsync();
                return new MediaData() {
                    Status = currentSession.GetPlaybackInfo().PlaybackStatus,
                    Artist = mediaProperties?.Artist,
                    Title = mediaProperties?.Title
                };
            }
            return new MediaData() { Status = GlobalSystemMediaTransportControlsSessionPlaybackStatus.Closed };
        }
    }
}
