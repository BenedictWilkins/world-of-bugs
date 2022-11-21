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
public class Controller : MonoBehaviour, IReset {

    [NotNull, NotEmpty]
    public WorldOfBugs.Agent[] Agents;

    public Bug[] Bugs {
        get {
            return GetComponents<Bug>().ToArray();
        }
    }

    private ConfigSideChannel configChannel;
    private LogSideChannel logChannel;

    void Awake() {
        enabled = true; // controller should always be enabled...
        // configuration side channel
        configChannel = new ConfigSideChannel(this);
        SideChannelManager.RegisterSideChannel(configChannel);
        // logging side channel
        logChannel = new LogSideChannel();
        SideChannelManager.RegisterSideChannel(logChannel);
        Application.logMessageReceived += logChannel.LogDebug;
    }

    public void OnDestroy() {
        // Deregister the Debug.Log callback
        try {
            Application.logMessageReceived -= logChannel.LogDebug;
        } catch { } // this probably means it wasnt set up in the first place... forgiveness :)

        if(Academy.IsInitialized) {
            SideChannelManager.UnregisterSideChannel(configChannel);
            SideChannelManager.UnregisterSideChannel(logChannel);
        }
    }
    public bool ShouldReset(GameObject agent) {
        return false; // if this is true, the given agent will reset
    }

    public void Reset() {
        Debug.Log($"RESTART {System.Diagnostics.Process.GetCurrentProcess().Id}");

        foreach(Bug option in Bugs) {
            // reset bugs (this will switch bugs that are enabled to enabled again, and bugs that are disabled to disabled again)
            option.enabled = !option.enabled;
            option.enabled = !option.enabled;
        }
    }

    public static Controller FindInstanceInScene() {
        Controller[] controllers = FindObjectsOfType<Controller>();

        if(controllers.Length != 1) {
            if(controllers.Length == 0) {
                throw new WorldOfBugsException("A global Controller object was not found, but needs to be defined in the scene.");
            } else {
                throw new WorldOfBugsException("More than one global Controller object was found, there can be at most one in a scene.");
            }
        }

        return controllers[0];
    }
}
