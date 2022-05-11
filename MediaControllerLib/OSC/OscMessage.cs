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
    public class OscMessage : OscPacket {
        public string Address;
        public List<object> Arguments;

        public OscMessage(string address, params object[] args) {
            this.Address = address;
            Arguments = new List<object>();
            Arguments.AddRange(args);
        }

        public override byte[] GetBytes() {
            List<byte[]> parts = new List<byte[]>();

            List<object> currentList = Arguments;
            int ArgumentsIndex = 0;

            string typeString = ",";
            int i = 0;
            while (i < currentList.Count) {
                var arg = currentList[i];

                string type = (arg != null) ? arg.GetType().ToString() : "null";
                switch (type) {
                    case "System.Int32":
                        typeString += "i";
                        parts.Add(setInt((int)arg));
                        break;
                    case "System.Single":
                        if (float.IsPositiveInfinity((float)arg)) {
                            typeString += "I";
                        } else {
                            typeString += "f";
                            parts.Add(setFloat((float)arg));
                        }
                        break;
                    case "System.String":
                        typeString += "s";
                        parts.Add(setString((string)arg));
                        break;
                    case "System.Byte[]":
                        typeString += "b";
                        parts.Add(setBlob((byte[])arg));
                        break;
                    case "System.Int64":
                        typeString += "h";
                        parts.Add(setLong((Int64)arg));
                        break;
                    case "System.UInt64":
                        typeString += "t";
                        parts.Add(setULong((UInt64)arg));
                        break;
                    case "SharpOSC.Timetag":
                        typeString += "t";
                        parts.Add(setULong(((Timetag)arg).Tag));
                        break;
                    case "System.Double":
                        if (Double.IsPositiveInfinity((double)arg)) {
                            typeString += "I";
                        } else {
                            typeString += "d";
                            parts.Add(setDouble((double)arg));
                        }
                        break;

                    case "System.Char":
                        typeString += "c";
                        parts.Add(setChar((char)arg));
                        break;

                    case "System.Boolean":
                        typeString += ((bool)arg) ? "T" : "F";
                        break;
                    case "null":
                        typeString += "N";
                        break;

                    // This part handles arrays. It points currentList to the array and resets i
                    // The array is processed like normal and when it is finished we replace  
                    // currentList back with Arguments and continue from where we left off
                    case "System.Object[]":
                    case "System.Collections.Generic.List`1[System.Object]":
                        if (arg.GetType() == typeof(object[]))
                            arg = ((object[])arg).ToList();

                        if (Arguments != currentList)
                            throw new Exception("Nested Arrays are not supported");
                        typeString += "[";
                        currentList = (List<object>)arg;
                        ArgumentsIndex = i;
                        i = 0;
                        continue;

                    default:
                        throw new Exception("Unable to transmit values of type " + type);
                }

                i++;
                if (currentList != Arguments && i == currentList.Count) {
                    // End of array, go back to main Argument list
                    typeString += "]";
                    currentList = Arguments;
                    i = ArgumentsIndex + 1;
                }
            }

            int addressLen = (Address.Length == 0 || Address == null) ? 0 : Utils.AlignedStringLength(Address);
            int typeLen = Utils.AlignedStringLength(typeString);

            int total = addressLen + typeLen + parts.Sum(x => x.Length);

            byte[] output = new byte[total];
            i = 0;

            Encoding.ASCII.GetBytes(Address).CopyTo(output, i);
            i += addressLen;

            Encoding.ASCII.GetBytes(typeString).CopyTo(output, i);
            i += typeLen;

            foreach (byte[] part in parts) {
                part.CopyTo(output, i);
                i += part.Length;
            }

            return output;
        }
    }
}
