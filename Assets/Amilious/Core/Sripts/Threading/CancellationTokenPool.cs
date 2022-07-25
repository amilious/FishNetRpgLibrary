using System.Threading;
using System.Collections.Generic;

namespace Amilious.Core.Threading {
    
    public static class CancellationTokenPool {

        private static readonly Queue<CancellationTokenSource> Tokens = new();

        public static CancellationTokenSource GetToken() {
            return Tokens.TryDequeue(out var token) ? token : new CancellationTokenSource();
        }

        public static void ReturnToken(CancellationTokenSource tokenSource) {
            if(tokenSource.IsCancellationRequested) {
                tokenSource.Dispose();
                return;
            };
            Tokens.Enqueue(tokenSource);
        }

        public static void ReturnToPool(this CancellationTokenSource token) {
            ReturnToken(token);
        }
        
    }
    
}