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
        public class NamesScript : Script
        {
            private const string scriptName = "name";

            private const string switchForAllConnectedGrids = "all";
            private const string switchForAddAsPrefix = "prefix";

            private readonly Dictionary<string, Action> commands;
            private readonly BlockProvider blockProvider;
            private readonly IMyTerminalBlock myBlock;

            public NamesScript(TerminalLogger logger, BlockProvider blockProvider, IMyTerminalBlock myBlock) : base(logger)
            {
                this.blockProvider = blockProvider;
                this.myBlock = myBlock;

                commands = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
                {
                    ["replace"] = Replace,
                    ["replace-filtered"] = ReplaceFiltered,
                    ["add"] = Add,
                    ["add-filtered"] = AddFiltered
                };
            }

            /// <summary>
            /// Adds the text in the 3rd paramter to the Custom Names of the blocks.
            /// </summary>
            public void Add()
            {
                switch (commandLine.ArgumentCount)
                {
                    case 2:
                        logger.Debug("Missing Parameter 3!");
                        break;
                    default:
                        break;
                }

                string text = commandLine.Argument(2);
                var blocks = blockProvider.GetBlocks();

                bool all = commandLine.Switch(switchForAllConnectedGrids);
                bool prefix = commandLine.Switch(switchForAddAsPrefix);

                blocks.ForEach(block => { 
                    if(all || block.IsSameConstructAs(myBlock))
                    {
                        block.CustomName = prefix ?   text + block.CustomName : block.CustomName + text;
                    }
                });
            }

            /// <summary>
            /// Adds the text in the 3rd paramter to the Custom Names of the blocks,
            /// provided that the name of the block containes the 3rd parameter.
            /// </summary>
            public void AddFiltered()
            {
                switch (commandLine.ArgumentCount)
                {
                    case 2:
                        logger.Debug("Missing Parameter 3 and 4!");
                        break;                    
                    case 3:
                        logger.Debug("Missing Parameter 4!");
                        break;
                    default:
                        break;
                }

                string filter = commandLine.Argument(2);
                string text = commandLine.Argument(3);
                var blocks = blockProvider.GetBlocks();

                bool prefix = commandLine.Switch(switchForAddAsPrefix);
                bool all = commandLine.Switch(switchForAllConnectedGrids);

                blocks.ForEach(block => {
                    if ((all || block.IsSameConstructAs(myBlock)) && block.CustomName.Contains(filter))
                    {
                        block.CustomName = prefix ? text + block.CustomName : block.CustomName + text;
                    }
                });
            }

            /// <summary>
            /// Replaces the text in the Custom Names of all blocks text as the 3rd paramater to the 4th parameter's text.
            /// </summary>
            public void Replace()
            {
                switch (commandLine.ArgumentCount)
                {
                    case 2:
                        logger.Debug("Missing Parameter 3 and 4!");
                        break;
                    case 3:
                        logger.Debug("Missing Parameter 4!");
                        break;
                    default:
                        break;
                }

                string original = commandLine.Argument(2);
                string replace = commandLine.Argument(3);

                var blocks = blockProvider.GetBlocks();

                bool all = commandLine.Switch(switchForAllConnectedGrids);

                blocks.ForEach(block => { 
                    if(all || block.IsSameConstructAs(myBlock))
                    {
                        ReplaceInBlock(block, original, replace);
                    }
                });
            }

            /// <summary>
            /// Replaces the text in the Custom Names of all blocks text as the 4th paramater to the 5th parameter's text,
            /// provided that the name of the block containes the 3rd parameter.
            /// </summary>
            public void ReplaceFiltered()
            {
                switch (commandLine.ArgumentCount)
                {
                    case 2:
                        logger.Debug("Missing Parameter 3 and 4 and 5!");
                        break;
                    case 3:
                        logger.Debug("Missing Parameter 4 and 5!");
                        break;                    
                    case 4:
                        logger.Debug("Missing Parameter 5!");
                        break;
                    default:
                        break;
                }

                string filter = commandLine.Argument(2);
                string original = commandLine.Argument(3);
                string replace = commandLine.Argument(4);

                var blocks = blockProvider.GetBlocks();

                bool all = commandLine.Switch(switchForAllConnectedGrids);

                blocks.ForEach(block => {
                    if ((all || block.IsSameConstructAs(myBlock)) && block.CustomName.Contains(filter))
                    {
                        ReplaceInBlock(block, original, replace);
                    }
                });
            }

            private void ReplaceInBlock(IMyTerminalBlock block, string original, string replace)
            {
                block.CustomName = block.CustomName.Replace(original, replace);
            }

            public override string ScriptName()
            {
                return scriptName;
            }

            protected override Dictionary<string, Action> Commands()
            {
                return commands;
            }
        }
    }
}
