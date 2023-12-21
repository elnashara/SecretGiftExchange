using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SecretGiftExchange.Services;
using System.IO;

namespace SecretGiftExchange.Tests
{
    [TestClass]
    public class ParticipantServiceTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ParticipantService _participantService;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Mock<IConfiguration> _configurationMock;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [TestInitialize]
        public void Setup()
        {
            // Mock IConfigurationSection
            var configurationSectionMock = new Mock<IConfigurationSection>();
            configurationSectionMock.Setup(a => a.Value).Returns("TestData/test_participants.json");

            // Mock IConfiguration
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(a => a.GetSection("ParticipantFilePath")).Returns(configurationSectionMock.Object);

            // Ensure the test data directory exists
            var testDataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
            if (!Directory.Exists(testDataDirectory))
            {
                Directory.CreateDirectory(testDataDirectory);
            }

            // Ensure the test JSON file exists
            var testFilePath = Path.Combine(testDataDirectory, "test_participants.json");
            if (!File.Exists(testFilePath))
            {
                File.WriteAllText(testFilePath, "[]");
            }

            _participantService = new ParticipantService(_configurationMock.Object);
        }

        [TestMethod]
        public void AddParticipant_ShouldAddUniqueParticipant()
        {
            // Arrange
            var uniqueName = "John Doe";
            var email = "john@example.com";

            // Act
            _participantService.AddParticipant(uniqueName, email);

            // Assert
            var participants = _participantService.GetAllParticipants();
            Assert.IsTrue(participants.Any(p => p.Name == uniqueName));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddParticipant_ShouldNotAllowDuplicateName()
        {
            // Arrange
            var duplicateName = "Jane Doe";
            var email1 = "jane1@example.com";
            var email2 = "jane2@example.com";
            _participantService.AddParticipant(duplicateName, email1);

            // Act & Assert
            _participantService.AddParticipant(duplicateName, email2); // This should throw an InvalidOperationException
        }


        [TestMethod]
        public void UpdateParticipant_ShouldUpdateParticipantName()
        {
            _participantService.RemoveAllParticipants();
            // Arrange
            var originalName = "John Doe";
            var originalEmail = "John.Doe@yahoo.com";

            var updatedName = "Jane Doe";
            var updatedEmail = "Jane.Doe@yahoo.com";

            _participantService.AddParticipant(originalName, originalEmail);
            var participant = _participantService.GetAllParticipants().First(p => p.Name == originalName);

            // Act
            _participantService.UpdateParticipant(participant.Id, updatedName, updatedEmail);

            // Assert
            Assert.AreEqual(updatedName, _participantService.GetAllParticipants().First(p => p.Id == participant.Id).Name);
        }

        [TestMethod]
        public void RemoveParticipant_ShouldRemoveParticipant()
        {
            _participantService.RemoveAllParticipants();
            // Arrange
            var participantName = "John Doe";
            var participantEmail = "John.Doe@yahoo.com";
            _participantService.AddParticipant(participantName, participantEmail);
            var participant = _participantService.GetAllParticipants().First(p => p.Name == participantName);

            // Act
            _participantService.RemoveParticipant(participant.Id);

            // Assert
            Assert.IsFalse(_participantService.GetAllParticipants().Any(p => p.Id == participant.Id));
        }

        [TestMethod]
        public void ListParticipants_ShouldListAllParticipants()
        {
            _participantService.RemoveAllParticipants();
            // Arrange
            _participantService.AddParticipant("John Doe", "John.Doe@yahoo.com");
            _participantService.AddParticipant("Jane Doe", "Jane.Doe@yahoo.com");

            // Act
            var participants = _participantService.GetAllParticipants();

            // Assert
            Assert.AreEqual(2, participants.Count);
        }

        [TestMethod]
        public void AssignGifts_EachParticipantAssignedToAnother()
        {
            _participantService.RemoveAllParticipants();

            // Arrange
            _participantService.AddParticipant("Ashraf Elnashar", "Ashraf.Elnashar@yahoo.com");
            _participantService.AddParticipant("Abc Abc", "Abc.Abc@yahoo.com");
            _participantService.AddParticipant("John Doe", "John.Doe@yahoo.com");
            _participantService.AddParticipant("Jane Doe", "Jane.Doe@yahoo.com");
            _participantService.AddParticipant("Bob Smith", "Bob.Smith@yahoo.com");

            // Act
            _participantService.AssignGifts();
            var participants = _participantService.GetAllParticipants();

            // Assert
            Assert.IsTrue(participants.All(p => p.AssignedRecipientId != Guid.Empty && p.AssignedRecipientId != p.Id));
            Assert.AreEqual(participants.Count, participants.Select(p => p.AssignedRecipientId).Distinct().Count());
        }
    }
}
