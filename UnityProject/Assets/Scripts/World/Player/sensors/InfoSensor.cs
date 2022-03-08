using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Unity.MLAgents.Sensors {

    public class InfoSensor : ISensor {
        
        public static readonly int SIZE = 7;
        protected Behaviour behaviour;
        protected string name;
        protected Vector3 p_position;
        protected Vector3 p_rotation;

        public InfoSensor(Behaviour _behaviour, string _name) {
            //base(SIZE, name);
            behaviour = _behaviour;
            name = _name;
            p_position = behaviour.Player.transform.position;
            p_rotation = behaviour.Player.transform.rotation.eulerAngles;
        }

        public int Write(ObservationWriter writer) {
            List<float> result = new List<float>(SIZE);
            Vector3 p = behaviour.Player.transform.position;
            Vector3 r = behaviour.Player.transform.rotation.eulerAngles;
            float dp = Vector3.Dot(p - p_position, Vector3.one);
            float dr = Vector3.Dot(r - p_rotation, Vector3.one);
            int action = behaviour.Action; // action that lead to this observation (A_t)

            //Debug.Log($"{Time.frameCount} {action} {dp} {dr}");

            result.Add((float)action);
            result.Add(p.x);
            result.Add(p.y);
            result.Add(p.z);
            result.Add(r.x);
            result.Add(r.y);
            result.Add(r.z);
            writer.AddList(result); // S_t+1
            p_position = p;
            p_rotation = r;
            return SIZE;
        }

        public int[] GetObservationShape() {
            return new int[] { SIZE };
        }

        public string GetName(){
            return name;
        }

        public virtual SensorCompressionType GetCompressionType() {
            return SensorCompressionType.None;
        }

        public BuiltInSensorType GetBuiltInSensorType() {
            return BuiltInSensorType.VectorSensor;
        }

        public virtual byte[] GetCompressedObservation() {
            return null;
        }

        public void Update() {}

        public void Reset() {}

    }
}
