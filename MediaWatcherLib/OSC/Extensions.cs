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
using System.Collections.Generic;
using System.Linq;

namespace MediaControllerLib.OSC {
    internal static class Extensions {
        public static int FirstIndexAfter<T>(this IEnumerable<T> items, int start, Func<T, bool> predicate) {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");
            if (start >= items.Count()) throw new ArgumentOutOfRangeException("start");

            int retVal = 0;
            foreach (var item in items) {
                if (retVal >= start && predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }

        public static List<List<T>> Split<T>(this IEnumerable<T> data, Func<T, bool> predicate) {
            var output = new List<List<T>>();
            var curr = new List<T>();
            output.Add(curr);
            foreach (var x in data) {
                if (predicate(x)) {
                    curr = new List<T>();
                    output.Add(curr);
                } else
                    curr.Add(x);
            }

            return output;
        }



        public static T[] SubArray<T>(this T[] data, int index, int length) {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}
