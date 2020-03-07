using System;
using System.Collections.Generic;
using System.Text;
using RegistroDetalle.Entidades;
using RegistroDetalle.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace RegistroDetalle.BLL
{
    public class PersonasBLL
    {
        public static bool Guardar(Persona persona)
        {
            Contexto db = new Contexto();
            bool paso = false;

            try
            {
                if (db.Personas.Add(persona) != null)
                    paso = db.SaveChanges() > 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
            return paso;
        }

        public static bool Modificar(Persona persona)
        {
            Contexto db = new Contexto();
            bool paso = false;

            try
            {//buscar entidades que no estan para removerlas
                var Anterior = PersonasBLL.Buscar(persona.PersonaId);

                //para borrar de la db los telefonos que ya no existen
                foreach (var item in Anterior.Telefonos)
                {
                    if (!persona.Telefonos.Exists(t => t.Id == item.Id))
                        db.Entry(item).State = EntityState.Deleted;
                }

                //para agregar o modificar los nuevos telefonos de la persona
                foreach(var item in persona.Telefonos)
                {
                    var estado = item.Id > 0 ? EntityState.Modified : EntityState.Added;
                    db.Entry(item).State = estado;
                }

                db.Entry(persona).State = EntityState.Modified;
                paso = db.SaveChanges() > 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
            return paso;
        }

        public static bool Eliminar(int id)
        {
            Contexto db = new Contexto();
            bool paso = false;

            try
            {
                var eliminar = PersonasBLL.Buscar(id);
                db.Entry(eliminar).State = EntityState.Deleted;
                paso = db.SaveChanges() > 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
            return paso;
        }

        public static Persona Buscar(int id)
        {
            Contexto db = new Contexto();
            Persona persona = new Persona();

            try
            {
                persona = db.Personas.Include(t => t.Telefonos).Where(p => p.PersonaId == id).SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
            return persona;
        }

        public static List<Persona> GetList(Expression<Func<Persona, bool>> persona)
        {
            Contexto db = new Contexto();
            List<Persona> Lista = new List<Persona>();

            try
            {
                Lista = db.Personas.Where(persona).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
            return Lista;
        }
    }
}
