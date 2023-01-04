using SodukoSolver;
using NUnit.Framework;

namespace SodukoSolverTests
{
    public class SolverTests
    {
        private UI _ui;

        [SetUp]
        public void Setup()
        {
            _ui = new UI();
        }

        [Test]
        public void ValidationAndStart_SolutionTest() // testing different sodukos and their solutions
        {
            // Arrange
            string input1 = "259867000000000000000300085040023050970580000038046071487635102091200068025008437"; // 9X9 puzzle
            string expectedSolve1 = "259867314813459726764312985146723859972581643538946271487635192391274568625198437";

            string input2 = "1030042100020040"; // 4X4 puzzle
            string expectedSolve2 = "1234342143122143";

            // 16X16 puzzle
            string input3 = "10023400<06000700080007003009:6;0<00:0010=0;00>0300?200>000900<0=000800:0<201?000;76000@000?005=000:05?0040800;0@0059<00100000800200000=00<580030=00?0300>80@000580010002000=9?000<406@0=00700050300<0006004;00@0700@050>0010020;1?900=002000>000>000;0200=3500<";
            string expectedSolve3 = "15:2349;<@6>?=78>@8=5?7<43129:6;9<47:@618=?;35>236;?2=8>75:94@<1=4>387;:5<261?@98;76412@9:>?<35=<91:=5?634@8>2;7@?259<>31;7=:68462@>;94=?1<587:37=91?235;>8:@<46583;1:<7264@=9?>?:<4>6@8=9372;152358<>:?6794;1=@:7=<@359>8;1642?;1?968=4@25<7>3:4>6@7;12:?=3589<";

            string input4 = "0"; // 1X1 puzzle
            string expectedSolve4 = "1";
            
            // Act
            string actualSolve1 = _ui.ValidationAndStart(input1);
            string actualSolve2 = _ui.ValidationAndStart(input2);
            string actualSolve3 = _ui.ValidationAndStart(input3);
            string actualSolve4 = _ui.ValidationAndStart(input4);


            // Assert
            Assert.AreEqual(expectedSolve1, actualSolve1);
            Assert.AreEqual(expectedSolve2, actualSolve2);
            Assert.AreEqual(expectedSolve3, actualSolve3);
            Assert.AreEqual(expectedSolve4, actualSolve4);
        }

        [Test]
        public void ValidationAndStart_InputExceptionsTest() // testing exceptions being raised because of varius invalid sodukos
        {
            // Act and assert
            Assert.Throws<InvalidInputCharException>(() => _ui.ValidationAndStart("22"));

            Assert.Throws<InvalidInputLengthException>(() => _ui.ValidationAndStart(""));
            
            Assert.Throws<InvalidInputPlaceException>(() => _ui.ValidationAndStart("259867000000000000000300085040023050970580000038046071487635102091200068025008433"));

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
            // Act
            _ui.getInputAsFile();
            string actualOutput = stringWriter.ToString();
            // Assert
            Assert.AreEqual(expectedOutput, actualOutput);

            // For a valid file -> same result as at ValidationAndStart_SolutionTest()
        }
    }
}