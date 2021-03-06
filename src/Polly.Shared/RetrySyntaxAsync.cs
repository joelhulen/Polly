﻿#if SUPPORTS_ASYNC

using System;
using System.Collections.Generic;
using System.Linq;
using Polly.Retry;

namespace Polly
{
    /// <summary>
    ///     Fluent API for defining a Retry <see cref="Policy" />.
    /// </summary>
    public static partial class RetrySyntax
    {
        /// <summary>
        /// Builds a <see cref="Policy"/> that will retry once.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <returns>The policy instance.</returns>
        public static Policy RetryAsync(this PolicyBuilder policyBuilder)
        {
            return RetryAsync(policyBuilder, false);
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will retry once.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The policy instance.</returns>
        public static Policy RetryAsync(this PolicyBuilder policyBuilder, bool continueOnCapturedContext)
        {
            return policyBuilder.RetryAsync(1, continueOnCapturedContext);
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will retry <paramref name="retryCount" /> times.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <returns>The policy instance.</returns>
        public static Policy RetryAsync(this PolicyBuilder policyBuilder, int retryCount)
        {
            return RetryAsync(policyBuilder, retryCount, false);
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will retry <paramref name="retryCount" /> times.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The policy instance.</returns>
        public static Policy RetryAsync(this PolicyBuilder policyBuilder, int retryCount, bool continueOnCapturedContext)
        {
            Action<Exception, int> doNothing = (_, __) => { };

            return policyBuilder.RetryAsync(retryCount, continueOnCapturedContext, doNothing);
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will retry once
        ///     calling <paramref name="onRetry" /> on retry with the raised exception and retry count.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">retryCount;Value must be greater than zero.</exception>
        /// <exception cref="System.ArgumentNullException">onRetry</exception>
        public static Policy RetryAsync(this PolicyBuilder policyBuilder, Action<Exception, int> onRetry)
        {
            return RetryAsync(policyBuilder, false, onRetry);
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will retry once
        ///     calling <paramref name="onRetry" /> on retry with the raised exception and retry count.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">retryCount;Value must be greater than zero.</exception>
        /// <exception cref="System.ArgumentNullException">onRetry</exception>
        public static Policy RetryAsync(this PolicyBuilder policyBuilder, bool continueOnCapturedContext, Action<Exception, int> onRetry)
        {
            return policyBuilder.RetryAsync(1, continueOnCapturedContext, onRetry);
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will retry <paramref name="retryCount" /> times
        ///     calling <paramref name="onRetry" /> on each retry with the raised exception and retry count.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">retryCount;Value must be greater than zero.</exception>
        /// <exception cref="System.ArgumentNullException">onRetry</exception>
        public static Policy RetryAsync(this PolicyBuilder policyBuilder, int retryCount, Action<Exception, int> onRetry)
        {
            return RetryAsync(policyBuilder, retryCount, false, onRetry);
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will retry <paramref name="retryCount" /> times
        ///     calling <paramref name="onRetry" /> on each retry with the raised exception and retry count.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">retryCount;Value must be greater than zero.</exception>
        /// <exception cref="System.ArgumentNullException">onRetry</exception>
        public static Policy RetryAsync(this PolicyBuilder policyBuilder, int retryCount, bool continueOnCapturedContext, Action<Exception, int> onRetry)
        {
            if (retryCount < 0)
                throw new ArgumentOutOfRangeException("retryCount", "Value must be equal to or greater than zero.");
            if (onRetry == null) throw new ArgumentNullException("onRetry");

            return new Policy(
                action => RetryPolicy.ImplementationAsync(
                    action,
                    policyBuilder.ExceptionPredicates,
                    () => new RetryPolicyStateWithCount(retryCount, onRetry), 
                    continueOnCapturedContext),
                policyBuilder.ExceptionPredicates
            );
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will retry indefinitely.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <returns>The policy instance.</returns>
        public static Policy RetryForeverAsync(this PolicyBuilder policyBuilder)
        {
            return RetryForeverAsync(policyBuilder, false);
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will retry indefinitely.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The policy instance.</returns>
        public static Policy RetryForeverAsync(this PolicyBuilder policyBuilder, bool continueOnCapturedContext)
        {
            Action<Exception> doNothing = _ => { };

            return policyBuilder.RetryForeverAsync(continueOnCapturedContext, doNothing);
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will retry indefinitely
        ///     calling <paramref name="onRetry" /> on each retry with the raised exception.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="System.ArgumentNullException">onRetry</exception>
        public static Policy RetryForeverAsync(this PolicyBuilder policyBuilder, Action<Exception> onRetry)
        {
            return RetryForeverAsync(policyBuilder, false, onRetry);
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will retry indefinitely
        ///     calling <paramref name="onRetry" /> on each retry with the raised exception.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="System.ArgumentNullException">onRetry</exception>
        public static Policy RetryForeverAsync(this PolicyBuilder policyBuilder, bool continueOnCapturedContext, Action<Exception> onRetry)
        {
            if (onRetry == null) throw new ArgumentNullException("onRetry");

            return new Policy(
                action => RetryPolicy.ImplementationAsync(
                    action,
                    policyBuilder.ExceptionPredicates,
                    () => new RetryPolicyState(onRetry), 
                    continueOnCapturedContext),
                policyBuilder.ExceptionPredicates
            );
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will wait and retry as many times as there are provided
        ///     <paramref name="sleepDurations" />
        ///     On each retry, the duration to wait is the current <paramref name="sleepDurations" /> item.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurations">The sleep durations to wait for on each retry.</param>
        /// <returns>The policy instance.</returns>
        public static Policy WaitAndRetryAsync(this PolicyBuilder policyBuilder, IEnumerable<TimeSpan> sleepDurations)
        {
            return WaitAndRetryAsync(policyBuilder, sleepDurations, false);
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will wait and retry as many times as there are provided
        ///     <paramref name="sleepDurations" />
        ///     On each retry, the duration to wait is the current <paramref name="sleepDurations" /> item.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurations">The sleep durations to wait for on each retry.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The policy instance.</returns>
        public static Policy WaitAndRetryAsync(this PolicyBuilder policyBuilder, IEnumerable<TimeSpan> sleepDurations, bool continueOnCapturedContext)
        {
            Action<Exception, TimeSpan> doNothing = (_, __) => { };

            return policyBuilder.WaitAndRetryAsync(sleepDurations, continueOnCapturedContext, doNothing);
        }

        /// <summary>
        ///     Builds a <see cref="Policy"/> that will wait and retry <paramref name="retryCount"/> times.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider"/> with
        ///     the current retry attempt allowing an exponentially increasing wait time (exponential backoff).
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <returns>The policy instance.</returns>
        public static Policy WaitAndRetryAsync(this PolicyBuilder policyBuilder, int retryCount, Func<int, TimeSpan> sleepDurationProvider)
        {
            return WaitAndRetryAsync(policyBuilder, retryCount, sleepDurationProvider, false);
        }

        /// <summary>
        ///     Builds a <see cref="Policy"/> that will wait and retry <paramref name="retryCount"/> times.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider"/> with
        ///     the current retry attempt allowing an exponentially increasing wait time (exponential backoff).
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The policy instance.</returns>
        public static Policy WaitAndRetryAsync(this PolicyBuilder policyBuilder, int retryCount, Func<int, TimeSpan> sleepDurationProvider, bool continueOnCapturedContext)
        {
            Action<Exception, TimeSpan> doNothing = (_, __) => { };

            return policyBuilder.WaitAndRetryAsync(retryCount, sleepDurationProvider, continueOnCapturedContext, doNothing);
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will wait and retry <paramref name="retryCount" /> times
        ///     calling <paramref name="onRetry" /> on each retry with the raised exception and the current sleep duration.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with
        ///     the current retry attempt allowing an exponentially increasing wait time (exponential backoff).
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">retryCount;Value must be greater than zero.</exception>
        /// <exception cref="System.ArgumentNullException">
        ///     timeSpanProvider
        ///     or
        ///     onRetry
        /// </exception>
        public static Policy WaitAndRetryAsync(this PolicyBuilder policyBuilder, int retryCount, Func<int, TimeSpan> sleepDurationProvider, Action<Exception, TimeSpan> onRetry)
        {
            return WaitAndRetryAsync(policyBuilder, retryCount, sleepDurationProvider, false, onRetry);
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will wait and retry <paramref name="retryCount" /> times
        ///     calling <paramref name="onRetry" /> on each retry with the raised exception and the current sleep duration.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with
        ///     the current retry attempt allowing an exponentially increasing wait time (exponential backoff).
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">retryCount;Value must be greater than zero.</exception>
        /// <exception cref="System.ArgumentNullException">
        ///     timeSpanProvider
        ///     or
        ///     onRetry
        /// </exception>
        public static Policy WaitAndRetryAsync(this PolicyBuilder policyBuilder, int retryCount, Func<int, TimeSpan> sleepDurationProvider, bool continueOnCapturedContext, Action<Exception, TimeSpan> onRetry)
        {
            if (retryCount < 0)
                throw new ArgumentOutOfRangeException("retryCount", "Value must be equal to or greater than zero.");
            if (sleepDurationProvider == null) throw new ArgumentNullException("sleepDurationProvider");
            if (onRetry == null) throw new ArgumentNullException("onRetry");

            IEnumerable<TimeSpan> sleepDurations = Enumerable.Range(1, retryCount)
                .Select(sleepDurationProvider);

            return new Policy(
                action => RetryPolicy.ImplementationAsync(
                    action,
                    policyBuilder.ExceptionPredicates,
                    () => new RetryPolicyStateWithSleep(sleepDurations, onRetry), 
                    continueOnCapturedContext),
                policyBuilder.ExceptionPredicates
            );
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will wait and retry as many times as there are provided
        ///     <paramref name="sleepDurations" />
        ///     calling <paramref name="onRetry" /> on each retry with the raised exception and the current sleep duration.
        ///     On each retry, the duration to wait is the current <paramref name="sleepDurations" /> item.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurations">The sleep durations to wait for on each retry.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     sleepDurations
        ///     or
        ///     onRetry
        /// </exception>
        public static Policy WaitAndRetryAsync(this PolicyBuilder policyBuilder, IEnumerable<TimeSpan> sleepDurations, Action<Exception, TimeSpan> onRetry)
        {
            return WaitAndRetryAsync(policyBuilder, sleepDurations, false, onRetry);
        }

        /// <summary>
        ///     Builds a <see cref="Policy" /> that will wait and retry as many times as there are provided
        ///     <paramref name="sleepDurations" />
        ///     calling <paramref name="onRetry" /> on each retry with the raised exception and the current sleep duration.
        ///     On each retry, the duration to wait is the current <paramref name="sleepDurations" /> item.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurations">The sleep durations to wait for on each retry.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     sleepDurations
        ///     or
        ///     onRetry
        /// </exception>
        public static Policy WaitAndRetryAsync(this PolicyBuilder policyBuilder, IEnumerable<TimeSpan> sleepDurations, bool continueOnCapturedContext, Action<Exception, TimeSpan> onRetry)
        {
            if (sleepDurations == null) throw new ArgumentNullException("sleepDurations");
            if (onRetry == null) throw new ArgumentNullException("onRetry");

            return new Policy(
                action => RetryPolicy.ImplementationAsync(
                    action,
                    policyBuilder.ExceptionPredicates,
                    () => new RetryPolicyStateWithSleep(sleepDurations, onRetry), 
                    continueOnCapturedContext),
                policyBuilder.ExceptionPredicates
            );
        }
    }
}

#endif