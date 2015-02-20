using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ONtimer.Tests
{
    [TestClass]
    public class TimerTests
    {
        [TestMethod]
        public void TestValueStrings()
        {
            SessionTimer timer = new SessionTimer();

            timer.Minutes = 23;
            timer.Seconds = 45;

            Assert.AreEqual("2", timer.MinuteTens);
            Assert.AreEqual("3", timer.MinuteSingle);
            Assert.AreEqual("4", timer.SecondsTens);
            Assert.AreEqual("5", timer.SecondsSingle);
        }

        [TestMethod]
        public void TestReset()
        {
            SessionTimer timer = new SessionTimer();

            timer.Minutes = 23;
            timer.Seconds = 45;

            timer.Reset();

            Assert.AreEqual(timer.Minutes, 0);
            Assert.AreEqual(timer.Seconds, 0);
        }

        [TestMethod]
        public void TestSetMinutes()
        {
            SessionTimer timer = new SessionTimer();

            timer.Minutes = 23;
            timer.Seconds = 45;

            timer.Minutes = 11;

            Assert.AreEqual(45, timer.Seconds);
            Assert.AreEqual(11, timer.Minutes);
        }

        [TestMethod]
        public void TestSetSeconds()
        {
            SessionTimer timer = new SessionTimer();

            timer.Minutes = 23;
            timer.Seconds = 45;

            timer.Seconds = 11;

            Assert.AreEqual(11, timer.Seconds);
            Assert.AreEqual(23, timer.Minutes);
        }
    }
}
