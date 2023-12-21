using Microsoft.AspNetCore.Mvc;
using SecretGiftExchange.Services;
using System.Linq;

namespace SecretGiftExchange.API.Controllers
{
    public class ParticipantDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }


    [ApiController]
    [Route("[controller]")]
    public class GiftAssignmentsController : ControllerBase
    {
        private readonly ParticipantService _participantService;

        public GiftAssignmentsController(ParticipantService participantService)
        {
            _participantService = participantService;
        }

        /// <summary>
        /// Adds a new participant to the gift exchange.
        /// </summary>
        /// <param name="participantDto">The DTO containing name and email of the participant to add.</param>
        /// <returns>A response indicating the status of the operation.</returns>
        /// <response code="200">Participant added successfully.</response>
        /// <response code="400">Bad request if a participant with the same name already exists.</response>
        [HttpPost("add")]
        public IActionResult AddParticipant([FromBody] ParticipantDto participantDto)
        {
            try
            {
                _participantService.AddParticipant(participantDto.Name, participantDto.Email);
                return Ok("Participant added successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates the information of an existing participant.
        /// </summary>
        /// <param name="id">The unique identifier of the participant to update.</param>
        /// <param name="participantDto">The DTO containing the new name and email for the participant.</param>
        /// <returns>A response indicating the status of the operation.</returns>
        /// <response code="200">Participant updated successfully.</response>
        /// <response code="400">Bad request if the input is invalid.</response>
        /// <response code="404">Not found if the participant does not exist.</response>
        [HttpPut("update/{id}")]
        public IActionResult UpdateParticipant(Guid id, [FromBody] ParticipantDto participantDto)
        {
            try
            {
                _participantService.UpdateParticipant(id, participantDto.Name, participantDto.Email);
                return Ok("Participant updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Gets all participants in the gift exchange.
        /// </summary>
        /// <returns>A list of all participants.</returns>
        [HttpGet("getallparticipants")]
        public IActionResult GetAllParticipants()
        {
            var participants = _participantService.GetAllParticipants();
            return Ok(participants);
        }

        /// <summary>
        /// Assigns each participant another participant to whom they should give a gift.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when there are less than two participants in the exchange.</exception>
        /// <returns>A response indicating the status of the operation.</returns>
        [HttpPost("assign")]
        public IActionResult AssignGifts()
        {
            try
            {
                _participantService.AssignGifts();
                return Ok("Gifts assigned successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the current list of gift assignments.
        /// </summary>
        /// <returns>A list of gift assignments.</returns>
        [HttpGet]
        public IActionResult GetGiftAssignments()
        {
            var participants = _participantService.GetAllParticipants();
            var assignments = participants.Select(p => new
            {
                Giver = p.Name,
                Recipient = participants.FirstOrDefault(rec => rec.Id == p.AssignedRecipientId)?.Name
            });

            return Ok(assignments);
        }

        /// <summary>
        /// Removes a participant from the gift exchange.
        /// </summary>
        /// <param name="id">The unique identifier of the participant to remove.</param>
        /// <returns>Confirmation message.</returns>
        [HttpDelete("remove/{id}")]
        public IActionResult RemoveParticipant(Guid id)
        {
            _participantService.RemoveParticipant(id);
            return Ok("Participant removed successfully.");
        }

        /// <summary>
        /// Removes all participants from the gift exchange.
        /// </summary>
        /// <returns>Confirmation message.</returns>
        [HttpDelete("removeAll")]
        public IActionResult RemoveAllParticipants()
        {
            _participantService.RemoveAllParticipants();
            return Ok("All participants removed successfully.");
        }
    }
}
