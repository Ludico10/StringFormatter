using FormatterTests.SourceClasses;
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
            var exeption = Assert.Throws<ArgumentException>(() => StringFormatter.Shared.Format("aaaaaa{{{{{FirstName}}aaaaaa", user));
            Assert.Equal("Incorrect count of brackets", exeption.Message);

            exeption = Assert.Throws<ArgumentException>(() => StringFormatter.Shared.Format("a{{FirstName}}}}}a", user));
            Assert.Equal("Incorrect count of brackets", exeption.Message);

            exeption = Assert.Throws<ArgumentException>(() => StringFormatter.Shared.Format("a{{{FirstName", user));
            Assert.Equal("Incorrect count of brackets", exeption.Message);
        }

        [Fact]
        public void AlienIdentifireTest()
        {
            var user = new User("Петя", "Иванов");
            var exeption = Assert.Throws<ArgumentException>(() => StringFormatter.Shared.Format("FirstName{email}LastName", user));
            Assert.Equal("Incorrect identifire email at 11", exeption.Message);
        }

        [Fact]
        public void AllCompleteTransitionsTest()
        {
            var cases = new AllCases(5, 5.5, 'c');
            // [1, S]
            string formatted = StringFormatter.Shared.Format("!!", cases);
            Assert.Equal("!!", formatted);
            // [1, I]
            formatted = StringFormatter.Shared.Format("!a", cases);
            Assert.Equal("!a", formatted);
            // [1, N]
            formatted = StringFormatter.Shared.Format("!1", cases);
            Assert.Equal("!1", formatted);
            // [1, '}'] -> [4, '}']
            formatted = StringFormatter.Shared.Format("!}}", cases);
            Assert.Equal("!}", formatted);
            // [1, '{'] - > [3, '{']
            formatted = StringFormatter.Shared.Format("!{{", cases);
            Assert.Equal("!{", formatted);
            // [1, '{'] -> [3, I] -> [2, '}']
            formatted = StringFormatter.Shared.Format("!{a}", cases);
            Assert.Equal("!5", formatted);
            // [1, '{'] -> [3, I] -> [2, N] -> [2, '}']
            formatted = StringFormatter.Shared.Format("!{a1}", cases);
            Assert.Equal("!5.5", formatted);
            // [1, '{'] -> [3, I] -> [2, N] -> [2, I] -> [2, '}']
            formatted = StringFormatter.Shared.Format("!{a1a}", cases);
            Assert.Equal("!c", formatted);
        }

        [Fact]
        public void CasheTest()
        {
            var case1 = new AllCases(5, 5.5, 'c');
            var case2 = new AllCases(8, 88.8, 'p');

            var cashe = new TestCashe();
            var formatter = new StringFormatter(cashe);

            var res = formatter.Format("a: {a}, a1: {a1}, a1a: {a1a}", case1);
            Assert.Equal("a: 5, a1: 5.5, a1a: c", res);
            res = formatter.Format("a: {a}, a1: {a1}, a1a: {a1a}", case2);
            Assert.Equal("a: 8, a1: 88.8, a1a: p", res);
            Assert.Equal(6, cashe.readTry);
            Assert.Equal(3, cashe.readCnt);
            Assert.Equal(3, cashe.writeTry);
            Assert.Equal(3, cashe.writeCnt);
        }
    }
}