using System;
using System.Linq;
using Xunit;

namespace UniSpyServer.Servers.ServerBrowser.Test
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; }
        public DateTime FavouriteDay { get; set; }
    }
    public class FilterTest
    {
        [Fact]
        public void SimpleQueryFilter()
        {
            // Given
            var temp = "(country = 'US' and numplayers > 5) or hostname like '%GameSpy%'";
            var delimiterChars = new char[] { '(', ')' };
            var frags = temp.Split(delimiterChars).ToList();
            frags.RemoveAll(s => s == "");
            // When
            Assert.Equal("country = 'US' and numplayers > 5", frags[0]);
            Assert.Equal("or hostname like '%GameSpy%'", frags[1]);
            // Then
        }
    }
}