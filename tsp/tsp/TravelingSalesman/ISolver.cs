using System.Collections.Generic;namespace TravelingSalesman{	/// <summary>    /// An interface for a tsp problem solver	/// </summary>	public interface ISolver    {        TspGraph Solve(IEnumerable<Node> items);

        TspGraph Graph { get; }    }}