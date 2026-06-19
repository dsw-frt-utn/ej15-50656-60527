using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Dsw2026Ej15.Domain;
using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Data
{
    public class PersistenceInMemory : IPersistence
    {
        public List<Speciality> Specialities { get; private set; } = new List<Speciality>();
        public List<Doctor> Doctors { get; private set; } = new List<Doctor>();

        public PersistenceInMemory()
        {
            LoadSpecialities();
        }

        private void LoadSpecialities()
        {
            // Busca el archivo json en el directorio donde se ejecuta la API
            string path = "specialities.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                var loaded = JsonSerializer.Deserialize<List<Speciality>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (loaded != null)
                {
                    Specialities = loaded;
                }
            }
        }

        public void AddDoctor(Doctor doctor)
        {
            Doctors.Add(doctor);
        }

        public Doctor? GetDoctorById(Guid id)
        {
            return Doctors.FirstOrDefault(d => d.Id == id);
        }

        public List<Doctor> GetActiveDoctors()
        {
            return Doctors.Where(d => d.IsActive).ToList();
        }
    }
}