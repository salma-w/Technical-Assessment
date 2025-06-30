using Common;
using Moq;
using Moq.Protected;
using System.Net;

namespace ExtractPhysicianNote.Tests
    {
        public class ExtractPhysicianNoteTest
        {
            [Fact]
            public async Task SendTextToApiAsync_ReturnsExpectedResponse()
            {
                // Arrange
                var expectedResponse = "Success";
            // Mock the HttpMessageHandler to simulate API response
            var handlerMock = new Mock<HttpMessageHandler>();
                handlerMock.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(expectedResponse)
                    });

                var client = new HttpClient(handlerMock.Object);
            // Load the API endpoint from configuration
            string endpointUrl = AppUtilities.LoadApiEndpoint("ApiSettings:ExtractUrl");

                string testPayload = "Hello, test.";

                // Act
                var result = await APICall.SendTextToApiAsync(client, endpointUrl, testPayload);

                // Assert
                Assert.Equal(expectedResponse, result);
            }
        }
    }

