using UnityEditor;
using UnityEngine;

using System.Linq;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

// command line utils https://github.com/commandlineparser/commandline
using CommandLine;
using CommandLine.Text;

namespace WorldOfBugs {

    public static class CommandLineExtensions {

        /// <summary>
        /// Throws errors if they occur when parsing commandline arguments.
        /// </summary>
        /// <param name="result">Result of ParseArguments using a CommandLine Parser</param>
        /// <typeparam name="T">Class containing CommandLine attributes</typeparam>
        public static void ThrowOnParseError<T>(this ParserResult<T> result) {
            result.WithNotParsed<T>(_ => {
                var builder = SentenceBuilder.Create();
                var errorMessages = HelpText.RenderParsingErrorsTextAsLines(result, builder.FormatError,
                                    builder.FormatMutuallyExclusiveSetErrors, 1);
                var excList = errorMessages.Select(msg => new ArgumentException(msg)).ToList();

                if(excList.Any()) {
                    throw new AggregateException(excList);
                }
            });
        }
    }

}
