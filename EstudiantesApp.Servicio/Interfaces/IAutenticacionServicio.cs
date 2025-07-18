using EstudiantesApp.Servicio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudiantesApp.Servicio.Interfaces
{
    public interface IAutenticacionServicio
    {
        Task<AutenticacionDTO> Autenticar(string email, string password);
    }
}
