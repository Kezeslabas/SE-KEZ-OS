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
        public interface IContinuousService
        {
            void Continue();
        }

        public class ContinuousServiceHandler
        {
            private readonly IContinuousService[] services;
            private byte index = 0;

            public ContinuousServiceHandler(byte maxServices)
            {
                services = new IContinuousService[maxServices];
            }

            public void RegisterService(IContinuousService service)
            {
                if(index >= services.Length)
                {
                    return;
                }

                services[index] = service;
                index++;
            }

            public void Continue()
            {
                for (int i = 0; i < services.Length; i++)
                {
                    services[i].Continue();
                }
            }
        }
    }
}
