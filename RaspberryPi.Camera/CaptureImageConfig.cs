using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RaspberryPi.Camera
{
    public class CaptureImageConfig
    {
        public bool FlipH { get; set; } = false;
        public bool FlipV { get; set; } = false;
        public bool Preview { get; set; } = false;
        public Rectangle PreviewRect { get; set; } = new Rectangle(0, 0, 640, 480);
        public byte Opacity { get; set; } = 255;
        public CameraRotation Rotate { get; set; } = CameraRotation.NoRotate;
        public string AdditionnalParams { get; set; }
        internal string OutputFile { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("-t 1 ");
            if (this.FlipH) sb.Append("--hflip ");
            if (this.FlipV) sb.Append("--vflip ");
            if (this.Preview)
                sb.AppendFormat("--preview {0},{1},{2},{3} ", this.PreviewRect.X, this.PreviewRect.Y, this.PreviewRect.Width, this.PreviewRect.Height);
            else
                sb.Append("--nopreview ");
            if (this.Opacity != 255)
                sb.AppendFormat("--opacity {0} ", this.Opacity);
            if (this.Rotate != CameraRotation.NoRotate)
                sb.AppendFormat("--rotation {0} ", (int)this.Rotate);

            if (!string.IsNullOrEmpty(this.OutputFile))
                sb.AppendFormat("--output \"{0}\" ", this.OutputFile);

            sb.Append(this.AdditionnalParams);

            return sb.ToString();
        }
    }
}
