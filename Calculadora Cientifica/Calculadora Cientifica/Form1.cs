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
            // Inicializador el ComboBox con las funciones
            comboBox_Functions.Items.Add("FN1");
            comboBox_Functions.Items.Add("FN2");
            comboBox_Functions.SelectedIndex = 0; 
        }



        //Aquí todos los botones que usarán: 
        private void btn_n1_Click_1(object sender, EventArgs e)
        {
            txt_nums.Text += "1";

        }

        private void btn_n2_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "2";
        }


        private void btn_n3_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "3";

        }

        private void btn_n4__Click(object sender, EventArgs e)
        {
            txt_nums.Text += "4";

        }

        private void btn_n5__Click(object sender, EventArgs e)
        {
            txt_nums.Text += "5";

        }

        private void btn_n6__Click(object sender, EventArgs e)
        {
            txt_nums.Text += "6";

        }

        private void btn_n7__Click(object sender, EventArgs e)
        {
            txt_nums.Text += "7";

        }

        private void btn_n8__Click(object sender, EventArgs e)
        {
            txt_nums.Text += "8";

        }

        private void btn_n9__Click(object sender, EventArgs e)
        {
            txt_nums.Text += "9";

        }

        private void btn_punto_Click(object sender, EventArgs e)
        {
            txt_nums.Text += ".";

        }


        private void btn_restas_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "-";

        }

        private void btn_sumas_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "+";

        }

        private void btn_x_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "x";

        }

        private void btn_division_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "÷";

        }

        private void btn_parentesisdere_Click(object sender, EventArgs e)
        {
            txt_nums.Text += ")";

        }

        private void btn_parentesisizq_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "(";

        }

        private void btn_n0__Click(object sender, EventArgs e)
        {
            txt_nums.Text += "0";
        }

        private void btn_ceros_Click(object sender, EventArgs e)
        {
            txt_nums.Text = string.Empty;
        }

        private void btn_deltodo_Click(object sender, EventArgs e)
        {
            if (txt_nums.Text.Length > 0)
            {
                txt_nums.Text = txt_nums.Text.Substring(0, txt_nums.Text.Length - 1);
            }
        }

        private void btn_Ans1_Click(object sender, EventArgs e)
        {
            txt_nums.Text += lastResult.ToString();

        }

        private void btn_e_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "2.7182818284590452353602874713527";
        }

        private void btn_pi_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "3.1415926535897932384626433832795";
        }


        private void btn_tan_Click(object sender, EventArgs e)
        {
            txt_nums.Text += "tan(";

        }

        private void btn_xsobre1_Click(object sender, EventArgs e)
        {
            txt_nums.Text += " ^";
        }

        private void btn_raizc_Click(object sender, EventArgs e)
        {
            try
            {
                // Calcula la raíz cuadrada del número en txt_nums
                double resultado = Raiz(txt_nums.Text);
                txt_nums.Text = resultado.ToString();

            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, introduce un número válido.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //Aquí hacemos publica la función donde el número que seleccionemos nos dara la raíz
        public double Raiz(string numeroTexto)
        {
            // Intenta convertir el texto a un número
            if (double.TryParse(numeroTexto, out double numero))
            {
                if (numero < 0)
                {
                    throw new ArgumentOutOfRangeException("El número no puede ser negativo.");
                }

                return Math.Sqrt(numero);
            }
            else
            {
                throw new FormatException("El texto ingresado no es un número válido.");
            }
        }




        private void btn_igual_Click(object sender, EventArgs e)
        {
            try
            {
                // Reemplaza operadores personalizados por los que entiende NCalc
                string expressionText = txt_nums.Text.Replace("x", "*").Replace("÷", "/");

                string[] parts = txt_nums.Text.Split('^');
                if (parts.Length == 2)
                {
                    double baseNum = Convert.ToDouble(parts[0]);
                    double exponent = Convert.ToDouble(parts[1]);
                    double result = Math.Pow(baseNum, exponent);
                    txt_nums.Text = result.ToString();
                }

                if (expressionText.Contains("+") || expressionText.Contains("*") || expressionText.Contains("/") || expressionText.Contains("-"))
                {
                    // Usar NCalc para evaluar la expresión
                    Expression expression = new Expression(expressionText);

                    // Evaluar la expresión
                    object evalResult = expression.Evaluate();
                    txt_nums.Text = evalResult.ToString();
                    lastResult = Convert.ToDouble(evalResult);
                }


                // Detecta y maneja la función tan()
                if (expressionText.Contains("tan("))
                {
                    expressionText = ProcessTanFunction(expressionText);
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la expresión: " + ex.Message);
            }
        }


        private string ProcessTanFunction(string expression)
        {
            try
            {
                while (expression.Contains("tan("))
                {
                    // Encuentra la posición de la función tan(
                    int startIndex = expression.IndexOf("tan(") + 4;
                    int endIndex = expression.IndexOf(")", startIndex);

                    if (endIndex > startIndex)
                    {
                        // Extrae el número dentro de la función tan()
                        string numberStr = expression.Substring(startIndex, endIndex - startIndex);

                        // Evalúa la expresión interna si es necesario
                        Expression internalExpression = new Expression(numberStr);
                        double number = Convert.ToDouble(internalExpression.Evaluate());

                        // Calcula la tangente
                        double result = Math.Tan(number);

                        // Reemplaza la función completa con el resultado
                        string fullFunction = $"tan({numberStr})";
                        expression = expression.Replace(fullFunction, result.ToString());
                    }
                    else
                    {
                        throw new FormatException("Paréntesis de cierre faltante en tan().");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar la función tan(): " + ex.Message);
            }

            return expression;
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
                panel3.Visible = false;
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
