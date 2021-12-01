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
    partial class Program
    {
        public abstract class Script
        {
            protected MyCommandLine commandLine;
            protected readonly TerminalLogger logger;
            private Action action;

            public Script(TerminalLogger logger)
            {
                this.logger = logger;
            }

            public abstract string ScriptName();

            protected abstract Dictionary<string, Action> Commands();

            public void Run(ref MyCommandLine commandLine) {
                this.commandLine = commandLine;
                Dictionary<string, Action> commands = Commands();

                logger.Debug("Script: " + commandLine.Argument(0) + " !");

                string command = commandLine.Argument(1);

                if(!commands.TryGetValue(command, out action))
                {
                    logger.Debug("Command: " + command + " not found !");
                    return;
                }

                logger.Debug("Command: " + command + " !");
                action.Invoke();
            }
        }
    }
}
