using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class ValuesController : ApiController
    {


        // GET api/values
        public HttpResponseMessage Get()
        {
            try
            {

                using (Models.DBPRUEBASEntities db = new Models.DBPRUEBASEntities()) 
                {
                    var Libreria = db.Libro.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, Libreria);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, ex);
            }




        }

        // GET api/values/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                using (Models.DBPRUEBASEntities db = new Models.DBPRUEBASEntities())
                {
                    var LB = db.Libro.FirstOrDefault(e => e.ID == id);

                    if (LB != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, LB);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Libro con el ID " + id + "no se encontro");
                    }

                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict,ex);
            }


        }

        // POST api/values
        public HttpResponseMessage Post([FromBody] Models.Libro LB)
        {
            try
            {
                using (Models.DBPRUEBASEntities db = new Models.DBPRUEBASEntities())
                {
                    db.Libro.Add(LB);
                    db.SaveChanges();
                    var mostrar = Request.CreateResponse(HttpStatusCode.Created, LB);
                    mostrar.Headers.Location = new Uri(Request.RequestUri + LB.ID.ToString());
                    return mostrar;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

       

        // PUT api/values/5
        public HttpResponseMessage Put(int id, [FromBody] Models.Libro LIBO)
        {
            try
            {
                using (Models.DBPRUEBASEntities db = new Models.DBPRUEBASEntities())
                {
                    var LB = db.Libro.FirstOrDefault(e => e.ID == id);
                    if (LB == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Libro con el ID " + id + "no se encontro");
                    }
                    else
                    {
                        LB.Titulo = LIBO.Titulo;
                        LB.Paginas = LIBO.Paginas;
                        LB.Precio = LIBO.Precio;
                        LB.Publicacion = LIBO.Publicacion;
                        LB.Autor = LIBO.Autor;
                        LB.Categoria = LIBO.Categoria;
                        LB.Editorial = LIBO.Editorial;
                        LB.Existencias = LIBO.Existencias;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, LB);
                    }
              

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex);
            }

        }

        // DELETE api/values/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (Models.DBPRUEBASEntities db = new Models.DBPRUEBASEntities())
                {
                    var LB = db.Libro.FirstOrDefault(e => e.ID == id);
                    if (LB == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Libro con el ID " + id + "no se encontro");
                    }
                    else
                    {
                        db.Libro.Remove(db.Libro.FirstOrDefault(e => e.ID == id));
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        public HttpResponseMessage Delete()
        {
            try
            {
                using (Models.DBPRUEBASEntities db = new Models.DBPRUEBASEntities())
                {
                    var Libreria = db.Libro.ToList();
                    foreach (Models.Libro LB in Libreria)
                    {
                        
                        db.Libro.Remove(db.Libro.FirstOrDefault(e => e.ID == LB.ID));
                    }

                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
