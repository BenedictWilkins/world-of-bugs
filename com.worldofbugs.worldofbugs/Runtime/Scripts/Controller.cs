using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.SideChannels;

using WorldOfBugs;

[ExecuteInEditMode]
public class Controller : MonoBehaviour {
    
    


    /* [Serializable]
    public class BugOption { 

        public Bug _bug;
        public Bug Bug {
            get { return _bug; }
            set { _bug = value; enabled = false; }
        }

        public BugOption(Bug bug) {
            _bug = bug;
        }
        
        public bool enabled { 
            get { return Bug.gameObject.activeInHierarchy; } 
            set { Debug.Log($"{Bug} {Bug?.gameObject}"); Bug.gameObject.SetActive(value); }
        }
    } */

    [NotNull, NotEmpty]
    public WorldOfBugs.Agent[] Agents; 

    public Bug[] Bugs { get {  return GetComponents<Bug>().ToArray(); }} 

    private ConfigSideChannel configChannel;
    private LogSideChannel logChannel;
    
    void Awake() {
        // configuration side channel
        configChannel = new ConfigSideChannel(this);
        SideChannelManager.RegisterSideChannel(configChannel);

        // logging side channel
        logChannel = new LogSideChannel();
        SideChannelManager.RegisterSideChannel(logChannel);
        Application.logMessageReceived += logChannel.LogDebug;
    }
    
   

    void Start() {
        if (!Application.isEditor) { // all bugs are turned off initially.
            foreach (Bug bug in Bugs) {
                bug.enabled = false;
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
        // Debug.Log($"RESTART {System.Diagnostics.Process.GetCurrentProcess().Id}");
        foreach (Bug option in Bugs) {
            if (option.enabled) { // re-enable all bugs 
                option.enabled = false;
                option.enabled = true;
            }
        }
    }
}
