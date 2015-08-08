using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace TravelingSalesman
{
    public static class InputParser
    {
        /// <summary>
        ///   Parses the input, returning the nodes
        /// </summary>
        /// <returns> The nodes </returns>
        /// <param name="args"> The Command-line arguments. </param>
        public static IEnumerable<Node> ParseInput(string[] args)
        {
            string fileName = null;

            // get the temp file name
            foreach (var arg in args.Where(arg => arg.StartsWith("-file=")))
            {
                fileName = arg.Substring(6);
            }



            return ParseInput(fileName);
        }

        public static IEnumerable<Node> ParseInput(string fileName)
        {
            var result = new List<Node>();

            if (fileName == null)
                return result;

            // read the lines out of the file
            var lines = File.ReadAllLines(fileName);

            // parse the data in the file
            var firstLine = lines[0].Split(' ');
            int nodeCount = int.Parse(firstLine[0]);

            for (int i = 1; i < nodeCount + 1; i++)
            {
                var line = lines[i];
                var parts = line.Split(' ');
                var x = double.Parse(parts[0], CultureInfo.InvariantCulture);
                var y = double.Parse(parts[1], CultureInfo.InvariantCulture);

                // add the node
                result.Add(new Node(i - 1, x, y));
            }
            return result;
        }
    }
}
