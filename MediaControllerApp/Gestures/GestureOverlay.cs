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
using System.Numerics;
using OVRSharp;
using OVRSharp.Math;
using Valve.VR;

namespace MediaControllerApp.Gestures {
    class GestureOverlay : Overlay {

        public GestureHand Hand {
            get; private set;
        }

        public GestureOverlay(GestureHand hand) : base(hand.Key, hand.Name) {
            Hand = hand;
            WidthInMeters = 0.12f;

            float radians = (float)((Math.PI / 180) * (hand.IsLeft ? -10 : 10));
            var rotation = Matrix4x4.CreateRotationY(radians);
            var translation = Matrix4x4.CreateTranslation((hand.IsLeft ? -1 : 1) * 0.27f, -0.39f, -0.95f);
            var transform = Matrix4x4.Multiply(rotation, translation);

            TrackedDevice = TrackedDeviceRole.Hmd;
            Transform = transform.ToHmdMatrix34_t();
        }

        public GestureType Type {
            set {
                var path = Hand.IsLeft ? value.LeftImagePath : value.RightImagePath;
                SetTextureFromFile(path);
                SetThumbnailTextureFromFile(path);
            }
        }

        public Matrix4x4 ToMatrix4x4(HmdMatrix44_t matrix) {
            return new Matrix4x4(
                matrix.m0, matrix.m1, matrix.m2, 0,
                matrix.m4, matrix.m5, matrix.m6, 0,
                matrix.m8, matrix.m9, matrix.m10, 0,
                matrix.m3, matrix.m7, matrix.m11, 1
            );
        }
    }
}
