using System;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NLog;

namespace CopyingFileWinForms
{
    public partial class MainForm : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static ConfigCollection ConfigCollection { get; set; }

        public MainForm()
        {
            InitializeComponent();

            FileDialog.Filter = "Configuration file(*.xml)|*.xml";
        }

        private void ChooseFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileDialog.ShowDialog() == DialogResult.OK)
                {
                    ConfigCollection = GetConfigCollection();
                    if (ConfigCollection.FilesForCopy.Length == 0)
                    {
                        return;
                    }
                    MessageBox.Show(Messages.Success.SucessfullyLoaded);
                    CopyButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(String.Format(Messages.Error.NotLoadedErrorForLog, ex.Message));
                MessageBox.Show(Messages.Error.NotLoadedError);
                return;
            }
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            int resultCounter = 0;
            int copiedFiles = 0;

            try
            {
                ConfigCollection configCollection = new ConfigCollection();

                XmlSerializer serializer = new XmlSerializer(typeof(ConfigCollection));

                StreamReader reader = new StreamReader(FileDialog.FileName);

                configCollection = (ConfigCollection)serializer.Deserialize(reader);

                reader.Close();

                progressBar1.Maximum = ConfigCollection.FilesForCopy.Length;

                foreach (var item in configCollection.FilesForCopy)
                {
                    if (!ValidatePath(item.SourcePath))
                    {
                        ResultsListBox.Items.Insert(resultCounter, String.Format(Messages.Info.AbsentSourcePath, item.SourcePath));
                        resultCounter++;
                        progressBar1.Value++;
                        continue;
                    }
                    else if (!ValidatePath(item.DestinationPath))
                    {
                        ResultsListBox.Items.Insert(resultCounter, String.Format(Messages.Info.AbsentDestinationPath, item.DestinationPath));                        
                        progressBar1.Value++;
                        continue;
                    }
                    else if (!ValidateFile(item.SourcePath, item.FileName))
                    {
                        ResultsListBox.Items.Insert(resultCounter, String.Format(Messages.Info.AbsentDestinationPath, item.FileName));
                        resultCounter++;
                        progressBar1.Value++;
                        continue;
                    }

                    var sourcePath = Path.Combine(item.SourcePath, item.FileName);
                    var destinationPath = Path.Combine(item.DestinationPath, item.FileName);
                    File.Copy(sourcePath, destinationPath, true);
                    ResultsListBox.Items.Insert(resultCounter, String.Format(Messages.Success.SucessfullyDone, item.FileName));
                    resultCounter++;
                    copiedFiles++;
                    progressBar1.Value++;
                }
                MessageBox.Show(String.Format(Messages.Success.SucessfullyCopied, copiedFiles, configCollection.FilesForCopy.Length));
                progressBar1.Value = 0;
            }
            catch (XmlException)
            {
                MessageBox.Show(Messages.Error.NotCopiedError);
                return;
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private bool ValidatePath(string path)
        {
            return !string.IsNullOrWhiteSpace(path) && Directory.Exists($"" + path);
        }

        private bool ValidateFile(string sourcePath, string fileName)
        {
            return File.Exists(Path.Combine(sourcePath, fileName));
        }

        private ConfigCollection GetConfigCollection()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(ConfigCollection));
                using (var reader = new StreamReader(FileDialog.FileName))
                {
                    return (ConfigCollection)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                logger.Error(String.Format(Messages.Error.DeserializeErrorForLog, ex.Message));
                MessageBox.Show(Messages.Error.DeserializeError, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new ConfigCollection
                {
                    FilesForCopy = Array.Empty<CopyFile>(),
                };
            }
        }

    }
}
