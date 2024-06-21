using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExamenDesrrollo.Models
{
    public class UsuariosModelS
    {
        public string nombreUsuario { get; set; }
        public string passwordUsuario { get; set; } 
        public bool isActive { get; set; }
    }
}
