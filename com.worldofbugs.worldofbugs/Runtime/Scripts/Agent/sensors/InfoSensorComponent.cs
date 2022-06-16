
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.MLAgents.Sensors;
using TypeReferences; // External


namespace WorldOfBugs {

    // TODO protection level of MenuGroup bugged...? write an issue for this?
    [AddComponentMenu("ML Agents/Info Sensor", 0)]
    public class InfoSensorComponent : SensorComponent {

        [Inherits(typeof(ISensor))]
        public TypeReference InfoSensorType;

        [SerializeField, FormerlySerializedAs("sensorName")]
        string m_SensorName = "info";

        public string SensorName
        {
            get { return m_SensorName; }
            set { m_SensorName = value; }
        }

        public override ISensor[] CreateSensors() {
            ISensor sensor = Activator.CreateInstance(InfoSensorType, new object[] { gameObject.GetComponent<Agent>(), m_SensorName}) as ISensor;
            return new ISensor[] { sensor };
        }
    }
}
