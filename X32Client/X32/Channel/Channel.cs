using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X32.OSC;
using X32.Enums;

namespace X32.Channels
{
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
                    OscMessage command = new OscMessage($"{_requestPrefix}/config/name");
                    var response = _x32Client.SendAndReceive(command);
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
                    OscMessage command = new OscMessage($"{_requestPrefix}/config/icon");
                    var response = _x32Client.SendAndReceive(command);
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
                    OscMessage command = new OscMessage($"{_requestPrefix}/config/color");
                    var response = _x32Client.SendAndReceive(command);
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
                    OscMessage command = new OscMessage($"{_requestPrefix}/config/source");
                    var response = _x32Client.SendAndReceive(command);
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

        public Compressor Compressor { get; private set; }

        public Channel(int channelNumber, X32Client client)
        {
            this._x32Client = client;
            ChannelNumber = channelNumber;
            _requestPrefix = $"/ch/{ChannelNumber.ToString().PadLeft(2, '0')}";

            Delay = new Delay(_x32Client, _requestPrefix);
            Preamp = new Preamp(_x32Client, _requestPrefix);
            Gate = new Gate(_x32Client, _requestPrefix);
            Compressor = new Compressor(_x32Client, _requestPrefix);
        }
    }
}
