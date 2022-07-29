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
using OVRSharp;

namespace MediaControllerApp.Gestures {
    class GesturesManager {

        private Application _application;
        private GestureOverlay _right;
        private GestureOverlay _left;
        private float _alpha;

        public GesturesManager(float alpha = 1.0f) {
            _alpha = alpha;
        }

        public void Start() {
            _application = new Application(Application.ApplicationType.Overlay);
            _right = new GestureOverlay(GestureHand.Right);
            _left = new GestureOverlay(GestureHand.Left);
        }

        public void Shutdown() {
            _application?.Shutdown();
        }

        public void SetGestureType(bool left, int value) {
            var overlay = left ? _left : _right;
            if (overlay == null) {
                return;
            }

            float alpha = 1;
            if (value > 0) {
                var type = GestureType.GetForValue(value);
                if (type != null) {
                    alpha = _alpha;
                    overlay.Type = type;
                }
            }
            overlay.Alpha = alpha;
        }

        public void SetGestureWeight(bool left, float weight) {
            var overlay = left ? _left : _right;
            if (overlay == null) {
                return;
            }

            if (weight > 0.1) {
                overlay.Show();
            } else {
                overlay.Hide();
            }
        }
    }
}
