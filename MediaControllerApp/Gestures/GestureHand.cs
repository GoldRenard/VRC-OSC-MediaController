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

namespace MediaControllerApp.Gestures {
    class GestureHand {
        private GestureHand(string key, string name, bool isLeft) {
            Key = key;
            Name = name;
            IsLeft = isLeft;
        }

        public string Key {
            get; private set;
        }

        public string Name {
            get; private set;
        }

        public bool IsLeft {
            get; private set;
        }

        public static GestureHand Right {
            get {
                return new GestureHand("gesture_right", "Right Hand Gesture", false);
            }
        }

        public static GestureHand Left {
            get {
                return new GestureHand("gesture_left", "Left Hand Gesture", true);
            }
        }
    }
}
