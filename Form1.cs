using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using IniParser;
using IniParser.Model;

namespace Barcode_Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Read data from the INI file.
            string ProjectCode = ReadIniValue("BarcodeData", "ProjectCode");
            string CenterCode = ReadIniValue("BarcodeData", "CenterCode");

            // Generate a combined string with date, time, and INI data.
            string combinedData = $"{ProjectCode}-{CenterCode}-{DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss")}";


            if (!string.IsNullOrEmpty(combinedData))
                {
                    // Create a BarcodeWriter instance.
                    BarcodeWriter barcodeWriter = new BarcodeWriter();

                    // Set encoding properties (you can customize these as needed).
                    barcodeWriter.Format = BarcodeFormat.CODE_128 ; // You can choose different barcode formats.
                    barcodeWriter.Options = new ZXing.Common.EncodingOptions
                    {
                        Width = 100,  // Adjust the width of the barcode image.
                        Height = 170  // Adjust the height of the barcode image.
                    };

                    // Generate the barcode image.
                    pictureBox1.Image = barcodeWriter.Write(combinedData);
                }
                else
                {
                    MessageBox.Show("Failed to generate barcode. Data is empty.");
                }
        }

        // Read a value from the INI file.
        private string ReadIniValue(string section, string key)
        {
            // Get the path to the directory containing the executable.
            string executablePath = AppDomain.CurrentDomain.BaseDirectory;

            // Combine the executable path with the name of your INI file.
            string iniFilePath = Path.Combine(executablePath, "bcgen.ini"); // Specify the path to your INI file.
            string defaultValue = ""; // Default value if key is not found.

            if (File.Exists(iniFilePath))
            {
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(iniFilePath);

                if (data.Sections.ContainsSection(section))
                {
                    if (data[section].ContainsKey(key))
                    {
                        return data[section][key];
                    }
                }
            }

            return defaultValue;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Get the barcode image from the PictureBox.
            Image barcodeImage = pictureBox1.Image;

            // Draw the barcode image on the print page.
            e.Graphics.DrawImage(barcodeImage, new Point(100, 100)); // Adjust the coordinates as needed.

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Display the PrintDialog to allow the user to choose a printer.
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                // Set the selected printer for the PrintDocument.
                printDocument1.PrinterSettings = printDialog1.PrinterSettings;

                // Print the document.
                printDocument1.Print();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
