namespace Assignment_Dice_Roll_Game.Tests;
using Moq;

[TestFixture]
public class DiceRollGameTests
{
    private Mock<IDice> _diceMock;
    private Mock<IGameIO> _ioMock;
    private DiceRollGame _cut;

    [SetUp]
    public void Setup()
    {
        _diceMock = new Mock<IDice>();
        _ioMock = new Mock<IGameIO>();
        _cut = new DiceRollGame(_ioMock.Object, _diceMock.Object, 3);
    }

    [Test]
    public void Play_Should_Win_When_Correct_Guess_On_First_Try()
    {
        const int NumberOnDie = 3;
        _diceMock.Setup(d => d.Roll()).Returns(NumberOnDie);
        _ioMock
            .Setup(io => io.ReadInteger(It.IsAny<string>()))
            .Returns(NumberOnDie);
        var result = _cut.Play();

        Assert.That(result, Is.EqualTo(GameResult.Win));
        _ioMock.Verify(io => io.WriteLine(GameMessages.Win), Times.Once);
        _ioMock.Verify(io => io.ReadInteger(GameMessages.EnterNumber), Times.Once);
    }

    [Test]
    public void Play_Should_Win_When_Correct_Guess_On_Second_Try()
    {
        const int NumberOnDie = 3;
        _diceMock.Setup(d => d.Roll()).Returns(NumberOnDie);
        _ioMock
            .SetupSequence(io => io.ReadInteger(It.IsAny<string>()))
            .Returns(1)
            .Returns(NumberOnDie);

        var result = _cut.Play();

        Assert.That(result, Is.EqualTo(GameResult.Win));
        _ioMock.Verify(io => io.WriteLine(GameMessages.Win), Times.Once);
        _ioMock.Verify(io => io.ReadInteger(GameMessages.EnterNumber), Times.Exactly(2));
    }

    [Test]
    public void Play_Should_Win_When_Correct_Guess_On_Third_Try()
    {
        const int NumberOnDie = 3;
        _diceMock.Setup(d => d.Roll()).Returns(NumberOnDie);
        _ioMock
            .SetupSequence(io => io.ReadInteger(It.IsAny<string>()))
            .Returns(1)
            .Returns(2)
            .Returns(NumberOnDie);

        var result = _cut.Play();

        Assert.That(result, Is.EqualTo(GameResult.Win));
        _ioMock.Verify(io => io.WriteLine(GameMessages.Win), Times.Once);
        _ioMock.Verify(io => io.ReadInteger(GameMessages.EnterNumber), Times.Exactly(3));
    }

    [Test]
    public void Play_Should_Lose_When_Wrong_Guess_On_Fourth_Try()
    {
        const int NumberOnDie = 3;
        _diceMock.Setup(d => d.Roll()).Returns(NumberOnDie);
        _ioMock
            .SetupSequence(io => io.ReadInteger(It.IsAny<string>()))
            .Returns(1)
            .Returns(2)
            .Returns(4)
            .Returns(5);

        var result = _cut.Play();

        Assert.That(result, Is.EqualTo(GameResult.Lose));
        _ioMock.Verify(io => io.WriteLine(GameMessages.Lose), Times.Once);
    }

    [Test]
    public void Play_Should_Lose_When_Wrong_Guess_On_All_Tries()
    {
        const int NumberOnDie = 3;
        _diceMock.Setup(d => d.Roll()).Returns(NumberOnDie);
        const int WrongGuess = 4;
        _ioMock
            .Setup(io => io.ReadInteger(It.IsAny<string>()))
            .Returns(WrongGuess);
        var result = _cut.Play();

        Assert.That(result, Is.EqualTo(GameResult.Lose));
        _ioMock.Verify(io => io.WriteLine(GameMessages.Lose), Times.Once);
        _ioMock.Verify(io => io.ReadInteger(GameMessages.EnterNumber), Times.Exactly(3));
    }

    [Test]
    public void Play_Should_Count_Out_Of_Range_Number_As_Wrong_Guess()
    {
        _ioMock
            .SetupSequence(io => io.ReadInteger(It.IsAny<string>()))
            .Returns(10)
            .Returns(99)
            .Returns(-5);
        var result = _cut.Play();

        Assert.That(result, Is.EqualTo(GameResult.Lose));
        _ioMock.Verify(io => io.WriteLine(GameMessages.WrongNumber), Times.Exactly(3));
        _ioMock.Verify(io => io.WriteLine(GameMessages.Lose), Times.Once);
    }

    [Test]
    public void Play_ShowWelcomeMessage()
    {
        _cut.Play();
        _ioMock.Verify(io => io.WriteLine(GameMessages.DiceRolled), Times.Once);
    }
}
