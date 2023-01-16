using NUnit.Framework;
using SodukoSolver;

namespace SodukoSolverTests
{
    public class ValidatorTests
    {
        // This class contains test cases for the "ValidationAndStart" and "GetInputAsFile" functiona
        // It uses the NUnit framework to test the function's behavior and output
        // The test cases test different sizes of soduko puzzles and also covers different scenarios and inputs

        private UI _ui; // private field to hold an instance of the UI class

        [SetUp]
        public void Setup()
        {
            _ui = new UI(); // instantiating the UI class before each test case is run
        }

        [Test]
        public void ValidationAndStart_Test_Unsolvable() // testing an unsolvable soduko puzzle
        {
            // Arrange
            string input = "000005080000601043000000000010500000000106000300000005530000061000000004000000000";
            string expectedSolve = "***The soduko is unsolvable***";

            // Act
            string actualSolve = _ui.ValidationAndStart(input);

            // Assert
            Assert.That(actualSolve, Is.EqualTo(expectedSolve));
        }

        [Test]
        public void ValidationAndStart_Test_Invalid_1() // testing an invalid input of soduko puzzle -> throwing InvalidInputPlaceException for coliding numbers in the same row / column or cube
        {
            // Act and assert
            Assert.Throws<InvalidInputPlaceException>(() => _ui.ValidationAndStart("000005080000601043000000000010500000000106000300000005530000061000000004000000077"));
        }

        [Test]
        public void ValidationAndStart_Test_Invalid_2() // testing an invalid input of soduko puzzle -> throwing InvalidInputPlaceException for coliding numbers in the same row / column or cube
        {
            // Act and assert
            Assert.Throws<InvalidInputPlaceException>(() => _ui.ValidationAndStart("259867000000000000000300085040023050970580000038046071487635102091200068025008433"));
        }

        [Test]
        public void ValidationAndStart_InputExceptionsTest_1() // testing exceptions being raised because of varius invalid sodukos
        {
            // Act and assert
            Assert.Throws<InvalidInputCharException>(() => _ui.ValidationAndStart("22")); // -> throwing InvalidInputCharException in case an invalid char is used for a certain sudoku size 
        }

        [Test]
        public void ValidationAndStart_InputExceptionsTest_2() // testing exceptions being raised because of varius invalid sodukos
        {
            // Act and assert
            Assert.Throws<InvalidInputLengthException>(() => _ui.ValidationAndStart("")); // -> throwing InvalidInputLengthException for an invalid size (not: 1/4/9/16/25)
        }

        [Test]
        public void ValidationAndStart_FileExceptionsTest() // testing responses to file input
        {
            // For a non existing file:
            // Arrange
            string input = "C:\\Users\\user\\Downloads\\not_on_pc.txt";
            StringReader stringReader = new(input);
            Console.SetIn(stringReader);

            string expectedOutput = "\nEnter the file path:\r\n\nError: File not found -> file path should be like C:\\Users\\user\\Downloads\\sudoku_example.txt\r\n";
            StringWriter stringWriter = new();
            Console.SetOut(stringWriter);

            // for a non txt file:

            // Act
            _ui.GetInputAsFile();
            string actualOutput = stringWriter.ToString();

            // Assert
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));

            // For a valid file -> same result as being inputted as a string
        }
    }
}