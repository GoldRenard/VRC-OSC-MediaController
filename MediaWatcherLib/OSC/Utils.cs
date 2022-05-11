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
    public class Utils {
        public static DateTime TimetagToDateTime(UInt64 val) {
            if (val == 1)
                return DateTime.Now;

            UInt32 seconds = (UInt32)(val >> 32);
            var time = DateTime.Parse("1900-01-01 00:00:00");
            time = time.AddSeconds(seconds);
            var fraction = TimetagToFraction(val);
            time = time.AddSeconds(fraction);
            return time;
        }

        public static double TimetagToFraction(UInt64 val) {
            if (val == 1)
                return 0.0;

            UInt32 seconds = (UInt32)(val & 0x00000000FFFFFFFF);
            double fraction = (double)seconds / (UInt32)(0xFFFFFFFF);
            return fraction;
        }

        public static UInt64 DateTimeToTimetag(DateTime value) {
            UInt64 seconds = (UInt32)(value - DateTime.Parse("1900-01-01 00:00:00.000")).TotalSeconds;
            UInt64 fraction = (UInt32)(0xFFFFFFFF * ((double)value.Millisecond / 1000));

            UInt64 output = (seconds << 32) + fraction;
            return output;
        }

        public static int AlignedStringLength(string val) {
            int len = val.Length + (4 - val.Length % 4);
            if (len <= val.Length) len += 4;

            return len;
        }
    }
}
