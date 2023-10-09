using System.Reflection.Metadata.Ecma335;
using Cadeterias;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CadeteriaController : ControllerBase
{
    private Cadeteria cadeteria;
    private AccesoADatos accesoADatos;

    private readonly ILogger<CadeteriaController> _logger;

    public CadeteriaController(ILogger<CadeteriaController> logger)
    {
        _logger = logger;
        cadeteria=Cadeteria.GetCadeteria();
        accesoADatos = new AccesoJSON("Cadetes.json", "Cadeteria.json");
        List<Cadete> cadetes = accesoADatos.CargarCadetes();
        List<Cadeteria> cadeterias = accesoADatos.CargarCadeterias();
        if (cadeterias.Count>0)
        {
            cadeteria = cadeterias[0];
        }
        cadeteria.ListaCadetes=cadetes;
    }

    [HttpGet]
    public ActionResult<string> GetNombreCadeteria()
    {
        return Ok(cadeteria.Nombre);
    }

    [HttpGet ("Cadetes")]
    public ActionResult<IEnumerable<Cadete>> GetCadetes(){
        try
        {
            var listaCadetes = cadeteria.ListadoCadetes();
            return Ok(listaCadetes);
        }
         catch (Exception ex)
        {
            // Registra el error en la consola o en un archivo de registro
            Console.WriteLine($"Error en GetCadetes: {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
        
    }

    [HttpPost ("AddCadete")]
    public ActionResult<Cadete> PostCadete(string nombre, string direccion, string telefono){
        var nuevoCadete = cadeteria.AgregarCadete(nombre,direccion,telefono);
        accesoADatos.GuardarNuevoCadete(nuevoCadete);
        return Ok(nuevoCadete);
    }

   
}
