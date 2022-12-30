using SodukoSolver;

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
        public void ValidationAndStart_EqualTest()
        {
            // Arrange
            string puzzle = "259867000000000000000300085040023050970580000038046071487635102091200068025008437";
            string expectedSolution = "259867314813459726764312985146723859972581643538946271487635192391274568625198437";

            // Act
            string actualSolution = _ui.ValidationAndStart(puzzle);

            // Assert
            Assert.AreEqual(expectedSolution, actualSolution);
        }
    }
}