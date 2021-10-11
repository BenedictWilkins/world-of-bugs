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
    public RenderTexture bugMask;

    private ConfigSideChannel configChannel;
    private LogSideChannel logChannel;


    [Serializable]
    public class BugOption { 
        public Bug bug;
        public bool enabled = false;
        // TODO there might be more too it with enabling/disabling...
        public void Enable() {
            //Debug.Log("ENABLE");
            if (!enabled) {
                //Debug.Log("REAL ENABLE");
                bug.Enable();
                enabled = true;
            }
        }
        public void Disable() {
           // Debug.Log("DISABLE");
            if (enabled) {
                //Debug.Log("REAL DISABLE");
                bug.Disable();
                enabled = false;
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
        
        // if running in the editor, use the options provided, otherwise assume all bugs are turned off and wait for input from the config side channel.
        if (Application.isEditor) {
            BugOption[] bugOptions = Array.FindAll(bugs, x => x.enabled);
            foreach (BugOption bugOption in bugOptions) {
                bugOption.bug.Enable();
            }
        } else {
            // initially disable all bugs in a build version, they must be set explicitly using the config side channel
            foreach (BugOption bugOption in bugs) {
                bugOption.enabled = false; 
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

    public bool BugMaskActive() {
        Texture2D tex = new Texture2D(bugMask.width, bugMask.height);
        int mipmapLevel = (int) Mathf.Log(bugMask.width, 2);
        int activeLowerBound = 0; // change this?? TODO 

        RenderTexture.active = bugMask;
        tex.ReadPixels(new Rect(0, 0, bugMask.width, bugMask.height), 0, 0, true);
        tex.Apply();
        Color32 ac = tex.GetPixels32(mipmapLevel)[0];
        bool active = (ac[0] + ac[1] + ac[2]) > activeLowerBound;
        return active;
    }

    public bool BugInView(Player player) {
        // if the player is using a camera then check if it is in view

        BugMaskActive();

        Camera camera = player.camera;
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
                    //Debug.Log("ENABLED BUG : " + name);
                } else {
                    bugOption.Disable();
                    //Debug.Log("DISABLED BUG : " + name);
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
