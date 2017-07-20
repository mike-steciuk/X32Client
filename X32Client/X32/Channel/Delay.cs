using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X32.OSC;

namespace X32.Channels
{
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

        public Delay(X32Client x32Client, string channelRequestPrefix)
        {
            _x32Client = x32Client;
            _requestPrefix = channelRequestPrefix + "/delay";
        }

    }
}
