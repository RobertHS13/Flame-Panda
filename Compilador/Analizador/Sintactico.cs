using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using System.Windows.Forms;
using Programa_de_Arboles.sol.analizador;

namespace Compilador.Analizador
{
    class Sintactico : Grammar
    {
        public static string analizar(String cadena)
        {
            Form1.errores = "";
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode raiz = arbol.Root;


            if (raiz == null)// || Form1.errores.CompareTo("") != 0)
            {
                return Form1.errores;
            }

            return "Análisis exitoso!!";
        }

        public static ParseTreeNode AnalizarEnteros(String cadena)
        {
            GramaticaEnteros gramatica = new GramaticaEnteros();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            return arbol.Root;
        }

        public static ParseTreeNode AnalizarDecimal(String cadena)
        {
            GramaticaDecimal gramatica = new GramaticaDecimal();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            return arbol.Root;
        }

        public static ParseTreeNode AnalizarCaracteres(String cadena)
        {
            GramaticaCaracteres gramatica = new GramaticaCaracteres();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            return arbol.Root;
        }

        public static ParseTreeNode AnalizarCadena(String cadena)
        {
            GramaticaCadena gramatica = new GramaticaCadena();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            return arbol.Root;
        }

        public bool isValid(string sourceCode)
        {
            Form1.errores = "";
            Gramatica gramatica = new Gramatica();
            LanguageData language = new LanguageData(gramatica);

            Parser parser = new Parser(language);

            ParseTree parseTree = parser.Parse(sourceCode);

            ParseTreeNode root = parseTree.Root;

            return root != null;

        }

        //Obtene la raiz del codigo a parsear
        public ParseTreeNode getRoot(string sourceCode, Grammar grammar)
        {

            LanguageData language = new LanguageData(grammar);

            Parser parser = new Parser(language);

            ParseTree parseTree = parser.Parse(sourceCode);

            ParseTreeNode root = parseTree.Root;

            Console.WriteLine("raiz " + root.ToString());
            return root;

        }

        //Despliega el arbol de la gramatica
        public void dispTree(ParseTreeNode node, int level)
        {
            for (int i = 0; i < level; i++)
                Console.Write("  ");

            Console.WriteLine(node);

            foreach (ParseTreeNode child in node.ChildNodes)
            {
                dispTree(child, level + 1);
                Console.WriteLine("padre " + node + "->hijo " + child);
            }
        }

        //Recibe el nodo como parametro y escribe primero la raiz y luego los hijos visitandolos de izquierda a derecha.
        public void preorden(ParseTreeNode node)
        {
            Console.WriteLine(node);
            foreach (ParseTreeNode child in node.ChildNodes)
                preorden(child);
        }

        //Primero visita los hijos de izquierda a derecha y por ultimo la raiz.
        public void postorden(ParseTreeNode node)
        {
            foreach (ParseTreeNode child in node.ChildNodes)
                postorden(child);

            Console.WriteLine(node);
        }

        public static Image getImage(ParseTreeNode raiz)
        {
            String grafoDOT = ControlDOT.getDOT(raiz);
            WINGRAPHVIZLib.DOT dot = new WINGRAPHVIZLib.DOT();
            WINGRAPHVIZLib.BinaryImage img = dot.ToPNG(grafoDOT);
            byte[] imageBytes = Convert.FromBase64String(img.ToBase64String());
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            Image imagen = Image.FromStream(ms, true);
            return imagen;
        }

        private static void generarImagen(ParseTreeNode raiz)
        {
            String grafoDOT = ControlDOT.getDOT(raiz);
            WINGRAPHVIZLib.DOT dot = new WINGRAPHVIZLib.DOT();
            WINGRAPHVIZLib.BinaryImage img = dot.ToPNG(grafoDOT);
            img.Save("AST.png");
        }
    }
}