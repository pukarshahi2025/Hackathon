using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows;

namespace Hackathon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string selectedFilePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChooseFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                selectedFilePath = dialog.FileName;
                FilePathTextBlock.Text = selectedFilePath;
                OutputBox.Clear();
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Please select a file.");
                return;
            }

            var lines = File.ReadAllLines(selectedFilePath);
            var errors = new List<string>();
            var validRecords = new List<TRRModel>();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int lineNumber = i + 1;

                try
                {
                    var record = FixedWidthParser.ParseLine<TRRModel>(line);
                    var validationErrors = FixedWidthParser.Validate(record, lineNumber);

                    if (validationErrors.Any())
                        errors.AddRange(validationErrors);
                    else
                        validRecords.Add(record);
                }
                catch (Exception ex)
                {
                    errors.Add($"Line {lineNumber}: {ex.Message}");
                }
            }

            OutputBox.Text = errors.Any() ? string.Join(Environment.NewLine, errors) : "All records are valid!";
        }

        private int GetColumnIndex(string field) => field switch
        {
            "Name" => 0,
            "Age" => 1,
            "Email" => 2,
            _ => -1
        };
    }
}