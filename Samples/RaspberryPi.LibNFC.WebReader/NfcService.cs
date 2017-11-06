using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using RaspberryPi.LibFreefare;

namespace RaspberryPi.LibNFC.WebReader
{
    public class NfcService : HostedService
    {
        private readonly HubContext<NfcReaderHub, INfcReaderHubClient> m_hub;
        private readonly ILogger<NfcService> m_logger;

        public NfcService(HubContext<NfcReaderHub, INfcReaderHubClient> hub, ILogger<NfcService> logger)
        {
            this.m_hub = hub;
            this.m_logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            this.m_logger.LogInformation("Started nfcService");
            //byte pollNr = 20;
            //byte period = 2;
            //var modulations = new[]
            //{
            //    new NfcModulation(NfcModulationType.ISO14443A, NfcBaudRate.BR106),
            //    new NfcModulation(NfcModulationType.ISO14443B, NfcBaudRate.BR106),
            //    new NfcModulation(NfcModulationType.Felica, NfcBaudRate.BR212),
            //    new NfcModulation(NfcModulationType.Felica, NfcBaudRate.BR424),
            //    new NfcModulation(NfcModulationType.Jewel, NfcBaudRate.BR106),
            //};


            NfcModulation mod = new NfcModulation(NfcModulationType.ISO14443A, NfcBaudRate.BR106);

            using (NfcContext ctx = new NfcContext())
            using (var device = ctx.Open(null))
            {
                var initiator = device.InitInitiator(false);

                while (!cancellationToken.IsCancellationRequested)
                {
                    var tag = initiator.SelectMifareTag();
                    if (tag == null)
                    {
                        await Task.Delay(100, cancellationToken);
                        continue;
                    }

                    using (tag)
                    {
                        this.m_logger.LogInformation("Card detected");
                        this.m_hub.All.cardOn();
                        while (tag.IsPresent() && !cancellationToken.IsCancellationRequested)
                        {
                            await Task.Delay(100, cancellationToken);
                        }
                        this.m_logger.LogInformation("Card removed");
                        this.m_hub.All.cardOff();
                    }
                    await Task.Delay(1000, cancellationToken);
                }
            }
            this.m_logger.LogInformation("Stopped nfcService");
        }
    }
}
