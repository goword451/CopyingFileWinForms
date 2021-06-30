using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CopyingFileWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            openFileDialog1.Filter = "Configuration file(*.xml)|*.xml";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            MessageBox.Show("Файл загружен");
        }

        private async void Button2_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                int copyCount = 0;
                int filesCount = 0;

                StringBuilder sb = new StringBuilder();

                XDocument xDoc = XDocument.Load(openFileDialog1.FileName);

                foreach (XElement fileElement in xDoc.Element("config").Elements("file"))
                {
                    filesCount++;
                }

                progressBar1.Maximum = filesCount;

                sb.AppendLine(DateTime.Now.ToString());

                foreach (XElement fileElement in xDoc.Element("config").Elements("file"))
                {
                    XAttribute sourcePath = fileElement.Attribute("source_path");
                    XAttribute destinationPath = fileElement.Attribute("destination_path");
                    XAttribute fileName = fileElement.Attribute("file_name");

                    if (!string.IsNullOrWhiteSpace(sourcePath?.Value) && Directory.Exists(@"" + sourcePath.Value) &&
                        !string.IsNullOrWhiteSpace(destinationPath?.Value) && Directory.Exists(@"" + destinationPath.Value) &&
                        !string.IsNullOrWhiteSpace(fileName?.Value) && File.Exists(Path.Combine(sourcePath.Value, fileName.Value)))
                    {

                        CopyFile(sourcePath, destinationPath, fileName);
                        label1.Text = $"Файл {fileName.Value} успешно скопирован.";
                        sb.AppendLine($"Файл {fileName.Value} успешно скопирован.");
                        progressBar1.Value++;
                        copyCount++;
                    }

                    else if (sourcePath == null || string.IsNullOrWhiteSpace(sourcePath.Value) || !Directory.Exists(@"" + sourcePath.Value))
                    {
                        sb.AppendLine($"Отсутствует путь источника {sourcePath.Value}.");
                        progressBar1.Value++;
                    }

                    else if (destinationPath == null || string.IsNullOrWhiteSpace(destinationPath.Value) || !Directory.Exists(@"" + destinationPath.Value))
                    {
                        sb.AppendLine($"Отсутствует путь назначения {destinationPath.Value}.");
                        progressBar1.Value++;
                    }

                    else
                    {
                        sb.AppendLine($"Отсутствует файл с именем {fileName.Value}.");
                        progressBar1.Value++;
                    }
                }
                File.AppendAllText(@"C:\Users\gowor\Desktop\log.txt", sb.ToString());
                sb.Clear();
                await Task.Delay(1000);
                MessageBox.Show($"Копирование завершено. Успешно скопировано {copyCount} из {filesCount} файлов.");
                progressBar1.Value = 0;
            }
            catch (XmlException)
            {
                MessageBox.Show($"Копирование не произведено. Отсутствует корневой элемент в файле конфигурации.");
                return;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void CopyFile(XAttribute sourcePath, XAttribute destinationPath, XAttribute fileName)
        {
            try
            {
                File.Copy(Path.Combine(sourcePath.Value, fileName.Value), Path.Combine(destinationPath.Value, fileName.Value));
            }
            catch (Exception)
            {
                File.Copy(Path.Combine(sourcePath.Value, fileName.Value), Path.Combine(destinationPath.Value, fileName.Value), true);
            }
        }
    }
}
