using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
namespace Programa_de_Arboles.sol.analizador
{
    class GramaticaEnteros : Grammar
    {
        public GramaticaEnteros() : base(caseSensitive: false)
        {
            #region ER
            RegexBasedTerminal numero = new RegexBasedTerminal("numero", "-?[0-9]+(\\?[0-9])?");
            #endregion

            #region Terminales
            var mas = ToTerm("+");
            var menos = ToTerm("-");
            var por = ToTerm("*");
            var div = ToTerm("/");
            var pot = ToTerm("^");
            #endregion

            #region No Terminales
            NonTerminal S = new NonTerminal("S"),
                E = new NonTerminal("E");
            #endregion


            #region Gramatica
            //Gramatica ambigua:
            S.Rule = E;
            E.Rule = E + mas + E
                | E + menos + E
                | E + por + E
                | E + div + E
                | E + pot + E
                | ToTerm("(") + E + ToTerm(")")
                | numero;

            #endregion

            #region Preferencias
            this.Root = S;
            this.RegisterOperators(20, Associativity.Left, mas, menos);
            this.RegisterOperators(30, Associativity.Left, por, div);
            this.RegisterOperators(40, Associativity.Left, pot);
            //this.MarkPunctuation(".");
            #endregion

        }
    }
}