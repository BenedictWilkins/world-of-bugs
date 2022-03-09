using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.SideChannels;


using WorldOfBugs;

public class Controller : MonoBehaviour {
    
    public WorldOfBugs.Agent player; 
    public BugOption[] bugs;

    private ConfigSideChannel configChannel;
    private LogSideChannel logChannel;

    [Serializable]
    public class BugOption { 
        public Bug bug;
        public bool enabled = false;
        // TODO there might be more too it with enabling/disabling...
        public void Enable() {
            bug.gameObject.SetActive(true);
        }
        public void Disable() {
            bug.gameObject.SetActive(false);
        }
        public void Initialise() {
            if (enabled) {
                Enable();
            } else {
                Disable();
            }
        }
    }

    void Awake() {
        // set up side channels for communicating with Python
        configChannel = new ConfigSideChannel(this);
        SideChannelManager.RegisterSideChannel(configChannel);

        logChannel = new LogSideChannel();
        SideChannelManager.RegisterSideChannel(logChannel);
        Application.logMessageReceived += logChannel.LogDebug;
    }

    void Start() {
        // if running in the editor, use the options provided, otherwise assume all bugs are turned off and wait for input from the config side channel.
        if (Application.isEditor) {
            Debug.Log("RUNNING IN EDITOR MODE");
            foreach (BugOption bugOption in bugs) {
                //Debug.Log(bugOption.bug);
                bugOption.Initialise();
            }
        } else {
            Debug.Log("RUNNING IN STANDALONE MODE");
            // initially disable all bugs in a build version, they must be set explicitly using the config side channel
            foreach (BugOption bugOption in bugs) {
                bugOption.enabled = false; 
                bugOption.Initialise();
            }
        }
    }

    public void OnDestroy() {
        // Deregister the Debug.Log callback
        Application.logMessageReceived -= logChannel.LogDebug;
        if (Academy.IsInitialized) {
            SideChannelManager.UnregisterSideChannel(configChannel);
            SideChannelManager.UnregisterSideChannel(logChannel);
        }
    }

    public void OnEpisodeBegin() {
        // renable bugs
        Debug.Log($"RESTART {System.Diagnostics.Process.GetCurrentProcess().Id}");
        foreach (BugOption bugOption in bugs) {
            //Debug.Log($"ENABLED: {bugOption} {bugOption.enabled}");
            if (bugOption.enabled) {
                bugOption.bug.OnDisable();
                bugOption.bug.OnEnable();
            }
        }
    }

    public bool BugInView(WorldOfBugs.AgentFirstPerson player) {
        Camera camera = player.CameraBugMask;
        foreach (BugOption bugOption in bugs) {
            bool inview = bugOption.bug.InView(camera);
            if (inview) {
                return true;
            }
        }
        return false;
    }

    public class ConfigSideChannel : SideChannel {
        Controller contr;

        public class ConfigurationException : Exception{
                public ConfigurationException(string message): base(message) {}
                public ConfigurationException(string message, Exception inner): base(message, inner) {}
        }


        public ConfigSideChannel(Controller controller) {
            contr = controller;
            // GUID is needed by python to communicate with this channel...
            ChannelId = new Guid("621f0a70-4f87-11ea-a6bf-784f4387d1f7");
        }

        protected override void OnMessageReceived(IncomingMessage msg) {
            // process bug enable/disable message...
            try {
                string[] received = msg.ReadString().Split(':');
                string name = received[0];
                bool enable = bool.Parse(received[1]);
                if (ConfigureBugOption(name, enable)) {
                    return;
                }
                if (ConfigurePlayer(name, enable)) {
                    return;
                }
                  
            } catch  (Exception e){
                Debug.LogException(e, contr);
            }
        }
   

        protected bool ConfigurePlayer(string name, bool enable) {
            if (! enable) { 
                //throw new ConfigurationException("Player behaviours can only be enabled.");
                return false;
            }
            // TODO
            //BehaviourType value =  (BehaviourType) Enum.Parse(typeof(BehaviourType), name);
            //contr.player.BehaviourType = value;
            return true;
        }

        protected bool ConfigureBugOption(string name, bool enable) {
            // enable/disable bugs
            BugOption bugOption = Array.Find(contr.bugs, x => x.bug.GetType().Name.Equals(name));
            
            if (bugOption != null) {
                if (enable) {
                    bugOption.enabled = true;
                    bugOption.Enable();
                } else {
                    bugOption.enabled = false;
                    bugOption.Disable();
                }
            }
            return bugOption != null;        
        }
    }

    public class LogSideChannel : SideChannel {

         public LogSideChannel() {
            // GUID is needed by python to communicate with this channel...
            ChannelId = new Guid("c601d1fe-b487-454e-a6b3-b7305d884442");
        }

        protected override void OnMessageReceived(IncomingMessage msg) {
            //Debug.Log("From Python : " + msg.ReadString());
        }

        public void LogDebug(string msg, string stackTrace, LogType type) {
            using (var msgOut = new OutgoingMessage()) {
                msgOut.WriteString(type.ToString() + ": " + msg);
                //msgOut.WriteString(type.ToString() + ": " + msg + "\n" + stackTrace);
                QueueMessageToSend(msgOut);
            }
        }
    }
}
