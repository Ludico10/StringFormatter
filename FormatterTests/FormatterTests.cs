using String_Formatter;

namespace FormatterTests
{
    public class FormatterTests
    {
        [Fact]
        public void ExampleTest()
        {
            var user = new User("Петя", "Иванов");
            var fullName = user.GetGreeting();
            Assert.Equal("Привет, Петя Иванов!", fullName);
        }

        [Fact]
        public void ScreeningTest()
        {
            var user = new User("Петя", "Иванов");
            string formatted = StringFormatter.Shared.Format("{{FirstName}} транслируется в {FirstName}", user);
            Assert.Equal("{FirstName} транслируется в Петя", formatted);
        }

        [Fact]
        public void MultiscreeningTest()
        {
            var user = new User("Петя", "Иванов");
            user.age = 10;
            string formatted = StringFormatter.Shared.Format("{{{{{{{{{age}}}}}}}}}", user);
            Assert.Equal("{{{{10}}}}", formatted);
        }

        [Fact]
        public void UnbalansedBracketsTest()
        {
            var user = new User("Петя", "Иванов");
            var exeption = Assert.Throws<ArgumentException>(() => StringFormatter.Shared.Format("aaaaaa{{{{FirstName}}aaaaaa", user));
            Assert.Equal("Incorrect count of brackets", exeption.Message);

            exeption = Assert.Throws<ArgumentException>(() => StringFormatter.Shared.Format("a{{{FirstName}}}}a", user));
            Assert.Equal("Incorrect symbol } at 16", exeption.Message);
        }

        [Fact]
        public void AlienIdentifireTest()
        {
            var user = new User("Петя", "Иванов");
            var exeption = Assert.Throws<ArgumentException>(() => StringFormatter.Shared.Format("FirstName{email}LastName", user));
            Assert.Equal("Incorrect identifire email at 11", exeption.Message);
        }
    }
}