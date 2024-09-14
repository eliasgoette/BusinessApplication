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

        [TestMethod]
        public void CorrectPasswordIsMatched()
        {
            var passwd = "asdf$%&123";

            var hash = PasswordEncryption.HashPassword(passwd);
            var match = PasswordEncryption.MatchPassword(passwd, hash);

            Assert.IsTrue(match);
        }

        [TestMethod]
        public void WrongPasswordIsNotMatchedPasswordIsNotMatched()
        {
            var passwd = "asdf$%&123";
            var wrongAttempt = "aaaaaaaaaa";

            var hash = PasswordEncryption.HashPassword(passwd);
            var match = PasswordEncryption.MatchPassword(wrongAttempt, hash);

            Assert.IsFalse(match);
        }

        [TestMethod]
        public void PartiallyCorrectPasswordIsNotMatched()
        {
            var passwd = "asdf$%&123";
            var wrongAttempt = passwd.Substring(0, passwd.Length - 2);

            var hash = PasswordEncryption.HashPassword(passwd);
            var match = PasswordEncryption.MatchPassword(wrongAttempt, hash);

            Assert.IsFalse(match);
        }
    }
}
