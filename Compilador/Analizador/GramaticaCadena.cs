using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace Programa_de_Arboles.sol.analizador
{
    class GramaticaCadena : Grammar
    {
        public GramaticaCadena() : base(caseSensitive: false)
        {
            CommentTerminal cadena = new CommentTerminal("string", "\"", ".", "\"");

            #region No Terminales
            NonTerminal S = new NonTerminal("S");
            #endregion

            #region Gramatica
            //Gramatica ambigua:
            S.Rule = cadena;
            #endregion

            #region Preferencias
            this.Root = S;
            #endregion
        }
    }
}