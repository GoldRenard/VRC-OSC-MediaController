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
using System.Text;

namespace MediaControllerLib.OSC {
    public class OscBundle : OscPacket {
        Timetag _timetag;

        public UInt64 Timetag {
            get {
                return _timetag.Tag;
            }
            set {
                _timetag.Tag = value;
            }
        }

        public DateTime Timestamp {
            get {
                return _timetag.Timestamp;
            }
            set {
                _timetag.Timestamp = value;
            }
        }

        public List<OscMessage> Messages;

        public OscBundle(UInt64 timetag, params OscMessage[] args) {
            _timetag = new Timetag(timetag);
            Messages = new List<OscMessage>();
            Messages.AddRange(args);
        }

        public override byte[] GetBytes() {
            string bundle = "#bundle";
            int bundleTagLen = Utils.AlignedStringLength(bundle);
            byte[] tag = setULong(_timetag.Tag);

            List<byte[]> outMessages = new List<byte[]>();
            foreach (OscMessage msg in Messages) {
                outMessages.Add(msg.GetBytes());
            }

            int len = bundleTagLen + tag.Length + outMessages.Sum(x => x.Length + 4);

            int i = 0;
            byte[] output = new byte[len];
            Encoding.ASCII.GetBytes(bundle).CopyTo(output, i);
            i += bundleTagLen;
            tag.CopyTo(output, i);
            i += tag.Length;

            foreach (byte[] msg in outMessages) {
                var size = setInt(msg.Length);
                size.CopyTo(output, i);
                i += size.Length;

                msg.CopyTo(output, i);
                i += msg.Length; // msg size is always a multiple of 4
            }

            return output;
        }

    }
}
