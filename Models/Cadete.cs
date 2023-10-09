using System.IO;
namespace Cadeterias;



public class Cadete
{
    private int id;
    private string nombre;
    private string direccion;
    private string telefono;

    public int Id { get => id; set => id = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Direccion { get => direccion; set => direccion = value; }
    public string Telefono { get => telefono; set => telefono = value; }

    public Cadete(int n, string nom, string dir, string tel)
    {
        Id=n;
        Nombre=nom;
        Direccion=dir;
        Telefono=tel;
    }
    public Cadete()
    {
        Id=-1;
        Nombre="nn";
        Direccion="nn";
        Telefono="000000000";
    }
}
    

