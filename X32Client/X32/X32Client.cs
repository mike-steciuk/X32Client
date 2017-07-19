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

            var responseBytes = this._udpClient.Receive(ref this._consoleEndPoint);

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

    public class Channel
    {
        private X32Client _x32Client;
        private string _requestPrefix;

        private string _name;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name) && _x32Client != null)
                {
                    OscMessage getChannelName = new OscMessage($"{_requestPrefix}/config/name");
                    var response = _x32Client.SendAndReceive(getChannelName);
                    _name = response.Arguments[0].ToString();
                }

                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/config/name", new object[] { _name });
                    _x32Client.Send(command);
                }
            }
        }

        private int _icon = -1;
        public int Icon
        {
            get
            {
                if (_icon == -1 && _x32Client != null)
                {
                    OscMessage getChannelName = new OscMessage($"{_requestPrefix}/config/icon");
                    var response = _x32Client.SendAndReceive(getChannelName);
                    _icon = int.Parse(response.Arguments[0].ToString());
                }

                return _icon;
            }
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/config/icon", new object[] { _icon });
                    _x32Client.Send(command);
                }
            }
        }

        private X32Color _color = X32Color.NotSet;
        public X32Color Color
        {
            get
            {
                if (_color == X32Color.NotSet && _x32Client != null)
                {
                    OscMessage getChannelName = new OscMessage($"{_requestPrefix}/config/color");
                    var response = _x32Client.SendAndReceive(getChannelName);
                    _color = (X32Color)Enum.Parse(typeof(X32Color), response.Arguments[0].ToString());
                }

                return _color;
            }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/config/color", new object[] { (int)_color });
                    _x32Client.Send(command);
                }
            }
        }

        private X32Source _source = X32Source.NotSet;
        public X32Source Source
        {
            get
            {
                if (_source == X32Source.NotSet && _x32Client != null)
                {
                    OscMessage getChannelName = new OscMessage($"{_requestPrefix}/config/source");
                    var response = _x32Client.SendAndReceive(getChannelName);
                    _source = (X32Source)Enum.Parse(typeof(X32Source), response.Arguments[0].ToString());
                }

                return _source;
            }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/config/source", new object[] { (int)_source });
                    _x32Client.Send(command);
                }
            }
        }

        public int ChannelNumber { get; private set; }
        public Delay Delay { get; private set; }
        public Preamp Preamp { get; private set; }
        public Gate Gate { get; private set; }

        public Channel(int channelNumber, X32Client client)
        {
            this._x32Client = client;
            ChannelNumber = channelNumber;
            _requestPrefix = $"/ch/{ChannelNumber.ToString().PadLeft(2, '0')}";

            Delay = new Delay(_x32Client, _requestPrefix);
            Preamp = new Preamp(_x32Client, _requestPrefix);
            Gate = new Gate(_x32Client, _requestPrefix);
        }
    }

    public class Delay
    {
        private X32Client _x32Client;
        private string _requestPrefix;

        private bool? _on = null;
        public bool On
        {
            get
            {
                if (_on == null && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/on");
                    var response = _x32Client.SendAndReceive(command);
                    _on = int.Parse(response.Arguments[0].ToString()) == 1;
                }

                return _on ?? false;
            }
            set
            {
                if (_on != value)
                {
                    _on = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/on", new object[] { _on == true ? 1 : 0 });
                    _x32Client.Send(command);
                }
            }
        }

        private float _time = -1;
        /// <summary>
        /// This one does not always return the appropriate value;
        /// TODO: Research why?
        /// </summary>
        public float Time
        {
            get
            {
                if (_time == -1 && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/time");
                    var response = _x32Client.SendAndReceive(command);
                    _time = float.Parse(response.Arguments[0].ToString());
                }

                return _time;
            }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/time", new object[] { _time });
                    _x32Client.Send(command);
                }
            }
        }

        public Delay (X32Client x32Client, string channelRequestPrefix)
        {
            _x32Client = x32Client;
            _requestPrefix = channelRequestPrefix + "/delay";
        }

    }

    public class Preamp
    {
        private X32Client _x32Client;
        private string _requestPrefix;

        private float _trim = -1;
        public float Trim
        {
            get
            {
                if (_trim == -1 && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/trim");
                    var response = _x32Client.SendAndReceive(command);
                    _trim = float.Parse(response.Arguments[0].ToString());
                }

                return _trim;
            }
            set
            {
                if (_trim != value)
                {
                    _trim = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/trim", new object[] { _trim });
                    _x32Client.Send(command);
                }
            }
        }

        private bool? _invert = null;
        public bool Invert
        {
            get
            {
                if (_invert == null && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/invert");
                    var response = _x32Client.SendAndReceive(command);
                    _invert = int.Parse(response.Arguments[0].ToString()) == 1;
                }

                return _invert ?? false;
            }
            set
            {
                if (_invert != value)
                {
                    _invert = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/invert", new object[] { _invert == true ? 1 : 0 });
                    _x32Client.Send(command);
                }
            }
        }

        private bool? _highPassOn = null;
        public bool HighPassOn
        {
            get
            {
                if (_highPassOn == null && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/hpon");
                    var response = _x32Client.SendAndReceive(command);
                    _highPassOn = int.Parse(response.Arguments[0].ToString()) == 1;
                }

                return _highPassOn ?? false;
            }
            set
            {
                if (_highPassOn != value)
                {
                    _highPassOn = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/hpon", new object[] { _highPassOn == true ? 1 : 0 });
                    _x32Client.Send(command);
                }
            }
        }

        private float _HighPassFreq = -1;
        public float HighPassFreq
        {
            get
            {
                if (_HighPassFreq == -1 && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/hpf");
                    var response = _x32Client.SendAndReceive(command);
                    _HighPassFreq = float.Parse(response.Arguments[0].ToString());
                }

                return _HighPassFreq;
            }
            set
            {
                if (_HighPassFreq != value)
                {
                    _HighPassFreq = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/hpf", new object[] { _HighPassFreq });
                    _x32Client.Send(command);
                }
            }
        }

        public Preamp(X32Client x32Client, string channelRequestPrefix)
        {
            _x32Client = x32Client;
            _requestPrefix = channelRequestPrefix + "/preamp";
        }
    }

    public class Gate
    {
        private X32Client _x32Client;
        private string _requestPrefix;

        private bool? _on = null;
        public bool On
        {
            get
            {
                if (_on == null && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/on");
                    var response = _x32Client.SendAndReceive(command);
                    _on = int.Parse(response.Arguments[0].ToString()) == 1;
                }

                return _on ?? false;
            }
            set
            {
                if (_on != value)
                {
                    _on = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/on", new object[] { _on == true ? 1 : 0 });
                    _x32Client.Send(command);
                }
            }
        }

        private X32GateMode _gateMode = X32GateMode.NotSet;
        public X32GateMode GateMode
        {
            get
            {
                if (_gateMode == X32GateMode.NotSet && _x32Client != null)
                {
                    OscMessage getChannelName = new OscMessage($"{_requestPrefix}/mode");
                    var response = _x32Client.SendAndReceive(getChannelName);
                    _gateMode = (X32GateMode)Enum.Parse(typeof(X32GateMode), response.Arguments[0].ToString());
                }

                return _gateMode;
            }
            set
            {
                if (_gateMode != value)
                {
                    _gateMode = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/mode", new object[] { (int)_gateMode });
                    _x32Client.Send(command);
                }
            }
        }

        private float _threshold = -90;
        public float Threshold
        {
            get
            {
                if (_threshold == -90 && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/thr");
                    var response = _x32Client.SendAndReceive(command);
                    _threshold = float.Parse(response.Arguments[0].ToString());
                }

                return _threshold;
            }
            set
            {
                if (_threshold != value)
                {
                    _threshold = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/thr", new object[] { _threshold });
                    _x32Client.Send(command);
                }
            }
        }

        private float _range = -90;
        public float Range
        {
            get
            {
                if (_range == -90 && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/range");
                    var response = _x32Client.SendAndReceive(command);
                    _range = float.Parse(response.Arguments[0].ToString());
                }

                return _range;
            }
            set
            {
                if (_range != value)
                {
                    _range = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/range", new object[] { _range });
                    _x32Client.Send(command);
                }
            }
        }

        private float _attack = -1;
        public float Attack
        {
            get
            {
                if (_attack == -1 && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/attack");
                    var response = _x32Client.SendAndReceive(command);
                    _attack = float.Parse(response.Arguments[0].ToString());
                }

                return _attack;
            }
            set
            {
                if (_attack != value)
                {
                    _attack = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/attack", new object[] { _attack });
                    _x32Client.Send(command);
                }
            }
        }

        private float _hold = -1;
        public float Hold
        {
            get
            {
                if (_hold == -1 && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/hold");
                    var response = _x32Client.SendAndReceive(command);
                    _hold = float.Parse(response.Arguments[0].ToString());
                }

                return _hold;
            }
            set
            {
                if (_hold != value)
                {
                    _hold = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/hold", new object[] { _hold });
                    _x32Client.Send(command);
                }
            }
        }

        private float _release = -1;
        public float Release
        {
            get
            {
                if (_release == -1 && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/release");
                    var response = _x32Client.SendAndReceive(command);
                    _release = float.Parse(response.Arguments[0].ToString());
                }

                return _release;
            }
            set
            {
                if (_release != value)
                {
                    _release = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/release", new object[] { _release });
                    _x32Client.Send(command);
                }
            }
        }

        private X32Source _keySource = X32Source.NotSet;
        public X32Source KeySource
        {
            get
            {
                if (_keySource == X32Source.NotSet && _x32Client != null)
                {
                    OscMessage getChannelName = new OscMessage($"{_requestPrefix}/keysrc");
                    var response = _x32Client.SendAndReceive(getChannelName);
                    _keySource = (X32Source)Enum.Parse(typeof(X32Source), response.Arguments[0].ToString());
                }

                return _keySource;
            }
            set
            {
                if (_keySource != value)
                {
                    _keySource = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/keysrc", new object[] { (int)_keySource });
                    _x32Client.Send(command);
                }
            }
        }

        private X32FilterType _filterType = X32FilterType.NotSet;
        public X32FilterType FilterType
        {
            get
            {
                if (_filterType == X32FilterType.NotSet && _x32Client != null)
                {
                    OscMessage getChannelName = new OscMessage($"{_requestPrefix}/filter/type");
                    var response = _x32Client.SendAndReceive(getChannelName);
                    _filterType = (X32FilterType)Enum.Parse(typeof(X32FilterType), response.Arguments[0].ToString());
                }

                return _filterType;
            }
            set
            {
                if (_filterType != value)
                {
                    _filterType = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/filter/type", new object[] { (int)_filterType });
                    _x32Client.Send(command);
                }
            }
        }

        private bool? _filterOn = null;
        public bool FilterOn
        {
            get
            {
                if (_filterOn == null && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/filter/on");
                    var response = _x32Client.SendAndReceive(command);
                    _filterOn = int.Parse(response.Arguments[0].ToString()) == 1;
                }

                return _filterOn ?? false;
            }
            set
            {
                if (_filterOn != value)
                {
                    _filterOn = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/filter/on", new object[] { _filterOn == true ? 1 : 0 });
                    _x32Client.Send(command);
                }
            }
        }

       

        private float _filterFreq = -1;
        public float FilterFreq
        {
            get
            {
                if (_filterFreq == -1 && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/filter/f");
                    var response = _x32Client.SendAndReceive(command);
                    _filterFreq = float.Parse(response.Arguments[0].ToString());
                }

                return _filterFreq;
            }
            set
            {
                if (_filterFreq != value)
                {
                    _filterFreq = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/filter/f", new object[] { _filterFreq });
                    _x32Client.Send(command);
                }
            }
        }

        public Gate(X32Client x32Client, string channelRequestPrefix)
        {
            _x32Client = x32Client;
            _requestPrefix = channelRequestPrefix + "/gate";
        }
    }


    public enum X32Color
    {
        NotSet = -1,
        Black = 0,
        Red,
        Green,
        Yellow,
        Blue,
        Purple,
        Cyan,
        White,
        InvertedBlack,
        InvertedRed,
        InvertedGreen,
        InvertedYellow,
        InvertedBlue,
        InvertedPurple,
        InvertedCyan,
        InvertedWhite
    }

    public enum X32Source
    {
        NotSet = -1,
        Off = 0,

        In01,
        In02,
        In03,
        In04,
        In05,
        In06,
        In07,
        In08,
        In09,

        In10,
        In11,
        In12,
        In13,
        In14,
        In15,
        In16,
        In17,
        In18,
        In19,

        In20,
        In21,
        In22,
        In23,
        In24,
        In25,
        In26,
        In27,
        In28,
        In29,

        In30,
        In31,
        In32,

        Aux1,
        Aux2,
        Aux3,
        Aux4,
        Aux5,
        Aux6,

        USBL,
        USBR,

        FX1L,
        FX1R,
        FX2L,
        FX2R,
        FX3L,
        FX3R,
        FX4L,
        FX4R,

        Bus1,
        Bus2,
        Bus3,
        Bus4,
        Bus5,
        Bus6,
        Bus7,
        Bus8,
        Bus9,
        Bus10,
        Bus11,
        Bus12,
        Bus13,
        Bus14,
        Bus15,
        Bus16
    }

    public enum X32GateMode
    {
        NotSet = -1,
        Exp2 = 0,
        Exp3,
        Exp4,
        Gate,
        Duck
    }

    public enum X32FilterType
    {
        NotSet = -1,
        LC6 = 0,
        LC12,
        HC6,
        HC12,
        One,
        Two,
        Three,
        Five,
        Ten
    }

    public static class X32Extensions
    {
        public static string GetName(this X32Source val)
        {
            return Enum.GetName(typeof(X32Source), val);
        }

        public static string GetName(this X32Color val)
        {
            return Enum.GetName(typeof(X32Color), val);
        }

        public static string GetName(this X32GateMode val)
        {
            return Enum.GetName(typeof(X32GateMode), val);
        }

        public static string GetName(this X32FilterType val)
        {
            return Enum.GetName(typeof(X32FilterType), val);
        }
    }
}
