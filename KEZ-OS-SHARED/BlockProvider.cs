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
        public class BlockProvider : IContinuousService
        {
            private readonly Action<List<IMyTerminalBlock>> getBlocks;
            private List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();

            private bool blocksWereSearched = false;

            public BlockProvider(Action<List<IMyTerminalBlock>> getBlocks)
            {
                this.getBlocks = getBlocks;
            }

            public void Continue()
            {
                blocksWereSearched = false;
            }

            public List<IMyTerminalBlock> GetBlocks()
            {
                if (!blocksWereSearched)
                {
                    getBlocks.Invoke(blocks);
                }

                return blocks;
            }
        }
    }
}
