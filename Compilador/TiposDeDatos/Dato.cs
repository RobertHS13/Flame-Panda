using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.TiposDeDatos
{
    public class Dato
    {
        string tipo = "";
        string identificador = "";
        string valor = "";
        string pertenece = "";

        public Dato()
        {

        }

        public Dato(string tipo, string identificador)
        {
            this.tipo = tipo;
            this.Identificador = identificador;
        }

        public Dato(string tipo, string identificador, string valor)
        {
            this.tipo = tipo;
            this.identificador = identificador;
            this.Valor = valor;
        }



        public string Tipo { get => tipo; set => tipo = value; }
        public string Identificador { get => identificador; set => identificador = value; }
        public string Valor { get => valor; set => valor = value; }

        public override bool Equals(object obj)
        {
            return obj is Dato dato &&
                   Identificador == dato.Identificador;
        }
        override
        public string ToString()
        {
            return "Tipo: " + Tipo + "\nIdentificador: " + Identificador + "\nValor: " + Valor;
        }
    }
}