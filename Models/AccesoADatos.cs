using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;
namespace Cadeterias;

public abstract class AccesoADatos
{
   protected string archivoCadeterias;
   protected string archivoCadetes;

   public AccesoADatos(string archivoCadetes, string archivoCadeterias)
        {
            this.archivoCadetes = archivoCadetes;
            this.archivoCadeterias = archivoCadeterias;
        }

        public bool ArchivosExistentes()
        {
            return File.Exists(archivoCadetes) && File.Exists(archivoCadeterias);
        }
        public abstract List<Cadete> CargarCadetes();
        public abstract List<Cadeteria> CargarCadeterias(List<Cadete> listadoCadete);
        public abstract void GuardarNuevoCadete(int id, string nombre, string direccion, string telefono);
        public abstract void GuardarNuevaCadeteria( string nombre, string telefono);
}

public class AccesoCSV: AccesoADatos
{
    public AccesoCSV(string archivoCadetes, string archivoCadeterias) : base(archivoCadetes, archivoCadeterias)
    {
        // Constructor de AccesoCSV
    }
    public override List<Cadete> CargarCadetes()
    {
        var nuevaLista = new List<Cadete>();
        var listaCsv = LeerArchivoCSV(archivoCadetes);

        foreach (var cadeteInfo in listaCsv)
        {
            if (cadeteInfo == null || cadeteInfo.Length < 4)
                continue;

            // Obtener los datos del CSV
            int id = Convert.ToInt32(cadeteInfo[0]);
            string nombre = cadeteInfo[1];
            string direccion = cadeteInfo[2];
            string telefono = cadeteInfo[3];

            // Crear una instancia de Cadete y agregarla a la lista
            var nuevoCadete = new Cadete(id, nombre, direccion, telefono);
            nuevaLista.Add(nuevoCadete);
        }
         if (nuevaLista.Count == 0)
        {
            Console.WriteLine("\n(no se encontraron cadetes para cargar)");
        }

        return nuevaLista;
    }
    public override List<Cadeteria> CargarCadeterias(List<Cadete> listadoCadete)
    {
        var ListaCadeterias = new List<Cadeteria>();
        var datos = LeerArchivoCSV(archivoCadeterias);

        foreach (var cadeteriaInfo in datos)
        {
            if (cadeteriaInfo == null || cadeteriaInfo.Length < 3)
                continue;

            // Obtener los datos del CSV
            
            string nombreCadeteria = cadeteriaInfo[0];
            string telefonoCadeteria = cadeteriaInfo[1];
            int numeroPedido = Convert.ToInt32(cadeteriaInfo[2]);

            // Crear una instancia de Cadeteria y agregarla a la lista
            var nuevaCadeteria = new Cadeteria(nombreCadeteria, telefonoCadeteria);
            nuevaCadeteria.NroUltimoPedido=numeroPedido;
            ListaCadeterias.Add(nuevaCadeteria);
        }

        return ListaCadeterias;
    }
    public override void GuardarNuevoCadete(int id, string nombre, string direccion, string telefono)
    {
        // Leer la lista actual de cadetes
        var listaCadetes = CargarCadetes();

        // Crear una nueva instancia de Cadete
        var nuevoCadete = new Cadete(id, nombre, direccion, telefono);

        // Agregar el nuevo cadete a la lista
        listaCadetes.Add(nuevoCadete);

        // Guardar la lista actualizada en el archivo
        GuardarCadetesEnArchivo(archivoCadetes, listaCadetes);
    }

    public override void GuardarNuevaCadeteria( string nombre, string telefono)
    {
        // Leer la lista actual de cadeterías
        var listaCadeterias = CargarCadeterias(null);

        // Crear una nueva instancia de Cadetería
        var nuevaCadeteria = new Cadeteria(nombre, telefono);

        // Agregar la nueva cadetería a la lista
        listaCadeterias.Add(nuevaCadeteria);

        // Guardar la lista actualizada en el archivo
        GuardarCadeteriasEnArchivo(archivoCadeterias, listaCadeterias);
    }

    private static void GuardarCadetesEnArchivo(string nombreArchivo, List<Cadete> cadetes)
    {
        try
        {
            using (var archivo = new FileStream(nombreArchivo, FileMode.Create, FileAccess.Write))
            using (var strWriter = new StreamWriter(archivo))
            {
                strWriter.WriteLine("IdCadete,NombreCadete,TelefonoCadete");
                foreach (var cadete in cadetes)
                {
                    string linea = $"{cadete.Id},{cadete.Nombre},{cadete.Direccion},{cadete.Telefono}";
                    strWriter.WriteLine(linea);
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine("Error al guardar cadetes en el archivo CSV: " + ex.Message);
        }
    }

    private static void GuardarCadeteriasEnArchivo(string nombreArchivo, List<Cadeteria> cadeterias)
    {
        try
        {
            using (var archivo = new FileStream(nombreArchivo, FileMode.Create, FileAccess.Write))
            using (var strWriter = new StreamWriter(archivo))
            {
                strWriter.WriteLine("Nombre,Telefono,NroUltimoPedido");
                foreach (var cadeteria in cadeterias)
                {
                    string linea = $"{cadeteria.Nombre},{cadeteria.Telefono},{cadeteria.NroUltimoPedido}";
                    strWriter.WriteLine(linea);
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine("Error al guardar cadeterías en el archivo CSV: " + ex.Message);
        }
    }

    private static List<string[]> LeerArchivoCSV(string nombreArchivo)
    {
        var lectura = new List<string[]>();
        if (!File.Exists(nombreArchivo))
        {
            Console.WriteLine("Archivo CSV no encontrado: {0}", nombreArchivo);
            throw new FileNotFoundException($"Archivo CSV no encontrado: {nombreArchivo}");
        }

        using (var archivo = new FileStream(nombreArchivo, FileMode.Open))
        using (var strReader = new StreamReader(archivo))
        {
            // Omitir la primera línea (encabezado)
            strReader.ReadLine();
            string linea;
            while ((linea = strReader.ReadLine()) != null)
            {
                string[] fila = linea.Split(',');
                lectura.Add(fila);
            }
        }
        return lectura;
    }
}

public class AccesoJSON : AccesoADatos
{
    public AccesoJSON(string archivoCadetes, string archivoCadeterias) : base(archivoCadetes, archivoCadeterias)
    {
        // Constructor de AccesoJSON
    }

    public override List<Cadete> CargarCadetes()
    {
        List<Cadete> cadetes = new List<Cadete>();

        try
        {
            if (File.Exists(archivoCadetes))
            {
                var json = File.ReadAllText(archivoCadetes);
                cadetes = JsonSerializer.Deserialize<List<Cadete>>(json);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar datos desde archivo JSON: {ex.Message}");
        }

        return cadetes;
    }

    public override List<Cadeteria> CargarCadeterias(List<Cadete> listadoCadete)
    {
        List<Cadeteria> cadeterias = new List<Cadeteria>();

        try
        {
            if (File.Exists(archivoCadeterias))
            {
                var json = File.ReadAllText(archivoCadeterias);
                cadeterias = JsonSerializer.Deserialize<List<Cadeteria>>(json);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar datos desde archivo JSON: {ex.Message}");
        }

        return cadeterias;
    }

    public override void GuardarNuevoCadete(int id, string nombre, string direccion, string telefono)
    {
        // Implementa aquí la lógica para guardar un nuevo cadete en formato JSON
        try
        {
            // Cargar la lista actual de cadetes desde el archivo JSON si existe
            var cadetes = CargarCadetes();

            // Crear una nueva instancia de Cadete
            var nuevoCadete = new Cadete(id, nombre, direccion, telefono);

            // Agregar el nuevo cadete a la lista
            cadetes.Add(nuevoCadete);

            // Serializar la lista de cadetes a formato JSON
            var json = JsonSerializer.Serialize(cadetes);

            // Guardar la lista actualizada en el archivo JSON
            File.WriteAllText(archivoCadetes, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar datos en archivo JSON: {ex.Message}");
        }
    }

    public override void GuardarNuevaCadeteria( string nombre, string telefono)
    {
        // Implementa aquí la lógica para guardar una nueva cadetería en formato JSON
        try
        {
            // Cargar la lista actual de cadeterías desde el archivo JSON si existe
            var cadeterias = CargarCadeterias(null); // Puedes pasar null para listadoCadete porque no lo usamos aquí

            // Crear una nueva instancia de Cadetería
            var nuevaCadeteria = new Cadeteria(nombre, telefono);

            // Agregar la nueva cadetería a la lista
            cadeterias.Add(nuevaCadeteria);

            // Serializar la lista de cadeterías a formato JSON
            var json = JsonSerializer.Serialize(cadeterias);

            // Guardar la lista actualizada en el archivo JSON
            File.WriteAllText(archivoCadeterias, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar datos en archivo JSON: {ex.Message}");
        }
    }
}