using Windows.Media.Control;

namespace MediaWatcherLib {
    class PlaybackInfo {
        public GlobalSystemMediaTransportControlsSessionPlaybackStatus Status {
            get; set;
        }
        public string Artist {
            get; set;
        }
        public string Title {
            get; set;
        }

        public MediaEventArgs CreateEvent() {
            return new MediaEventArgs() {
                IsPlaying = Status == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing,
                Artist = Artist,
                Title = Title
            };
        }
    }
}
