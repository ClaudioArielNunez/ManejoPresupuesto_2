using Dapper;
using ManejoPresupuesto_2.Models;
using ManejoPresupuesto_2.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto_2.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IServicioUsuarios servicioUsuarios;

        public TiposCuentasController(IRepositorioTiposCuentas RepositorioTiposCuentas, IServicioUsuarios servicioUsuarios)
        {
            repositorioTiposCuentas = RepositorioTiposCuentas;
            this.servicioUsuarios = servicioUsuarios;
        }

        //Metodo para listar tiposCuentas
        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
            return View(tiposCuentas);
        }

        public IActionResult Crear()
        {
            
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = servicioUsuarios.ObtenerUsuarioId();

            //validamos que no exista ya ese nombre con el mismo id de usuario

            var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (yaExisteTipoCuenta)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe");
                
                return View(tipoCuenta);
            }
            await repositorioTiposCuentas.Crear(tipoCuenta);

            //return View();
            return RedirectToAction("Index"); //ahora q tengo index lo puedo redireccionar
        }

        //Metodo para ACTUALIZAR GET
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            if(tipoCuenta is null)
            {
                //si el usuarioId es erroneo no lo va a encontrar
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tipoCuenta);
        }

        //Metodo para ACTUALIZAR POST
        [HttpPost]
        public async Task<IActionResult> Editar(TipoCuenta tipoCuenta)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuentaExiste = await repositorioTiposCuentas.ObtenerPorId(tipoCuenta.Id, usuarioId);
            if (tipoCuentaExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioTiposCuentas.Actualizar(tipoCuenta);
            return RedirectToAction("Index");
        }

        //Metodo para borrar, GET para Mostrar, POST para Borrar
        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            //si es nulo o el usuario no tiene permiso
            if(tipoCuenta is null)
            {
                RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoCuenta);
        }

        //Metodo para borrar POST
        [HttpPost]
        //public async Task<IActionResult> Borrar(TipoCuenta tipoCta) //Puede funcionar si se lo llama igual, pero con distinto parametro
        public async Task<IActionResult> BorrarTipoCuenta(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                RedirectToAction("NoEncontrado", "Home");
            }
            //En esta linea borramos
            await repositorioTiposCuentas.Borrar(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();

            var yaExisteNombre = await repositorioTiposCuentas.Existe(nombre, usuarioId);

            if (yaExisteNombre)
            {
                return Json($"El nombre {nombre} ya existe ahorita!!!");
            }

            return Json(true);
        }

        //Metodo para ordenar y poner el valor al atributo Orden
        
    }
}
