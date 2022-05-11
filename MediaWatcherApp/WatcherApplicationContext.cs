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
        private readonly MenuItem currentState;
        private readonly MenuItem songName;
        private readonly MediaWatcher watcher;

        public WatcherApplicationContext() {
            currentState = new MenuItem("Waiting") { Enabled = false };
            songName = new MenuItem("") { Enabled = false, Visible = false };

            var contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(currentState);
            contextMenu.MenuItems.Add(songName);
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(new MenuItem("Exit", Exit));

            trayIcon = new NotifyIcon() {
                Icon = Resource.Icon,
                ContextMenu = contextMenu,
                Visible = true
            };

            watcher = new MediaWatcher();
            watcher.MediaChanged += (s, e) => {
                trayIcon.Icon = e.IsPlaying ? Resource.Icon : Resource.Stop;
                currentState.Text = e.IsPlaying ? "Playing" : "Stopped/Paused";
                if (e.Artist != null && e.Title != null) {
                    songName.Text = $"{e.Artist} - {e.Title}";
                    songName.Visible = true;
                } else {
                    songName.Visible = false;
                }
            };

            watcher.Start();
        }

        void Exit(object sender, EventArgs e) {
            trayIcon.Visible = false;
            watcher.Shutdown();
            Application.Exit();
        }
    }
}
