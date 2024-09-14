using BusinessApplication.Utility;

namespace BusinessApplication.Tests
{
    [TestClass]
    public class PasswordEncryptionUnitTest
    {
        [TestMethod]
        public void EncryptsPasswordsWithSpecialCharacters()
        {
            var passwd = "^!§\"$%&/()=?`´\\{}[]";
            var res = PasswordEncryption.HashPassword(passwd);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void LengthCanNotBeDerivedFromHash()
        {
            var shortPasswd = "asdf";
            var longPasswd = "dsuifhsiofhidhffbsilfidfoifbfoinoiscnpdnifsofiosijfij";

            var shortHash = PasswordEncryption.HashPassword(shortPasswd);
            var longHash = PasswordEncryption.HashPassword(longPasswd);

            Assert.AreEqual(shortHash?.Length, longHash?.Length);
        }

        [TestMethod]
        public void HashMethodIsNotDeterministic()
        {
            var passwd = "Pa$$w0rd";

            var res1 = PasswordEncryption.HashPassword(passwd);
            var res2 = PasswordEncryption.HashPassword(passwd);

            Assert.AreNotEqual(res1, res2);
        }
    }
}
