using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace Compilador.Analizador
{
    class GramaticaDecimal: Grammar
    {
        public GramaticaDecimal() : base(caseSensitive: false)
        {
            #region ER
            RegexBasedTerminal numero = new RegexBasedTerminal("numero", "-?[0-9]+(\\?[0-9])?");
            RegexBasedTerminal numerodecimal = new RegexBasedTerminal("decimal", "[0-9]+[.][0-9]+");
            IdentifierTerminal id = new IdentifierTerminal("id");
            #endregion

            #region Terminales
            var mas = ToTerm("+");
            var menos = ToTerm("-");
            var por = ToTerm("*");
            var div = ToTerm("/");
            var pot = ToTerm("^");

            var reservedint = ToTerm("int");
            var reservedpuntoycoma = ToTerm(";");
            #endregion

            #region No Terminales
            NonTerminal S = new NonTerminal("S"),
                E = new NonTerminal("E"),
                T = new NonTerminal("T"),
                G = new NonTerminal("G"),
                F = new NonTerminal("F"),
        DECLARACION = new NonTerminal("DECLARACION");
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
                | numero
                | id;

            DECLARACION.Rule = reservedint + id + reservedpuntoycoma;
            DECLARACION.ErrorRule = SyntaxError + reservedpuntoycoma;

            //Gramatica no ambigua:
            /*S.Rule = E;

            E.Rule = E + mas + T
                | E + menos + T
                | T;

            T.Rule = T + por + T
                | T + div + T
               // | a + T + c 
                | a + T + div + T + c
                | a + T + por + T + c
                | G;

            G.Rule = G + pot + F
                | F;

            F.Rule = id
                | numero
                | numerodecimal;*/
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
