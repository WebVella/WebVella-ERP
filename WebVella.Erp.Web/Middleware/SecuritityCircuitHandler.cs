using Microsoft.AspNetCore.Components.Server.Circuits;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebVella.Erp.Database;

namespace WebVella.Erp.Web.Middleware
{
    public class SecuritityCircuitHandler : CircuitHandler
    {
        private Dictionary<Circuit, Tuple<IDisposable, IDisposable>> contexts = new Dictionary<Circuit, Tuple<IDisposable, IDisposable>>();

        public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            IDisposable dbCtx = DbContext.CreateContext(ErpSettings.ConnectionString);
            IDisposable secCtx = WebVella.Erp.Api.SecurityContext.OpenSystemScope();
            contexts.Add(circuit, new Tuple<IDisposable, IDisposable>(dbCtx, secCtx));
            return Task.CompletedTask;
        }

        public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            if (contexts.ContainsKey(circuit))
            {
                Tuple<IDisposable, IDisposable> tuple = contexts[circuit];
                tuple.Item1.Dispose();
                tuple.Item2.Dispose();
                contexts.Remove(circuit);
            }
            return Task.CompletedTask;
        }

        public int ConnectedCircuits => contexts.Count;
    }
}
