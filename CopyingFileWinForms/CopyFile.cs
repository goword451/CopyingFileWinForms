using System;
using System.Xml;
using System.Xml.Serialization;

namespace CopyingFileWinForms
{
    [Serializable()]
    public class CopyFile
    {
        [XmlAttribute("source_path")]
        public string SourcePath { get; set; }

        [XmlAttribute("destination_path")]
        public string DestinationPath { get; set; }

        [XmlAttribute("file_name")]
        public string FileName { get; set; }
    }
}
