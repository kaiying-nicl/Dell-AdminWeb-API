using Dell_AdminWeb_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using Persistence.Data.Contracts;
using Persistence.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace API.UnitTests
{
    public class DocumentControllerTests
    {
        private readonly DocumentsController _controller;
        private readonly AutoMocker _mocker;

        public DocumentControllerTests()
        {
            _mocker = new AutoMocker();
            _controller = _mocker.CreateInstance<DocumentsController>();
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnDocuments()
        {
            _mocker.GetMock<IDocumentRepository>()
                .Setup(m => m.GetAllAsync())
                .ReturnsAsync(new List<Document>()
                {
                    new Document() { Id = 1, Name = "Testing1", Url = ""},
                    new Document() { Id = 2, Name = "Testing2", Url = ""}
                });

            var result = (await _controller.GetAll()).Result as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            var resultValue = (List<Document>)result.Value;
            Assert.Equal(2, resultValue.Count);

            _mocker.GetMock<IDocumentRepository>().Verify(m => m.GetAllAsync(), Times.Once());
        }
    }
}
