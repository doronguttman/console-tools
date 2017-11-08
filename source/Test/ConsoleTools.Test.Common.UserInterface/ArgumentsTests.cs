using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTools.Common.UserInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleTools.Test.Common.UserInterface
{
    [TestClass]
    public class ArgumentsTests
    {
        [TestMethod]
        public void EmptyArgs()
        {
            var mock = new string[0];
            var args = new Arguments(mock);

            Assert.IsNotNull(args);
            Assert.IsFalse(args.Any);
            Assert.AreEqual(0, args.Count);
            Assert.ThrowsException<KeyNotFoundException>(() => args["FOO"]);
        }

        [TestMethod]
        public void WhitespaceArg()
        {
            var mock = new[] { "" };
            var args = new Arguments(mock);

            Assert.IsNotNull(args);
            Assert.IsFalse(args.Any);
            Assert.AreEqual(0, args.Count);
            Assert.ThrowsException<KeyNotFoundException>(() => args["FOO"]);
        }

        [TestMethod]
        public void NoArgs()
        {
            var args = new Arguments(null);

            Assert.IsNotNull(args);
            Assert.IsFalse(args.Any);
            Assert.AreEqual(0, args.Count);
            Assert.ThrowsException<KeyNotFoundException>(() => args["FOO"]);
            Assert.ThrowsException<ArgumentNullException>(() => args[null]);
        }

        [TestMethod]
        public void SingleArg()
        {
            var mock = new[] { "Foo" };
            var args = new Arguments(mock);

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
            var mock = new[] { "Foo", "Bar" };
            var args = new Arguments(mock);

            Assert.IsNotNull(args);
            Assert.IsTrue(args.Any);
            Assert.AreEqual(2, args.Count);

            foreach (var key in mock)
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
            var mock = new[] { "+Foo", "++Bar", "-alice", "--bob", "/john" };
            var args = new Arguments(mock);

            foreach (var key in args.Properties.Keys)
            {
                var arg = args[key];
                Assert.IsTrue(arg.HasFlag);
            }
        }

        [TestMethod]
        public void FlagsOn()
        {
            var mock = new[] { "+Foo", "++Bar" };
            var args = new Arguments(mock);

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
            var mock = new[] { "-Foo", "--Bar" };
            var args = new Arguments(mock);

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
            var mock = new[] { "Foo:true", "Bar=false", "-alice:\"one two\"", "bob=\"two one\"" };
            var args = new Arguments(mock);

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
            var mock = new[] { "+foO=", "Foo=bar", "foo", "-fOo=BAR" };
            var args = new Arguments(mock);

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
            var mock = new[] { "Foo=true", "Bar=false", "-alice:\"one two\"", "bob=\"two one\"" };
            var args = new Arguments(mock);

            for (var i = 0; i < args.Count; i++)
            {
                var arg = args[i];
                Assert.IsNotNull(arg);

                Assert.AreEqual(mock[i], arg.Source);
            }
        }
    }
}
