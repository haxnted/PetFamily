using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromQuery] Guid id, [FromBody] CreateVolunteerRequest request)
    {
        var volunteerId = VolunteerId.Create(id);
        
        var fullName = FullName.Create(request.Name, request.Surname, request.Patronymic);
        if (fullName.IsFailure)
            return BadRequest(fullName.Error);

        var generalDescription = Description.Create(request.Description);
        if (generalDescription.IsFailure)
            return BadRequest(generalDescription.Error);

        var ageExperience = AgeExperience.Create(request.AgeExperience);
        if (ageExperience.IsFailure)
            return BadRequest(ageExperience.Error);
        
        var phoneNumber = PhoneNumber.Create(request.Number);
        if (phoneNumber.IsFailure)
            return BadRequest(phoneNumber.Error);

        var volunteer = Volunteer.Create(volunteerId, fullName.Value,
            generalDescription.Value, ageExperience.Value, phoneNumber.Value);
        if (volunteer.IsFailure)
            return BadRequest(volunteer.Error);

        return Ok(volunteerId);
    }
}

