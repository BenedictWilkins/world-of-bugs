using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.AddressableAssets;

using WorldOfBugs;

public class TestPlayer {

    public static string PLAYER_ADDRESS = "Assets/World-v1/Prefabs/Player.prefab";
    public GameObject Player;

    [SetUp]
    public void Setup() {
        Player = Addressables.InstantiateAsync(PLAYER_ADDRESS).WaitForCompletion();
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestPlayerReset() {
        Player.transform.position = new Vector3(-50, -50, -50);
        yield return new WaitForFixedUpdate();
        Assert.GreaterOrEqual(Player.transform.position.y, 0);
    }
}
