using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using WebLedMatrixImageViewer.Models;
using RaspberryPi.Userland;

namespace WebLedMatrixImageViewer.Controllers
{
    public class HomeController : Controller
    {
        private readonly LedMatrixManager m_matrixManager;

        public HomeController(LedMatrixManager ledmatrix)
        {
            this.m_matrixManager = ledmatrix;
        }

        public IActionResult Index()
        {
            HomeIndexModel model = new HomeIndexModel()
            {
                Brightness = this.m_matrixManager.Brightness,
            };

            return this.View(model);
        }

        private void SetModelData(HomeIndexModel model)
        {
            model.CpuTemp = MetricsHelper.CpuTemp();
            model.GpuTemp = MetricsHelper.GpuTemp();
        }

        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> CurrentImage()
        {
            var data = await this.m_matrixManager.GetCurrentImage();
            if (data.Data == null)
            {
                return this.NotFound();
            }
            return this.File(data.Data, data.Type);
        }

        [HttpPost]
        public async Task<IActionResult> Set(HomeIndexModel model)
        {
            if (!this.ModelState.IsValid)
            {
                if (model == null)
                    model = new HomeIndexModel();
                this.SetModelData(model);
                return this.View("Index", model);
            }
            if (model.File != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await model.File.CopyToAsync(ms);
                    await this.m_matrixManager.SetImage(ms.ToArray(), model.File.ContentType);
                }
            }
            await this.m_matrixManager.SetBrightness(model.Brightness);
                this.SetModelData(model);
            return this.View("Index", model);
        }
    }
}