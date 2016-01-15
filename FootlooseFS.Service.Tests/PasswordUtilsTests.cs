using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FootlooseFS.Service.Tests
{
    [TestClass]
    public class PasswordUtilsTests
    {
        [TestMethod]
        public void TestBlankPasswordValidation()
        {
            var status = PasswordUtils.ValidatePassword(string.Empty);

            Assert.IsFalse(status.Success);
        }

        [TestMethod]
        public void TestPasswordMissingCapitalCaseFailsValidation()
        {
            var status = PasswordUtils.ValidatePassword("tesssssst!1");

            Assert.IsFalse(status.Success);
        }

        [TestMethod]
        public void TestPasswordMissingNumberFailsValidation()
        {
            var status = PasswordUtils.ValidatePassword("Teserewt!@");

            Assert.IsFalse(status.Success);
        }

        [TestMethod]
        public void TestMissingSpecialCharacters()
        {
            var status = PasswordUtils.ValidatePassword("Tese2rewt");

            Assert.IsFalse(status.Success);
        }

        [TestMethod]
        public void TestPasswordNotMinimumLenghtFailsValidation()
        {
            var status = PasswordUtils.ValidatePassword("Test1@");

            Assert.IsFalse(status.Success);
        }

        [TestMethod]
        public void TestPasswordNotMaximumLenghtFailsValidation()
        {
            var status = PasswordUtils.ValidatePassword("Tesrrrrrrrrrrrrrrrrrrrrrrrrrrt1@");

            Assert.IsFalse(status.Success);
        }

        [TestMethod]
        public void TestValidPasswordPassesValidation()
        {
            var status = PasswordUtils.ValidatePassword("1Teserewt!@");

            Assert.IsTrue(status.Success);
        }
    }
}
