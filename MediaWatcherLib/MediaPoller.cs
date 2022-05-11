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
