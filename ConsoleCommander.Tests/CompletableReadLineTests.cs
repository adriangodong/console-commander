using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleCommander.Tests
{
    [TestClass]
    public class CompletableReadLineTests
    {

        private CompletableReadLine completableReadLine;

        [TestInitialize]
        public void Init()
        {
            completableReadLine = new CompletableReadLine(
                new TreeNode<string>(
                    string.Empty,
                    new List<TreeNode<string>>()));
        }

        [TestMethod]
        public void ReadKey_NonSpecificKeysShouldSucceed()
        {
            // Act
            var result = completableReadLine.ReadKey(new ConsoleKeyInfo('A', ConsoleKey.A, false, false, false));

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("A", completableReadLine.GetReadLine());
        }

        [TestMethod]
        public void ReadKey_SpaceKeyShouldSucceed()
        {
            // Act
            var result = completableReadLine.ReadKey(new ConsoleKeyInfo('A', ConsoleKey.A, false, false, false));
            result = result && completableReadLine.ReadKey(new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false));

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("A ", completableReadLine.GetReadLine());
        }

        [TestMethod]
        public void ReadKey_BackspaceKeyOnEmptyShouldSucceed()
        {
            // Act
            var result = completableReadLine.ReadKey(new ConsoleKeyInfo('A', ConsoleKey.Backspace, false, false, false));

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("", completableReadLine.GetReadLine());
        }

        [TestMethod]
        public void ReadKey_BackspaceKeyShouldSucceed()
        {
            // Act
            var result = completableReadLine.ReadKey(new ConsoleKeyInfo('A', ConsoleKey.A, false, false, false));
            result = result && completableReadLine.ReadKey(new ConsoleKeyInfo('B', ConsoleKey.BrowserForward, false, false, false));
            result = result && completableReadLine.ReadKey(new ConsoleKeyInfo('A', ConsoleKey.Backspace, false, false, false));

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("A", completableReadLine.GetReadLine());
        }

    }
}
