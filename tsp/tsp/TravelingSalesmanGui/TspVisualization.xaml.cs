using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelingSalesman;
using System.Windows.Threading;

namespace TravelingSalesmanGui
{
    /// <summary>
    /// Interaction logic for TspVisualization.xaml
    /// </summary>
    public partial class TspVisualization : UserControl
    {
        public TspVisualization()
        {
            this.InitializeComponent();
            Loaded += new RoutedEventHandler(this.TspVisualization_Loaded);
        }

        public TspViewModel TspVm { get; private set; }

        public void Setup()
        {
            this.TspVm = this.DataContext as TspViewModel;
            this.SubscribeToEvents();
        }

        public void SubscribeToEvents()
        {
            this.TspVm.DataChanged += new Action(this.TspVm_DataChanged);
            this.TspVm.EdgeUpdated += new Action<Edge>(this.TspVm_EdgeUpdated);
        }

        public void TspVisualization_Loaded(object sender, RoutedEventArgs e)
        {
            //this.DrawEllipse(this.visualizationCanvas.ActualWidth/2, this.visualizationCanvas.ActualHeight/2);
            System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.StartNew<TspGraph>(this.Solve);
            Loaded -= new RoutedEventHandler(this.TspVisualization_Loaded);
        }

        public void DrawEllipse(double x, double y, double width, double height)
        {
            // scale position by max x and y
            double scaledX = x / this.TspVm.Max_x * this.visualizationCanvas.ActualWidth;
            double scaledY = y / this.TspVm.Max_y * this.visualizationCanvas.ActualHeight;

            Ellipse circle1 = new Ellipse();

            // Create a SolidColorBrush with a red color to fill the  
            // Ellipse with.
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            // Describes the brush's color using RGB values.  
            // Each value has a range of 0-255.
            mySolidColorBrush.Color = Color.FromArgb(100, 0, 0, 0);
            circle1.Fill = mySolidColorBrush;
            circle1.StrokeThickness = 2;
            circle1.Stroke = Brushes.Black;

            // Set the width and height of the Ellipse.
            circle1.Width = width;
            circle1.Height = height;

            this.visualizationCanvas.Children.Add(circle1);
            Canvas.SetLeft(circle1, scaledX);
            Canvas.SetBottom(circle1, scaledY);
        }

        public Line DrawLine(double x1, double x2, double y1, double y2)
        {
            Line myLine = this.GetScaledLine(x1, x2, y1, y2);
            this.visualizationCanvas.Children.Add(myLine);
            return myLine;
        }

        private Tuple<double, double, double, double> GetScaledLineCoords(Edge edge)
        {
            return this.GetScaledLineCoords(edge.Start.X, edge.End.X, edge.Start.Y, edge.End.Y);
        }

        private Tuple<double, double, double, double> GetScaledLineCoords(double x1, double x2, double y1, double y2)
        {
            // scale positions by max x and y
            double scaledX1 = x1 / this.TspVm.Max_x * this.visualizationCanvas.ActualWidth;
            double scaledX2 = x2 / this.TspVm.Max_x * this.visualizationCanvas.ActualWidth;
            double scaledY1 = y1 / this.TspVm.Max_y * this.visualizationCanvas.ActualHeight;
            double scaledY2 = y2 / this.TspVm.Max_y * this.visualizationCanvas.ActualHeight;

            double transformedY1 = this.visualizationCanvas.ActualHeight - scaledY1;
            double transformedY2 = this.visualizationCanvas.ActualHeight - scaledY2;

            return new Tuple<double, double, double, double>(scaledX1, scaledX2, transformedY1, transformedY2);
        }

        private Line GetScaledLine(double x1, double x2, double y1, double y2)
        {
            // scale positions by max x and y
            double scaledX1 = x1 / this.TspVm.Max_x * this.visualizationCanvas.ActualWidth;
            double scaledX2 = x2 / this.TspVm.Max_x * this.visualizationCanvas.ActualWidth;
            double scaledY1 = y1 / this.TspVm.Max_y * this.visualizationCanvas.ActualHeight;
            double scaledY2 = y2 / this.TspVm.Max_y * this.visualizationCanvas.ActualHeight;

            double transformedY1 = this.visualizationCanvas.ActualHeight - scaledY1;
            double transformedY2 = this.visualizationCanvas.ActualHeight - scaledY2;

            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine.X1 = scaledX1;
            myLine.X2 = scaledX2;
            myLine.Y1 = transformedY1;
            myLine.Y2 = transformedY2;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.StrokeThickness = 2;

            return myLine;
        }

        private void ModifyEdge(Line line, Edge edge)
        {
            Tuple<double, double, double, double> newLineCoords = this.GetScaledLineCoords(edge);
            line.X1 = newLineCoords.Item1;
            line.X2 = newLineCoords.Item2;
            line.Y1 = newLineCoords.Item3;
            line.Y2 = newLineCoords.Item4;
        }

        public Line DrawEdge(Edge edge)
        {
            return this.DrawLine(edge.Start.X, edge.End.X, edge.Start.Y, edge.End.Y);
        }

        public void DrawNode(Node node)
        {
            this.DrawEllipse(node.X, node.Y);
        }

        public void DrawEllipse(double x, double y)
        {
            this.DrawEllipse(x, y, 10, 10);
        }

        private void TspVm_DataChanged()
        {
            this.Render();
        }

        private void TspVm_EdgeUpdated(Edge edge)
        {
            this.TspVm.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate() { this.ModifyEdge(this.TspVm.EdgeLineMap[edge], edge); }));
        }

        private void Render()
        {
            bool test = true;
            visualizationCanvas.Children.Clear();
            foreach (Node node in this.TspVm.Nodes)
            {
                this.DrawNode(node);
            }
            foreach (Edge edge in this.TspVm.Edges.Values)
            {
                Line drawnEdge = this.DrawEdge(edge);
                this.TspVm.EdgeLineMap[edge] = drawnEdge;
                //if(test)
                //{
                //    this.TspVm.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate() { this.ModifyEdge(drawnEdge); }));
                //}
            }
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Render();
        }

        private TspGraph Solve()
        {
            return this.TspVm.Solver.Solve(InputParser.ParseInput(@"..\..\..\data\tsp_51_1"));
        }

        //private void ModifyEdge(Line prevEdge, Edge newEdge)
        //{
        //    this.visualizationCanvas.Children.Remove(prevEdge);
        //    this.DrawEdge(newEdge);      
        //}
    }
}
