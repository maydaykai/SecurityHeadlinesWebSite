using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace APICloud.Rest
{
    public class UploadFile
    {
        public string Name { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public FileStream Stream { get; set; }
    }
}
