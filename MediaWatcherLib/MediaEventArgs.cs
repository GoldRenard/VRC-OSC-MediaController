using System;

namespace MediaWatcherLib {

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
}
