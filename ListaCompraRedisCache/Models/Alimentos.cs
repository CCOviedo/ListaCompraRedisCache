using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ListaCompraRedisCache.Models
{
    public class Alimentos
    {
        public int IdAlimento { get; set; }
        public String Nombre { get; set; }
        public String Descripcion { get; set; }
        public double Precio { get; set; }
        public String Imagen { get; set; }
    }
}