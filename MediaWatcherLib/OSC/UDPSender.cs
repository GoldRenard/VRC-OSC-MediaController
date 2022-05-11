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
using System.Net;
using System.Net.Sockets;

namespace MediaWatcherLib.OSC {
    public class UDPSender {
        public int Port {
            get {
                return _port;
            }
        }
        int _port;

        public string Address {
            get {
                return _address;
            }
        }
        string _address;

        IPEndPoint RemoteIpEndPoint;
        Socket sock;

        public UDPSender(string address, int port) {
            _port = port;
            _address = address;

            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            var addresses = System.Net.Dns.GetHostAddresses(address);
            if (addresses.Length == 0) throw new Exception("Unable to find IP address for " + address);

            RemoteIpEndPoint = new IPEndPoint(addresses[0], port);
        }

        public void Send(byte[] message) {
            sock.SendTo(message, RemoteIpEndPoint);
        }

        public void Send(OscPacket packet) {
            byte[] data = packet.GetBytes();
            Send(data);
        }

        public void Close() {
            sock.Close();
        }
    }
}
