using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Reflection;
using System.Globalization;

using UnityEngine;

namespace WorldOfBugs {

    public class ConfigAPI { 

        private Controller controller;

        public abstract class Configurator {} 

        public class BugConfigurator : Configurator { 
            public Bug Bug;
            // public API to enable a bug
            public string enabled { 
                get { throw new NotImplementedException(); } 
                set { Bug.enabled = Convert.ToBoolean(value); Debug.Log($"Bug {Bug.GetType().Name} enabled = {value}"); }
            }
        }

        public class PolicyConfigurator : Configurator { 
            public Agent Agent;
            public HeuristicComponent Heuristic;

            // public API to set the current heuristic
            public string heuristic { 
                get { throw new NotImplementedException(); } 
                set { 
                    bool v = Convert.ToBoolean(value); // turn on this heuristic behaviour.
                    Agent.SetHeuristic(Heuristic);
                    Debug.Log($"{Agent} is using Heuristic {Heuristic.GetType().Name}.");
                }
            }
        }

        public Dictionary<string, BugConfigurator> Bugs { 
            get { return controller.Bugs.ToDictionary(
                x => x.GetType().Name, 
                x => new BugConfigurator() { Bug = x }); } 
        }
        public Dictionary<string, PolicyConfigurator> Heuristics { 
            // TODO support multiple agents...
            get { return controller.Agents[0].GetComponents<HeuristicComponent>().ToDictionary(
                x => x.GetType().Name, 
                x => new PolicyConfigurator() { Agent = controller.Agents[0], Heuristic = x });
            }
        }

        public ConfigAPI(Controller _controller) {
            controller = _controller;

            //Debug.Log($"MAD {Bugs.Count} {Heuristics.Count}");
        }

        // sets an field value given a dot seperated string (Attribute) and a string (Value) which may be converted to the correct type.
        public void Resolve(ConfigMessage message) {
            string[] chain = message.Attribute.Split('.');
            if (chain.Length != 3) {
                throw new ConfigurationException($"Invalid configurable attribute {message.Attribute} provided, must be a dot-seperated string with three components. e.g. `Bugs.MissingTexture.enabled`. " 
                + "Configurable attributes should follow the form <GROUP>.<PROPERTY>.<MEMBER>.");
            }
            PropertyInfo info = this.GetType().GetProperty(chain[0]); // choose amoung the Dictionary properties of the API
            if (info == null) {
                string err_groups = string.Join(",", this.GetType().GetProperties().Select(x=>x.Name));
                throw new ConfigurationException($"Group: {chain[0]} not found, valid configuration groups are: {err_groups}");
            }

            IDictionary _configurators = (IDictionary) info.GetValue(this);
            Dictionary<string, Configurator> configurators = IDictionaryEnumerate(_configurators).ToDictionary(x => (string)x.Key, x => (Configurator)x.Value);
            
            if (!configurators.ContainsKey(chain[1])) {
                string err_properties = string.Join(",", configurators.Keys);
                throw new ConfigurationException($"Property {chain[1]} not found, valid properties for {chain[0]} are: {err_properties}");
            }
            Configurator configurator = configurators[chain[1]]; 

            info = configurator.GetType().GetProperty(chain[2]);
            if (info == null) {
                string err_members = string.Join(",", configurator.GetType().GetProperties().Select(x=>x.Name));
                throw new ConfigurationException($"Member: {chain[2]} not found, valid configuration members for {chain[0]}.{chain[1]} are: {err_members}");
            }
            info.SetValue(configurator, message.Value); // set the config option
        }

        
        
        private IEnumerable<DictionaryEntry> IDictionaryEnumerate(IDictionary dictionary) {
            foreach (DictionaryEntry entry in dictionary) {
                yield return entry;
            }
        }
    }
}