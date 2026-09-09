using DynamicExpresso;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

namespace CalculatorDynamicEspresso
{
    /// <summary>
    /// This calculator uses the Dynamic Espresso library to evaluate mathematical expressions.
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _displayText = string.Empty;

        public string DisplayText
        {
            get => _displayText;
            set
            {
                  if (_displayText != value)
                  {
                      _displayText = value;
                      OnPropertyChanged();
                  }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ClearEntryButton_Click(object sender, RoutedEventArgs e)
        {
            // Delete the last entry
            if (!string.IsNullOrEmpty(DisplayText))
            {
                DisplayText = DisplayText.Substring(0, DisplayText.Length - 1);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button b)
            {
                HandleInput(b.Content?.ToString());
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Clear the display
            DisplayText = string.Empty;
        }

        private void MultiplyButton_Click(object sender, RoutedEventArgs e)
        {
            HandleInput("x");
        }

        private void DivideButton_Click(object sender, RoutedEventArgs e)
        {
            HandleInput("/");
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            HandleInput("+");
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            HandleInput("-");
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {

            try      //NEED TO FIX IT
            {
                var interpreter = new Interpreter();
                var expr = Regex.Replace(DisplayText ?? string.Empty, @"\p{C}+", ""); // Steuerzeichen entfernen
                expr = expr.Trim();
                var result = interpreter.Eval(expr);
                DisplayText = result?.ToString() ?? string.Empty;
            }
            catch (DivideByZeroException ex)
            {
                DisplayText = "Division by zero not allowed";
            }
            catch (DynamicExpresso.Exceptions.ParseException ex)
            {
                DisplayText = "Invalid expression";
            }
        }

        private void HandleInput(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                DisplayText += input;
            }
        }
    }
}