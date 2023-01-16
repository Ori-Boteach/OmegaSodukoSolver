using NUnit.Framework;
using SodukoSolver;

namespace SodukoSolverTests
{
    public class SolverTests
    {
        // This class contains test cases for the "ValidationAndStart" function
        // It uses the NUnit framework to test the function's behavior and output
        // The test cases test different sizes of soduko puzzles and their answers

        private UI _ui; // private field to hold an instance of the UI class

        [SetUp]
        public void Setup()
        {
            _ui = new UI(); // instantiating the UI class before each test case is run
        }

        [Test]
        public void ValidationAndStart_Test_1X1() // testing soduko of size 1X1
        {
            UI.choseDLX = false; // testing this solving operation with my backtracking algorithm (the default setting is true -> using dlx)

            // Arrange
            string input = "1";
            string expectedSolve = "1";

            // Act
            string actualSolve = _ui.ValidationAndStart(input);

            // Assert
            Assert.That(actualSolve, Is.EqualTo(expectedSolve));
        }

        [Test]
        public void ValidationAndStart_Test_1X1_Empty() // testing empty soduko of size 1X1
        {
            UI.choseDLX = false; // testing this solving operation with my backtracking algorithm (the default setting is true -> using dlx)

            // Arrange
            string input = "0"; // empty puzzle
            string expectedSolve = "1";

            // Act
            string actualSolve = _ui.ValidationAndStart(input);

            // Assert
            Assert.That(actualSolve, Is.EqualTo(expectedSolve));
        }

        [Test]
        public void ValidationAndStart_Test_4X4() // testing different sodukos of size 4X4
        {
            UI.choseDLX = true; // now solving with the dancing links algorithm

            // Arrange
            string input = "1030042100020040";
            string expectedSolve = "1234342143122143";

            // Act
            string actualSolve = _ui.ValidationAndStart(input);

            // Assert
            Assert.That(actualSolve, Is.EqualTo(expectedSolve));
        }

        [Test]
        public void ValidationAndStart_Test_9X9_backtracking() // testing different sodukos of size 9X9 using the backtracking algorithm
        {
            UI.choseDLX = false; // testing this solving operation with my backtracking algorithm (the default setting is true -> using dlx)

            // Arrange
            string input = "259867000000000000000300085040023050970580000038046071487635102091200068025008437";
            string expectedSolve = "259867314813459726764312985146723859972581643538946271487635192391274568625198437";

            // Act
            string actualSolve = _ui.ValidationAndStart(input);

            // Assert
            Assert.That(actualSolve, Is.EqualTo(expectedSolve));
        }

        [Test]
        public void ValidationAndStart_Test_9X9_dlx() // testing EMPTY soduko of size 9X9 using the dancing links algorithm
        {
            UI.choseDLX = true; // now solving with the dancing links algorithm

            // Arrange
            string input = "000000000000000000000000000000000000000000000000000000000000000000000000000000000"; // empty 9 by 9 puzzle
            string expectedSolve = "123456789687139254495278136712893465956714823348625917261347598879561342534982671";

            // Act
            string actualSolve = _ui.ValidationAndStart(input);

            // Assert
            Assert.That(actualSolve, Is.EqualTo(expectedSolve));
        }

        [Test]
        public void ValidationAndStart_Test_16X16() // testing different sodukos of size 16X16
        {
            // Arrange
            string input = "10023400<06000700080007003009:6;0<00:0010=0;00>0300?200>000900<0=000800:0<201?000;76000@000?005=000:05?0040800;0@0059<00100000800200000=00<580030=00?0300>80@000580010002000=9?000<406@0=00700050300<0006004;00@0700@050>0010020;1?900=002000>000>000;0200=3500<";
            string expectedSolve = "15:2349;<@6>?=78>@8=5?7<43129:6;9<47:@618=?;35>236;?2=8>75:94@<1=4>387;:5<261?@98;76412@9:>?<35=<91:=5?634@8>2;7@?259<>31;7=:68462@>;94=?1<587:37=91?235;>8:@<46583;1:<7264@=9?>?:<4>6@8=9372;152358<>:?6794;1=@:7=<@359>8;1642?;1?968=4@25<7>3:4>6@7;12:?=3589<";

            // Act
            string actualSolve = _ui.ValidationAndStart(input);

            // Assert
            Assert.That(actualSolve, Is.EqualTo(expectedSolve));
        }

        [Test]
        public void ValidationAndStart_Test_25X25() // testing EMPTY soduko of size 25X25
        {
            // Arrange
            string input = "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
            string expectedSolve = "123456789:;<=>?@ABCDEFGHIDBGH<15;FI48:EC3?>962@=7AF;8EI2?@CG15DAB4=H7<39:6>9:=>@3DA<H276GF15I8E4?B;C?A67C4>E=B39IH@2:F;G158<DE1234IH5896D<:>;G@A?7BC=FCDIF>@16G?=357H829<B;4AE:=@9BH:27;>F1A8G645ECD3I?<;<AG?D3=EFC4B9I71:H>6258@5678:<4BAC?2@;ED3=IFG1>9H4I123?B958DE><:GCA6;@H7F=GH@<9FI16=BC357ED2?8:>4A;:E?CA>@27;HF1=8BI459<G3D6>8F=;CG3D<IA469H71@:?E25B75D6BHE4:A@G2?;<>3F=I81C934E1289?H5:=;F<A6CB@>IDG7AFCIDG=>167@E35:982HB;<4?H>:@GBFD278I?1A=;<45C693E<=B?8;CI3E9HG46>FD17A:@25695;7A:<4@>BC2D?EG3IF=H182349156F?D<:8C=I@;>AH7EBGIC>AE78GB15;9D3FH6:2=<?@4BGHDF9;CI2E>7@15<?=48A6:3@?<:=EAH>3G6FB4987D15C;I287;56=<:@4A?HI2CBEG39DF>1";

            // Act
            string actualSolve = _ui.ValidationAndStart(input);

            // Assert
            Assert.That(actualSolve, Is.EqualTo(expectedSolve));
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
        public void GetInputAsFile_FileExceptionsTest2() // testing response to file input which is not type .txt
        {
            // Arrange
            string input = "C:\\Users\\user\\Downloads\\not_on_pc.pptx";
            StringReader stringReader = new(input);
            Console.SetIn(stringReader);

            Assert.Throws<InvalidFileTypeException>(() => _ui.GetInputAsFile()); // -> throwing InvalidFileTypeException
        }

        [Test]
        public void GetInputAsFile_FileExceptionsTest() // testing response to file input
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