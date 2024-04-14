using DataLayer;
using Xunit;

namespace DataLayerTests
{
    public class FirstDataTest
    {
        [Fact]
        public void GetR()
        {
            // Arrange
            IBall ball = new Ball(5,5,5, 20.0);
            // Act
            int R = ball.R;
            // Assert
            Assert.Equal(5, R);

        }
    }
}
