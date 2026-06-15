using System;
using System.ComponentModel.DataAnnotations;

namespace Dsw2026Ej15.Api.Dtos
{
    public class CreateDoctorDto
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de licencia es requerido.")]
        public string LicenseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Id de la especialidad es requerido.")]
        public Guid SpecialityId { get; set; }
    }
}