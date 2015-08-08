using System.Collections.Generic;
using System;
using System.ComponentModel;namespace TravelingSalesman{	/// <summary>    /// An interface for a tsp problem solver	/// </summary>	public interface ISolver : INotifyPropertyChanged    {        event Action DataComplete;

        string SolverInfo { get; }        TspGraph Solve(IEnumerable<Node> items);

        TspGraph Graph { get; }    }}