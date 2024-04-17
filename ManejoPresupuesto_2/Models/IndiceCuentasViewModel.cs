namespace ManejoPresupuesto_2.Models
{
    public class IndiceCuentasViewModel
    {
        public string TipoCuenta { get; set; }
        public IEnumerable<Cuenta> Cuentas { get; set;}        
        public Decimal Balance => Cuentas.Sum(x => x.Balance);
        //calcula la suma del Balance de todas las Cuenta en la colección Cuentas
    }
}
