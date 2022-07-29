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

namespace MediaControllerApp.Gestures {
    class GestureType {
        private GestureType(string imageName) {
            LeftImagePath = AppDomain.CurrentDomain.BaseDirectory + $"Images\\Left_{imageName}.png";
            RightImagePath = AppDomain.CurrentDomain.BaseDirectory + $"Images\\Right_{imageName}.png";
        }

        public string LeftImagePath {
            get; private set;
        }

        public string RightImagePath {
            get; private set;
        }

        public static GestureType Fist {
            get {
                return new GestureType("Fist");
            }
        }

        public static GestureType Open {
            get {
                return new GestureType("OpenHand");
            }
        }

        public static GestureType Point {
            get {
                return new GestureType("Point");
            }
        }

        public static GestureType Victory {
            get {
                return new GestureType("Victory");
            }
        }

        public static GestureType RockAndRoll {
            get {
                return new GestureType("RockAndRoll");
            }
        }

        public static GestureType Gun {
            get {
                return new GestureType("FingerGun");
            }
        }

        public static GestureType ThumbsUp {
            get {
                return new GestureType("ThumbsUp");
            }
        }

        public static GestureType GetForValue(int value) {
            switch (value) {
                case 1:
                    return Fist;
                case 2:
                    return Open;
                case 3:
                    return Point;
                case 4:
                    return Victory;
                case 5:
                    return RockAndRoll;
                case 6:
                    return Gun;
                case 7:
                    return ThumbsUp;
                default:
                    return null;
            }
        }
    }
}
