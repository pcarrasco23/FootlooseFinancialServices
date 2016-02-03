

using NUnit.Framework;

namespace FootlooseFS.Service.Tests
{
    [TestFixture]
    public class PasswordUtilsTests
    {
        [Test]
        public void TestBlankPasswordValidation()
        {
            var status = PasswordUtils.ValidatePassword(string.Empty);

            Assert.IsFalse(status.Success);
        }

        [Test]
        public void TestPasswordMissingCapitalCaseFailsValidation()
        {
            var status = PasswordUtils.ValidatePassword("tesssssst!1");

            Assert.IsFalse(status.Success);
        }

        [Test]
        public void TestPasswordMissingNumberFailsValidation()
        {
            var status = PasswordUtils.ValidatePassword("Teserewt!@");

            Assert.IsFalse(status.Success);
        }

        [Test]
        public void TestMissingSpecialCharacters()
        {
            var status = PasswordUtils.ValidatePassword("Tese2rewt");

            Assert.IsFalse(status.Success);
        }

        [Test]
        public void TestPasswordNotMinimumLenghtFailsValidation()
        {
            var status = PasswordUtils.ValidatePassword("Test1@");

            Assert.IsFalse(status.Success);
        }

        [Test]
        public void TestPasswordNotMaximumLenghtFailsValidation()
        {
            var status = PasswordUtils.ValidatePassword("Tesrrrrrrrrrrrrrrrrrrrrrrrrrrt1@");

            Assert.IsFalse(status.Success);
        }

        [Test]
        public void TestValidPasswordPassesValidation()
        {
            var status = PasswordUtils.ValidatePassword("1Teserewt!@");

            Assert.IsTrue(status.Success);
        }
    }
}
