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
        readonly SchedulableScript debugScript2;

        bool mark = true;
        public Program()
        {
            SCRIPT_SCHEDULER = new ScriptScheduler();
            debugScript = new DebugScript();
            debugScript2 = new DebugScript2();

            SCRIPT_SCHEDULER.RegisterScript(debugScript);
            SCRIPT_SCHEDULER.RegisterScript(debugScript2);
        }

        public void Save()
        {

        }

        //Development started
        public void Main(string argument, UpdateType updateSource)
        {
            Echo((mark ? "#" : "") + " Running...");
            mark = !mark;
            SCRIPT_SCHEDULER.ContinueAll(updateSource);

            var script = SCRIPT_SCHEDULER.DecodeArgument(argument);
            if (script != null)
            {
                Echo("Script: " + script.ScriptType.ToString());
                script.Run();
            }
            else Echo("Script not found!");

            SCRIPT_SCHEDULER.ScheduleAll(SetUpdateFrequency);
            Echo("Schedule: " + Runtime.UpdateFrequency.ToString());
        }

        public void SetUpdateFrequency(UpdateFrequency frequency)
        {
            Runtime.UpdateFrequency = frequency;
        }

    }
}
