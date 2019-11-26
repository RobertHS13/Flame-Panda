using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace Compilador.Analizador
{
    class Gramatica : Grammar
    {
        public Gramatica() : base(caseSensitive: false)
        {
            #region ER
            //RegexBasedTerminal numero = new RegexBasedTerminal("numero", "[0-9]+");
            RegexBasedTerminal numeroentero = new RegexBasedTerminal("entero", "-?[0-9]+(\\?[0-9])?"); // este es un numero entero
            RegexBasedTerminal numerodecimal = new RegexBasedTerminal("decimanl", "[0-9]+[.][0-9]+"); //numero con punto flotante
            IdentifierTerminal id = new IdentifierTerminal("((?:[a-z][a-z0-9_]*))", "Identificador"); // es un identificador
            //RegexBasedTerminal id = new RegexBasedTerminal("((?:[a-z][a-z0-9_]*))", "Identificador");
            CommentTerminal cadena = new CommentTerminal("string", "\"", ".", "\""); // es una cadena String
            CommentTerminal r_char = new CommentTerminal("caracteres", "'", ".", "'"); // es un caracter char
            #endregion

            #region Comentarios del Lenguaje
            CommentTerminal comentarioenlinea = new CommentTerminal("comentario linea", "//", "\n", "\r\n"); // se declara el comentario de una linea
            CommentTerminal comentariomultilinea = new CommentTerminal("comentarioBloque", "/*", "*/"); // se declara el comentario multilinea
            base.NonGrammarTerminals.Add(comentarioenlinea); // se agrega a la lista de terminales que no afectan la gramatica
            base.NonGrammarTerminals.Add(comentariomultilinea); // se agrega a la lista de terminales que no afectan la gramatica
            #endregion

            #region Terminales
            var mas = ToTerm("+");
            var menos = ToTerm("-");
            var por = ToTerm("*");
            var div = ToTerm("/");
            var pot = ToTerm("^");
            var reservedclass = ToTerm("class");
            var reservedpublic = ToTerm("public");
            var reservedprivate = ToTerm("private");
            var reservedprotected = ToTerm("protected");
            var reservedint = ToTerm("int");
            var reserveddouble = ToTerm("double");
            var reservedchar = ToTerm("char");
            var reservedboolean = ToTerm("boolean");
            var reservedbyte = ToTerm("byte");
            var reservedlong = ToTerm("long");
            var reservedfloat = ToTerm("float");
            var reservedshort = ToTerm("short");
            var reservedstring = ToTerm("String");
            var reservedllaveabrir = ToTerm("{");
            var reservedllavecerrar = ToTerm("}");
            var reservedpuntoycoma = ToTerm(";");
            var reservedtrue = ToTerm("true");
            var reservedfalse = ToTerm("false");
            var reservedabrir = ToTerm("(");
            var reservedcerrar = ToTerm(")");
            var igual = ToTerm("=");
            var igualigual = ToTerm("==");
            var diferente = ToTerm("!=");
            var mayor = ToTerm(">");
            var menor = ToTerm("<");
            var menorigual = ToTerm("<=");
            var mayorigual = ToTerm(">=");
            var ror = ToTerm("||");
            var rand = ToTerm("&&");
            var reservedif = ToTerm("if");
            var reservedelse = ToTerm("else");
            var reservedwhile = ToTerm("while");
            var reserveddo = ToTerm("do");
            var reservedifelse = ToTerm("if else");
            var reservedswitch = ToTerm("switch");
            var reservedcase = ToTerm("case");
            var reservedfor = ToTerm("for");
            var reserveddospuntos = ToTerm(":");
            var reservedbreak = ToTerm("break");
            var reserveddefault = ToTerm("default");
            var reservedprint = ToTerm("print");
            #endregion

            #region No Terminales
            NonTerminal OPERACIONESDECIMAL = new NonTerminal("OPERACIONESDECIMAL");
            NonTerminal OPERACIONESPUNTO = new NonTerminal("OPERACIONESPUNTO");
            NonTerminal E = new NonTerminal("E");
            NonTerminal F = new NonTerminal("F");
            NonTerminal SUMACADENA = new NonTerminal("SUMACADENA");
            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal VISIBILIDAD = new NonTerminal("VISIBILIDAD");
            NonTerminal CUERPOCLASE = new NonTerminal("CUERPOCLASE");
            NonTerminal DECLARACION = new NonTerminal("DECLARACION");
            NonTerminal TIPO = new NonTerminal("TIPO");
            NonTerminal CONDICION = new NonTerminal("CONDICION");
            NonTerminal SIGNO = new NonTerminal("SIGNO");
            NonTerminal SICONDICION = new NonTerminal("SICONDICION");
            NonTerminal ELSECONDICION = new NonTerminal("ELSECONDICION");
            NonTerminal ELSEIFCONDICION = new NonTerminal("ELSECONDICION");
            NonTerminal ANIDADO = new NonTerminal("ANIDADO");
            NonTerminal WHILE = new NonTerminal("WHILE");
            NonTerminal DOWHILE = new NonTerminal("DOWHILE");
            NonTerminal FOR = new NonTerminal("FOR");
            NonTerminal MM = new NonTerminal("MM");
            NonTerminal SWITCH = new NonTerminal("SWITCH");
            NonTerminal CASE = new NonTerminal("CASE");
            NonTerminal CUERPOSWITCH = new NonTerminal("CUERPOSWITCH");
            NonTerminal VECTOR = new NonTerminal("VECTOR");
            NonTerminal ASIGNACION = new NonTerminal("ASIGNACION");
            NonTerminal IMPRIMIR = new NonTerminal("IMPRIMIR");
            //NonTerminal SENTENCIASINICIALVACIO = new NonTerminal("SENTENCIASINICIALVACIO");
            //NonTerminal SENTENCIASVACIO = new NonTerminal("SENTENCIASVACIO");
            #endregion

            #region Gramatica
            //Gramatica ambigua
            OPERACIONESDECIMAL.Rule = E;
            E.Rule = E + mas + E
                | E + menos + E
                | E + por + E
                | E + div + E
                | E + pot + E
                | ToTerm("(") + E + ToTerm(")")
                | numeroentero
                | id;
            //| E + "/" + E  **En caso de que no se algo relevante, solo algo que debe venir

            OPERACIONESPUNTO.Rule = F;
            F.Rule = F + mas + F
                | F + menos + F
                | F + por + F
                | F + div + F
                | F + pot + F
                | ToTerm("(") + F + ToTerm(")")
                | numerodecimal
                | id;

            SUMACADENA.Rule = SUMACADENA + mas + SUMACADENA
                | cadena
                | r_char
                | numerodecimal
                | numeroentero
                | id;

            //Public class
            //INICIO.Rule = DECLARACION;
            INICIO.Rule = VISIBILIDAD + reservedclass + id + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar;
            INICIO.ErrorRule = SyntaxError + Eof;

            VISIBILIDAD.Rule = Empty
                            | reservedprivate
                            | reservedprotected
                            | reservedpublic;

            CUERPOCLASE.Rule = Empty
                            | CUERPOCLASE + DECLARACION
                            | CUERPOCLASE + ANIDADO
                            | CUERPOCLASE + WHILE
                            | CUERPOCLASE + DOWHILE
                            | CUERPOCLASE + FOR
                            | CUERPOCLASE + MM + reservedpuntoycoma
                            | CUERPOCLASE + SWITCH
                            | CUERPOCLASE + ASIGNACION
                            | CUERPOCLASE + IMPRIMIR
                            | ANIDADO
                            | DECLARACION
                            | ASIGNACION
                            | WHILE
                            | DOWHILE
                            | FOR
                            | MM + reservedpuntoycoma
                            | SWITCH
                            | IMPRIMIR;
            //CUERPOCLASE.Rule = DECLARACION;                
            //CUERPOCLASE.ErrorRule = SyntaxError + reservedpuntoycoma;

            DECLARACION.Rule = VISIBILIDAD + reservedint + id + reservedpuntoycoma
                            | VISIBILIDAD + reservedint + id + igual + OPERACIONESDECIMAL + reservedpuntoycoma
                            | VISIBILIDAD + reservedlong + reservedint + id + reservedpuntoycoma
                            | VISIBILIDAD + reservedlong + reservedint + id + igual + OPERACIONESDECIMAL + reservedpuntoycoma
                            | VISIBILIDAD + reservedlong + reservedlong + reservedint + id + reservedpuntoycoma
                            | VISIBILIDAD + reservedlong + reservedlong + reservedint + id + igual + OPERACIONESDECIMAL + reservedpuntoycoma

                            | VISIBILIDAD + reservedfloat + id + reservedpuntoycoma
                            | VISIBILIDAD + reservedfloat + id + igual + OPERACIONESPUNTO + reservedpuntoycoma

                            | VISIBILIDAD + reserveddouble + id + reservedpuntoycoma
                            | VISIBILIDAD + reserveddouble + id + igual + OPERACIONESPUNTO + reservedpuntoycoma

                            | VISIBILIDAD + reservedstring + id + reservedpuntoycoma
                            | VISIBILIDAD + reservedstring + id + igual + SUMACADENA + reservedpuntoycoma

                            | VISIBILIDAD + reservedchar + id + reservedpuntoycoma
                            | VISIBILIDAD + reservedchar + id + igual + r_char + reservedpuntoycoma
                            | VISIBILIDAD + reservedchar + id + igual + id + reservedpuntoycoma

                            | VISIBILIDAD + reservedboolean + id + reservedpuntoycoma
                            | VISIBILIDAD + reservedboolean + id + igual + reservedfalse + reservedpuntoycoma
                            | VISIBILIDAD + reservedboolean + id + igual + reservedtrue + reservedpuntoycoma
                            | VISIBILIDAD + reservedboolean + id + igual + id + reservedpuntoycoma;

            ASIGNACION.Rule = id + igual + OPERACIONESDECIMAL + reservedpuntoycoma
                            | id + igual + OPERACIONESPUNTO + reservedpuntoycoma
                            | id + igual + SUMACADENA + reservedpuntoycoma
                            | id + igual + r_char + reservedpuntoycoma
                            | id + igual + reservedtrue + reservedpuntoycoma
                            | id + igual + reservedfalse + reservedpuntoycoma
                            | id + igual + id + reservedpuntoycoma;

            //DECLARACION.ErrorRule = SyntaxError + reservedpuntoycoma;

            CONDICION.Rule = CONDICION + SIGNO + CONDICION
                    | CONDICION + SIGNO + CONDICION + rand + CONDICION + SIGNO + CONDICION
                    | CONDICION + SIGNO + CONDICION + ror + CONDICION + SIGNO + CONDICION
                    | r_char
                    | id
                    | OPERACIONESDECIMAL
                    | OPERACIONESPUNTO
                    | SUMACADENA;
            //CONDICION.ErrorRule = SyntaxError + reservedpuntoycoma;

            SIGNO.Rule = SIGNO
                    | igualigual
                    | diferente
                    | menor
                    | mayor
                    | menorigual
                    | mayorigual;

            SICONDICION.Rule = reservedif + reservedabrir + CONDICION + reservedcerrar + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar;
                    /*| reservedif + reservedabrir + CONDICION + reservedcerrar + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar
                    + reservedelse + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar
                    | reservedif + reservedabrir + CONDICION + reservedcerrar + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar
                    + reservedelse + reservedif + reservedabrir + CONDICION + reservedcerrar + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar
                    | reservedif + reservedabrir + CONDICION + reservedcerrar + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar
                    + reservedelse + reservedif + reservedabrir + CONDICION + reservedcerrar + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar
                    + reservedelse + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar

                    | reservedif + reservedabrir + CONDICION + reservedcerrar + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar
                    + reservedelse + reservedif + reservedabrir + CONDICION + reservedcerrar + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar
                    + reservedelse + reservedif + reservedabrir + CONDICION + reservedcerrar + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar;*/;
            //SICONDICION.ErrorRule = SyntaxError + reservedpuntoycoma;

            ELSECONDICION.Rule = reservedelse + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar;

            ELSEIFCONDICION.Rule = reservedelse + reservedif + reservedabrir + CONDICION + reservedcerrar + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar;

            ANIDADO.Rule = ANIDADO + ANIDADO
                    | SICONDICION + ELSECONDICION
                    | SICONDICION + ELSEIFCONDICION
                    //| SICONDICION + ELSEIFCONDICION + ELSECONDICION
                    | ELSEIFCONDICION + ELSECONDICION
                    | SICONDICION;
                    //| ELSECONDICION
                    //| ELSEIFCONDICION
                    
            //ANIDADO.ErrorRule = SyntaxError + reservedpuntoycoma;

            WHILE.Rule = reservedwhile + reservedabrir + CONDICION + reservedcerrar + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar;
            //WHILE.ErrorRule = SyntaxError + reservedpuntoycoma;

            DOWHILE.Rule = reserveddo + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar + reservedwhile
                    + reservedabrir + CONDICION + reservedcerrar + reservedpuntoycoma;
            //DOWHILE.ErrorRule = SyntaxError + reservedpuntoycoma;

            FOR.Rule = reservedfor + reservedabrir + DECLARACION + CONDICION + reservedpuntoycoma + MM + reservedcerrar
                    + reservedllaveabrir + CUERPOCLASE + reservedllavecerrar;    

            MM.Rule = id + mas + mas
                    | id + menos + menos;

            SWITCH.Rule = reservedswitch + reservedabrir + id + reservedcerrar + reservedllaveabrir + CUERPOSWITCH + reservedllavecerrar;

            CASE.Rule = reservedcase  + numeroentero + reserveddospuntos + CUERPOCLASE  + reservedbreak + reservedpuntoycoma
                    | reservedcase + r_char + reserveddospuntos + CUERPOCLASE + reservedbreak + reservedpuntoycoma
                    | reservedcase + cadena + reserveddospuntos + CUERPOCLASE + reservedbreak + reservedpuntoycoma;

            CUERPOSWITCH.Rule = CUERPOSWITCH + CASE
                    | CUERPOSWITCH + reserveddefault + reserveddospuntos + CUERPOCLASE
                    | CASE
                    | reserveddefault + reserveddospuntos +  CUERPOCLASE;

            IMPRIMIR.Rule = reservedprint + id + reservedpuntoycoma;

            #endregion

           
            /*#region ER
            this.Root = S;
            #endregion*/

            #region Precedencia de Operadores
            this.Root = INICIO;
            this.RegisterOperators(20, Associativity.Left, mas, menos);
            this.RegisterOperators(30, Associativity.Left, por, div);
            this.RegisterOperators(40, Associativity.Left, pot);
            /*this.MarkTransient(INICIO, SENTENCIASINICIALVACIO, SENTENCIASVACIO);
            this.RegisterOperators(1, Associativity.Left, ror);
            this.RegisterOperators(2, Associativity.Left, rand);
            this.RegisterOperators(3, Associativity.Left, igualigual, mayor, menor);
            this.RegisterOperators(4, Associativity.Left, mas, menos);
            this.RegisterOperators(5, Associativity.Left, por, div);
            this.RegisterOperators(6, Associativity.Right, "!");
            this.MarkPunctuation("{", "}", "(", ")", ";", ",", "=", "Class", "public", "void", "main", 
                "if", "else", "while", "if else", "switch", "case:");*/
            #endregion
        }

        public override void ReportParseError(ParsingContext context)
        {
            base.ReportParseError(context);
            String error = context.CurrentToken.ValueString;
            int fila = 1;
            int columna = 1;
            string descripcion = "";
            if (error.Contains("Invalid character"))
            {
                fila = context.Source.Location.Line;
                columna = context.Source.Location.Column;

                string delimStr = ":";
                char[] delim = delimStr.ToCharArray();
                string[] division = error.Split(delim, 2);
                division = division[1].Split('.');
                descripcion = "Caracter Invalido " + division[0];
                Form1.errores += "lexico : fila " + fila + " col " + columna + "   " + division[0] + "\n";
            }
            else
            {
                fila = 1 + context.Source.Location.Line;
                columna = context.Source.Location.Column;
                String tokensesperados = ":";
                foreach (String data in context.GetExpectedTermSet())
                    tokensesperados += "," + "\"" + data + "\"";
                descripcion = "Se esperaba: " + tokensesperados;
                Form1.errores += "sintactico: fila: " + fila + " col:" + columna + " Token Erroneo:  " + context.CurrentToken + " " + descripcion + "\n";
            }
        }
        //if, else, if else, while, do while, switch, case, arreglos

        //https://github.com/Alxandr/Irony/blob/master/Irony.Samples/CSharp/CSharpGrammar.cs
        //https://www.youtube.com/watch?v=6Tvw4P0DbHI
    }
}