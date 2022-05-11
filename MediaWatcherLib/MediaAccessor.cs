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
using System.Threading.Tasks;
using Windows.Media.Control;

namespace MediaWatcherLib {
    public class MediaAccessor {

        private static GlobalSystemMediaTransportControlsSessionManager sessionManager;

        public static async Task<GlobalSystemMediaTransportControlsSessionManager> GetSessionManager() {
            if (sessionManager == null) {
                sessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            }
            return sessionManager;
        }

        public static PlaybackInfo GetPlaybackInfo() {
            var task = Task.Run(async () => await GetPlaybackInfoAsync());
            Task.WaitAll(task);
            return task.Result;
        }

        public static async Task<PlaybackInfo> GetPlaybackInfoAsync() {
            var sessionManager = await GetSessionManager();
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

        public static async Task<bool> TrySkipNextAsync() {
            var sessionManager = await GetSessionManager();
            var currentSession = sessionManager.GetCurrentSession();
            if (currentSession != null) {
                return await currentSession.TrySkipNextAsync();
            }
            return false;
        }

        public static async Task<bool> TrySkipPreviousAsync() {
            var sessionManager = await GetSessionManager();
            var currentSession = sessionManager.GetCurrentSession();
            if (currentSession != null) {
                return await currentSession.TrySkipPreviousAsync();
            }
            return false;
        }

        public static async Task<bool> TryPauseAsync() {
            var sessionManager = await GetSessionManager();
            var currentSession = sessionManager.GetCurrentSession();
            if (currentSession != null) {
                return await currentSession.TryPauseAsync();
            }
            return false;
        }

        public static async Task<bool> TryPlayAsync() {
            var sessionManager = await GetSessionManager();
            var currentSession = sessionManager.GetCurrentSession();
            if (currentSession != null) {
                return await currentSession.TryPlayAsync();
            }
            return false;
        }

        public static async Task<bool> TryStopAsync() {
            var sessionManager = await GetSessionManager();
            var currentSession = sessionManager.GetCurrentSession();
            if (currentSession != null) {
                return await currentSession.TryStopAsync();
            }
            return false;
        }

        public static async Task<bool> TryTogglePlayPauseAsync() {
            var sessionManager = await GetSessionManager();
            var currentSession = sessionManager.GetCurrentSession();
            if (currentSession != null) {
                return await currentSession.TryTogglePlayPauseAsync();
            }
            return false;
        }
    }
}
