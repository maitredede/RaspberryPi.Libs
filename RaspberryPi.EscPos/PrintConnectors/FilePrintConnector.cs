using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RaspberryPi.EscPos.PrintConnectors
{
    public sealed class FilePrintConnector : StreamConnector<FileStream>
    {
        public FilePrintConnector(string filename) :
            base(File.Open(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
        {
        }
    }
}
