
using System;
using MediaWatcherLib.OSC;

namespace MediaWatcherLib {
    public class MediaWatcher {

        public event EventHandler<MediaEventArgs> MediaChanged;

        private MediaPoller _poller;
        private UDPSender _sender;

        public void Start() {
            _sender = new UDPSender("127.0.0.1", 9000);
            _poller = new MediaPoller();
            _poller.MediaChanged += OnMediaChanged;
            _poller.Start();
        }

        public void Shutdown() {
            _poller.Shutdown();
            _sender.Close();
            _poller = null;
            _sender = null;
        }

        private void OnMediaChanged(object sender, MediaEventArgs e) {
            _sender.Send(new OscMessage("/avatar/parameters/ToggleMusic", e.IsPlaying ? 1 : 0));
            MediaChanged.Invoke(sender, e);
        }
    }
}
