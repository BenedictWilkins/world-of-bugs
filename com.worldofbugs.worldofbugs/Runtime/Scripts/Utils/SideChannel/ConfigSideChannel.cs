using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.SideChannels;

namespace WorldOfBugs {

    public class ConfigurationException : WorldOfBugsException {
        public ConfigurationException(string message): base(message) {}
        public ConfigurationException(string message, Exception inner): base(message, inner) {}
    }

    public class ConfigMessage {
        public ConfigMessage(string data) {
            string[] _data = data.Split(':');
            Attribute = _data[0];
            Value = _data[1];
        }
        public string Attribute { get; set; }
        public string Value { get; set; }
    }

    public class ConfigSideChannel : SideChannel {

        protected Controller controller;
        protected ConfigAPI configAPI;

        public ConfigSideChannel(Controller _controller) {
            // GUID is needed by python to communicate with this channel...
            ChannelId = new Guid("621f0a70-4f87-11ea-a6bf-784f4387d1f7");
            controller = _controller;
            configAPI = new ConfigAPI(controller);
        }
       
        protected override void OnMessageReceived(IncomingMessage msg) {
            ConfigMessage config = new ConfigMessage(msg.ReadString());
            configAPI.Resolve(config); // set the value in the message
        }
    }

        
    

}
 