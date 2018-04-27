using ListaCompraRedisCache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ListaCompraRedisCache.Controllers
{
    public class AlimentosController : Controller
    {
        ModeloAlimentos modelo;
        
        public void CrearModelo()
        {
            Uri uri = HttpContext.Request.Url;
            String rutauri = uri.Scheme + "://" + uri.Authority
                + "/Documentos/comida.xml";
            modelo = new ModeloAlimentos(rutauri);
        }

        // GET: Alimentos
        public ActionResult Index()
        {
            this.CrearModelo();
            List<Alimentos> lista = modelo.GetAlimentos();
            return View(lista);
        }

        public ActionResult Detalles(String idalimento)      
        {
            if (TempData["PRODUCTO"] == null)
            {
                this.CrearModelo();
                Alimentos alimento = modelo.BuscarAlimento(idalimento);
                return View(alimento);
            }
            else
            {
                Alimentos alimento = (Alimentos)TempData["PRODUCTO"];
                ViewBag.Mensaje =
                    TempData["MENSAJE"];
                return View(alimento);
            }

            
        }

        public ActionResult MisFavoritos(String idalimento)
        {
            this.CrearModelo();
            ModeloRedisCache modeloredis = new ModeloRedisCache();
            Alimentos alimento = modeloredis.BuscarAlimentoRedis(idalimento);
            if (alimento == null)
            {
                alimento = modelo.BuscarAlimento(idalimento);
                modeloredis.AlmacenarAlimento(alimento);
                return RedirectToAction("Index");
            }
            else     
            {
                TempData["MENSAJE"] =
                    "El producto ya está marcado como favorito";
                TempData["PRODUCTO"] = alimento;
                return RedirectToAction("Detalles", alimento);
            }
        }
        
        public ActionResult ProductosFavoritos()
        {
            ModeloRedisCache modeloredis = new ModeloRedisCache();
            List<Alimentos> listaproductos = 
                modeloredis.GetAlimentosCacheRedis();
            if (listaproductos == null)
            {
                ViewBag.Mensaje = "No existen productos en favoritos actualmente";
                return View();
            }
            else
            {
                return View(listaproductos);
            }
        }
        
        public ActionResult LimpiarFavoritos()
        {
            ModeloRedisCache modeloredis = new ModeloRedisCache();
            modeloredis.LimpiarCacheRedis();
            return RedirectToAction("ProductosFavoritos");
        }
    }
}