using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace ListaCompraRedisCache.Models
{
    public class ModeloAlimentos
    {
        XDocument docxml;
        
        public ModeloAlimentos(String uri)
        {
            docxml = XDocument.Load(uri);
        }

        public List<Alimentos> GetAlimentos()
        {
            var consulta = from datos in docxml.Descendants("producto")
                           select new Alimentos
                           {
                               IdAlimento = int.Parse(datos.Element("idalimento").Value),
                               Nombre = datos.Element("nombre").Value,
                               Descripcion = datos.Element("descripcion").Value,
                               Precio = int.Parse(datos.Element("precio").Value),
                               Imagen = datos.Element("imagen").Value
                           };
            return consulta.ToList();
        }
        
        public Alimentos BuscarAlimento(String idalimento)
        {
            var consulta = from datos in docxml.Descendants("producto")
                           where datos.Element("idalimento").Value == idalimento
                           select new Alimentos
                           {
                               IdAlimento = int.Parse(datos.Element("idalimento").Value),
                               Nombre = datos.Element("nombre").Value,
                               Descripcion = datos.Element("descripcion").Value,
                               Precio = int.Parse(datos.Element("precio").Value),
                               Imagen = datos.Element("imagen").Value
                           };
            return consulta.FirstOrDefault();
        }
    }
}