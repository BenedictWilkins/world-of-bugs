
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;

namespace WorldOfBugs {

    public abstract class InfoSensor : ISensor {

        public abstract int Size { get; }

        protected Agent Agent;
        protected string Name;
        private List<float> perception;

        public InfoSensor(Agent  _agent, string _name) {
            Agent = _agent;
            Name = _name;
            Debug.Log($"CREATED INFO SENSOR {Name}");
        }

        public abstract List<float> Perceive();
        public abstract void Reset();

        public int Write(ObservationWriter writer) {
            writer.AddList(perception);
            return Size;
        }

        public void Update() {
            perception = Perceive();
            if (perception.Count != Size) {
                throw new WorldOfBugsException($"Perception vector contains {perception.Count} elements, but should contain {Size} according to ObservationSpec.");
            }
        }

        public ObservationSpec GetObservationSpec() {
            return ObservationSpec.Vector(Size);
        }

        public string GetName(){
            return Name;
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

        public CompressionSpec GetCompressionSpec() {
            return new CompressionSpec(SensorCompressionType.None);
        }


    }
}
