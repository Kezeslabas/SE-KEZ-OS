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
        public class ScriptRouter
        {
            private readonly Dictionary<string, Script> scripts = new Dictionary<string, Script>();
            private Script script;

            private readonly TerminalLogger logger;

            public ScriptRouter(TerminalLogger logger)
            {
                this.logger = logger;
            }

            public void Route(ref MyCommandLine commandLine)
            {
                string scriptName = commandLine.Argument(0);
                if (!scripts.TryGetValue(scriptName, out script))
                {
                    logger.Debug("'" + scriptName + "' Not found!");
                    return;
                    
                }

                script.Run(ref commandLine);
            }

            public void RegisterScript(Script script)
            {
                scripts[script.ScriptName()] = script;
            }
        }
    }
}
