using Dapper;
using ManejoPresupuesto_2.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto_2.Servicios
{
    public interface IRepositorioCuentas
    {
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
        Task Crear(Cuenta cuenta);
    }
    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string connectionString;

        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //METODO PARA CREAR CUENTAS
        public async Task Crear(Cuenta cuenta)
        {
            using var conexion = new SqlConnection(connectionString);
            var id = await conexion.QuerySingleAsync<int>(@"INSERT INTO Cuentas(Nombre, TipoCuentaId, Descripcion, Balance)
                                                        VALUES(@Nombre, @TipoCuentaId, @Descripcion, @Balance);
                                                        SELECT SCOPE_IDENTITY();", cuenta);
            cuenta.Id = id; //Esto es mas por si queremos llevar a una pagina de detalle, donde para ubicar usariamos el id
        }

        //METDO PARA LISTAR LAS CUENTAS
        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId)
        {
            using var conexion = new SqlConnection(connectionString);

            return await conexion.QueryAsync<Cuenta>(@"SELECT C.Id, C.Nombre, Balance, T.Nombre as TipoCuenta
                                                       FROM CUENTAS AS C INNER JOIN TiposCuentas AS T
                                                       ON C.TipoCuentaId = @UsuarioId
                                                       order by T.Orden", new {usuarioId});
        }
    }
}
