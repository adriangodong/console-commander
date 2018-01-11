using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ConsoleCommander.Tests
{
    [TestClass]
    public class CommandParserTests
    {

        private Mock<ICommandParser> mockCommandParser;
        private CommandTokenizer commandParser;

        [TestInitialize]
        public void Init()
        {
            mockCommandParser = new Mock<ICommandParser>();
            commandParser = new CommandTokenizer(new List<ICommandParser>() { mockCommandParser.Object }, string.Empty);
        }

        [TestMethod]
        public void Parse_ShouldReturnNull_WithEmptyCommand()
        {
            // Act
            var nullResult = commandParser.Parse(null);
            var emptyResult = commandParser.Parse(string.Empty);
            var whiteSpaceResult = commandParser.Parse(" ");

            // Assert
            Assert.IsNull(nullResult.command);
            Assert.AreEqual(CommandTokenizer.Error_EmptyCommand, nullResult.error);
            Assert.IsNull(emptyResult.command);
            Assert.AreEqual(CommandTokenizer.Error_EmptyCommand, emptyResult.error);
            Assert.IsNull(whiteSpaceResult.command);
            Assert.AreEqual(CommandTokenizer.Error_EmptyCommand, whiteSpaceResult.error);
        }

        [TestMethod]
        public void Parse_ShouldReturnNull_WithUnknownOrderType()
        {
            // Act
            var parseResult = commandParser.Parse("unknown");

            // Assert
            Assert.IsNull(parseResult.command);
            Assert.AreEqual(
                string.Format(CommandTokenizer.Error_UnknownFirstToken, "unknown"),
                parseResult.error);
        }

        [TestMethod]
        public void Parse_ShouldCallCommandParser_WithTokenizedCommand()
        {
            // Arrange
            string[] actualCommandTokens = null;
            mockCommandParser
                .Setup(mock => mock.Parse(It.IsAny<string[]>()))
                .Callback<string[]>(commandTokens => actualCommandTokens = commandTokens);

            // Act
            commandParser.Parse("1 2 3");

            // Assert
            Assert.IsNotNull(actualCommandTokens);
            Assert.AreEqual(3, actualCommandTokens.Length);
            Assert.AreEqual("1", actualCommandTokens[0]);
            Assert.AreEqual("2", actualCommandTokens[1]);
            Assert.AreEqual("3", actualCommandTokens[2]);
        }

    }
}
