// <copyright file="ConsoleTraceService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Yuniql.Extensibility;

namespace Altinn.AccessGroups
{
    /// <summary>
    /// Copied from sample project.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ConsoleTraceService : ITraceService
    {
        /// <summary>
        /// Gets or sets a value indicating whether Debug is enabled or disabled.
        /// </summary>
        public bool IsDebugEnabled { get; set; } = false;

        /// <inheritdoc/>>
        public bool IsTraceSensitiveData { get; set; } = false;

        /// <inheritdoc/>>
        public string TraceToDirectory { get; set; }

        /// <inheritdoc/>>
        public bool IsTraceToFile { get; set; } = false;

        /// <inheritdoc/>>
        bool ITraceService.IsTraceToDirectory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc/>>
        string ITraceService.TraceDirectory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Logging info.
        /// <param name="message">The message to be logged.</paramref>
        /// <param name="payload">The payload.</paramref>
        /// </summary>
        public void Info(string message, object payload = null)
        {
            var traceMessage = $"INF   {DateTime.UtcNow.ToString("o")}   {message}{Environment.NewLine}";
            Console.Write(traceMessage);
        }

        /// <summary>
        /// Logging error.
        /// <param name="message">The message to log.</param>
        /// <param name="payload">The payload.</param>
        /// </summary>
        public void Error(string message, object payload = null)
        {
            var traceMessage = $"ERR   {DateTime.UtcNow.ToString("o")}   {message}{Environment.NewLine}";
            Console.Write(traceMessage);
        }

        /// <summary>
        /// Debug.
        /// <param name="message">The message to log.</param>
        /// <param name="payload">The payload.</param>
        /// </summary>
        public void Debug(string message, object payload = null)
        {
            if (this.IsDebugEnabled)
            {
                var traceMessage = $"DBG   {DateTime.UtcNow.ToString("o")}   {message}{Environment.NewLine}";
                Console.Write(traceMessage);
            }
        }

        /// <summary>
        /// Success.
        /// <param name="message">The message to log.</param>
        /// <param name="payload">The payload.</param>
        /// </summary>
        public void Success(string message, object payload = null)
        {
            var traceMessage = $"INF   {DateTime.UtcNow.ToString("u")}   {message}{Environment.NewLine}";
            Console.Write(traceMessage);
        }

        /// <summary>
        /// Warning.
        /// <param name="message">The message to log.</param>
        /// <param name="payload">The payload.</param>
        /// </summary>
        public void Warn(string message, object payload = null)
        {
            var traceMessage = $"WRN   {DateTime.UtcNow.ToString("o")}   {message}{Environment.NewLine}";
            Console.Write(traceMessage);
        }
    }
}
