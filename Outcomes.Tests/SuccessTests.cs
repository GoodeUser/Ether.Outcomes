﻿using System;
using System.Collections.Generic;
using Ether.Outcomes.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ether.Outcomes.Tests
{
    [TestClass]
    public class SuccessTests
    {
        [TestMethod]
        public void Success_Messages_Not_Null_By_Default()
        {
            IOutcome outcome = Outcomes.Success();

            Assert.IsTrue(outcome.Success);
            Assert.IsNotNull(outcome.Messages);
            Assert.IsTrue(outcome.ToString() == string.Empty);
        }

        public IOutcome<int> Method()
        {
            return Outcomes.Success<int>();
        }

        [TestMethod]
        public void Success_Messages_OfT_Not_Null_By_Default()
        {
            IOutcome<int> outcome = Outcomes.Success<int>();

            Assert.IsTrue(outcome.Success);
            Assert.IsNotNull(outcome.Messages);
            Assert.IsTrue(outcome.Value == 0);
            Assert.IsTrue(outcome.ToString() == string.Empty);
        }

        [TestMethod]
        public void Success_Basic_Chaining_Works()
        {
            var messages = new List<string> {"test2", "test3"};
            var outcome = Outcomes.Success<int>().WithValue(32)
                                                 .WithStatusCode(401)
                                                 .WithMessage("test1")
                                                 .WithMessage(messages);

            Assert.IsTrue(outcome.Success);
            Assert.IsTrue(outcome.Value == 32);
            Assert.IsTrue(outcome.StatusCode == 401);
            Assert.IsTrue(outcome.Messages.Count == 3);
            Assert.IsTrue(outcome.ToString() == "test1test2test3");
            Assert.IsTrue(outcome.ToMultiLine("<br>") == "test1<br>test2<br>test3<br>");
        }

        [TestMethod]
        public void Success_WithKeysFrom_Works()
        {
            var outcome1 = Outcomes.Success<int>()
                                  .WithKey("test", 35);

            var outcome2 = Outcomes.Success().WithKeysFrom(outcome1);
            var outcome3 = Outcomes.Success().FromOutcome(outcome1);

            Assert.IsTrue(outcome1.Keys["test"].Equals(35));
            Assert.IsTrue(outcome2.Keys["test"].Equals(35));
            Assert.IsTrue(outcome3.Keys["test"].Equals(35));
        }

        [TestMethod]
        public void Success_FromOutcome_Persists_Values()
        {
            var outcome1 = Outcomes.Success<int>()
                                   .WithValue(10);

            var outcome2 = Outcomes.Success()
                                   .WithValue(20);

            //In this case, some casting is going to happen.
            var outcome3 = Outcomes.Success<ExampleConcrete>()
                                   .WithValue(new ExampleConcrete() { SomeInt = 0, SomeString = "not important"});

            //In this case, there's a null value.
            var outcome4 = Outcomes.Success<ExampleConcrete>()
                                   .WithValue(null);

            //In this case, there's an incompatible type, so we should end up
            //without a value.
            var outcome5 = Outcomes.Success<string>()
                                   .WithValue("test");


            var from1 = Outcomes.Success().FromOutcome(outcome1);
            var from2 = Outcomes.Success().FromOutcome(outcome2);
            var from3 = Outcomes.Success<ExampleBase>().FromOutcome(outcome3);
            var from4 = Outcomes.Success<ExampleBase>().FromOutcome(outcome4);
            var from5 = Outcomes.Success<ExampleBase>().FromOutcome(outcome5);

            Assert.IsTrue(from1.Value.Equals(10));
            Assert.IsTrue(from2.Value.Equals(20));
            Assert.IsTrue(from3.Value.SomeString == "not important");
            Assert.IsTrue(from4.Value == null);
            Assert.IsTrue(from5.Value == null);
        }

        [TestMethod]
        public void Success_WithValue_Works()
        {
            var outcome = Outcomes.Success<Decimal>(23123.32M);

            Assert.IsTrue(outcome.Success);
            Assert.IsTrue(outcome.Messages.Count == 0);
            Assert.IsTrue(outcome.Value == 23123.32M);
            Assert.IsTrue(outcome.ToString() == string.Empty);
            Assert.IsTrue(outcome.ToMultiLine("<br>") == string.Empty);
        }

        [TestMethod]
        public void Success_WithValue_And_Message_Works()
        {
            var outcome = Outcomes.Success<string>()
                                  .WithValue("9An@nsd!d")
                                  .WithMessage("Encrypted value retrieved in 5s!");

            Assert.IsTrue(outcome.Success);
            Assert.IsTrue(outcome.Value == "9An@nsd!d");
            Assert.IsTrue(outcome.Messages.Count == 1);
            Assert.IsTrue(outcome.ToMultiLine() == "Encrypted value retrieved in 5s!");
        }

        [TestMethod]
        public void Success_WithValue_Works_Even_If_Generic_Not_Specified()
        {
            var outcome = Outcomes.Success(23123.32M);

            Assert.IsTrue(outcome.Success);
            Assert.IsTrue(outcome.Messages.Count == 0);
            Assert.IsTrue(outcome.Value == 23123.32M);
            Assert.IsTrue(outcome.ToString() == string.Empty);
            Assert.IsTrue(outcome.ToMultiLine("<br>") == string.Empty);
        }

        [TestMethod]
        public void Success_StatusCode_Is_NullByDefault()
        {
            var outcome = Outcomes.Success(23123.32M);

            Assert.IsNull(outcome.StatusCode);
        }

        [TestMethod]
        public void Success_StatusCode_WithStatusCode_Works()
        {
            var outcome = Outcomes.Success(23123.32M).WithStatusCode((int) StatusCodes.New);

            Assert.IsTrue(outcome.StatusCode == (int) StatusCodes.New);
        }

        [TestMethod]
        public void Success_Keys_WithKey_Works()
        {
            var outcome = Outcomes.Success(23123.32M)
                                  .WithKey("test1", "value1")
                                  .WithKey("test2", "value2");

            Assert.IsTrue((string) outcome.Keys["test1"] == "value1");
            Assert.IsTrue((string) outcome.Keys["test2"] == "value2");
        }
    }
}
