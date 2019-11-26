using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Compilador.Analizador;
using Irony.Parsing;

namespace Compilador
{
    public partial class Arbol : Form
    {
        public Arbol()
        {
            InitializeComponent();
        }

        string cadenaExpresion;

        private void Arbol_Load(object sender, EventArgs e)
        {
            
            ParseTreeNode resultado = Sintactico.AnalizarDecimal(cadenaExpresion);
            if (resultado != null)
            {
                label1.Text = "correcta";
                pictureBox1.Image = Sintactico.getImage(resultado);
                Recorrido.resolverOperacionDecimal(resultado);
            }
            else
            {
                label1.Text = "incorrecta";
            }
        }

        public void expresion(String texto)
        {
            cadenaExpresion = texto;
        }
    }
}