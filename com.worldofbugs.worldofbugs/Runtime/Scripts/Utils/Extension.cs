using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.AI;

namespace WorldOfBugs {

    /// <summary>
    /// Extensions that are useful for the Camera class.
    /// </summary>
    public static class CameraExtensions {

        public static readonly string CAMERA_TAG_OBSERVATION = "MainCamera";
        public static readonly string CAMERA_TAG_BUGMASK = "BugMaskCamera";

        /// <summary>
        /// Get all cameras that are used by agents to sense the environment.
        /// </summary>
        /// <returns>Observation cameras</returns>
        public static Camera[] GetObservationCameras() {
            return Camera.allCameras.Where(c => c.gameObject.CompareTag(
                                               CAMERA_TAG_OBSERVATION)).ToArray();
        }
        /// <summary>
        /// Get all cameras that render a mask over the agents observation.
        /// </summary>
        /// <returns>Bug mask cameras</returns>
        public static Camera[] GetBugMaskCamera() {
            return Camera.allCameras.Where(c => c.gameObject.CompareTag(
                                               CAMERA_TAG_BUGMASK)).ToArray();
        }

        /// <summary>
        /// Get all cameras that use the same given <paramref name="texture"/>.
        /// </summary>
        /// <param name="texture">Shared RenderTexture</param>
        /// <returns></returns>
        public static Camera[] GetCamerasByRenderTexture(RenderTexture texture) {
            Camera[] cameras = Array.FindAll(Camera.allCameras, x => x.targetTexture == texture);
            // sort by depth, the camera that renders last is first
            Array.Sort<Camera>(cameras, (x, y) => - x.depth.CompareTo(y.depth));
            return cameras.ToArray();
        }

        /*  TODO Get pairs of cameras for each agent.. when multi-agent environments are supported?
            public static (Camera,Camera)[] GetCameras() {

            Debug.Log(string.Join(",", GetObservationCameras().Select(c => c.gameObject.tag)));
            Debug.Log(string.Join(",", GetBugMaskCamera().Select(c => c.gameObject.tag)));

            var observation = Camera.allCameras.Where(c => c.gameObject.CompareTag(CAMERA_TAG_OBSERVATION)).ToArray();
            var bugmask = Camera.allCameras.Where(c => c.gameObject.CompareTag(CAMERA_TAG_BUGMASK)).ToArray();

            IEqualityComparer comparer = new IEqualityComparer() {

            };
            var join = observation.Join(bugmask, x => x.gameObject, y => y.gameObject, (x, y) => (x, y), ).ToArray();
            return join;
            } */


    }

    public static class ColliderExtensions {

        public static void Enable(this Collider collider) {
            collider.enabled = true;
            NavMeshModifier mod = collider.gameObject.GetComponent<NavMeshModifier>();

            if(mod != null) {
                //mod.ignore = false;
            }
        }

        public static void Disable(this Collider collider) {
            collider.enabled = false;
            NavMeshModifier mod = collider.gameObject.GetComponent<NavMeshModifier>();

            if(mod != null) {
                //mod.ignore = true;
            }
        }
    }

    public static class GameObjectExtensions {

        public static T AddComponent<T>(this GameObject go, T component) where T : Component {
            return go.AddComponent<T>().Clone(component) as T;
        }

        public static GameObject GetChildWithName(this GameObject go, string name) {
            return go.transform.Find(name)?.gameObject;
        }
    }

    public static class ComponentExtensions {

        // https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
        public static T Clone<T>(this Component comp, T other) where T : Component {
            Type type = comp.GetType();

            if(type != other.GetType()) {
                return null;    // type mis-match
            }

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                                 BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);

            foreach(var pinfo in pinfos) {
                if(pinfo.CanWrite) {
                    try {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    } catch { }
                }
            }

            FieldInfo[] finfos = type.GetFields(flags);

            foreach(var finfo in finfos) {
                finfo.SetValue(comp, finfo.GetValue(other));
            }

            return comp as T;
        }
    }


}
