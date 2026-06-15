using System;
using System.Collections.Generic;

namespace Dsw2026Ej15.Domain.Entities
{
    public interface IPersistence
    {
        List<Speciality> Specialities { get; }
        List<Doctor> Doctors { get; }
        
        void AddDoctor(Doctor doctor);
        Doctor? GetDoctorById(Guid id);
        List<Doctor> GetActiveDoctors();
    }
}