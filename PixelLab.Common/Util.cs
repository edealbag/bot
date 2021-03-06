﻿using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace PixelLab.Common
{
    /// <summary>
    ///     Contains general helper methods.
    /// </summary>
    public static class Util
    {
        #region Methods to verify conditions (Require)

        /// <summary>
        ///     If <paramref name="condition"/> is false, throw an empty <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="condition">The 'truth' to evaluate.</param>
        [DebuggerStepThrough]
        public static void Require(bool condition)
        {
            if (!condition)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     If <paramref name="condition"/> is false, throw an 
        ///     <see cref="InvalidOperationException"/> with the provided <paramref name="message"/>.
        /// </summary>
        /// <param name="condition">The 'truth' to evaluate.</param>
        /// <param name="message">
        ///     The <see cref="Exception.Message"/> if 
        ///     <paramref name="condition"/> is false.
        /// </param>
        [DebuggerStepThrough]
        public static void Require(bool condition, string message)
        {
            Contract.Requires(!message.IsNullOrWhiteSpace());
            Contract.Requires(!message.IsNullOrWhiteSpace());
            if (!condition)
            {
                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        ///     If <paramref name="condition"/> is false, throws 
        ///     <paramref name="exception"/>.    
        /// </summary>
        /// <param name="condition">The 'truth' to evaluate.</param>
        /// <param name="exception">
        ///     The <see cref="Exception"/> to throw if <paramref name="condition"/> is false.
        /// </param>
        [DebuggerStepThrough]
        public static void Require(bool condition, Exception exception)
        {
            Contract.Requires(exception != null);
            if (!condition)
            {
                throw exception;
            }
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentNullException"/> if the
        ///     provided string is null.
        ///     Throws an <see cref="ArgumentOutOfRangeException"/> if the
        ///     provided string is empty.
        /// </summary>
        /// <param name="stringParameter">The object to test for null and empty.</param>
        /// <param name="parameterName">The string for the ArgumentException parameter, if thrown.</param>
        [DebuggerStepThrough]
        public static void RequireNotNullOrEmpty(string stringParameter, string parameterName)
        {
            if (stringParameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            else if (stringParameter.Length == 0)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentNullException"/> if the
        ///     provided object is null.
        /// </summary>
        /// <param name="obj">The object to test for null.</param>
        /// <param name="parameterName">The string for the ArgumentNullException parameter, if thrown.</param>
        [DebuggerStepThrough]
        public static void RequireNotNull(object obj, string parameterName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentException"/> if the provided truth is false.
        /// </summary>
        /// <param name="truth">The value assumed to be true.</param>
        /// <param name="parameterName">The string for <see cref="ArgumentException"/>, if thrown.</param>
        [DebuggerStepThrough]
        public static void RequireArgument(bool truth, string parameterName)
        {
            Contract.Requires(!parameterName.IsNullOrWhiteSpace());

            if (!truth)
            {
                throw new ArgumentException(parameterName);
            }
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentException"/> if the provided truth is false.
        /// </summary>
        /// <param name="truth">The value assumed to be true.</param>
        /// <param name="paramName">The paramName for the <see cref="ArgumentException"/>, if thrown.</param>
        /// <param name="message">The message for <see cref="ArgumentException"/>, if thrown.</param>
        [DebuggerStepThrough]
        public static void RequireArgument(bool truth, string paramName, string message)
        {
            Contract.Assume(!paramName.IsNullOrWhiteSpace());
            Contract.Assume(!message.IsNullOrWhiteSpace());

            if (!truth)
            {
                throw new ArgumentException(message, paramName);
            }
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentOutOfRangeException"/> if the provided truth is false.
        /// </summary>
        /// <param name="truth">The value assumed to be true.</param>
        /// <param name="parameterName">The string for <see cref="ArgumentOutOfRangeException"/>, if thrown.</param>
        [DebuggerStepThrough]
        public static void RequireArgumentRange(bool truth, string parameterName)
        {
            Contract.Requires(!parameterName.IsNullOrWhiteSpace());
            if (!truth)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentOutOfRangeException"/> if the provided truth is false.
        /// </summary>
        /// <param name="truth">The value assumed to be true.</param>
        /// <param name="paramName">The paramName for the <see cref="ArgumentOutOfRangeException"/>, if thrown.</param>
        /// <param name="message">The message for <see cref="ArgumentOutOfRangeException"/>, if thrown.</param>
        [DebuggerStepThrough]
        public static void RequireArgumentRange(bool truth, string paramName, string message)
        {
            Contract.Requires(!paramName.IsNullOrWhiteSpace());
            Contract.Requires(!message.IsNullOrWhiteSpace());

            if (!truth)
            {
                throw new ArgumentOutOfRangeException(message, paramName);
            }
        }

        #endregion

        /// <summary>
        /// Returns an hash aggregation of an array of elements.
        /// </summary>
        /// <param name="items">An array of elements from which to create a hash.</param>
        public static int GetHashCode(params object[] items)
        {
            if (items == null)
            {
                return 0;
            }
            else
            {
                return items
                    .Select(item => (item == null) ? 0 : item.GetHashCode())
                    .Aggregate(0, (current, next) => current ^ next);
            }
        }

        /// <summary>
        ///     Wraps <see cref="Interlocked.CompareExchange{T}(ref T,T,T)"/> 
        ///     for atomically setting null fields.
        /// </summary>
        /// <typeparam name="T">The type of the field to set.</typeparam>
        /// <param name="location">
        ///     The field that, if null, will be set to <paramref name="value"/>.
        /// </param>
        /// <param name="value">
        ///     If <paramref name="location"/> is null, the object to set it to.
        /// </param>
        /// <returns>true if <paramref name="location"/> was null and has now been set; otherwise, false.</returns>
        public static bool InterlockedSetIfNotNull<T>(ref T location, T value) where T : class
        {
            Contract.Requires(value != null);

            // Strictly speaking, this null check is not nessesary, but
            // while CompareExchange is fast, it's still much slower than a 
            // null check. 
            if (location == null)
            {
                // This is a paranoid method. In a multi-threaded environment, it's possible
                // for two threads to get through the null check before a value is set.
                // This makes sure than one and only one value is set to field.
                // This is super important if the field is used in locking, for instance.

                T valueWhenSet = Interlocked.CompareExchange<T>(ref location, value, null);
                return (valueWhenSet == null);
            }
            else
            {
                return false;
            }
        }

        /// Returns true if the provided <see cref="Exception"/> is considered 'critical'
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to evaluate for critical-ness.</param>
        /// <returns>true if the Exception is conisdered critical; otherwise, false.</returns>
        /// <remarks>
        /// These exceptions are consider critical:
        /// <list type="bullets">
        ///     <item><see cref="OutOfMemoryException"/></item>
        ///     <item><see cref="StackOverflowException"/></item>
        ///     <item><see cref="ThreadAbortException"/></item>
        ///     <item><see cref="SEHException"/></item>
        /// </list>
        /// </remarks>
        public static bool IsCriticalException(this Exception exception)
        {
            Contract.Requires(exception != null);
            // Copied with respect from WPF WindowsBase->MS.Internal.CriticalExceptions.IsCriticalException
            // NullReferencException, SecurityException --> not going to consider these critical
            return exception is OutOfMemoryException ||
                    exception is StackOverflowException ||
                    exception is ThreadAbortException
#if !WP7
 || exception is SEHException
#endif
;
        } //*** static IsCriticalException

        public static Random Rnd
        {
            get
            {
                Contract.Ensures(Contract.Result<Random>() != null);
                var r = (Random)s_random.Target;
                if (r == null)
                {
                    s_random.Target = r = new Random();
                }
                return r;
            }
        }

        private static readonly WeakReference s_random = new WeakReference(null);


    } //*** public class Util

} //*** namespace PixelLab.Common