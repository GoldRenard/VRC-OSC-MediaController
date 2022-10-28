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
using System.Configuration;

namespace MediaControllerLib {
    public static class ConfigurationAccessor {

        public static string OSCTargetAddress {
            get => GetString("OSCTargetAddress", "127.0.0.1");
        }

        public static string OSCTargetParameter {
            get => GetString("OSCTargetParameter", "/avatar/parameters/ToggleMusic");
        }

        public static int OSCTargetPort {
            get => GetInt("OSCTargetPort", 9000);
        }

        public static int OSCListenPort {
            get => GetInt("OSCListenPort", 9001);
        }

        public static bool OSCListenDefaultEnabled {
            get => OverlayDefaultEnabled || GetBool("OSCListenDefaultEnabled", false);
        }

        public static string OSCListenSkipNextParameter {
            get => GetString("OSCListenSkipNextParameter", "/avatar/parameters/MediaSkipNext");
        }
        public static string OSCListenSkipPreviousParameter {
            get => GetString("OSCListenSkipPreviousParameter", "/avatar/parameters/MediaSkipPrevious");
        }
        public static string OSCListenPauseParameter {
            get => GetString("OSCListenPauseParameter", "/avatar/parameters/MediaPause");
        }
        public static string OSCListenStopParameter {
            get => GetString("OSCListenStopParameter", "/avatar/parameters/MediaStop");
        }
        public static string OSCListenPlayParameter {
            get => GetString("OSCListenPlayParameter", "/avatar/parameters/MediaPlay");
        }
        public static string OSCListenTogglePlayPauseParameter {
            get => GetString("OSCListenTogglePlayPauseParameter", "/avatar/parameters/MediaTogglePlayPause");
        }
        public static string OSCListenInputV {
            get => GetString("OSCListenInputV", "/avatar/parameters/MoveInputV");
        }
        public static string OSCListenInputH {
            get => GetString("OSCListenInputH", "/avatar/parameters/MoveInputH");
        }
        public static float OverlayAlpha {
            get => GetFloat("OverlayAlpha", 1.0f);
        }

        public static bool OverlayDefaultEnabled {
            get => GetBool("OverlayDefaultEnabled", false);
        }

        public static string OverlayAppKey {
            get => GetString("OverlayAppKey", "Caramel.VRCGestures");
        }

        private static string Get(string name) {
            return ConfigurationManager.AppSettings.Get(name);
        }

        private static string GetString(string name, string orDefault) {
            var result = Get(name);
            return result != null && result.Length > 0 ? result : orDefault;
        }

        private static int GetInt(string name, int orDefault) {
            var value = Get(name);
            return value != null && int.TryParse(value, out int result) ? result : orDefault;
        }

        private static bool GetBool(string name, bool orDefault) {
            var value = Get(name);
            return value != null ? value.ToLower().Equals("true") : orDefault;
        }
        private static float GetFloat(string name, float orDefault) {
            var value = Get(name);
            return value != null && float.TryParse(value, out float result) ? result : orDefault;
        }
    }
}
