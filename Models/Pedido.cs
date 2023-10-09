namespace Cadeterias;


public enum Estados
{
    Pendiente, Entregado
}
public class Pedido
{
    private int nro;
    private string obs;
    private Cliente nuevo=null;
    private Estados estado ;
    private int idCadete=-1;

    
    public int Nro{get=> nro; set=> nro=value;}
    public Estados Estado { get => estado; set => estado = value; }
    public int IdCadete { get => idCadete; set => idCadete = value; }
    public string Obs { get => obs; set => obs = value; }

    public Pedido(int numero, string observacion, string nom, string dir, int tel, string refe){
        nro=numero;
        Obs=observacion;
        Estado=Estados.Pendiente;
        nuevo=new Cliente(nom, dir, tel, refe);
    }

    public string VerDireccionCliente(){
        string dire = nuevo.VerDireccion();
        return dire;
    }
    public string VerDatosCliente(){
        string datos = nuevo.VerDatos();
        return datos;
    }


    public void CambiarEstado(){
        if (Estado== Estados.Pendiente)
        {
            Estado=Estados.Entregado;
        }else
        {
            Estado=Estados.Pendiente;
        }
    }
}