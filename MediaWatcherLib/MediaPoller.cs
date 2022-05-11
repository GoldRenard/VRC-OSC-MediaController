using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Media.Control;
using System.Threading;

namespace MediaWatcherLib {
    public class MediaPoller {

        public class MediaEventArgs : EventArgs {
            public bool IsPlaying {
                get; set;
            }
        }

        private Timer _timer;

        public event EventHandler<MediaEventArgs> MediaChanged;

        private GlobalSystemMediaTransportControlsSessionPlaybackStatus PlaybackStatus = GlobalSystemMediaTransportControlsSessionPlaybackStatus.Closed;

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
            var status = GetStatus();
            if (PlaybackStatus != status) {
                MediaChanged.Invoke(this, new MediaEventArgs() { IsPlaying = status == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing });
            }
            PlaybackStatus = status;
            Thread.Sleep(2000);
        }

        private GlobalSystemMediaTransportControlsSessionPlaybackStatus GetStatus() {
            var task = Task.Run(async () => await GetStatusAsync());
            return task.Result;
        }

        private async Task<GlobalSystemMediaTransportControlsSessionPlaybackStatus> GetStatusAsync() {
            var sessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            var currentSession = sessionManager.GetCurrentSession();
            if (currentSession != null) {
                return currentSession.GetPlaybackInfo().PlaybackStatus;
            }
            return GlobalSystemMediaTransportControlsSessionPlaybackStatus.Closed;
        }
    }
}
