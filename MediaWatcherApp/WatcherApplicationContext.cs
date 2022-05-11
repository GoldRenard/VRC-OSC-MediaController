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
using MediaControllerLib;

namespace MediaControllerApp {
    class WatcherApplicationContext : ApplicationContext {
        private readonly NotifyIcon _trayIcon;
        private readonly MenuItem _stateItem;
        private readonly MenuItem _songItem;
        private readonly MenuItem _controlsItem;
        private readonly MediaWatcher _watcher;
        private readonly MediaController _controller;

        public WatcherApplicationContext() {
            _stateItem = new MenuItem(Resource.MenuStateStopped) { Enabled = false };
            _songItem = new MenuItem("") { Enabled = false, Visible = false };
            _controlsItem = new MenuItem(Resource.MenuListenForControls, OnControls);

            var contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(_stateItem);
            contextMenu.MenuItems.Add(_songItem);
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(_controlsItem);
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

            _watcher.Start();

            if (ConfigurationAccessor.OSCListenDefaultEnabled) {
                OnControls(this, null); // just toggle it for once
            }
        }

        void OnExit(object sender, EventArgs e) {
            _trayIcon.Visible = false;
            _watcher.Shutdown();
            _controller.Shutdown();
            Application.Exit();
        }

        void OnControls(object sender, EventArgs e) {
            _controlsItem.Checked = !_controlsItem.Checked;
            if (_controlsItem.Checked) {
                try {
                    _controller.Start();
                } catch {
                    _controlsItem.Checked = false;
                }
            } else {
                _controller.Shutdown();
            }
        }
    }
}
