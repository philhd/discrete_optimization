using System.Collections.Generic;
using System;namespace TravelingSalesman{	/// <summary>    /// An interface for a tsp problem solver	/// </summary>	public interface ISolver    {        event Action DataComplete;        TspGraph Solve(IEnumerable<Node> items);

        TspGraph Graph { get; }    }}