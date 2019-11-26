using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.Windows.Forms;

namespace Compilador.Analizador
{
    class Recorrido
    {
        Form1 form1;

        public static Double resolverOperacionEntero(ParseTreeNode root)
        {
            return Convert.ToInt32(expresion(root.ChildNodes.ElementAt(0)));
        }

        public static Double resolverOperacionDecimal(ParseTreeNode root)
        {
            return Convert.ToDouble(expresion(root.ChildNodes.ElementAt(0)));
        }

        public static Double expresion(ParseTreeNode root)
        {
            switch (root.ChildNodes.Count)
            {
                case 1:
                    String[] numero = root.ChildNodes.ElementAt(0).ToString().Split(' ');
                    return Convert.ToDouble(numero[0]);
                case 3:
                    switch(root.ChildNodes.ElementAt(1).ToString().Substring(0, 1))
                    {
                        case "+":
                            return expresion(root.ChildNodes.ElementAt(0)) + expresion(root.ChildNodes.ElementAt(2));
                        case "-":
                            return expresion(root.ChildNodes.ElementAt(0)) - expresion(root.ChildNodes.ElementAt(2));
                        case "*":
                            return expresion(root.ChildNodes.ElementAt(0)) * expresion(root.ChildNodes.ElementAt(2));
                        case "/":
                            return expresion(root.ChildNodes.ElementAt(0)) / expresion(root.ChildNodes.ElementAt(2));
                        case "^":
                            return Math.Pow(expresion(root.ChildNodes.ElementAt(0)), expresion(root.ChildNodes.ElementAt(2)));
                        default:
                            return expresion(root.ChildNodes.ElementAt(1));
                    }
            }
            return 0.0;
        }
    }
}
