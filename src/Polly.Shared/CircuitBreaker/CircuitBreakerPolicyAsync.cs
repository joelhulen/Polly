﻿#if SUPPORTS_ASYNC

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polly.CircuitBreaker
{
    internal partial class CircuitBreakerPolicy
    {
        internal static async Task ImplementationAsync(bool continueOnCapturedContext, Func<Task> action, IEnumerable<ExceptionPredicate> shouldRetryPredicates, ICircuitBreakerState breakerState)
        {
            if (breakerState.IsBroken)
            {
                throw new BrokenCircuitException("The circuit is now open and is not allowing calls.", breakerState.LastException);
            }

            try
            {
                await action().ConfigureAwait(continueOnCapturedContext);

                breakerState.Reset();
            }
            catch (Exception ex)
            {
                if (!shouldRetryPredicates.Any(predicate => predicate(ex)))
                {
                    throw;
                }

                breakerState.TryBreak(ex);

                throw;
            }
        }
    }
}

#endif