using System;
using System.Windows.Forms;
using NCalc;
using System.Globalization;

namespace Calculadora_Cientifica
{
    public partial class Form1 : Form
    {

        int modo1 = 1;
        int modo2 = 2;

        public Form1()
        {
            InitializeComponent();


            pn_rk4.Enabled = false;
        }

        private double[] Solve(Func<double, double, double> f, double x0, double y0, double xEnd, double h, out string procedure)
        {
            int steps = (int)((xEnd - x0) / h);
            double[] results = new double[steps + 1];
            procedure = ""; // Guardar el procedimiento si es necesario

            double x = x0;
            double y = y0;
            results[0] = y;

            for (int i = 1; i <= steps; i++)
            {
                double k1 = h * f(x, y);
                double k2 = h * f(x + h / 2.0, y + k1 / 2.0);
                double k3 = h * f(x + h / 2.0, y + k2 / 2.0);
                double k4 = h * f(x + h, y + k3);

                y += (k1 + 2 * k2 + 2 * k3 + k4) / 6.0;
                x += h;
                results[i] = y;

                // Guardar el procedimiento si es necesario
                procedure += $"Paso {i} (x = {x:F1}): k1 = {k1:F6}, k2 = {k2:F6}, k3 = {k3:F6}, k4 = {k4:F6}, y = {y:F6}\n";
            }

            return results;
        }

        public static double FN1(double t, double y)
        {
            return y - Math.Pow(t, 2) + 1;
        }

        public static double FN2(double t, double y)
        {
            return t * y + Math.Pow(t, 3);
        }

        private double lastResult = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            // Inicializar el ComboBox con las funciones
            comboBox_Functions.Items.Add("FN1");
            comboBox_Functions.Items.Add("FN2");
            comboBox_Functions.SelectedIndex = 0; // Seleccionar FN1 por defecto
        }

        private void n1_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "1";
        }

        private void n2_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "2";
        }

        private void n3_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "3";
        }

        private void btn_n4_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "4";
        }

        private void btn_n5_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "5";
        }

        private void btn_n6_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "6";
        }

        private void btn_n7_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "7";
        }

        private void btn_n8_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "8";
        }

        private void bnt_n9_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "9";
        }

        private void btn_n0_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "0";
        }

        private void btn_suma_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "+";
        }

        private void btn_multi_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "*";
        }

        private void btn_div_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "/";
        }

        private void btn_rest_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "-";
        }

        private void bt_punto_Click(object sender, EventArgs e)
        {
            txt_nums.Text += ".";
        }

        private void btn_por10_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "*10";
        }

        private void btn_Ans_Click(object sender, EventArgs e)
        {
            txt_nums.Text += lastResult.ToString();
        }

        private void btn_parenz_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "(";
        }

        private void btn_parentD_Click(object sender, EventArgs e)
        {
            txt_nums.Text += ")";
        }

        private void btn_AC_Click(object sender, EventArgs e)
        {
            txt_nums.Text = string.Empty;
        }

        private void btn_Xsobre2_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "^";
        }

        private void btn_respuesta_Click(object sender, EventArgs e)
        {
            try
            {
                string expressionText = txt_nums.Text.Replace("x", "*").Replace("÷", "/");

                // Usar NCalc para evaluar la expresión
                Expression expression = new Expression(expressionText);

                // Manejar el operador de potencia
                expression.EvaluateFunction += (name, args) =>
                {
                    if (name == "pow")
                    {
                        args.Result = Math.Pow(Convert.ToDouble(args.Parameters[0].Evaluate()), Convert.ToDouble(args.Parameters[1].Evaluate()));
                    }
                };

                // Reemplazar ^ por pow para que NCalc pueda evaluar la expresión
                expressionText = expressionText.Replace("^", "pow(").Replace("(", ",").Replace(")", ")");

                expression = new Expression(expressionText);
                object result = expression.Evaluate();
                txt_nums.Text = result.ToString();
                lastResult = Convert.ToDouble(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la expresión: " + ex.Message);
            }
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            if (txt_nums.Text.Length > 0)
            {
                txt_nums.Text = txt_nums.Text.Substring(0, txt_nums.Text.Length - 1);
            }
        }

       

        private void txt_x0_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
        "¿Quieres usar la función de RK4?",
        "Confirmación",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

            // Verifica la respuesta del usuario
            if (result == DialogResult.Yes)
            {
                button2.Visible = false;
                pn_rk4.Enabled = true;
                btn_rk4act.Enabled = true;
                modo1 = 1;
            }
            else
            { 

            }
        }

        private void btn_rk4act_Click(object sender, EventArgs e)
        {
            try
            {
                // Usar la cultura invariante para garantizar el uso del punto decimal
                CultureInfo culture = CultureInfo.InvariantCulture;

                double x0 = Convert.ToDouble(txt_x0.Text, culture);
                double y0 = Convert.ToDouble(txt_y0.Text, culture);
                double xEnd = Convert.ToDouble(txt_xU.Text, culture);
                double h = Convert.ToDouble(txt_h.Text, culture);

                // Selección de la función correcta
                Func<double, double, double> f;
                string selectedFunction = comboBox_Functions.SelectedItem.ToString();

                if (selectedFunction == "FN1")
                {
                    f = FN1;
                }
                else if (selectedFunction == "FN2")
                {
                    f = FN2;
                }
                else
                {
                    throw new Exception("Función no seleccionada o desconocida.");
                }

                double[] results = Solve(f, x0, y0, xEnd, h, out string procedure);

                // Limpiar el texto del cuadro de texto antes de mostrar los resultados
                txt_nums.Text = procedure; // Mostrar el procedimiento

                double currentX = x0;
                for (int i = 0; i < results.Length; i++)
                {
                    // Mostrar los resultados en el cuadro de texto
                    txt_nums.AppendText($"x: {currentX:F1}, y: {results[i]:F3}\n");
                    currentX += h;
                }

                // Mostrar el resultado final
                txt_nums.AppendText($"\nResultado Final: y({xEnd}) = {results[results.Length - 1]:F3}\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
