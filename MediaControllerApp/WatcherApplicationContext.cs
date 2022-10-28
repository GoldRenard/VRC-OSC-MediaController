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
using System.Windows.Forms;
using MediaControllerApp.Gestures;
using MediaControllerLib;

namespace MediaControllerApp {
    class WatcherApplicationContext : ApplicationContext {
        private readonly NotifyIcon _trayIcon;
        private readonly MenuItem _stateItem;
        private readonly MenuItem _songItem;
        private readonly MenuItem _controlsItem;
        private readonly MenuItem _gesturesItem;

        private readonly MediaWatcher _watcher;
        private readonly MediaController _controller;
        private readonly GesturesManager _gesturesManager;

        private string ManifestFile {
            get => Application.StartupPath + "\\manifest.vrmanifest";
        }

        public WatcherApplicationContext() {
            _stateItem = new MenuItem(Resource.MenuStateStopped) { Enabled = false };
            _songItem = new MenuItem("") { Enabled = false, Visible = false };
            _controlsItem = new MenuItem(Resource.MenuListenForControls, OnControls);
            _gesturesItem = new MenuItem(Resource.MenuGesturesIndicator, OnGestures) { Enabled = false };

            var manifestMenu = new MenuItem(Resource.MenuManifest);
            manifestMenu.MenuItems.Add(new MenuItem(Resource.MenuInstall, OnManifestInstall));
            manifestMenu.MenuItems.Add(new MenuItem(Resource.MenuUninstall, OnManifestUninstall));

            var contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(_stateItem);
            contextMenu.MenuItems.Add(_songItem);
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(_controlsItem);
            contextMenu.MenuItems.Add(_gesturesItem);
            contextMenu.MenuItems.Add(manifestMenu);
            contextMenu.MenuItems.Add(new MenuItem(Resource.MenuExit, OnExit));

            _trayIcon = new NotifyIcon() {
                Icon = Resource.Stop,
                ContextMenu = contextMenu,
                Visible = true
            };

            _controller = new MediaController(
                ConfigurationAccessor.OSCListenPort,
                ConfigurationAccessor.OSCListenSkipNextParameter,
                ConfigurationAccessor.OSCListenSkipPreviousParameter,
                ConfigurationAccessor.OSCListenPauseParameter,
                ConfigurationAccessor.OSCListenPlayParameter,
                ConfigurationAccessor.OSCListenStopParameter,
                ConfigurationAccessor.OSCListenTogglePlayPauseParameter
            );

            _controller.OnOscMessage += OnOscMessage;

            _watcher = new MediaWatcher(
                ConfigurationAccessor.OSCTargetAddress,
                ConfigurationAccessor.OSCTargetPort,
                ConfigurationAccessor.OSCTargetParameter
            );
            _watcher.MediaChanged += (s, e) => {
                _trayIcon.Icon = e.IsPlaying ? Resource.Icon : Resource.Stop;
                _stateItem.Text = e.IsPlaying ? Resource.MenuStatePlaying : Resource.MenuStateStopped;
                if (e.Artist != null && e.Title != null) {
                    _songItem.Text = $"{e.Artist} - {e.Title}";
                    _songItem.Visible = true;
                } else {
                    _songItem.Visible = false;
                }
            };

            _gesturesManager = new GesturesManager(ConfigurationAccessor.OverlayAlpha);

            _watcher.Start();

            if (ConfigurationAccessor.OSCListenDefaultEnabled) {
                OnControls(this, null); // just toggle it for once
            }
            if (ConfigurationAccessor.OverlayDefaultEnabled) {
                OnGestures(this, null);
            }
        }

        private void OnOscMessage(object sender, MediaControllerLib.OSC.OscMessage e) {
            var value = e.Arguments[0];
            if (value == null) {
                return;
            }

            if (value is float floatValue) {
                // input redirect for useful things
                if (e.Address == ConfigurationAccessor.OSCListenInputH) {
                    _watcher.SendMessage("/input/Horizontal", floatValue);
                } else if (e.Address == ConfigurationAccessor.OSCListenInputV) {
                    _watcher.SendMessage("/input/Vertical", floatValue);
                } else if (e.Address == "/avatar/parameters/GestureLeftWeight") {
                    _gesturesManager.SetGestureWeight(true, floatValue);
                } else if (e.Address == "/avatar/parameters/GestureRightWeight") {
                    _gesturesManager.SetGestureWeight(false, floatValue);
                }
            }

            if (value is int intValue) {
                if (e.Address == "/avatar/parameters/GestureLeft") {
                    _gesturesManager.SetGestureType(true, intValue);
                } else if (e.Address == "/avatar/parameters/GestureRight") {
                    _gesturesManager.SetGestureType(false, intValue);
                }
            }
        }

        void OnExit(object sender, EventArgs e) {
            _trayIcon.Visible = false;
            _watcher.Shutdown();
            _controller.Shutdown();
            _gesturesManager.Shutdown();
            Application.Exit();
        }

        void OnControls(object sender, EventArgs e) {
            _controlsItem.Checked = !_controlsItem.Checked;
            if (_controlsItem.Checked) {
                try {
                    _controller.Start();
                    _gesturesItem.Enabled = true;
                } catch {
                    _controlsItem.Checked = false;
                }
            } else {
                _controller.Shutdown();
                _gesturesItem.Enabled = false;
            }
        }

        void OnGestures(object sender, EventArgs e) {
            _gesturesItem.Checked = !_gesturesItem.Checked;
            if (_gesturesItem.Checked) {
                try {
                    _gesturesManager.Start();
                } catch {
                    _gesturesItem.Checked = false;
                }
            } else {
                _gesturesManager.Shutdown();
            }
        }

        void OnManifestInstall(object sender, EventArgs e) {
            var apps = Valve.VR.OpenVR.Applications;
            if (apps == null) {
                MessageBox.Show("Please enable OpenVR first", "Manifest Install", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!apps.IsApplicationInstalled(ConfigurationAccessor.OverlayAppKey)) {
                var error = apps.AddApplicationManifest(ManifestFile, false);
                if (error != Valve.VR.EVRApplicationError.None) {
                    MessageBox.Show("Can't install manifest: " + error.ToString(), "Manifest Install Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    MessageBox.Show("Manifest has been installed", "Manifest Install", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            } else {
                MessageBox.Show("Manifest already exists", "Manifest Install", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void OnManifestUninstall(object sender, EventArgs e) {
            var apps = Valve.VR.OpenVR.Applications;
            if (apps == null) {
                MessageBox.Show("Please enable OpenVR first", "Manifest Install", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (apps.IsApplicationInstalled(ConfigurationAccessor.OverlayAppKey)) {
                var error = apps.RemoveApplicationManifest(ManifestFile);
                if (error != Valve.VR.EVRApplicationError.None) {
                    MessageBox.Show("Can't uninstall manifest: " + error.ToString(), "Manifest Install Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    MessageBox.Show("Manifest has been uninstalled", "Manifest Uninstall", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            } else {
                MessageBox.Show("Manifest doesn't exists", "Manifest Uninstall", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
