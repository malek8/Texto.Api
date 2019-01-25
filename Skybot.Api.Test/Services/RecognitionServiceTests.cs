using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skybot.Api.Services;
using Moq;
using Skybot.Api.Services.IntentsServices;
using Skybot.Api.Services.Settings;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Skybot.Api.Services.Luis;
using Skybot.Api.Models;
using System.Collections.Generic;

namespace Skybot.Api.Test.Services
{
    [TestClass]
    public class RecognitionServiceTests
    {
        private RecognitionService _testee;
        private Mock<ISettings> _settingsMock;
        private Mock<ILogger<RecognitionService>> _loggerMock;
        private Mock<IIntentService> _intentServiceMock;
        private Mock<ILuisService> _luisServiceMock;

        [TestInitialize]
        public void Init()
        {
            _settingsMock = new Mock<ISettings>();
            _loggerMock = new Mock<ILogger<RecognitionService>>();
            _intentServiceMock = new Mock<IIntentService>();
            _luisServiceMock = new Mock<ILuisService>();

            MockSettings();
        }

        [TestMethod]
        public async Task MessageWithTranslationIntent_Should_ReturnResponseWithTranslation()
        {
            var message = "Say hello in french";
            var resultModel = new LuisResultModel
            {
                Intents = new List<LuisIntent>
                {
                    new LuisIntent {Name = "Translate", Score = 0.99},
                    new LuisIntent {Name = "SomethingElse", Score = 0.008}
                },
                Entities = new List<LuisEntity>
                {
                    new LuisEntity {Name = "french", Score = 0.99, Type = "Dictionary.TargetLanguage"},
                    new LuisEntity {Name = "hello", Score = 0.906, Type = "Dictionary.Text"}
                }
            };

            MockLuisService(message, resultModel);
            MockIntentService("Translate", resultModel.Entities);

            _testee = new RecognitionService(_settingsMock.Object,
                _intentServiceMock.Object,
                _luisServiceMock.Object,
                _loggerMock.Object);

            var response = await _testee.Process(message);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Message, "Bonjour");
        }

        private void MockLuisService(string message, LuisResultModel resultModel)
        {
            _luisServiceMock.Setup(x => x.Query(message)).Returns(Task.FromResult(resultModel));
        }

        private void MockIntentService(string intent, IList<LuisEntity> entities)
        {
            _intentServiceMock.Setup(x => x.Execute(intent, entities))
                .Returns(Task.FromResult(new RecognitionResult
                {
                    Message = "Bonjour"
                }));
        }

        private void MockSettings()
        {
            _settingsMock.Setup(x => x.IntentThreshold).Returns(0.75);
            _settingsMock.Setup(x => x.LuisAppUri).Returns(string.Empty);
        }
    }
}
