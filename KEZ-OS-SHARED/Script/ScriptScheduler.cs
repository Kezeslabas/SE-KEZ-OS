using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        /// <summary>
        /// The collection of all the scripts
        /// </summary>
        public enum ScriptType : byte
        {
            DEBUG,
            DEBUG2
        }

        /// <summary>
        /// Manages the scripts used by the OS
        /// </summary>
        public class ScriptScheduler
        {
            private readonly FrequencySchedule frequencySchedule = new FrequencySchedule();
            private readonly Dictionary<ScriptType, SchedulableScript> scripts = new Dictionary<ScriptType, SchedulableScript>();

            /// <summary>
            /// Adds a Schedulable script to the Script Scheduler
            /// </summary>
            public void RegisterScript(SchedulableScript script)
            {
                scripts.Add(script.ScriptType, script);
            }

            /// <summary>
            /// Get the Dictinary storing the 
            public Dictionary<ScriptType, SchedulableScript> GetAllScripts()
            {
                return scripts;
            }

            /// <summary>
            /// Returns the script instance, that corresponds to the provided argument
            /// </summary>
            public SchedulableScript FindScriptByName(string name)
            {
                switch (name)
                {
                    case "DEBUG": return scripts[ScriptType.DEBUG];
                    case "DEBUG2": return scripts[ScriptType.DEBUG2];
                    default: return null;
                }
            }

            /// <summary>
            /// Calls the Continue Method of all the registered Schedulable scripts, if this run is an automatic run
            /// </summary>
            public void ContinueAll(UpdateType updateType)
            {
                if((updateType & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) != 0)
                {
                    foreach (var script in scripts.Values)
                    {
                        script.Continue();
                    }
                }
            }

            /// <summary>
            /// Returns the script that corresponds to the provided argument
            /// </summary>
            public SchedulableScript DecodeArgument(string argument)
            {
                return FindScriptByName(argument);
            }

            /// <summary>
            /// Checks all the registered scripts if they require any re-run, and schedules a re-run with the correct ferquencies
            /// </summary>
            public void ScheduleAll(Action<UpdateFrequency> schedulerAction)
            {
                frequencySchedule.Reset();

                foreach (var script in scripts.Values)
                {
                    frequencySchedule.Evaulate(script.GetSchedule());
                }

                schedulerAction.Invoke(frequencySchedule.CalculateScheduleFrequency());
            }

        }

        /// <summary>
        /// Maintains iformation about which type of frequencies should be scheduled as a re-run.
        /// 
        /// Usage:
        /// First Reset the class, then Evaulate the frequencies, then calculate the correct schedule frequency
        /// </summary>
        public class FrequencySchedule
        {
            private UpdateFrequency schdeuleFrequency;

            public bool schedule100 = false;
            public bool schedule10 = false;
            public bool schedule1 = false;

            /// <summary>
            /// Resets all the markes for the different frequencies
            /// </summary>
            public void Reset()
            {
                schedule100 = false;
                schedule10 = false;
                schedule1 = false;
            }

            /// <summary>
            /// Evaulate the provided frequency and set the different markers. May be called multiple types.
            /// </summary>
            public void Evaulate(UpdateFrequency frequency)
            {
                schedule100 = frequency == UpdateFrequency.Update100 || schedule100;
                schedule10 = frequency == UpdateFrequency.Update10 || schedule10;
                schedule1 = frequency == UpdateFrequency.Update1 || schedule1;
            }

            /// <summary>
            /// Calcualtes the flagged frequency based on teh evaluated markers.
            /// </summary>
            public UpdateFrequency CalculateScheduleFrequency()
            {
                schdeuleFrequency = UpdateFrequency.None;

                schdeuleFrequency = schedule100 ? schdeuleFrequency | UpdateFrequency.Update100 : schdeuleFrequency;
                schdeuleFrequency = schedule10 ? schdeuleFrequency | UpdateFrequency.Update10 : schdeuleFrequency;
                schdeuleFrequency = schedule1 ? schdeuleFrequency | UpdateFrequency.Update1 : schdeuleFrequency;

                return schdeuleFrequency;
            }
        }
    }
}
