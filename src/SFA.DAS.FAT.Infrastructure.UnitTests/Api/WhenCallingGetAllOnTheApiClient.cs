using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Infrastructure.Api;
using SFA.DAS.FAT.Infrastructure.UnitTests.HttpMessageHandlerMock;

namespace SFA.DAS.FAT.Infrastructure.UnitTests.Api
{
    public class WhenCallingGetAllOnTheApiClient
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called_With_Authentication_Header_And_Data_Returned(
            List<string> testObject, 
            FindApprenticeshipTrainingApi config)
        {
            //Arrange
            var configMock = new Mock<IOptions<FindApprenticeshipTrainingApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var getTestRequest = new GetTestRequest("https://test.local");
            
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(testObject)),
                StatusCode = HttpStatusCode.Accepted
            };
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, getTestRequest.GetAllUrl, config.Key);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new ApiClient(client, configMock.Object);

            //Act
            var actual = await apiClient.GetAll<string>(getTestRequest);
            
            //Assert
            actual.Should().BeEquivalentTo(testObject);
        }
        
        
        [Test, AutoData]
        public void Then_If_It_Is_Not_Successful_An_Exception_Is_Thrown(
            FindApprenticeshipTrainingApi config)
        {
            //Arrange
            var configMock = new Mock<IOptions<FindApprenticeshipTrainingApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var getTestRequest = new GetTestRequest("https://test.local");
            var response = new HttpResponseMessage
            {
                Content = new StringContent(""),
                StatusCode = HttpStatusCode.BadRequest
            };
            
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, getTestRequest.GetAllUrl, config.Key);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new ApiClient(client, configMock.Object);
            
            //Act Assert
            Assert.ThrowsAsync<HttpRequestException>(() => apiClient.GetAll<string>(getTestRequest));
            
        }

        private class GetTestRequest : IGetAllApiRequest
        {
            public GetTestRequest (string baseUrl)
            {
                BaseUrl = baseUrl;
            }
            public string BaseUrl { get; }
            public string GetAllUrl => $"{BaseUrl}/test-url/get";
            
        }
    }
}