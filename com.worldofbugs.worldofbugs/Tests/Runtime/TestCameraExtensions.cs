using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.AddressableAssets;

using WorldOfBugs;

public class TestCameraExtensions {
    // A Test behaves as an ordinary method
    public static string MAINCAMERA_ADRESS =
        "Packages/com.worldofbugs.worldofbugs/Tests/Runtime/Prefabs/MainCamera.prefab";
    public static string BUGMASKCAMERA_ADRESS =
        "Packages/com.worldofbugs.worldofbugs/Tests/Runtime/Prefabs/BugMaskCamera.prefab";

    public Camera main_camera;
    public Camera bugmask_camera;

    [SetUp]
    public void Setup() {
        main_camera = Addressables.InstantiateAsync(
                          MAINCAMERA_ADRESS).WaitForCompletion().GetComponent<Camera>();
        bugmask_camera = Addressables.InstantiateAsync(
                             BUGMASKCAMERA_ADRESS).WaitForCompletion().GetComponent<Camera>();
    }

    [Test]
    public void TestGetCameraMain() {
        Camera[] cameras = CameraExtensions.GetObservationCameras();
        Debug.Log(cameras);
        Assert.Greater(cameras.Length, 0);
        Assert.Contains(main_camera, cameras);
    }

    [Test]
    public void TestGetCameraBugMask() {
        Camera[] cameras = CameraExtensions.GetBugMaskCamera();
        Assert.Greater(cameras.Length, 0);
        Assert.Contains(bugmask_camera, cameras);
    }

    [Test]
    public void TestGetCameraByRenderTexture() {
        Camera[] cameras = CameraExtensions.GetCamerasByRenderTexture(
                               main_camera.targetTexture);
        Assert.AreEqual(cameras.Length, 1);
        Assert.AreSame(main_camera.targetTexture, cameras[0].targetTexture);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    //[UnityTest]
    //public IEnumerator TestCameraExtensionsWithEnumeratorPasses()
    //{
    // Use the Assert class to test conditions.
    // Use yield to skip a frame.
    //    yield return null;
    //}

    [TearDown]
    public void teardown() {
        Object.DestroyImmediate(main_camera.gameObject);
        Object.DestroyImmediate(bugmask_camera.gameObject);
    }
}
