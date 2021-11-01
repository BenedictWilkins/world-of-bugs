using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Unity.MLAgents.Sensors {

    public class InfoSensor : ISensor {
        
        public static readonly int SIZE = 7;
        protected Behaviour behaviour;
        protected string name;


        public InfoSensor(Behaviour _behaviour, string _name) {
            //base(SIZE, name);
            behaviour = _behaviour;
            name = _name;
        }

        public int Write(ObservationWriter writer) {
            List<float> result = new List<float>(SIZE);
            int action = behaviour.Action;
            Vector3 p = behaviour.Player.transform.position;
            Vector3 r = behaviour.Player.transform.rotation.eulerAngles;
            result.Add((float)action);
            result.Add(p.x);
            result.Add(p.y);
            result.Add(p.z);
            result.Add(r.x);
            result.Add(r.y);
            result.Add(r.z);
            writer.AddList(result);
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
