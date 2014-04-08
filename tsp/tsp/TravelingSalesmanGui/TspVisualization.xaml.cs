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
        }

        public void TspVisualization_Loaded(object sender, RoutedEventArgs e)
        {
            this.DrawEllipse(this.visualizationCanvas.ActualWidth/2, this.visualizationCanvas.ActualHeight/2);
            Loaded -= new RoutedEventHandler(this.TspVisualization_Loaded);
        }

        public void DrawEllipse(double x, double y, double width, double height)
        {
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
            Canvas.SetLeft(circle1, x);
            Canvas.SetBottom(circle1, y);
        }

        public void DrawEllipse(double x, double y)
        {
            this.DrawEllipse(x, y, 20, 20);
        }
    }
}
