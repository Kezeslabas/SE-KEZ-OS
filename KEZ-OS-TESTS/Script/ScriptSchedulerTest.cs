using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static IngameScript.Program;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
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
using Moq;

namespace KEZ_OS_TESTS
{
    /// <summary>
    /// TODO: Add More tests
    /// </summary>
    [TestClass]
    public class ScriptSchedulerTest
    {
        /// <summary>
        /// Test if the RegisterScript adds the scripts with the correct ScripType to the internal dictionary
        /// </summary>
        [TestMethod]
        public void Test_RegisterScript()
        {
            //Arrange
            ScriptScheduler scheduler = new ScriptScheduler();
            var mockScript = new Mock<SchedulableScript>(ScriptType.DEBUG);

            //Act
            scheduler.RegisterScript(mockScript.Object);

            //Assert
            Assert.AreEqual(mockScript.Object, scheduler.GetAllScripts()[ScriptType.DEBUG]);

        }

        /// <summary>
        /// Test if the FindScriptByName returns the correct script
        /// </summary>
        [TestMethod]
        public void Test_FindScriptByName()
        {
            //Arrange
            ScriptScheduler scheduler = new ScriptScheduler();
            var mockScript = new Mock<SchedulableScript>(ScriptType.DEBUG);

            scheduler.RegisterScript(mockScript.Object);

            //Act
            SchedulableScript resultScript = scheduler.FindScriptByName("DEBUG");
            SchedulableScript nullScript = scheduler.FindScriptByName("QWEASD");

            //Assert
            Assert.AreEqual(mockScript.Object, resultScript);
            Assert.IsNull(nullScript);
        }
    }
}
