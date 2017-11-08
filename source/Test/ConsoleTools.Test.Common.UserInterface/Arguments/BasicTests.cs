using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTools.Common.UserInterface.Arguments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleTools.Test.Common.UserInterface.Arguments
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void EmptyArgs()
        {
            var args = new ArgumentParser(Mocks.EmptySet);

            Assert.IsNotNull(args);
            Assert.IsFalse(args.Any);
            Assert.AreEqual(0, args.Count);
            Assert.ThrowsException<KeyNotFoundException>(() => args["FOO"]);
        }

        [TestMethod]
        public void WhitespaceArg()
        {
            var args = new ArgumentParser(Mocks.WhitespacedItem);

            Assert.IsNotNull(args);
            Assert.IsFalse(args.Any);
            Assert.AreEqual(0, args.Count);
            Assert.ThrowsException<KeyNotFoundException>(() => args["FOO"]);
        }

        [TestMethod]
        public void NoArgs()
        {
            var args = new ArgumentParser(null);

            Assert.IsNotNull(args);
            Assert.IsFalse(args.Any);
            Assert.AreEqual(0, args.Count);
            Assert.ThrowsException<KeyNotFoundException>(() => args["FOO"]);
            Assert.ThrowsException<ArgumentNullException>(() => args[null]);
        }

        [TestMethod]
        public void SingleArg()
        {
            var args = new ArgumentParser(Mocks.SingleItem);

            Assert.IsNotNull(args);
            Assert.IsTrue(args.Any);
            Assert.AreEqual(1, args.Count);

            Assert.ThrowsException<KeyNotFoundException>(() => args["BAR"]);

            var foo = args["FOO"];
            Assert.IsNotNull(foo);
            Assert.IsTrue(foo.HasKey);
            Assert.IsFalse(foo.HasFlag);
            Assert.IsTrue(foo.Key.Equals(foo.Value, StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void MultiArgs()
        {
            var args = new ArgumentParser(Mocks.MultipleItems);

            Assert.IsNotNull(args);
            Assert.IsTrue(args.Any);
            Assert.AreEqual(2, args.Count);

            foreach (var key in Mocks.MultipleItems)
            {
                var arg = args[key];
                Assert.IsTrue(arg.HasKey);
                Assert.IsFalse(arg.HasFlag);
                Assert.IsTrue(arg.Key.Equals(arg.Value, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        [TestMethod]
        public void HasFlags()
        {
            var args = new ArgumentParser(Mocks.FlaggedItems);

            foreach (var key in args.Properties.Keys)
            {
                var arg = args[key];
                Assert.IsTrue(arg.HasFlag);
            }
        }

        [TestMethod]
        public void FlagsOn()
        {
            var args = new ArgumentParser(Mocks.OnFlaggedItems);

            foreach (var key in args.Properties.Keys)
            {
                var arg = args[key];
                Assert.IsTrue(arg.HasFlag);
                Assert.IsTrue(arg.IsOn);
                Assert.IsFalse(arg.IsOff);
            }
        }

        [TestMethod]
        public void FlagsOff()
        {
            var args = new ArgumentParser(Mocks.OffFlaggedItems);

            foreach (var key in args.Properties.Keys)
            {
                var arg = args[key];
                Assert.IsTrue(arg.HasFlag);
                Assert.IsFalse(arg.IsOn);
                Assert.IsTrue(arg.IsOff);
            }
        }

        [TestMethod]
        public void Valued()
        {
            var args = new ArgumentParser(Mocks.ValuedItems);

            foreach (var key in args.Properties.Keys)
            {
                var arg = args[key];
                Assert.IsTrue(arg.HasKey);
                Assert.IsTrue(arg.HasValue);
            }
        }

        [TestMethod]
        public void IgnoreDuplicates()
        {
            var args = new ArgumentParser(Mocks.DuplicateItems);

            Assert.AreEqual(1, args.Count);
            var foo = args.Properties.Values.First();
            Assert.IsTrue(foo.HasKey);
            Assert.AreEqual("foo", foo.Key);
            Assert.IsTrue(foo.HasFlag);
            Assert.IsTrue(foo.IsOn);
            Assert.IsTrue(foo.Key.Equals(foo.Value, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsNotNull(foo.Value);
        }

        [TestMethod]
        public void Numbered()
        {
            var args = new ArgumentParser(Mocks.ValuedItems);

            for (var i = 0; i < args.Count; i++)
            {
                var arg = args[i];
                Assert.IsNotNull(arg);

                Assert.AreEqual(Mocks.ValuedItems[i], arg.Source);
            }
        }
    }
}
