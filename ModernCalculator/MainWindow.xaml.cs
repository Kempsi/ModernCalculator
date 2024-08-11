using ModernCalculator.UserControls;
using System.Text;
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

namespace ModernCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Needed variables
        double temp = 0;
        string output = "";
        string displayOutput = "0";
        string operation = "";

        string displayValues = "";
        string displayNumber = "";

        bool hadError = false;

        public MainWindow()
        {
            InitializeComponent();
            text_Output.Text = displayOutput;
        }

        #region Window controls


        // Event handler that closes the program with custom button
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        // Event handler that handles the dragging of the program
        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        #endregion Window controls


        #region Button event handlers


        // Event handler for all of the number buttons
        private void btn_number(object sender, RoutedEventArgs e)
        {

            text_Output.FontSize = 60;
            string name = ((NumberButton)sender).Name;

            // Switch case for handling each of the numbers
            switch (name)
            {
                case "btn_0":
                    AppendInput("0");
                    break;

                case "btn_1":
                    AppendInput("1");
                    break;

                case "btn_2":
                    AppendInput("2");
                    break;

                case "btn_3":
                    AppendInput("3");
                    break;

                case "btn_4":
                    AppendInput("4");
                    break;

                case "btn_5":
                    AppendInput("5");
                    break;

                case "btn_6":
                    AppendInput("6");
                    break;

                case "btn_7":
                    AppendInput("7");
                    break;

                case "btn_8":
                    AppendInput("8");
                    break;

                case "btn_9":
                    AppendInput("9");
                    break;

                case "btn_dot":

                    if (!IsOutputEmpty() && !output.Contains(','))
                    {
                        AppendInput(",");
                    }

                    break;

                default:
                    break;
            }
        }


        // Event handler for all of the action buttons
        private void btn_action(object sender, RoutedEventArgs e)
        {
            text_Output.FontSize = 60;

            string name;

            // Checks which custom usercontrol button we are dealing with
            if (sender is ActionButton)
            {

                name = ((ActionButton)sender).Name;

            }

            else
            {
                name = ((TotalButton)sender).Name;
            }


            // Switch case for dealing with each of the actions
            switch (name)
            {
                case "btn_clear":

                    ClearAllValues();

                    break;

                case "btn_backspace":

                    if (!IsOutputEmpty())
                    {
                        RemoveOneChar();
                    }

                    break;

                case "btn_percent":

                    if (!IsOutputEmpty())
                    {
                        TrimValues();
                        SaveValues();

                        SetOperation("%");
                        AppendOperationToDisplay();
                    }

                    break;

                case "btn_divide":

                    if (!IsOutputEmpty())
                    {
                        TrimValues();
                        SaveValues();

                        SetOperation("/");
                        AppendOperationToDisplay();
                    }

                    break;

                case "btn_multiply":

                    if (!IsOutputEmpty())
                    {
                        TrimValues();
                        SaveValues();

                        SetOperation("*");
                        AppendOperationToDisplay();
                    }

                    break;

                case "btn_minus":

                    if (!IsOutputEmpty())
                    {
                        TrimValues();
                        SaveValues();

                        SetOperation("-");
                        AppendOperationToDisplay();
                    }

                    break;

                case "btn_plus":

                    if (!IsOutputEmpty())
                    {
                        TrimValues();
                        SaveValues();

                        SetOperation("+");
                        AppendOperationToDisplay();

                    }

                    break;

                case "btn_total":

                    double outputTemp = 0;

                    // Inner switch case for dealing with operation after press sum/total button
                    switch (operation)
                    {
                        case "+":

                            if (!IsOutputEmpty())
                            {
                                TrimValues();
                                outputTemp = temp + double.Parse(output);
                            }

                            else
                            {
                                RemoveOneCharFromDisplayValue();
                                outputTemp = double.Parse(displayValues);
                            }

                            break;

                        case "-":

                            if (!IsOutputEmpty())
                            {
                                TrimValues();
                                outputTemp = temp - double.Parse(output);
                            }

                            else
                            {
                                RemoveOneCharFromDisplayValue();
                                outputTemp = double.Parse(displayValues);
                            }


                            break;

                        case "*":

                            if (!IsOutputEmpty())
                            {
                                TrimValues();
                                outputTemp = temp * double.Parse(output);

                            }

                            else
                            {
                                RemoveOneCharFromDisplayValue();
                                outputTemp = double.Parse(displayValues);
                            }

                            break;

                        case "/":

                            if (output != "0" && !IsOutputEmpty())
                            {
                                TrimValues();
                                outputTemp = temp / double.Parse(output);
                            }

                            else
                            {
                                hadError = true;
                            }


                            break;

                        case "%":

                            if (!IsOutputEmpty())
                            {
                                TrimValues();
                                outputTemp = temp % double.Parse(output);

                            }

                            else
                            {
                                RemoveOneCharFromDisplayValue();
                                outputTemp = double.Parse(displayValues);
                            }

                            break;

                        default:
                            break;
                    }

                    // Block of code that executes if number was divided by zero
                    if (hadError)
                    {
                        text_Output.FontSize = 20;
                        text_Output.Text = "Cannot divide with zero";

                        displayValues = "";
                        displayNumber = "";
                        text_displayValues.Text = displayValues;
                        output = displayNumber;
                        hadError = false;
                    }

                    // Otherwise, continue to show results
                    else
                    {
                        outputTemp = Math.Round(outputTemp, 6);

                        output = outputTemp.ToString();
                        text_Output.Text = output;

                        displayValues = outputTemp.ToString();
                        text_displayValues.Text = displayValues;
                    }



                    break;

                default:
                    break;
            }


        }


        #endregion Button event handlers


        #region Input validation and assistant methods


        // Checks and returns true if output is null or empty, otherwise false
        private bool IsOutputEmpty()
        {
            return string.IsNullOrEmpty(output);
        }


        // Trims outputs forbidden values. For example: if output includes only ',' and numbers.
        // Also removes extra ',' and trims ends
        private void TrimValues()
        {
            if (output.All(c => c == ',' && char.IsDigit(c)) && output.Any(char.IsDigit))
            {
                output = string.Empty;
            }

            output = Regex.Replace(output, @",+", ",");
            output = output.TrimEnd(',');
            displayValues = displayValues.TrimEnd(',');
        }


        // Method that appends a given string(number) into the output and displayValue variables
        private void AppendInput(string number)
        {
            output += number;
            text_Output.Text = output;

            displayValues += number;
            text_displayValues.Text = displayValues;
        }


        // Method that clears all the values
        private void ClearAllValues()
        {
            displayNumber = "0";

            output = string.Empty;
            text_Output.Text = displayNumber;

            displayValues = string.Empty;
            text_displayValues.Text = displayValues;
        }


        // Method that removes one character from both values
        private void RemoveOneChar()
        {
            output = output.Remove(output.Length - 1);
            text_Output.Text = output;

            displayValues = displayValues.Remove(displayValues.Length - 1);
            text_displayValues.Text = displayValues;
        }


        // Method that removes one character from displayed string
        private void RemoveOneCharFromDisplayValue()
        {
            displayValues = displayValues.Remove(displayValues.Length - 1);
        }


        // Method that sets the given operator to the global operator
        private void SetOperation(string oper)
        {
            operation = oper;
        }


        // Method that appends the operator to the display
        private void AppendOperationToDisplay()
        {
            displayValues += operation;
            text_displayValues.Text = displayValues;
        }


        // Method that saves values to variables
        private void SaveValues()
        {
            temp = double.Parse(output);

            displayOutput = temp.ToString();
            text_Output.Text = displayOutput;

            output = string.Empty;
        }


        #endregion Input validation and assistant methods


    }
}