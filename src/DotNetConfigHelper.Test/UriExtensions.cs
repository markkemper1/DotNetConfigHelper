using System;
using NUnit.Framework;

namespace DotNetConfigHelper.Test
{
    [TestFixture]
    public class UriExtensionsTest
    {
        [Test]    
        public void should_return_username()
        {
            Assert.That(new Uri("http://user%20name:password@test.com").Username(), Is.EqualTo("user name"));
        }
        

        [Test]    
        public void should_return_password()
        {
            Assert.That(new Uri("http://user%20name:passwo%3Ard@test.com").Password(), Is.EqualTo("passwo:rd") );
        }
    }
}
