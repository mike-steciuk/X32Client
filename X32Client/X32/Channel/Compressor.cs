using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X32.Enums;
using X32.OSC;

namespace X32.Channels
{
    public class Compressor
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

        private X32CompressorMode _mode = X32CompressorMode.NotSet;
        public X32CompressorMode Mode
        {
            get
            {
                if (_mode == X32CompressorMode.NotSet && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/mode");
                    var response = _x32Client.SendAndReceive(command);
                    _mode = (X32CompressorMode)Enum.Parse(typeof(X32CompressorMode), response.Arguments[0].ToString());
                }

                return _mode;
            }
            set
            {
                if (_mode != value)
                {
                    _mode = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/mode", new object[] { (int)_mode });
                    _x32Client.Send(command);
                }
            }
        }

        private X32CompressorDetectionType _det = X32CompressorDetectionType.NotSet;
        public X32CompressorDetectionType DetectionType
        {
            get
            {
                if (_det == X32CompressorDetectionType.NotSet && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/det");
                    var response = _x32Client.SendAndReceive(command);
                    _det = (X32CompressorDetectionType)Enum.Parse(typeof(X32CompressorDetectionType), response.Arguments[0].ToString());
                }

                return _det;
            }
            set
            {
                if (_det != value)
                {
                    _det = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/det", new object[] { (int)_det });
                    _x32Client.Send(command);
                }
            }
        }

        private X32CompressorEnvelopeType _env = X32CompressorEnvelopeType.NotSet;
        public X32CompressorEnvelopeType EnvelopeType
        {
            get
            {
                if (_env == X32CompressorEnvelopeType.NotSet && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/env");
                    var response = _x32Client.SendAndReceive(command);
                    _env = (X32CompressorEnvelopeType)Enum.Parse(typeof(X32CompressorEnvelopeType), response.Arguments[0].ToString());
                }

                return _env;
            }
            set
            {
                if (_env != value)
                {
                    _env = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/env", new object[] { (int)_env });
                    _x32Client.Send(command);
                }
            }
        }

        private float _threshold = -90;
        /// <summary>
        /// min|max|step = -60|0|0.5
        /// 
        /// </summary>
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

        private X32CompressorRatio _ratio = X32CompressorRatio.NotSet;
        public X32CompressorRatio Ratio
        {
            get
            {
                if (_ratio == X32CompressorRatio.NotSet && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/ratio");
                    var response = _x32Client.SendAndReceive(command);
                    _ratio = (X32CompressorRatio)Enum.Parse(typeof(X32CompressorRatio), response.Arguments[0].ToString());
                }

                return _ratio;
            }
            set
            {
                if (_ratio != value)
                {
                    _ratio = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/ratio", new object[] { (int)_ratio });
                    _x32Client.Send(command);
                }
            }
        }

        private float _knee = -1;
        public float Knee
        {
            get
            {
                if (_knee == -1 && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/knee");
                    var response = _x32Client.SendAndReceive(command);
                    _knee = float.Parse(response.Arguments[0].ToString());
                }

                return _knee;
            }
            set
            {
                if (_knee != value)
                {
                    _knee = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/knee", new object[] { _knee });
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

        private X32Position _pos = X32Position.NotSet;
        public X32Position Position
        {
            get
            {
                if (_pos == X32Position.NotSet && _x32Client != null)
                {
                    OscMessage command = new OscMessage($"{_requestPrefix}/pos");
                    var response = _x32Client.SendAndReceive(command);
                    _pos = (X32Position)Enum.Parse(typeof(X32Position), response.Arguments[0].ToString());
                }

                return _pos;
            }
            set
            {
                if (_pos != value)
                {
                    _pos = value;
                    OscMessage command = new OscMessage($"{_requestPrefix}/pos", new object[] { (int)_pos });
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
                    OscMessage command = new OscMessage($"{_requestPrefix}/keysrc");
                    var response = _x32Client.SendAndReceive(command);
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
                    OscMessage command = new OscMessage($"{_requestPrefix}/filter/type");
                    var response = _x32Client.SendAndReceive(command);
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

        public Compressor(X32Client x32Client, string channelRequestPrefix)
        {
            _x32Client = x32Client;
            _requestPrefix = channelRequestPrefix + "/dyn";
        }
    }
}
