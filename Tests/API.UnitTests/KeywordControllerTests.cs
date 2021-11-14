using Dell_AdminWeb_API.Controllers;
using Dell_AdminWeb_API.Models;
using Dell_AdminWeb_API.Models.PostBodies;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using Persistence.Data.Contracts;
using Persistence.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace API.UnitTests
{
    public class KeywordControllerTests
    {
        private readonly KeywordsController _controller;
        private readonly AutoMocker _mocker;

        public KeywordControllerTests()
        {
            _mocker = new AutoMocker();
            _controller = _mocker.CreateInstance<KeywordsController>();
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsKeywords()
        {
            _mocker.GetMock<IKeywordRepository>()
                .Setup(m => m.GetAllAsync())
                .ReturnsAsync(new List<Keyword>()
                {
                    new Keyword() { Id = 1, Value = "Test" },
                    new Keyword() { Id = 2, Value = "Test2" },
                });

            var result = (await _controller.GetAll()).Result as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            var resultValue = (List<KeywordDTO>)result.Value;
            Assert.Equal(2, resultValue.Count);

            _mocker.GetMock<IKeywordRepository>().Verify(m => m.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task Add_WhenCalled_WillCreate()
        {
            var newKeyword = new KeywordPostBody { Value = "Test" };
            Keyword param = null;

            _mocker.GetMock<IKeywordRepository>()
                .Setup(m => m.AddAsync(It.IsAny<Keyword>()))
                .Callback<Keyword>(k => param = k)
                .Returns(Task.CompletedTask);

            var result = (await _controller.Add(newKeyword)) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(param);
            Assert.Equal(newKeyword.Value, param.Value);

            _mocker.GetMock<IKeywordRepository>().Verify(m => m.AddAsync(param), Times.Once());
        }

        [Fact]
        public async Task Update_WhenCalled_WillUpdate()
        {
            var keyword = new KeywordDTO { Id = 1, Value = "Test" };
            Keyword param = null;

            _mocker.GetMock<IKeywordRepository>()
                .Setup(m => m.UpdateAsync(It.IsAny<Keyword>()))
                .Callback<Keyword>(k => param = k)
                .Returns(Task.CompletedTask);

            var result = (await _controller.Update(keyword)) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(param);
            Assert.Equal(keyword.Id, param.Id);

            _mocker.GetMock<IKeywordRepository>()
                .Verify(m => m.UpdateAsync(param), Times.Once());
        }

        [Fact]
        public async Task Delete_WhenCalled_WillDelete()
        {
            _mocker.GetMock<IKeywordRepository>()
                .Setup(m => m.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var result = (await _controller.Delete(1)) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            _mocker.GetMock<IKeywordRepository>()
                .Verify(m => m.DeleteAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetMappings_WhenCalled_WillReturnMappings()
        {
            var keywordId = 1;

            _mocker.GetMock<IKeywordDocumentMappingRepository>()
                .Setup(m => m.GetDocumentsByKeywordIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<Document>()
                {
                    new Document() { Id = 1, Name = "Testing1", Url = ""},
                    new Document() { Id = 2, Name = "Testing2", Url = ""}
                });

            var result = (await _controller.GetMappings(keywordId)).Result as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            var resultValue = (List<Document>)result.Value;
            Assert.Equal(2, resultValue.Count);

            _mocker.GetMock<IKeywordDocumentMappingRepository>()
                .Verify(m => m.GetDocumentsByKeywordIdAsync(keywordId), Times.Once());
        }

        [Fact]
        public async Task UpdateMappings_WhenCalled_WillUpdateMappings()
        {
            var keywordId = 1;
            var addIds = new List<int>();
            var deleteIds = new List<int>();

            _mocker.GetMock<IKeywordDocumentMappingRepository>()
                .Setup(m => m.GetAllByKeywordIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<KeywordDocumentMapping>()
                {
                    new KeywordDocumentMapping() { KeywordId = 1, DocumentId = 1, LastModified = DateTime.Now },
                    new KeywordDocumentMapping() { KeywordId = 1, DocumentId = 8, LastModified = DateTime.Now }
                });

            _mocker.GetMock<IKeywordDocumentMappingRepository>()
                .Setup(m => m.AddRangeAsync(It.IsAny<int>(), It.IsAny<List<int>>()))
                .Callback<int, List<int>>((a, b) => { keywordId = a; addIds = b; })
                .Returns(Task.CompletedTask);

            _mocker.GetMock<IKeywordDocumentMappingRepository>()
                .Setup(m => m.DeleteRangeAsync(It.IsAny<int>(), It.IsAny<List<int>>()))
                .Callback<int, List<int>>((a, b) => { keywordId = a; deleteIds = b; })
                .Returns(Task.CompletedTask);

            var result = (await _controller.UpdateMappings(
                            keywordId,
                            new MappingPostBody { documentIds = new List<int>() { 1, 2, 3 } })) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            _mocker.GetMock<IKeywordDocumentMappingRepository>()
                .Verify(m => m.GetAllByKeywordIdAsync(keywordId), Times.Once());

            _mocker.GetMock<IKeywordDocumentMappingRepository>()
                .Verify(m => m.AddRangeAsync(keywordId, addIds), Times.Once());

            _mocker.GetMock<IKeywordDocumentMappingRepository>()
                .Verify(m => m.DeleteRangeAsync(keywordId, deleteIds), Times.Once());

        }
    }
}