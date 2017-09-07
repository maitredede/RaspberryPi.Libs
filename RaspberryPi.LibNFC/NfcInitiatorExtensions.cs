using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPi.LibNFC
{
    public static class NfcInitiatorExtensions
    {
        public static Task<NfcInitiatorTarget> PollAsync(this NfcInitiator initiator, NfcModulation[] modulations, byte period, CancellationToken cancellationToken)
        {
            TaskCompletionSource<NfcInitiatorTarget> tcs = new TaskCompletionSource<NfcInitiatorTarget>();
            var data = new PollData
            {
                Initiator = initiator,
                Modulations = modulations,
                Period = period,
                CancellationToken = cancellationToken,
                Tcs = tcs,
            };
            ThreadPool.QueueUserWorkItem(PollAsync, data);
            return tcs.Task;
        }

        private sealed class PollData
        {
            public NfcInitiator Initiator { get; internal set; }
            public NfcModulation[] Modulations { get; internal set; }
            public byte Period { get; internal set; }
            public CancellationToken CancellationToken { get; internal set; }
            public TaskCompletionSource<NfcInitiatorTarget> Tcs { get; internal set; }
        }

        private static void PollAsync(object state)
        {
            PollData data = (PollData)state;
            NfcInitiatorTarget result = null;
            try
            {
                int i = 0;
                while (result == null && !data.CancellationToken.IsCancellationRequested)
                {
                    result = data.Initiator.Poll(new[] { data.Modulations[i] }, 1, data.Period);
                    i = (i + 1) % data.Modulations.Length;
                }
                if (data.CancellationToken.IsCancellationRequested)
                {
                    data.Tcs.TrySetCanceled(data.CancellationToken);
                }
                else
                {
                    data.Tcs.TrySetResult(result);
                }
            }
            catch (Exception ex)
            {
                data.Tcs.TrySetException(ex);
            }
            finally
            {
                data.Tcs.TrySetResult(result);
            }
        }
    }
}
