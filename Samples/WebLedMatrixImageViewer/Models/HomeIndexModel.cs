using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLedMatrixImageViewer.Models
{
    public class HomeIndexModel
    {
        [Range(1, 100)]
        public int Brightness { get; set; }

        public IFormFile File { get; set; }
        public double CpuTemp { get; internal set; }
        public double GpuTemp { get; internal set; }
    }
}
