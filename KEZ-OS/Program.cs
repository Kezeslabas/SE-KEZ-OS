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
    partial class Program : MyGridProgram
    {
        readonly ScriptScheduler SCRIPT_SCHEDULER;
        readonly SchedulableScript debugScript;
        public Program()
        {
            SCRIPT_SCHEDULER = new ScriptScheduler();
            debugScript = new DebugScript();

            SCRIPT_SCHEDULER.RegisterScript(debugScript);
        }

        public void Save()
        {

        }

        //Development started
        public void Main(string argument, UpdateType updateSource)
        {
            SCRIPT_SCHEDULER.ContinueAll(updateSource);

            SCRIPT_SCHEDULER.DecodeArgument(argument).Run();

            SCRIPT_SCHEDULER.ScheduleAll(SetUpdateFrequency);
        }

        public void SetUpdateFrequency(UpdateFrequency frequency)
        {
            Runtime.UpdateFrequency = frequency;
        }

    }
}
