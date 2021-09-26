using ###.DataAccess;
using ###.Dominio;
using ###.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ###.BusinessLogic.Dominio
{
    public class Controlador
    {
        private iAccesoDatos accesoDatos;
        public Controlador()
        {
            accesoDatos = new AccesoDatos();
        }

        private void AgregarEntidad(Entidad unaEntidad)
        {
            this.accesoDatos.AgregarEntidad(unaEntidad);
        }

        public void RegistrarEntidad(string unTextoIngresado)
        {
            string unNombreEntidad = unTextoIngresado.ToLower();
            if (unNombreEntidad.Length == 0) 
                throw new ArgumentOutOfRangeException("La entidad esta vacio");
            Entidad unaEntidad = new Entidad(unNombreEntidad);
            if (GetListaEntidades().Contains(unaEntidad))
                throw new ArgumentOutOfRangeException("La entidad ya existe");
            AgregarEntidad(unaEntidad);
            Trace.WriteLine("Se crea la entidad con nombre " + unaEntidad.Nombre);
        }

        public List<AlarmaDeAutor> GetListaAlarmasDeAutores()
        {
            return accesoDatos.GetListaAlarmasDeAutores();
        }

        public List<AlarmaDeEntidad> GetListaAlarmasConEntidades()
        {
            return accesoDatos.GetListaAlarmasConEntidades();
        }

        public List<Autor> GetAutoresAlarma(AlarmaDeAutor unaAlarma)
        {
            return accesoDatos.GetAutoresAlarma(unaAlarma);
        }

        public double GetPorcentajeFrasesPositivasAutor(Autor unAutor)
        {
            double countFrasesDeAutor = 0;
            double countFrasesPositivasDeAutor = 0;
            foreach (Frase unaFrase in GetListaFrasesConAutores())
            {
                if (unAutor.Equals(unaFrase.Autor))
                {
                    if (unaFrase.SuCategoria == Categoria.Positiva)
                        countFrasesPositivasDeAutor++;
                    countFrasesDeAutor++;
                }
            }
            if (countFrasesDeAutor != 0)
                return (countFrasesPositivasDeAutor / countFrasesDeAutor) * 100;
            else return 0;
        }

        public List<Frase> GetListaFrasesConEntidad()
        {
            return this.accesoDatos.GetListaFrasesConEntidad();
        }

        public List<Autor> GetListaAutores()
        {
            return this.accesoDatos.GetListaAutores();
        }

        public List<Sentimiento> GetListaSentimientos()
        {
            return this.accesoDatos.GetListaSentimientos();
        }

        public double GetPorcentajeFrasesNegativasAutor(Autor unAutor)
        {
            double countFrasesDeAutor = 0;
            double countFrasesPositivasDeAutor = 0;
            foreach (Frase unaFrase in GetListaFrasesConAutores())
            {
                if (unAutor.Equals(unaFrase.Autor))
                {
                    if (unaFrase.SuCategoria == Categoria.Negativa)
                        countFrasesPositivasDeAutor++;
                    countFrasesDeAutor++;
                }
            }
            if (countFrasesDeAutor != 0)
                return (countFrasesPositivasDeAutor / countFrasesDeAutor) * 100;
            else return 0;
        }

        public List<Frase> GetListaFrases()
        {
            return this.accesoDatos.GetListaFrases();
        }

        public void EliminarAutor(string unNombreUsuario)
        {
            if (unNombreUsuario.Length == 0) 
                throw new ArgumentOutOfRangeException("El nombre de usuario esta vacio");
            Autor autorParaPasarPorParametro = new Autor();
            autorParaPasarPorParametro.NombreUsuario = unNombreUsuario;
            if (EsAutorRepetido(autorParaPasarPorParametro))
                BorrarAutorConTodasSusFrases(autorParaPasarPorParametro);
            else 
                throw new ArgumentOutOfRangeException("El nombre de usuario no se ecuentra en la lista de usuarios");
            ActualizarAlarmas();
        }

        public double GetPromedioFrasesDiarioAutor(Autor unAutor)
        {
            double countFrasesDeAutor = 0;
            DateTime masAntigua = DateTime.Now;
            foreach (Frase unaFrase in GetListaFrasesConAutores())
            {
                if (unAutor.Equals(unaFrase.Autor))
                {
                    if (masAntigua.CompareTo(unaFrase.Fecha) > 0)
                        masAntigua = unaFrase.Fecha;
                    countFrasesDeAutor++;
                }
            }
            int diasTotalesEntrePrimerYUltimaFrase = (DateTime.Now - masAntigua).Days;
            if (countFrasesDeAutor == 0) 
                return 0;
            if (diasTotalesEntrePrimerYUltimaFrase == 0) 
                return countFrasesDeAutor;
            return (countFrasesDeAutor / diasTotalesEntrePrimerYUltimaFrase);
        }

        public double GetCantidadEntidadesMencionadasPorAutor(Autor unAutor)
        {
            List<Entidad> unaListaEntidades = new List<Entidad>();
            foreach (Frase unaFrase in GetListaFrasesConEntidad())
            {
                if (unAutor.Equals(unaFrase.Autor))
                    if (!unaListaEntidades.Contains(unaFrase.EntidadRelacionada))
                        unaListaEntidades.Add(unaFrase.EntidadRelacionada);
            }
            return unaListaEntidades.Count();
        }

        private void BorrarAutorConTodasSusFrases(Autor unAutor)
        {
            BorrarFrasesAutor(unAutor);
            BorrarAutor(unAutor);
        }

        private void BorrarAutor(Autor unAutor)
        {
            foreach (Autor otroAutor in GetListaAutores())
            {
                if (otroAutor.Equals(unAutor)) 
                    unAutor = otroAutor;
            }
            EliminarAutor(unAutor);
        }

        private void EliminarAutor(Autor unAutor)
        {
            this.accesoDatos.EliminarAutor(unAutor);
        }

        private void BorrarFrasesAutor(Autor unAutor)
        {
            this.accesoDatos.BorrarFrasesAutor(unAutor);
        }

        public void ActualizarAutor(Autor unAutor, Autor autorAntiguo)
        {
            accesoDatos.ActualizarAutor(unAutor, autorAntiguo);
            ActualizarAlarmas();
        }

        public List<Frase> GetListaFrasesConAutores()
        {
            return this.accesoDatos.GetListaFrasesConAutores();
        }

        public List<AlarmaDeEntidad> GetListaAlarmasEntidad()
        {
            return this.accesoDatos.GetListaAlarmasDeEntidades();
        }

        public List<Entidad> GetListaEntidades()
        {
            return this.accesoDatos.GetListaEntidades();
        }

        private void AgregarFrase(Frase unaFrase)
        {
            this.accesoDatos.AgregarFrase(unaFrase);
        }

        public void RegistrarFrase(string unTextoIngresado, DateTime unaFecha, Autor unAutorSelec)
        {
            string unTextoFrase = unTextoIngresado;
            if (unTextoFrase.Length == 0) 
                throw new ArgumentOutOfRangeException("El texto esta vacio");
            if (!EsFechaCorrecta(unaFecha)) 
                throw new ArgumentOutOfRangeException("La fecha no es correcta");
            Frase unaFrase = new Frase(unTextoFrase, unaFecha);
            unaFrase.ProcesarFrase(GetListaEntidades(), GetListaSentimientos());
            unaFrase.Autor = BuscarAutorAPartirDeNombreUsuario(unAutorSelec);
            AgregarFrase(unaFrase);
            ActualizarAlarmas();
            Trace.WriteLine("Se crea la frase con texto " + unaFrase.Texto);
        }

        private Autor BuscarAutorAPartirDeNombreUsuario(Autor unAutorSelec)
        {
            Autor autorDeseado = null;
            foreach (Autor unAutor in GetListaAutores())
            {
                if (unAutor.Equals(unAutorSelec)) 
                    autorDeseado = unAutor;
            }
            return autorDeseado;
        }

        public void RegistrarAutor(Autor unAutor)
        {
            if (unAutor.Validarse())
            {
                if (!EsAutorRepetido(unAutor))
                    AgregarAutor(unAutor);
                else 
                    throw new ArgumentOutOfRangeException("El autor ya existe.");
            }
            else
                throw new ArgumentOutOfRangeException("No se pudo crear el autor.");
        }

        private void AgregarAutor(Autor unAutor)
        {
            this.accesoDatos.AgregarAutor(unAutor);
        }

        private bool EsAutorRepetido(Autor unAutor)
        {
            foreach (Autor aut in GetListaAutores())
                if (aut.Equals(unAutor)) 
                    return true;
            return false;
        }

        private void ActualizarAlarmas()
        {
            foreach (AlarmaDeEntidad unaAlarma in GetListaAlarmasConEntidades())
            {
                accesoDatos.ActualizarAlarmaDeEntidad(unaAlarma);
            }
            foreach (AlarmaDeAutor unaAlarma in GetListaAlarmasAutor())
            {
                accesoDatos.ActualizarAlarmaDeAutor(unaAlarma);
            }
        }

        private bool EsFechaCorrecta(DateTime unaFecha)
        {
            return (OcurrioEnElUltimoAno(unaFecha)) & (NoEsFuturo(unaFecha));
        }

        private bool NoEsFuturo(DateTime unaFecha)
        {
            return DateTime.Compare(unaFecha, DateTime.Now) <= 0;
        }

        private bool OcurrioEnElUltimoAno(DateTime unaFecha)
        {
            return DateTime.Compare(unaFecha, DateTime.Now.AddYears(-1)) >= 0;
        }

        public void AgregarAlarma(AlarmaDeEntidad unaAlarma)
        {
            if (!EsAlarmaRepetida(unaAlarma))
                this.accesoDatos.AgregarAlarma(unaAlarma);
            else 
                throw new ArgumentOutOfRangeException("La alarma ya existe.");
        }

        public void AgregarAlarma(AlarmaDeAutor unaAlarma)
        {
            if (!EsAlarmaRepetida(unaAlarma))
                this.accesoDatos.AgregarAlarma(unaAlarma);
            else 
                throw new ArgumentOutOfRangeException("La alarma ya existe.");
        }

        private bool EsAlarmaRepetida(iAlarma unaAlarma)
        {
            foreach (AlarmaDeEntidad otraAlarma in GetListaAlarmasConEntidades())
            {
                if (unaAlarma.Equals(otraAlarma)) 
                    return true;
            }
            foreach (AlarmaDeAutor otraAlarma in GetListaAlarmasAutor())
            {
                if (unaAlarma.Equals(otraAlarma)) 
                    return true;
            }
            return false;
        }

        private List<AlarmaDeAutor> GetListaAlarmasAutor()
        {
            return this.accesoDatos.GetListaAlarmasDeAutores();
        }

        private void AgregarSentimiento(Sentimiento unSentimiento)
        {
            this.accesoDatos.AgregarSentimiento(unSentimiento);
        }

        public bool CompararListas<T>(List<T> lista1, List<T> lista2)
        {
            List<T> unaLista = new List<T>(lista1);
            List<T> otraLista = new List<T>(lista2);
            foreach (T elem in unaLista.ToList())
            {
                if (otraLista.Contains(elem))
                {
                    unaLista.Remove(elem);
                    otraLista.Remove(elem);
                }
            }
            return (unaLista.Count == 0) & (otraLista.Count == 0);
        }

        public void RegistrarSentimiento(string unNombreSentimiento, TipoSentimiento unTipo)
        {
            unNombreSentimiento = unNombreSentimiento.ToLower();
            if (unNombreSentimiento.Length == 0)
                throw new ArgumentOutOfRangeException("El sentimiento esta vacio");
            Sentimiento unSentimiento = new Sentimiento(unNombreSentimiento, unTipo);
            if (EsSentimientoRepetido(unSentimiento))
                throw new ArgumentOutOfRangeException("El sentimiento ya existe");
            AgregarSentimiento(unSentimiento);
            Trace.WriteLine("Se crea el Sentimiento con nombre " + unSentimiento.Nombre + " y tipo "
                + unSentimiento.TipoDeSentimiento);
        }

        private bool EsSentimientoRepetido(Sentimiento unSentimiento)
        {
            foreach (Sentimiento otroSentimiento in GetListaSentimientos())
                if (unSentimiento.Nombre.Contains(otroSentimiento.Nombre)
                    | otroSentimiento.Nombre.Contains(unSentimiento.Nombre)) 
                    return true;
            return false;
        }

        public void EliminarSentimiento(string unNombreSentimiento, TipoSentimiento unTipo)
        {
            unNombreSentimiento = unNombreSentimiento.ToLower();
            if (unNombreSentimiento.Length == 0)
                throw new ArgumentOutOfRangeException("El sentimiento esta vacio");
            Sentimiento unSentimiento = new Sentimiento(unNombreSentimiento, unTipo);
            if (GetListaSentimientos().Contains(unSentimiento))
            {
                this.accesoDatos.EliminarSentimiento(unNombreSentimiento, unTipo);
                Trace.WriteLine("Se elimina el Sentimiento con nombre " + unSentimiento.Nombre + " y tipo "
                + unSentimiento.TipoDeSentimiento);
            }
            else
                throw new ArgumentOutOfRangeException("El sentimiento no existe.");
        }
    }
}