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
        /// A script that may be ran multiple times autoamtically.
        /// </summary>
        public abstract class SchedulableScript
        {
            public ScriptType ScriptType { get; }

            private bool ImActive;
            private UpdateFrequency nextFrequency = UpdateFrequency.None;

            protected SchedulableScript(ScriptType scriptType)
            {
                ScriptType = scriptType;
            }

            /// <summary>
            /// Continue the workflow of the script from the previous run.
            /// </summary>
            public void Continue()
            {
                if (ImActive) DoContinue();
            }

            /// <summary>
            /// Implement it with the actual worflow logic of the script
            /// </summary>
            protected abstract void DoContinue();

            /// <summary>
            /// Actiavate this script as a re-runnable script
            /// </summary>
            protected void Activate(UpdateFrequency frequency)
            {
                nextFrequency = frequency;
                ImActive = true;
            }

            /// <summary>
            /// DeActivate this script, so it won't run again with a re-run.
            /// </summary>
            protected void DeActivate()
            {
                nextFrequency = UpdateFrequency.None;
                ImActive = false;
            }

            /// <summary>
            /// Start the main logic of the script.
            /// </summary>
            public abstract void Run();

            /// <summary>
            /// Get the frequnecy that this script should be scheduled
            /// </summary>
            public UpdateFrequency GetSchedule()
            {
                return nextFrequency;
            }
        }
    }
}
