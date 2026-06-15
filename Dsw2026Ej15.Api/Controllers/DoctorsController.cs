using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Domain;
using Dsw2026Ej15.Api.Dtos;
using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IPersistence _persistence;

        // Inyección de dependencias
        public DoctorsController(IPersistence persistence)
        {
            _persistence = persistence;
        }

        [HttpPost]
        public IActionResult CreateDoctor([FromBody] CreateDoctorDto request)
        {
            // Nota: Las validaciones de [Required] en el DTO son manejadas automáticamente 
            // por el [ApiController], pero para reglas de negocio lanzamos nuestra excepción:
            
            var speciality = _persistence.Specialities.FirstOrDefault(s => s.Id == request.SpecialityId);
            if (speciality == null)
            {
                // AQUÍ LANZAMOS LA EXCEPCIÓN PERSONALIZADA
                throw new ValidationException("La especialidad indicada no existe.");
            }

            var newDoctor = new Doctor
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                LicenseNumber = request.LicenseNumber,
                IsActive = true,
                Speciality = speciality
            };

            _persistence.AddDoctor(newDoctor);

            return CreatedAtAction(nameof(GetDoctorById), new { id = newDoctor.Id }, newDoctor);
        }

        [HttpGet]
        public IActionResult GetAllActiveDoctors()
        {
            var doctors = _persistence.GetActiveDoctors();
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public IActionResult GetDoctorById(Guid id)
        {
            var doctor = _persistence.GetDoctorById(id);
            
            if (doctor == null || !doctor.IsActive)
            {
                return NotFound("El médico no existe o se encuentra inactivo.");
            }

            var response = new DoctorResponseDto
            {
                Name = doctor.Name,
                LicenseNumber = doctor.LicenseNumber,
                SpecialityName = doctor.Speciality.Name
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDoctor(Guid id)
        {
            var doctor = _persistence.GetDoctorById(id);
            
            if (doctor == null || !doctor.IsActive)
            {
                return NotFound("El médico no existe o ya se encuentra inactivo.");
            }

            doctor.IsActive = false; // Baja lógica
            
            return NoContent();
        }
    }
}
