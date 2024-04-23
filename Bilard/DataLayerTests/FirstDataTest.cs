using DataLayer;
using Xunit;

namespace DataLayerTests
{
    public class FirstDataTest
    {
        [Fact]
        public void AddBall()
        {
            // Arrange
            IBall ball = new Ball(5,5,5, 20.0);

            DataAbstractApi dataLayer;
            dataLayer = DataAbstractApi.CreateApi();

            dataLayer.balls.Add(ball);
            // Act
            List<IBall> newBalls = dataLayer.balls;
            Assert.Contains(ball, newBalls);
            // Assert
            foreach (IBall obj in newBalls)
            {
                Assert.Equal(ball.R, obj.R);
                Assert.Equal(ball.X, obj.X);
                Assert.Equal(ball.Y, obj.Y);
            }

        }
    }
}
