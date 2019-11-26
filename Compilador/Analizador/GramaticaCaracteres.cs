using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
namespace Programa_de_Arboles.sol.analizador
{
    class GramaticaCaracteres : Grammar
    {
        public GramaticaCaracteres() : base(caseSensitive: false)
        {
            #region ER
            CommentTerminal r_char = new CommentTerminal("caracteres", "'", ".", "'"); // es un caracter char
            #endregion

            #region No Terminales
            NonTerminal S = new NonTerminal("S"),
                E = new NonTerminal("E");
            #endregion

            #region Gramatica
            //Gramatica ambigua:
            S.Rule = E;
            E.Rule = r_char;
            #endregion

            #region Preferencias
            this.Root = S;
            #endregion

        }
    }
}