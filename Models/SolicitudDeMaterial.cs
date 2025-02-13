namespace Inventario360.Models
{
    public class SolicitudDeMaterial
    {
        public int ID { get; set; }
        public int ITEM { get; set; }
        public int Cantidad { get; set; }
        public string NombreTecnico { get; set; }
        public string Medida { get; set; }
        public string UnidadMedida { get; set; }
        public string Marca { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public int Producto { get; set; }
        public string PosibleProveedor { get; set; }
    }
}
