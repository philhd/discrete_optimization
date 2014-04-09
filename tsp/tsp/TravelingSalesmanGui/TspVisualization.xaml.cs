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
            this.TspVm.DataChanged +=new Action(this.TspVm_DataChanged);
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

        public void DrawLine(double x1, double x2, double y1, double y2)
        {
            // scale positions by max x and y
            double scaledX1 = x1 / this.TspVm.Max_x * this.visualizationCanvas.ActualWidth;
            double scaledX2 = x2 / this.TspVm.Max_x * this.visualizationCanvas.ActualWidth;
            double scaledY1 = y1 / this.TspVm.Max_y * this.visualizationCanvas.ActualHeight;
            double scaledY2 = y2 / this.TspVm.Max_y * this.visualizationCanvas.ActualHeight;

            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine.X1 = scaledX1;
            myLine.X2 = scaledX2;
            myLine.Y1 = scaledY1;
            myLine.Y2 = scaledY2;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.StrokeThickness = 2;
            this.visualizationCanvas.Children.Add(myLine);
        }

        public void DrawEdge(Edge edge)
        {
            this.DrawLine(edge.Start.X, edge.End.X, edge.Start.Y, edge.End.Y);
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

        private void Render()
        {
            visualizationCanvas.Children.Clear();
            foreach (Node node in this.TspVm.Nodes)
            {
                this.DrawNode(node);
            }
            foreach (Edge edge in this.TspVm.Edges)
            {
                this.DrawEdge(edge);
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
    }
}
