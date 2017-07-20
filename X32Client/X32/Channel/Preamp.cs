using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X32.OSC;

namespace X32.Channels
{
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
}
