using String_Formatter;

namespace FormatterTests
{
    public class FormatterTests
    {
        [Fact]
        public void ExampleTest()
        {
            var user = new User("����", "������");
            var fullName = user.GetGreeting();
            Assert.Equal("������, ���� ������!", fullName);
        }

        [Fact]
        public void ScreeningTest()
        {
            var user = new User("����", "������");
            string formatted = StringFormatter.Shared.Format("{{FirstName}} ������������� � {FirstName}", user);
            Assert.Equal("{FirstName} ������������� � ����", formatted);
        }

        [Fact]
        public void MultiscreeningTest()
        {
            var user = new User("����", "������");
            user.age = 10;
            string formatted = StringFormatter.Shared.Format("{{{{{{{{{age}}}}}}}}}", user);
            Assert.Equal("{{{{10}}}}", formatted);
        }

        [Fact]
        public void UnbalansedBracketsTest()
        {
            var user = new User("����", "������");
            var exeption = Assert.Throws<ArgumentException>(() => StringFormatter.Shared.Format("aaaaaa{{{{FirstName}}aaaaaa", user));
            Assert.Equal("Incorrect count of brackets", exeption.Message);

            exeption = Assert.Throws<ArgumentException>(() => StringFormatter.Shared.Format("a{{{FirstName}}}}a", user));
            Assert.Equal("Incorrect symbol } at 16", exeption.Message);
        }

        [Fact]
        public void AlienIdentifireTest()
        {
            var user = new User("����", "������");
            var exeption = Assert.Throws<ArgumentException>(() => StringFormatter.Shared.Format("FirstName{email}LastName", user));
            Assert.Equal("Incorrect identifire email at 11", exeption.Message);
        }
    }
}