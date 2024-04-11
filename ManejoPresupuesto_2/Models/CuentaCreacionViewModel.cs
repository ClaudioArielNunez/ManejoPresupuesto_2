using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto_2.Models
{
    public class CuentaCreacionViewModel : Cuenta
    {
        public IEnumerable<SelectListItem> TiposCuentas { get; set; }
    }

}
