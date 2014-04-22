using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using TravelingSalesman;
using System.Windows.Threading;
using System.Windows.Shapes;

namespace TravelingSalesmanGui
{
    public class TspViewModel
    {
        private Dispatcher dispatcher;

        public event Action DataChanged;

        public event Action<Edge> EdgeUpdated;

        public TspViewModel(ISolver solver, Dispatcher dispatcher)
        {
            this.Nodes = new ObservableCollection<Node>();
            this.Edges = new Dictionary<int, Edge>();
            this.EdgeLineMap = new Dictionary<Edge, Line>();
            this.dispatcher = dispatcher;
            this.Solver = solver;
            this.SubscribeToEvents();
        }

        public ISolver Solver { get; private set; }

        public ObservableCollection<Node> Nodes { get; set; }

        public Dictionary<int,Edge> Edges { get; set; }

        public Dictionary<Edge, Line> EdgeLineMap { get; set; }

        public double Max_x { get; set; }

        public double Max_y { get; set; }

        public Dispatcher Dispatcher
        {
            get
            {
                return this.dispatcher;
            }
        }

        public void SubscribeToEvents()
        {
            this.Solver.Graph.NodeAdded += new Action<Node>(this.Graph_NodeAdded);
            this.Solver.Graph.EdgeUpdated += new Action<Edge>(this.Graph_EdgeUpdated);
            this.Solver.DataComplete += new Action(this.Graph_DataComplete);
            this.Solver.Graph.RenderNeeded += new Action(this.Graph_DataComplete);
        }

        private void Graph_NodeAdded(Node node)
        {
            this.dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate() { this.OnNodeAdded(node); }));
        }

        private void Graph_EdgeUpdated(Edge edge)
        {
            this.dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate() { this.OnEdgeUpdated(edge); }));
        }

        private void Graph_DataComplete()
        {
            this.dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(this.RaiseDataChanged));
        }

        private void OnNodeAdded(Node node)
        {
            if (this.Max_x < node.X)
            {
                this.Max_x = node.X;
            }
            if (this.Max_y < node.Y)
            {
                this.Max_y = node.Y;
            }
            this.Nodes.Add(node);
            //this.RaiseDataChanged();
        }

        private void OnEdgeUpdated(Edge edge)
        {
            this.Edges[edge.Id] = edge;
            this.RaiseEdgeUpdated(edge);
        }

        private void RaiseDataChanged()
        {
            if (this.DataChanged != null)
            {
                this.DataChanged();
            }
        }

        private void RaiseEdgeUpdated(Edge edge)
        {
            if (this.EdgeUpdated != null)
            {
                this.EdgeUpdated(edge);
            }
        }
    }
}
