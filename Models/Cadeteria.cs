namespace Cadeterias;


public class Cadeteria
{
    
    private static Cadeteria cadeteria;
    private string nombre;
    private string telefono;
    private List<Cadete> listaCadetes = null;
    private List<Pedido> listaPedidos = null;
    private int nroUltimoPedido;


    public string Nombre { get => nombre; set => nombre = value; }
    public int NroUltimoPedido { get => nroUltimoPedido; set => nroUltimoPedido = value; }
    internal List<Cadete> ListaCadetes { get => listaCadetes; set => listaCadetes = value; }
    internal List<Pedido> ListaPedidos { get => listaPedidos; set => listaPedidos = value; }
    public string Telefono { get => telefono; set => telefono = value; }

    public Cadeteria(string nom, string tel)
    {
        Nombre=nom;
        Telefono = tel;
        ListaCadetes = new List<Cadete>();
        ListaPedidos = new List<Pedido>();
        NroUltimoPedido = 0;
    }
    public Cadeteria(){
        Nombre="Default";
        Telefono="----------";
        ListaCadetes = new List<Cadete>();
        ListaPedidos = new List<Pedido>();
        NroUltimoPedido = 0;
    }

    public static Cadeteria GetCadeteria(){
        
        if (cadeteria==null)
        {
            cadeteria=new Cadeteria();
        }
        return cadeteria;
    }

    public List<Cadete> ListadoCadetes(){
        return listaCadetes;
    }
    public Cadete AgregarCadete(string nombre, string telefono, string direccion){
        int n = ListaCadetes.Count;
        Cadete nuevo = new Cadete(n++, nombre, direccion, telefono);
        ListaCadetes.Add(nuevo);

        return nuevo;
    }

    public Pedido CreaPedido( string obrservacion, string nom, string dir, int tel, string refe){
        int nro = NroUltimoPedido+1;
        Pedido nuevo = new Pedido(nro, obrservacion, nom, dir, tel, refe);
        NroUltimoPedido++;
        return nuevo;
    }

    public void AsignarCadeteAPedido(int idPedido, int idCadete){
        Cadete cadete =  ListaCadetes.FirstOrDefault(l=> l.Id == idCadete);
        if (cadete != null)
        {
            Pedido pedido = ListaPedidos.FirstOrDefault(p=> p.Nro == idPedido);
            pedido.IdCadete=idCadete;
        }else
        {
            Console.WriteLine("error cadete inexistente");
        }
    }

    public void MoverPedido(int idCadeteRemover, int idCadeteAsignar, int nroPedido){

        Cadete cadete1 =  ListaCadetes.FirstOrDefault(l=> l.Id == idCadeteRemover);
        Cadete cadete2 =  ListaCadetes.FirstOrDefault(l=> l.Id == idCadeteAsignar);
        if (cadete1 != null && cadete2 !=null)
        {
            Pedido mover = ListaPedidos.FirstOrDefault(p=> p.Nro == nroPedido);
            if (mover != null)
            {
                mover.IdCadete=idCadeteAsignar;
            }else
            {
                Console.WriteLine("error pedido inexistente");
            }
        }else
        {
            Console.WriteLine("error cadete inexistente");
        }

    }
    public void CambiarEstadoPedido(int idpedido){
        Pedido cambiar = ListaPedidos.FirstOrDefault(p=> p.Nro == idpedido);
        cambiar.CambiarEstado();
    }
    private void asignarLista(List<Cadete> list){
        ListaCadetes=list;
    }
   

    public float JornalACobrar(int idcadete){
        float total=0;
        int cantPedidos=0;
        foreach (var item in listaPedidos)
        {
            if (item.IdCadete==idcadete && item.Estado==Estados.Entregado)
            {
                cantPedidos++;
            }
        }
        total=cantPedidos*500;
        return total;
    }
}