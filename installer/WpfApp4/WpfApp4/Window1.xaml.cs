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
using System.Windows.Shapes;
using WpfApp4.NewFolder;
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
using System.Drawing;
using System.ComponentModel;
using iText.Kernel.Colors;
using Org.BouncyCastle.Utilities;


namespace WpfApp4
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {



        public Window1()
        {
            InitializeComponent();
                       

            
        }

        private void pasteChart(CartesianChart chart)
        {

            Grafic.Series= chart.Series;
            Grafic.XAxes = chart.XAxes;
            Grafic.YAxes = chart.YAxes;

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            //GeneratePdf(title, cartesianCharts); //con esta funcion pasan las cosas}

        }


        public async void GeneratePdf(List<string> Titles , List<CartesianChart> cartesianCharts, string filePath = "")
        {
            var listB = new List<byte[]>();
            var listT = Titles.ToList();

            foreach (var chart in cartesianCharts)
            {

                Dispatcher.Invoke(() =>
                {
                    pasteChart(chart);
                });


                await Task.Delay(550);


                await Dispatcher.InvokeAsync(() =>
                {

                    Grafic.UpdateLayout();
                });


                await Task.Delay(200);


                listB.Add(ToBytes(Grafic));
            }

            GenerateDocument(listT, listB, filePath);

        }


        public void GenerateDocument(List<string> Titles, List<byte[]> Bytes, string filePath )
        {
            using (var writer = new PdfWriter(filePath))
            {
                // Crear un documento PDF
                using (var pdf = new PdfDocument(writer))
                {

                    //color de las los titulos 
                    var doc = new Document(pdf);
                    var grayColor = new DeviceGray(0.5f);

                    for (int i = 0; i < Bytes.Count; i++)
                    {
                        doc.Add(new iText.Layout.Element.Paragraph(Titles[i]).SetMarginLeft(220).SetFontColor(grayColor));

                        // Crear un objeto ImageData para la imagen
                        var imageData = ImageDataFactory.Create(Bytes[i]);

                        // Crear un objeto Image y añadirlo al documento
                        var image = new iText.Layout.Element.Image(imageData)
                            //.SetAutoScale(true) // Ajusta automáticamente el tamaño de la imagen
                            .SetWidth(300) 
                            .SetHeight(350)
                            .SetMarginLeft(110); 
                                                
                        doc.Add(image);
                    }            

                }
            }
        }

        public byte[] ToBytes(CartesianChart chart)
        {

            chart.Measure(new System.Windows.Size(540, 500));
            chart.Arrange(new Rect(0, 0, 540, 500));
            chart.UpdateLayout(); // Asegura que el layout está actualizado
            // Crear un RenderTargetBitmap del gráfico
            var bitmap = new RenderTargetBitmap(
                560, 750,
                96, 96, PixelFormats.Pbgra32);

            // Renderizar el gráfico al RenderTargetBitmap
            bitmap.Render(chart);

            // Convertir RenderTargetBitmap a un arreglo de bytes
            using (var memoryStream = new MemoryStream())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(memoryStream);

                return memoryStream.ToArray();
            }
        }

    }
}
