using Dapper;
using ManejoPresupuesto_2.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto_2.Servicios
{
    public interface IRepositorioCuentas
    {
        Task Crear(Cuenta cuenta);
    }
    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string connectionString;

        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Crear(Cuenta cuenta)
        {
            using var conexion = new SqlConnection(connectionString);
            var id = await conexion.QuerySingleAsync<int>(@"INSERT INTO Cuentas(Nombre, TipoCuentaId, Descripcion, Balance)
                                                        VALUES(@Nombre, @TipoCuentaId, @Descripcion, @Balance);
                                                        SELECT SCOPE_IDENTITY();", cuenta);
            cuenta.Id = id; //Esto es mas por si queremos llevar a una pagina de detalle, donde para ubicar usariamos el id
        }
    }
}
