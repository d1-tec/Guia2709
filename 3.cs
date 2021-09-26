using System;
using Dominio;
using Persistencia;

namespace Logica
{
    public class ControladorPizarron
    {
        private Repositorio repositorio;

        public ControladorPizarron(Repositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        public void AgregarPizarron(Pizarron pizarron)
        {
            if (ValidarPizarron(pizarron))
            {
                repositorio.AgregarPizarronAlRepositorio(pizarron);
            }
        }

        public int CantidadPizarronesRegistrados()
        {
            return repositorio.CantidadPizarronesRepositorio();
        }

        public bool ValidarPizarron(Pizarron unPizarron)
        {
            bool esValido = false;
            if (!(EsPizarronNull(unPizarron)) && Utilidades.EsStringValido(unPizarron.Nombre) && Utilidades.ValidarEquipo(unPizarron.Equipo) && Utilidades.ValidarUsuario(unPizarron.UsuarioCreador) && Utilidades.EsStringValido(unPizarron.Descripcion) && Utilidades.NumeroMayorQueCero(unPizarron.Altura) && Utilidades.NumeroMayorQueCero(unPizarron.Ancho) && ValidarFecha(unPizarron.FechaMod))
            {
                esValido = true;
            }

            return esValido;
        }

        public bool ValidarFecha(DateTime fecha)
        {
            bool esValida = true;

            if (fecha == null)
            {
                esValida = false;
            }
            return esValida;
        }
        public bool EsPizarronNull(Pizarron unPizarron)
        {
            if (unPizarron == null)
            {
                throw new System.InvalidOperationException("El pizarron no puede ser null");
            }
            return false;
        }

        public Pizarron ObtenerPizarron(Pizarron pizarron)
        {
            if (ValidarOperacionSobreListaPizarrones(pizarron))
            {
                return repositorio.ObtenerPizarronDelRepositorio(pizarron);
            }
            else
            {
                throw new System.InvalidOperationException("No se puede realizar la operacion");
            }

        }

       

        public bool EsListaPizarronesVacia()
        {
            return repositorio.EsListaPizarronesVacia();
        }

        public bool ValidarOperacionSobreListaPizarrones(Pizarron pizarron)
        {
            if (EsListaPizarronesVacia())
            {
                throw new System.InvalidOperationException("No hay pizarrones registrados");
            }
            else if (pizarron == null || !(ExistePizarron(pizarron)))
            {
                throw new System.InvalidOperationException("El pizarron no existe");

            }
            return true;

        }

        public bool ExistePizarron(Pizarron pizarron)
        {
            bool existe = true;
            if (repositorio.pizarrones.IndexOf(pizarron) == -1)
            {
                existe = false;
            }
            return existe;

        }


        public void AgregarElementoTextoAPizarron(ElementoTexto unElemento, Pizarron unPizarron)
        {
            if (Utilidades.ValidarElemento(unElemento, unPizarron) && Utilidades.validarElementoTexto(unElemento))
            {
                unPizarron.ListaElementoTexto.Add(unElemento);
            }
        }

        public void EliminarElementoTexto(Pizarron unPizarron, ElementoTexto unElemento)
        {
            if (CantidadElementosTextoEnPizarron(unPizarron)>0 && Utilidades.ValidarElemento(unElemento, unPizarron) && Utilidades.validarElementoTexto(unElemento))
            {
                unPizarron.ListaElementoTexto.Remove(unElemento);
            }
        }
        //faltan pruebas de este
        public void EliminarElementoImagen(Pizarron unPizarron, ElementoImagen unElemento)
        {
            if (CantidadElementosImagenesEnPizarron(unPizarron) > 0 && Utilidades.ValidarElemento(unElemento, unPizarron) && Utilidades.validarElementoImagen(unElemento))
            {
                unPizarron.ListaElementoImagen.Remove(unElemento);
            }
        }


        public int CantidadElementosTextoEnPizarron(Pizarron unPizarron)
        {

            return unPizarron.ListaElementoTexto.Count;
        }
        //faltan prubas ade este
        public int CantidadElementosImagenesEnPizarron(Pizarron unPizarron)
        {

            return unPizarron.ListaElementoImagen.Count;
        }

        public void EliminarPizarron(Pizarron pizarron)
        {
            if (ValidarOperacionSobreListaPizarrones(pizarron))
            {
                repositorio.EliminarPizarronDelRepositorio(pizarron);
            }
        }


        public Elemento ObtenerElemento(Pizarron pizarron, ElementoTexto unElemento)
        {
            return pizarron.ListaElementoTexto.Find(elemento => elemento.Equals(unElemento));
        }

        //FALTAN PRUEBAS DE ESTE METODO
        public ElementoImagen ObtenerElementoImagenPorNombre(Pizarron pizarron, string name)
        {
            return pizarron.ListaElementoImagen.Find(elemento => elemento.Nombre.Equals(name));
        }
        public ElementoTexto ObtenerElementoPorNombre(Pizarron pizarron, string name)
        {
            return pizarron.ListaElementoTexto.Find(elemento => elemento.Nombre.Equals(name));
        }

        public void ModificarAnchoPizarron(Pizarron pizarron, int ancho)
        {
            if (Utilidades.NumeroMayorQueCero(ancho))
            {
                pizarron.Ancho = ancho;
            }
        }

        public void ModificarAlturaPizarron(Pizarron pizarron, int altura)
        {
            if (Utilidades.NumeroMayorQueCero(altura))
            {
                pizarron.Altura = altura;
            }
        }

        public void ModificarDescripcionPizarron(Pizarron pizarron, string descripcion)
        {
            if (Utilidades.EsStringValido(descripcion))
            {
                pizarron.Descripcion = descripcion;
            }
        }

        //FALTAN PRUEBAS DE ESTE METODO
        public void ModificarNombrePizarron(Pizarron pizarron, string nombre)
        {
            if (Utilidades.EsStringValido(nombre))
            {
                pizarron.Nombre = nombre;
            }
        }

        public void ModificarFechaModPizarron(Pizarron pizarron, DateTime fechaNueva)
        {
            pizarron.FechaMod = fechaNueva;
        }

        public int MinimoNuevoAnchoPosible(Pizarron unPizarron)
        {
            int retorno = 0;
            int maximoX = 0;
            int anchoElemento = 0;
            if (unPizarron.ListaElementoTexto.Count == 0)
            {
                retorno = 500;
            }else
            {
                
                foreach(Elemento e in unPizarron.ListaElementoTexto)
                {
                    if (e.PuntoX > maximoX)
                    {
                        maximoX = e.PuntoX;
                        anchoElemento = e.Ancho;
                    }else if(e.PuntoX == maximoX)
                    {
                        if (e.Ancho > anchoElemento)
                        {
                            anchoElemento = e.Ancho;
                        }
                    }
                }
                retorno = maximoX + anchoElemento;
            }
            return retorno;
        }
        public int MinimoNuevoAltaPosible(Pizarron unPizarron)
        {
            int retorno = 0;
            int maximoY = 0;
            int altoElemento = 0;
            if (unPizarron.ListaElementoTexto.Count == 0)
            {
                retorno = 500;
            }
            else
            {

                foreach (Elemento e in unPizarron.ListaElementoTexto)
                {
                    if (e.PuntoY > maximoY)
                    {
                        maximoY = e.PuntoY;
                        altoElemento = e.Altura;
                    }
                    else if (e.PuntoY == maximoY)
                    {
                        if (e.Altura > altoElemento)
                        {
                            altoElemento = e.Altura;
                        }
                    }
                }
                retorno = maximoY + altoElemento;
            }
            return retorno;
        }
        //FALTAN PRUEBAS DE ESTE METODO

        public void AgregarElementoImagenAPizarron(ElementoImagen unElemento, Pizarron unPizarron)
        {
            if (Utilidades.ValidarElemento(unElemento, unPizarron) && Utilidades.validarElementoImagen(unElemento))
            {
                unPizarron.ListaElementoImagen.Add(unElemento);
            }
        }
    }
}