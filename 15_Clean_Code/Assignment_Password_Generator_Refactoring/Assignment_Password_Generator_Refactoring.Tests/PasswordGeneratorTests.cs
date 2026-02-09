namespace Assignment_Password_Generator_Refactoring.Tests;

using Assignment_Password_Generator_Refactoring;

[TestFixture]
public class PasswordGeneratorTests
{
    [TestCase(0, 10, true)]
    [TestCase(-10, 5, false)]
    public void Generate_WhenMinLengthIsLessThan1_ShouldThrowArgumentOutOfRangeException(int min, int max, bool useSpecialCharacters)
    {
        var generator = new PasswordGenerator(new FakeRandomGenerator());
        Assert.Throws<ArgumentOutOfRangeException>(() => generator.Generate(min, max, useSpecialCharacters));
    }

    [TestCase(10, 5, true)]
    [TestCase(18, 6, false)]
    public void Generate_WhenMinLengthIsGreaterThanMaxLength_ShouldThrowArgumentOutOfRangeException(int min, int max, bool useSpecialCharacters)
    {
        var generator = new PasswordGenerator(new FakeRandomGenerator());
        Assert.Throws<ArgumentOutOfRangeException>(() => generator.Generate(min, max, useSpecialCharacters));
    }

    [TestCase(5, new[] { 5, 0, 1, 2, 3, 4 }, "ABCDE")]
    [TestCase(6, new[] { 6, 20, 21, 22, 23, 24, 25 }, "UVWXYZ")]
    [TestCase(8, new[] { 8, 26, 27, 28, 29, 30, 31, 32, 33 }, "01234567")]
    public void Generate_WhenUseSpecialCharactersIsFalse_ShouldReturnOnlyAlphanumeric(int passwordLength, int[] values, string expectedPassword)
    {
        var fakeRandom = new FakeRandomGenerator(values);
        var generator = new PasswordGenerator(fakeRandom);
        var password = generator.Generate(passwordLength, passwordLength, false);

        Assert.That(password, Is.EqualTo(expectedPassword));
        Assert.That(password.Length, Is.EqualTo(passwordLength));
    }

    [TestCase(5, new[] { 5, 36, 37, 38, 39, 40 }, "!@#$%")]
    [TestCase(6, new[] { 6, 41, 42, 43, 44, 45, 0 }, "^&*()A")]
    [TestCase(7, new[] { 7, 0, 1, 2, 46, 47, 48, 49 }, "ABC_-+=")]
    public void Generate_WhenUseSpecialCharactersIsTrue_ShouldReturnPasswordWithSpecialCharacters(int passwordLength, int[] values, string expectedPassword)
    {
        var fakeRandom = new FakeRandomGenerator(values);
        var generator = new PasswordGenerator(fakeRandom);
        var password = generator.Generate(passwordLength, passwordLength, true);
        Assert.That(password, Is.EqualTo(expectedPassword));
        Assert.That(password.Length, Is.EqualTo(passwordLength));
    }
}
