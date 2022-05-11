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
using MediaWatcherLib;

namespace MediaWatcherApp {
    class WatcherApplicationContext : ApplicationContext {
        private readonly NotifyIcon trayIcon;
        private readonly MenuItem stateItem;
        private readonly MenuItem songItem;
        private readonly MenuItem controlsItem;
        private readonly MediaWatcher watcher;
        private readonly MediaController controller;

        public WatcherApplicationContext() {
            stateItem = new MenuItem(Resource.MenuStateStopped) { Enabled = false };
            songItem = new MenuItem("") { Enabled = false, Visible = false };
            controlsItem = new MenuItem(Resource.MenuListenForControls, OnControls);

            var contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(stateItem);
            contextMenu.MenuItems.Add(songItem);
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(controlsItem);
            contextMenu.MenuItems.Add(new MenuItem(Resource.MenuExit, OnExit));

            trayIcon = new NotifyIcon() {
                Icon = Resource.Stop,
                ContextMenu = contextMenu,
                Visible = true
            };

            controller = new MediaController(
                ConfigurationAccessor.OSCListenPort,
                ConfigurationAccessor.OSCListenSkipNextParameter,
                ConfigurationAccessor.OSCListenSkipPreviousParameter,
                ConfigurationAccessor.OSCListenPauseParameter,
                ConfigurationAccessor.OSCListenPlayParameter,
                ConfigurationAccessor.OSCListenStopParameter,
                ConfigurationAccessor.OSCListenTogglePlayPauseParameter
            );

            watcher = new MediaWatcher(
                ConfigurationAccessor.OSCTargetAddress,
                ConfigurationAccessor.OSCTargetPort,
                ConfigurationAccessor.OSCTargetParameter
            );
            watcher.MediaChanged += (s, e) => {
                trayIcon.Icon = e.IsPlaying ? Resource.Icon : Resource.Stop;
                stateItem.Text = e.IsPlaying ? Resource.MenuStatePlaying : Resource.MenuStateStopped;
                if (e.Artist != null && e.Title != null) {
                    songItem.Text = $"{e.Artist} - {e.Title}";
                    songItem.Visible = true;
                } else {
                    songItem.Visible = false;
                }
            };

            watcher.Start();

            if (ConfigurationAccessor.OSCListenDefaultEnabled) {
                OnControls(this, null); // just toggle it for once
            }
        }

        void OnExit(object sender, EventArgs e) {
            trayIcon.Visible = false;
            watcher.Shutdown();
            Application.Exit();
        }

        void OnControls(object sender, EventArgs e) {
            controlsItem.Checked = !controlsItem.Checked;
            if (controlsItem.Checked) {
                try {
                    controller.Start();
                } catch (Exception ex) {
                    controlsItem.Checked = false;
                }
            } else {
                controller.Shutdown();
            }
        }
    }
}
