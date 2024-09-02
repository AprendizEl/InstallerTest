using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Kernel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;
using LiveChartsCore.SkiaSharpView.WPF;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.IO;

namespace WpfApp4.NewFolder
{
    /// <summary>
    /// Lógica de interacción para Gr3.xaml
    /// </summary>
    public partial class Gr3 : UserControl
    {
        public Gr3()
        {
            InitializeComponent();


            var points = new List<ObservablePoint>();

            var trianglePoints = new List<ObservablePoint>
            {
                new ObservablePoint(-1, -1), // Primer vértice
                new ObservablePoint(-3, -1), // Segundo vértice
                new ObservablePoint(-2, -0.5), // Tercer vértice

            };


            for (int i = 0; i < 20; i++)
            {
                double y = i * Math.Sin(i);
                double x = i * 3; // Datos Y como función del nuevo X
                points.Add(new ObservablePoint(x, y));
            }

            Grafic3.Series = new ISeries[]
            {
                new ScatterSeries<ObservablePoint>
                {
                    Values = trianglePoints,

                    //Stroke = new SolidColorPaint(SKColors.Blue),


                },

                 new LineSeries<ObservablePoint>
                {
                    Values = points,
                    //Stroke = new SolidColorPaint(SKColors.Red),
                    Fill = null,

                }

            };

            Grafic3.XAxes = new Axis[]
            {
                new Axis
                {
                     Name = "(M) Bending Moment [Tonf-m]",
                    NamePaint = new SolidColorPaint(SKColors.LightSlateGray),
                    TextSize = 15,
                    Labeler = value => Math.Round(value, 2).ToString(),
                }
            };

            Grafic3.YAxes = new Axis[]
            {
                new Axis
                {
                Name = "(P) Axial Force [Tonf]",
                NamePaint = new SolidColorPaint(SKColors.LightSlateGray),
                //LabelsPaint = new SolidColorPaint(SKColors.Green),
                TextSize = 15,
                SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
                    {
                        StrokeThickness = 0.5f,
                        PathEffect = new DashEffect(new float[] { 10, 10 })
                    }
                }
            };

            Muro = "Muro Gr3";
            Piso = "piso 2";

            DataContext = this;

        }

        public string Muro;
        public string Piso;
        public void generatepdf(string filepath)
        {

            using (var writer = new PdfWriter(filepath))
            {
                // Crear un documento PDF
                using (var pdf = new PdfDocument(writer))
                {
                    var doc = new Document(pdf);
                    // Crear un objeto ImageData para la imagen
                    var imageData = ImageDataFactory.Create("C:\\Users\\eecheto\\Desktop\\Pruebas\\prueba2\\WpfApp4\\WpfApp4\\WpfApp4.csprojchart.png");

                    // Crear un objeto Image y añadirlo al documento
                    var image = new iText.Layout.Element.Image(imageData)
                        //.SetAutoScale(true) // Ajusta automáticamente el tamaño de la imagen
                        .SetWidth(300) // Ajusta el ancho de la imagen (opcional)
                        .SetHeight(350)
                        .SetMarginLeft(100); // Ajusta la altura de la imagen (opcional)


                    doc.Add(image);
                    doc.Add(image);
                    doc.Add(image);
                    doc.Add(image);
                    doc.Add(image);
                }
            }


        }
        public void ExportChart(CartesianChart chart)
        {

            var bitmap = new RenderTargetBitmap(
                (int)chart.ActualWidth, (int)chart.ActualHeight,
                96, 96, PixelFormats.Pbgra32);

            bitmap.Render(chart);

            using (var fileStream = new FileStream("C:\\Users\\eecheto\\Desktop\\Pruebas\\prueba2\\WpfApp4\\WpfApp4\\WpfApp4.csprojchart.png", FileMode.Create))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(fileStream);
            }
        }

        public ISeries[] Series { get; set; } = {
            new ScatterSeries<ObservablePoint>{

            Values = new ObservableCollection<ObservablePoint>
            {
                new(2.2, 5.4),
                new(4.5, 2.5),
                new(4.2, 7.4),
                new(6.4, 9.9),
                new(4.2, 9.2),
                new(5.8, 3.5),
                new(7.3, 5.8),
                new(8.9, 3.9),
                new(6.1, 4.6),
                new(9.4, 7.7),
                new(8.4, 8.5),
                new(3.6, 9.6),
                new(4.4, 6.3),
                new(5.8, 4.8),
                new(6.9, 3.4),
                new(7.6, 1.8),
                new(8.3, 8.3),
                new(9.9, 5.2),
                new(8.1, 4.7),
                new(7.4, 3.9),
                new(6.8, 2.3),
                new(5.3, 7.1),
                             }
            }
        };

        public Axis[] XAxes { get; set; } = {
            new Axis {

                Name = "(M) Bending Moment [Tonf-m]",
                NamePaint = new SolidColorPaint(SKColors.LightSlateGray),
                TextSize = 15,
                Labeler = value => Math.Round(value, 2).ToString(),

            }
        };

        public Axis[] YAxes { get; set; } ={
            new Axis
            {
                Name = "(P) Axial Force [Tonf]",
                NamePaint = new SolidColorPaint(SKColors.LightSlateGray),
                //LabelsPaint = new SolidColorPaint(SKColors.Green),
                TextSize = 15,
                SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
                {
                    StrokeThickness = 0.5f,
                    PathEffect = new DashEffect(new float[] { 10, 10 })
                }
                //Labeler = value => Math.Round(value, 2).ToString()
            }
        };

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExportChart(Grafic3);
            generatepdf("C:\\Users\\eecheto\\Desktop\\Pruebas\\prueba2\\WpfApp4\\WpfApp4\\documents.pdf");
        }
    }
}