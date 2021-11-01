using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.MLAgents.Sensors {

    // protection level of MenuGroup bugged...? 
    [AddComponentMenu("ML Agents/Info Sensor", 0)]
    public class InfoSensorComponent : SensorComponent {
        InfoSensor m_Sensor;

        [SerializeField, FormerlySerializedAs("sensorName")]
        string m_SensorName = "info";

        public string SensorName
        {
            get { return m_SensorName; }
            set { m_SensorName = value; }
        }

        public override ISensor CreateSensor() {
            m_Sensor = new InfoSensor(gameObject.GetComponent<Behaviour>(), SensorName);
            return m_Sensor;
        }

        public override int[] GetObservationShape() {
            int[] observationShape = new[] { InfoSensor.SIZE };
            return observationShape;
        }
        
        internal void UpdateSensor() {}
    }
}
