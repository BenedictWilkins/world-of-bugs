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
        if (!Application.isEditor) {
            //foreach (WorldOfBugs.Agent agent in Agents) {
            //    agent.SetHeuristic(null); // all heuristics
            //}

            // all bugs are turned off initially.
            foreach (Bug bug in Bugs) {
                bug.enabled = false;
            }
        }
    }

    public void OnDestroy() {
        // Deregister the Debug.Log callback
        try {
            Application.logMessageReceived -= logChannel.LogDebug;
        } catch { } // this probably means it wasnt set up in the first place... forgiveness :)

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
