using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretGiftExchange.Models
{
    /// <summary>
    /// Represents a participant in the Secret Gift Exchange.
    /// </summary>
    public class Participant
    {
        /// <summary>
        /// Gets or sets the unique identifier for each participant.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the participant.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email of the participant.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the ID of the participant this participant is assigned to give a gift to.
        /// </summary>
        public Guid AssignedRecipientId { get; set; }

        /// <summary>
        /// Constructor for Participant. Initializes a new participant with a unique ID.
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Participant()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Id = Guid.NewGuid();
        }
    }
}
