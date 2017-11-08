using ConsoleTools.Common.UserInterface.Arguments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleTools.Test.Common.UserInterface.Arguments
{
    [TestClass]
    public class SupportedArgumentsTests
    {
        [TestMethod]
        public void NamedArguments()
        {
            var args = new ArgumentParser(Mocks.MultipleItems);
            Assert.IsNotNull(Mocks.NamedArgument.Find(args));

            args = new ArgumentParser(Mocks.WhitespacedItem);
            Assert.IsNull(Mocks.NamedArgument.Find(args));
        }

        [TestMethod]
        public void PositionalArguments()
        {
            var args = new ArgumentParser(Mocks.FlaggedItems);
            Assert.IsNotNull(Mocks.PositionalArgument.Find(args));

            args = new ArgumentParser(Mocks.WhitespacedItem);
            Assert.IsNull(Mocks.PositionalArgument.Find(args));

            args = new ArgumentParser(Mocks.EmptySet);
            Assert.IsNull(Mocks.PositionalArgument.Find(args));

            args = new ArgumentParser(null);
            Assert.IsNull(Mocks.PositionalArgument.Find(args));
        }

        [TestMethod]
        public void ShorkKeyedArguments()
        {
            var args = new ArgumentParser(Mocks.MultipleItems);
            Assert.IsNotNull(Mocks.ShortKeyedArgument.Find(args));

            args = new ArgumentParser(new[] { "Bar", "f" });
            Assert.IsNotNull(Mocks.ShortKeyedArgument.Find(args));
        }

        [TestMethod]
        public void OnableArguments()
        {
            var args = new ArgumentParser(Mocks.FlaggedItems);
            Assert.IsTrue(((IOnable) Mocks.OnableArgument).IsOn(args));

            args = new ArgumentParser(Mocks.OffFlaggedItems);
            Assert.IsFalse(((IOnable)Mocks.OnableArgument).IsOn(args));
        }

        [TestMethod]
        public void RequiredArguments()
        {
            var args = new ArgumentParser(Mocks.MultipleItems);
            Assert.IsTrue(SupportedArgument.RequirementsMet(args, Mocks.AllArguments));

            args = new ArgumentParser(Mocks.DuplicateItems);
            Assert.IsFalse(SupportedArgument.RequirementsMet(args, Mocks.AllArguments));
        }
    }
}
