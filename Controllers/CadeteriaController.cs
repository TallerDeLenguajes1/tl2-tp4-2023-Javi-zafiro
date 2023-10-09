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
        accesoADatos = new AccesoJSON("Cadete.json", "Cadeteria.json");
    }

    [HttpGet]
    public ActionResult<string> GetNombreCadeteria()
    {
        List<Cadete> cadetes = accesoADatos.CargarCadetes();
        List<Cadeteria> cadeterias = accesoADatos.CargarCadeterias(cadetes);
        if (cadeterias.Count>0)
        {
            cadeteria = cadeterias[0];
        }
        return Ok(cadeteria.Nombre);
    }
}
