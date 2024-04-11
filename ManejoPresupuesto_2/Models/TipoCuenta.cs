using ManejoPresupuesto_2.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto_2.Models
{
    public class TipoCuenta
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(maximumLength:30, MinimumLength =4, ErrorMessage ="La longitud del campo debe estar entre {2} y {1}")]
        [Display(Name ="Nombre del tipo de cuenta:")]
        [PrimeraLetraMayusculaAtribute]
        [Remote(action: "VerificarExisteTipoCuenta", controller: "TiposCuentas")]
        public string Nombre { get; set; }

        [Display(Name ="Id de Usuario:")]
        public int UsuarioId { get; set; }
        public int Orden { get; set; }
    }
}
