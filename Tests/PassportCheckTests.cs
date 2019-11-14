using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using vproker.Services;

namespace vproker_tests
{
    [TestFixture]
    class PassportCheckTests
    {
        [Test]
        public void InvalidKurskPassport()
        {
            Assert.IsFalse(PassportCheck.Validate("3804246970"), "The passport has to be invalid");
        }
    }
}
