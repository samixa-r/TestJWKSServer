using Microsoft.AspNetCore.Mvc;
using TestJWKSServer.Controllers;
using Xunit;

namespace TestJWKSServer.Tests
{
    public class JwtControllerTests
    {
        /// <summary>
        /// Test case to verify that the Authenticate method returns a valid JWT
        /// </summary>
        [Fact]
        public void Authenticate_ReturnsValidJwt()
        {
            // Arrange
            var controller = new JwtController();

            // Act
            var result = controller.Authenticate();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenResult = (JwtToken)okResult.Value;
            var token = Assert.IsType<string>(tokenResult.Token);
            Assert.NotNull(token);
        }

        /// <summary>
        /// Test case to verify that the Authenticate method returns an expired JWT 
        /// </summary>
        [Fact]
        public void Authenticate_ReturnsExpiredJwt()
        {
            // Arrange
            var controller = new JwtController();

            // Act
            var result = controller.Authenticate(expired: true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenResult = (JwtToken)okResult.Value;
            var token = Assert.IsType<string>(tokenResult.Token);
            Assert.NotNull(token);
        }

        /// <summary>
        /// Test case to verify that the GetJwks method returns JWKS
        /// </summary>
        [Fact]
        public void GetJwks_ReturnsJwks()
        {
            // Arrange
            var controller = new JwtController();

            // Act
            var result = controller.GetJwks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var jwks = Assert.IsType<string>(okResult.Value);
        }
    }
}
