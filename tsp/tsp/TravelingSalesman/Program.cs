using System;
using System.Collections.Generic;

namespace TravelingSalesman
{
    internal class MainClass
    {
        /// <summary>
        ///   Solve the specified input.
        /// </summary>
        /// <param name="input"> Input. </param>
        public static TspGraph Solve(IEnumerable<Node> input)
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
            var parsedInput = InputParser.ParseInput(args);

            // use a solver to solve the problem
            var solution = Solve(parsedInput);
            //Console.WriteLine(solution.ShowEdges());
            //Console.WriteLine(string.Format("Distance: {0}", solution.GetDistance()));
            // print the solution (in the appropriate format) to the Console
            // so that it can be submitted
            Console.Write(solution.ToString());
        }
    }
}