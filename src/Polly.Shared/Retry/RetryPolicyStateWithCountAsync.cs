﻿#if SUPPORTS_ASYNC

using System;
using System.Threading.Tasks;

namespace Polly.Retry
{
    internal partial class RetryPolicyStateWithCount : IRetryPolicyState
    {
        public Task<bool> CanRetryAsync(Exception ex, bool continueOnCapturedContext)
        {
            return Task.FromResult(CanRetry(ex));
        }
    }
}

#endif