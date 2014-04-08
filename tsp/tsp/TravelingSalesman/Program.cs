using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace TravelingSalesman
{
    internal class MainClass
    {
        /// <summary>
        ///   Solve the specified input.
        /// </summary>
        /// <param name="input"> Input. </param>
        public static SolutionResult Solve(IEnumerable<Node> input)
        {
            // todo: replace the right-hand side with the ISolver you've implemented
            ISolver solver = new ExampleSolver();
            ISolver twoOptSolver = new TwoOptSolver();
            return twoOptSolver.Solve(input);
        }

        /// <summary>
        ///   The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args"> The command-line arguments. </param>
        public static void Main(string[] args)
        {
            // parse the input
            var parsedInput = ParseInput(args);

            // use a solver to solve the problem
            var solution = Solve(parsedInput);
            //Console.WriteLine(solution.ShowEdges());
            //Console.WriteLine(string.Format("Distance: {0}", solution.GetDistance()));
            // print the solution (in the appropriate format) to the Console
            // so that it can be submitted
            Console.Write(solution.ToString());
        }

        /// <summary>
        ///   Parses the input, returning the nodes
        /// </summary>
        /// <returns> The nodes </returns>
        /// <param name="args"> The Command-line arguments. </param>
        public static IEnumerable<Node> ParseInput(string[] args)
        {
            string fileName = null;
            var result = new List<Node>();

            // get the temp file name
            foreach (var arg in args.Where(arg => arg.StartsWith("-file=")))
            {
                fileName = arg.Substring(6);
            }

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