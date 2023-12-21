using SecretGiftExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


namespace SecretGiftExchange.Services
{
    /// <summary>
    /// Provides services for managing participants in the Secret Gift Exchange,
    /// including persistent storage of participant data in a JSON file.
    /// </summary>
    public class ParticipantService
    {
        private readonly string _filePath;
        private List<Participant> _participants;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticipantService"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration where settings like file paths are stored.
        /// This configuration is used to get the relative path for the participants' data file.</param>
        public ParticipantService(IConfiguration configuration)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string relativePath = configuration.GetValue<string>("ParticipantFilePath");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
#pragma warning restore CS8604 // Possible null reference argument.
            _participants = LoadParticipantsFromFile();
        }

        /// <summary>
        /// Loads participant data from the JSON file.
        /// </summary>
        /// <returns>A list of participants.</returns>
        private List<Participant> LoadParticipantsFromFile()
        {
            try
            {
                var jsonData = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Participant>>(jsonData) ?? new List<Participant>();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("No existing data file found. Starting with an empty list.");
                return new List<Participant>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Saves the current participant data to the JSON file.
        /// </summary>
        private void SaveParticipantsToFile()
        {
            var jsonData = JsonSerializer.Serialize(_participants, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }

        /// <summary>
        /// Adds a new participant to the exchange.
        /// </summary>
        /// <param name="name">The name of the participant to add.</param>
        /// <param name="email">The email of the participant.</param>
        /// <exception cref="InvalidOperationException">Thrown when a participant with the provided name already exists.</exception>
        public void AddParticipant(string name, string email)
        {
            if (_participants.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A participant with the name '{name}' already exists.");
            }

            var newParticipant = new Participant { Name = name, Email = email };
            _participants.Add(newParticipant);
            SaveParticipantsToFile();
        }

        /// <summary>
        /// Updates the name and email of an existing participant.
        /// </summary>
        /// <param name="id">The unique identifier of the participant to update.</param>
        /// <param name="newName">The new name for the participant.</param>
        /// <param name="newEmail">The new email for the participant.</param>
        /// <exception cref="ArgumentException">Thrown when no participant is found with the given ID.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a participant with the provided new name already exists.</exception>
        public void UpdateParticipant(Guid id, string newName, string newEmail)
        {
            var participant = _participants.FirstOrDefault(p => p.Id == id);
            if (participant == null)
            {
                throw new ArgumentException("Participant not found.");
            }

            if (_participants.Any(p => p.Name.Equals(newName, StringComparison.OrdinalIgnoreCase) && p.Id != id))
            {
                throw new InvalidOperationException($"A participant with the name '{newName}' already exists.");
            }

            participant.Name = newName;
            participant.Email = newEmail;
            SaveParticipantsToFile();
        }


        /// <summary>
        /// Removes a participant from the exchange and updates the JSON storage.
        /// </summary>
        /// <param name="id">The unique identifier of the participant to remove.</param>
        public void RemoveParticipant(Guid id)
        {
            var participant = _participants.FirstOrDefault(p => p.Id == id);
            if (participant == null)
            {
                Console.WriteLine("Participant not found.");
                return;
            }

            _participants.Remove(participant);
            SaveParticipantsToFile();
            Console.WriteLine($"Participant {id} removed successfully.");
        }

        /// <summary>
        /// Removes all participants from the exchange.
        /// </summary>
        public void RemoveAllParticipants()
        {
            _participants.Clear();
            SaveParticipantsToFile();
        }

        /// <summary>
        /// Lists all participants currently in the exchange.
        /// </summary>
        public void ListParticipants()
        {
            foreach (var participant in _participants)
            {
                Console.WriteLine($"ID: {participant.Id}, Name: {participant.Name}");
            }
        }

        /// <summary>
        /// Retrieves all participants in the exchange.
        /// </summary>
        /// <returns>A list of all participants.</returns>
        public List<Participant> GetAllParticipants()
        {
            return _participants.ToList();
        }

        /// <summary>
        /// Assigns each participant another participant to whom they should give a gift.
        /// Ensures no participant is assigned to themselves.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when there are less than two participants in the exchange.</exception>
        public void AssignGifts()
        {
            Random rng = new Random();
            int n = _participants.Count;

            // If there are less than 2 participants, assignment is not possible
            if (n < 2)
            {
                throw new InvalidOperationException("Not enough participants for gift exchange.");
            }

            // Create a list of indices
            var indices = Enumerable.Range(0, n).ToList();

            // Shuffle the list of indices with the constraint that indices[i] != i
            for (int i = 0; i < n - 1; i++)
            {
                int j;
                do
                {
                    j = rng.Next(i + 1, n);
                } while (j == i);

                (indices[i], indices[j]) = (indices[j], indices[i]);
            }
             
            // Assign gifts based on shuffled indices
            for (int i = 0; i < n; i++)
            {
                _participants[i].AssignedRecipientId = _participants[indices[i]].Id;
            }
            SaveParticipantsToFile();
        }
    }
}
