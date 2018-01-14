using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ConsoleCommander.Tests
{
    [TestClass]
    public class CompletableReadLineTests
    {

        private Mock<ICommandParser> mockCommandParser;
        private CompletableReadLine completableReadLine;

        [TestInitialize]
        public void Init()
        {
            mockCommandParser = new Mock<ICommandParser>();
            completableReadLine = new CompletableReadLine(
                new CommandNode(string.Empty, mockCommandParser.Object, null));
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
