using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using X32.OSC;
using X32;
using X32.Channels;

namespace X32
{
    public class X32Client
    {
        public int ReceiveTimeout
        {
            get
            {
                if (this._udpClient != null)
                    return this._udpClient.Client.ReceiveTimeout;
                else
                    return -1;
            }
            set
            {
                if (this._udpClient != null)
                    this._udpClient.Client.ReceiveTimeout = value;
            }
        }

        private UdpClient _udpClient;
        private IPEndPoint _consoleEndPoint;

        #region Constructors
        public X32Client(string consoleIpAddress, int consolePort, int clientPort)
        {

            this._consoleEndPoint = new IPEndPoint(IPAddress.Parse(consoleIpAddress), consolePort);
            this._udpClient = InitUdpClient(clientPort, this._consoleEndPoint);
        }

        public X32Client(string consoleIpAddress, int clientPort)
        {
            this._consoleEndPoint = new IPEndPoint(IPAddress.Parse(consoleIpAddress), 10023);
            this._udpClient = InitUdpClient(clientPort, this._consoleEndPoint);
        }

        public X32Client(string consoleIpAddress)
        {
            this._consoleEndPoint = new IPEndPoint(IPAddress.Parse(consoleIpAddress), 10023);
            this._udpClient = InitUdpClient(10023, this._consoleEndPoint);
        }

        #endregion // Constructors

        private UdpClient InitUdpClient(int clientPort, IPEndPoint consoleEndPoint)
        {
            var udpClient = new UdpClient(clientPort);

            udpClient.Connect(consoleEndPoint);
            Thread.Sleep(500);
            return udpClient;
        }

        public OscMessage SendAndReceive(OscPacket packet)
        {
            var messageBytes = packet.GetBytes();
            this._udpClient.Send(messageBytes, messageBytes.Count());
            Thread.Sleep(75);

            byte[] responseBytes;
            responseBytes = this._udpClient.Receive(ref this._consoleEndPoint);


            OscMessage response = OscMessage.GetPacket(responseBytes) as OscMessage;
            return response;
        }

        public void Send(OscPacket packet)
        {
            var messageBytes = packet.GetBytes();
            this._udpClient.Send(messageBytes, messageBytes.Count());
            Thread.Sleep(50);
        }

        public Channel GetChannel(int channelNumber)
        {
            return new Channel(channelNumber, this);
        }
    }
}
