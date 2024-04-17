using Dapper;
using ManejoPresupuesto_2.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Abstractions;

namespace ManejoPresupuesto_2.Servicios
{
    public interface IRepositorioTiposCuentas
    {
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Borrar(int id);
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioTiposCuentas:IRepositorioTiposCuentas
    {
        private readonly string connectionString;

        //constructor
        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        //METODO para crear tipo de cuenta
        //public void Crear(TipoCuenta tipoCuenta)
        //{
        //    using var conexion = new SqlConnection(connectionString);
        //    var id = conexion.QuerySingle<int>($@"INSERT INTO TiposCuentas(Nombre, UsuarioId, Orden) VALUES(@Nombre, @UsuarioId, 0);
        //                                        SELECT SCOPE_IDENTITY();", tipoCuenta);
        //    tipoCuenta.Id = id;
        //}

        //METODO ASINCRONO
        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var conexion = new SqlConnection(connectionString);
            var id = await conexion.QuerySingleAsync<int>($@"INSERT INTO TiposCuentas(Nombre, UsuarioId, Orden)
                                                          VALUES(@Nombre, @UsuarioId, 0);
                                                          SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;
        }

        //METODO PARA VALIDAR Q NO SE INGRESE EL MISMO TIPO DE CUENTA CON EL MISMO ID
        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            //PODEMOS CREAR UN OBJETO ANONIMO AQUI Y LUEGO PASARLO
            //var parametros = new {Nombre = nombre, UsuarioId = usuarioId };
            using var conexion = new SqlConnection(connectionString);
            var existe = await conexion.QueryFirstOrDefaultAsync<int>("SELECT 1 FROM TiposCuentas WHERE UsuarioId = @usuarioId AND NOMBRE = @nombre", new {nombre, usuarioId});

            return existe == 1;
        }

        //METODO PARA LISTAR LOS TIPOS DE CUENTAS de un usuario en particular
        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var conexion = new SqlConnection (connectionString);
            return await conexion.QueryAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden FROM TiposCuentas WHERE UsuarioId = @UsuarioId", new { usuarioId });
        }

        //METODO PARA ACTUALIZAR TIPOS DE CUENTA
        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var conexion = new SqlConnection(connectionString);
            //Execute permite ejecuta un query q no retorna nada
            await conexion.ExecuteAsync(@"UPDATE TiposCuentas SET Nombre = @Nombre WHERE Id = @Id",tipoCuenta);
        }

        //METODO PARA OBTENER TIPOS DE CUENTA SEGUN ID
        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var conexion = new SqlConnection(connectionString);
            return await conexion.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden FROM TiposCuentas WHERE Id = @Id AND UsuarioId = @UsuarioId", new {id, usuarioId});
        }

        //METODO PARA BORRAR TIPOS DE CUENTA CON ID
        public async Task Borrar(int id)
        {
            using var conexion = new SqlConnection(connectionString);
            await conexion.ExecuteAsync("DELETE FROM TiposCuentas WHERE Id = @Id", new {id});
            
        }
    }
}
