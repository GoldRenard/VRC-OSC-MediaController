/*
MIT License

Copyright (c) 2012 Valdemar Örn Erlingsson

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

namespace MediaWatcherLib.OSC {
    public struct Timetag {
        public UInt64 Tag;

        public DateTime Timestamp {
            get {
                return Utils.TimetagToDateTime(Tag);
            }
            set {
                Tag = Utils.DateTimeToTimetag(value);
            }
        }

        /// <summary>
        /// Gets or sets the fraction of a second in the timestamp. the double precision number is multiplied by 2^32
        /// giving an accuracy down to about 230 picoseconds ( 1/(2^32) of a second)
        /// </summary>
        public double Fraction {
            get {
                return Utils.TimetagToFraction(Tag);
            }
            set {
                Tag = (Tag & 0xFFFFFFFF00000000) + (UInt32)(value * 0xFFFFFFFF);
            }
        }

        public Timetag(UInt64 value) {
            this.Tag = value;
        }

        public Timetag(DateTime value) {
            Tag = 0;
            this.Timestamp = value;
        }

        public override bool Equals(System.Object obj) {
            if (obj.GetType() == typeof(Timetag)) {
                if (this.Tag == ((Timetag)obj).Tag)
                    return true;
                else
                    return false;
            } else if (obj.GetType() == typeof(UInt64)) {
                if (this.Tag == ((UInt64)obj))
                    return true;
                else
                    return false;
            } else
                return false;
        }

        public static bool operator ==(Timetag a, Timetag b) {
            if (a.Equals(b))
                return true;
            else
                return false;
        }

        public static bool operator !=(Timetag a, Timetag b) {
            if (a.Equals(b))
                return true;
            else
                return false;
        }

        public override int GetHashCode() {
            return (int)(((uint)(Tag >> 32) + (uint)(Tag & 0x00000000FFFFFFFF)) / 2);
        }
    }
}
