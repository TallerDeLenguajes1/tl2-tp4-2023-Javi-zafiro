namespace Cadeterias;


public class Cliente
{
    private string nombre;
    private string direccion;
    private int telefono;
    private string referenciaDireccion;

    public Cliente (string nom, string dir, int tel, string refe){
        nombre=nom;
        direccion=dir;
        telefono=tel;
        referenciaDireccion=refe;
    }

    public string VerDireccion(){
        string dire= "Direccion: "+direccion+", Referencia: "+referenciaDireccion;
        return dire;
    }

    public string VerDatos(){
        string datos= "Nombre: "+nombre+", Telefono: "+telefono;
        return datos;
    }
}