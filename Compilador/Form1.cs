using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Compilador.Analizador;
using Compilador.TiposDeDatos;
using Irony.Parsing;
using Compilador.Salida;

namespace Compilador
{
    /// Lógica de interacción
    public partial class Form1 : Form
    {
        RegexLexer csLexer = new RegexLexer();
        bool load;
        List<string> palabrasReservadas;
        List<string> palabrasTipo;
        private List<string> reservadasCiclos;
        private List<string> ambito;
        List<Dato> listaDatos = null;
        List<string> repetidos;
        List<string> yaDeclaradas;
        List<string> noDeclaradas;
        List<string> erroresSemantica;
        string nombreArchivo;
        public static String errores = "";
        bool termino = false;

        public Form1()
        {
            InitializeComponent();

            using (StreamReader sr = new StreamReader(@"..\..\RegexL\RegexLexer.cs"))
            {
                csLexer.AddTokenRule(@"\s+", "ESPACIO", true);
                csLexer.AddTokenRule(@"\b[_a-zA-Z][\w]*\b", "IDENTIFICADOR");
                csLexer.AddTokenRule("\".*?\"", "CADENA");
                csLexer.AddTokenRule("\'.*?\'", "CHAR");
                csLexer.AddTokenRule(@"'\\.'|'[^\\]'", "CARACTER");
                csLexer.AddTokenRule("//[^\r\n]*", "COMENTARIO1");
                csLexer.AddTokenRule("/[*].*?[*]/", "COMENTARIO2");
                csLexer.AddTokenRule(@"\d*\.\d+", "NUMERO_DECIMAL");
                csLexer.AddTokenRule(@"\-?\d+", "NUMERO_ENTERO");
                csLexer.AddTokenRule(@">=|<=|>|<|==|!=", "COMPARADOR");
                csLexer.AddTokenRule(@"[\|][\|]|[\&][\&]", "OPERADOR_LOGICO");
                csLexer.AddTokenRule(@"[\+][\+]|[\-][\-]", "OPERADOR_INC_O_DEC");
                csLexer.AddTokenRule(@"[\(\)\{\}\[\];,]", "DELIMITADOR");
                csLexer.AddTokenRule(@"\+=|\-=|\*=|\/=", "OPERADOR_ASIGNACION");
                csLexer.AddTokenRule(@"[\.=|\+|\-|/|*|%]", "OPERADOR");

                palabrasReservadas = new List<string>() {
                    "System", "abstract","as","async","await","checked",
                    "const", "continue", "default", "delegate",
                    "base", "break", "case","else", "enum","event",
                    "explicit", "extern", "false", "finally",
                    "fixed","goto","implicit","in", "interface",
                    "is", "lock", "new","null", "operator",
                    "catch","out", "override","params","readonly",
                    "ref", "return", "sealed", "sizeof",
                    "stackalloc", "static","switch", "this",
                    "throw","true", "try", "typeof", "namespace",
                    "unchecked","unsafe", "virtual", "void",
                    "object","get", "set", "new","partial", "yield",
                    "add", "remove", "value","alias", "ascending",
                    "descending", "from","group", "into", "orderby",
                    "select", "where","join", "equals", "using",
                     "class", "struct", "print" };

                reservadasCiclos = new List<string>()
                {
                     "while","do","for","foreach","if"
                };

                ambito = new List<string>()
                {
                    "public","private", "protected","internal"
                };

                palabrasTipo = new List<String>()
                {
                    "bool", "byte", "char", "decimal", "double","sbyte", "short", "string", "uint",
                "ulong", "ushort", "var", "float", "int", "long","void"
                };

                csLexer.Compile(RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

                load = true;
                AnalizeCode();
                tbxCode.Focus();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateMyMultilineTextBox();
        }

        private void AnalizeCode()
        {
            lvToken.Rows.Clear();

            int n = 0, e = 0;

            foreach (var tk in csLexer.GetTokens(tbxCode.Text))
            {
                if (tk.Name == "ERROR") e++;

                if (tk.Name == "IDENTIFICADOR")
                {
                    if (palabrasReservadas.Contains(tk.Lexema))
                        tk.Name = "RESERVADO";
                    if (palabrasTipo.Contains(tk.Lexema))
                        tk.Name = "TIPODATO";
                    if (reservadasCiclos.Contains(tk.Lexema))
                        tk.Name = "CICLOS";
                    if (ambito.Contains(tk.Lexema))
                        tk.Name = "AMBITO";
                }

                lvToken.Rows.Add(tk.Name, tk.Lexema, tk.Linea, tk.Columna, tk.Index);
                n++;
            }

            tx.Text = string.Format("Analizador Lexico - {0} tokens {1} errores", n, e);
        }

        private void CodeChanged(object sender, EventArgs e)
        {
            if (load)
            {
                AnalizeCode();
            }
        }

        public void IngresarVariables()
        {
            listaDatos = new List<Dato>();
            Dato datos = new Dato();
            repetidos = new List<string>();
            yaDeclaradas = new List<string>();
            noDeclaradas = new List<string>();
            string tipo = "", variable="", valor = "", contenidoValor="", noDeclarada="";

            for (int i = 0; i < lvToken.Rows.Count; i++)
            {
                if (lvToken.Rows[i].Cells[0].Value.ToString().Equals("TIPODATO"))
                {
                    tipo = lvToken.Rows[i].Cells[1].Value.ToString();
                    variable = lvToken.Rows[i + 1].Cells[1].Value.ToString();
                    foreach (Dato d in listaDatos)
                    { 
                        if (d.Identificador.Equals(variable))
                        {
                            if(!repetidos.Contains(variable))
                                repetidos.Add(variable);
                        }
                    }
                    if (lvToken.Rows[i + 2].Cells[1].Value.ToString().Equals("="))
                    {
                        //filas = i + 3;
                        i = i + 2;
                        while (contenidoValor != ";")
                        {
                            i++;
                            valor = valor + contenidoValor;
                            contenidoValor = lvToken.Rows[i].Cells[1].Value.ToString();

                            if (contenidoValor.Equals(variable))
                                noDeclaradas.Add(variable);

                            foreach (Dato d in listaDatos)
                            {
                                if (d.Identificador.Equals(contenidoValor))
                                {
                                    contenidoValor = contenidoValor.Replace(contenidoValor, d.Valor);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        valor = null;
                    }
                    datos = new Dato(tipo, variable, valor);
                    listaDatos.Add(datos);
                    valor = "";
                    contenidoValor = "";
                    yaDeclaradas.Add(variable);
                    
                }else if (lvToken.Rows[i].Cells[0].Value.ToString().Equals("IDENTIFICADOR") && i>3)
                {
                    noDeclarada = lvToken.Rows[i].Cells[1].Value.ToString();
                    if (!yaDeclaradas.Contains(noDeclarada))
                    {
                        if (!noDeclaradas.Contains(noDeclarada))
                            noDeclaradas.Add(noDeclarada);
                    }
                    else
                    {
                        variable = lvToken.Rows[i].Cells[1].Value.ToString();
                        i++;
                        if (lvToken.Rows[i].Cells[1].Value.ToString().Equals("="))
                        {
                            while (contenidoValor != ";")
                            {
                                i++;
                                valor = valor + contenidoValor;
                                contenidoValor = lvToken.Rows[i].Cells[1].Value.ToString();
                                foreach (Dato d in listaDatos)
                                {
                                    if (d.Identificador.Equals(contenidoValor))
                                    {
                                        contenidoValor = contenidoValor.Replace(contenidoValor, d.Valor);
                                        break;
                                    }
                                }
                            }
                            foreach (Dato d in listaDatos)
                            {
                                if (d.Identificador.Equals(variable))
                                {
                                    d.Valor = valor;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            valor = null;
                        }
                        valor = "";
                        contenidoValor = "";
                    }
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lvToken_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void ErroresVariables()
        {
            int tope = 0;
            if (repetidos.Count > 0)
            {
                if (rtxterrores.Text == "Análisis exitoso!!")
                    rtxterrores.Text = "";
                rtxterrores.Text = rtxterrores.Text + "Error, variable(s) repetidas: ";
                foreach (string x in repetidos)
                {
                    rtxterrores.Text = rtxterrores.Text + x;
                    tope++;
                    if (tope < repetidos.Count)
                    {
                        rtxterrores.Text = rtxterrores.Text + ", ";
                    }
                    else
                    {
                        rtxterrores.Text = rtxterrores.Text + "\r\n";
                    }
                }
            }
            int tope2 = 0;
            if(noDeclaradas.Count > 0)
            {
                if (rtxterrores.Text == "Análisis exitoso!!")
                    rtxterrores.Text = "";
                rtxterrores.Text = rtxterrores.Text + "Error, variable(s) no declaradas: ";
                foreach(string x in noDeclaradas)
                {
                    rtxterrores.Text = rtxterrores.Text + x;
                    tope2++;
                    if (tope2 < noDeclaradas.Count)
                    {
                        rtxterrores.Text = rtxterrores.Text + ", ";
                    }
                }
            }
        }

        public void ValidarValores() {
            erroresSemantica = new List<string>();
            ParseTreeNode resultado;
            if (rtxterrores.Text == "Análisis exitoso!!")
            {
                foreach (Dato d in listaDatos)
                {
                    if (d.Tipo.Equals("int"))
                    {
                        resultado = Sintactico.AnalizarEnteros(d.Valor);
                        if (resultado != null)
                        {
                            d.Valor = Recorrido.resolverOperacionEntero(resultado).ToString();
                        }
                        else
                        {
                            erroresSemantica.Add(d.Identificador);
                        }
                    }

                    if (d.Tipo.Equals("float") || d.Tipo.Equals("double"))
                    {
                        resultado = Sintactico.AnalizarDecimal(d.Valor);
                        if (resultado != null)
                        {
                            d.Valor = Recorrido.resolverOperacionDecimal(resultado).ToString();
                        }
                        else
                        {
                            erroresSemantica.Add(d.Identificador);
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rtxterrores.Text = " ";
            rtxterrores.Text = Sintactico.analizar(tbxCode.Text)+"";
            IngresarVariables();
            ErroresVariables();
            ValidarValores();
            ImprimirValor();
        }

        private void ImprimirValor()
        {
            foreach (Dato d in listaDatos)
            {
                rtxterrores.Text = rtxterrores.Text + " **Tipo: " + d.ToString();
            }
        }

        public System.Windows.Forms.ScrollBars ScrollBars { get; set; }

        public void CreateMyMultilineTextBox()
        {
            tbxCode.Multiline = true;
            tbxCode.ScrollBars = ScrollBars.Vertical;
            tbxCode.AcceptsReturn = true;
            tbxCode.AcceptsTab = true;
            tbxCode.WordWrap = true;
        }

        private string GuardarRutaArchivo()
        {
            var dialogo = new SaveFileDialog()
            {
                FileName = "NewFlameClass.fp",
                Filter = "Flame files (*.fp) | *.fp",
            };

            if (dialogo.ShowDialog() != DialogResult.OK)
                return null;

            nombreArchivo = dialogo.FileName;

            return dialogo.FileName;
        }

        private string ObtenerRutaArchivo()
        {
            var dialogo = new OpenFileDialog()
            {
                Filter = "Flame files (*.fp) | *.fp",
                RestoreDirectory = true,
            };

            if (dialogo.ShowDialog() != DialogResult.OK)
                return null;

            nombreArchivo = dialogo.FileName;

            return dialogo.FileName;
        }

        private string LeerArchivo(string rutaArchivo)
        {
            return File.ReadAllText(rutaArchivo);
        }

        private void EscribirArchivo(string rutaArchivo, string contenido)
        {
            using (var escritor = new StreamWriter(rutaArchivo, false))
            {
                escritor.Write(contenido);
            }
        }
        
        private void nuevo_Click(object sender, EventArgs e)
        {
            string rutaArchivo = GuardarRutaArchivo();

            if (rutaArchivo == null)
                return;

            using (var escritor = new StreamWriter(rutaArchivo, true))
            {
                escritor.WriteLine("using System;\r\npublic class NewFlameClass {\r\n" +
                    "   public static void Main(){\r\n\r\n   }\r\n}");
            }

            tbxCode.Text = LeerArchivo(rutaArchivo);
        }

        private void abrir_Click(object sender, EventArgs e)
        {
            string rutaArchivo = ObtenerRutaArchivo();

            if (rutaArchivo == null)
                return;

            string contenidoArchivo = LeerArchivo(rutaArchivo);

            tbxCode.Text = contenidoArchivo;
        }

        private void guardar_Click(object sender, EventArgs e)
        {
            string nombrePestanaSeleccionada = nombreArchivo;

            if (nombrePestanaSeleccionada == null)
                return;

            string codigoPestanaSeleccionada = tbxCode.Text;

            EscribirArchivo(nombrePestanaSeleccionada, codigoPestanaSeleccionada);

            System.Windows.MessageBox.Show("Flame file saved");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                console.ClearOutput();
                if (Ejecucion.Compilar(tbxCode.Text))
                {
                    console.WriteOutput("Begin process\n", System.Drawing.Color.Red);
                    EscribirArchivo(nombreArchivo, tbxCode.Text);
                    termino = false;
                    console.StartProcess(Ejecucion.Exe, null);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Exception found; please calm down");
                console.StopProcess();
            }
        }

        private void console_OnConsoleOutput(object sender, ConsoleControl.ConsoleEventArgs args)
        {
            /*if (!termino)
            {
                if (console.IsProcessRunning)
                    return;

                console.WriteOutput("End process\n", System.Drawing.Color.Red);

                termino = true;
            }*/
        }

        private void console_Load(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lvToken_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}