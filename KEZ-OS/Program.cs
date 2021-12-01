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
        // Base
        private readonly TerminalLogger logger;
        private MyCommandLine commandLine;
        private readonly BlockProvider blockProvider;

        // Scripts
        private readonly NamesScript namesScript;

        // Handlers & Services
        private readonly ScriptRouter scriptRouter;
        private readonly ContinuousServiceHandler continuousServiceHandler;

        public Program()
        {
            // Base
            logger = new TerminalLogger(Echo);
            commandLine = new MyCommandLine();
            blockProvider = new BlockProvider(GridTerminalSystem.GetBlocks);

            // Scripts
            namesScript = new NamesScript(logger, blockProvider, Me);

            // Handlers & Services
            scriptRouter = new ScriptRouter(logger);
            scriptRouter.RegisterScript(namesScript);

            continuousServiceHandler = new ContinuousServiceHandler(1);
            continuousServiceHandler.RegisterService(blockProvider);
        }

        public void Save()
        {

        }

        //Development started
        public void Main(string argument, UpdateType updateSource)
        {
            continuousServiceHandler.Continue();

            if (commandLine.TryParse(argument))
            {    
                scriptRouter.Route(ref commandLine);
            }

            
        }

        
    }
}
