using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.SideChannels;

public class Controller : MonoBehaviour {
    
    public BugOption[] bugs;
    public Agent player; 
    private ConfigSideChannel configChannel;
    private LogSideChannel logChannel;

    [Serializable]
    public class BugOption { 
        public Bug bug;
        public bool enabled = false;
        // TODO there might be more too it with enabling/disabling...
        public void Enable() {
            if (!enabled) {
                bug.Enable();
            }
        }
        public void Disable() {
            if (enabled) {
                bug.Disable();
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
        

        Debug.Log("???");
        if (Application.isEditor) {
            Debug.Log("RUNNING EDITOR");
            BugOption[] bugOptions = Array.FindAll(bugs, x => x.enabled);
            foreach (BugOption bugOption in bugOptions) {
                bugOption.bug.Enable();
            }
        } else {
            
            Debug.Log("RUNNING STANDALONE");

            // BugOption enabled will be ignored as used, enable bugs based on command line arguments
            Bug[] bugs_ = bugs.Select(x => x.bug).ToArray();
            string[] args = System.Environment.GetCommandLineArgs();
            char[] trimChars = {'-'};
            for (var i = 0; i < args.Length - 1; i++) {  
                Debug.Log(args[i]); 
                if (args[i].StartsWith("-")) {
                    string name = args[i].Trim(trimChars);
                    Bug bug = Array.Find(bugs_, x => x.GetType().Name.Equals(name));
                    if (bug != null) {
                        bug.Enable();
                        Debug.Log("ENABLE BUG: " + name);
                    }
                }
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

    void Reset() {
        player.EndEpisode();
        // read the environment channel TODO
    }

    // Update is called once per frame
    void Update() {
        // the agent fell out of the world
        if (player.transform.localPosition.y < -1) {
            Reset();
        }
    }

    public class ConfigSideChannel : SideChannel {
        Controller contr;

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

                BugOption bugOption = Array.Find(contr.bugs, x => x.bug.GetType().Name.Equals(name));

                if (bugOption == null) {
                    throw new KeyNotFoundException("Requested bug was not found: " + name);
                } else if (enable) {
                    bugOption.Enable();
                    Debug.Log("ENABLED BUG : " + name);
                } else {
                    bugOption.Disable();
                    Debug.Log("DISABLED BUG : " + name);
                }
                
            } catch  (Exception e){
                Debug.LogException(e, contr);
            }
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
