using System;
using System.Xml;
using System.Xml.Serialization;

namespace CopyingFileWinForms
{
    [Serializable()]
    [XmlRoot("config")]
    public class ConfigCollection
    {
        [XmlElement("file", typeof(CopyFile))]
        public CopyFile[] FilesForCopy { get; set; }
    }
}
