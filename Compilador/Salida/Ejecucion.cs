using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compilador.Salida
{
    public static class Ejecucion
    {
        // Con este nombre se genera el ejecutable
        public static readonly string Exe = "FlamePanda.exe";

        // La lista de errores que se genera si no se puede compilar el codigo C#
        public static List<CompilerError> Errores = new List<CompilerError>();

        // Esto no se utiliza creo :v
        public static List<string> Salida = new List<string>();
        internal static string exe;

        /// <summary>
        /// Se usa este metodo para compilar codigo C#
        /// </summary>
        /// <param name="codigoFuente">El codigo se manda como parametro</param>
        /// <returns></returns>
        public static bool Compilar(string codigoFuente)
        {
            Errores.Clear();
            Salida.Clear();

            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" }, Exe, true)
            {
                GenerateExecutable = true,
                OutputAssembly = Exe,
            };

            CompilerResults results = csc.CompileAssemblyFromSource(parameters, codigoFuente);

            Errores = results.Errors.Cast<CompilerError>().ToList();

            foreach (var error in Errores)
            {
                MessageBox.Show(error.ErrorText);
            }

            if (Errores.Count != 0)
            {
                return false;
            }

            foreach (string s in results.Output)
            {
                Salida.Add(s);
            }

            return true;
        }
    }
}