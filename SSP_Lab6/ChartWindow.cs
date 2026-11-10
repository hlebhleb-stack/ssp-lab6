using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace SSP_Lab6;

public class ChartWindow : Window
{
    private readonly List<double> _temps = new();
    private readonly TextBlock _lblTemp;
    private readonly TextBlock _lblStatus;
    private readonly Canvas _canvas;

    public ChartWindow()
    {
        Title = "IoT Датчик температуры - График";
        Width = 700;
        Height = 500;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Background = Brushes.White;

        var infoPanel = new StackPanel
        {
            Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
            Height = 70,
            Orientation = Orientation.Vertical
        };

        _lblTemp = new TextBlock
        {
            Text = "Температура: --°C",
            FontSize = 14,
            FontWeight = FontWeight.Bold,
            Foreground = Brushes.Red,
            Margin = new Thickness(20, 10, 0, 0)
        };

        _lblStatus = new TextBlock
        {
            Text = "Статус: Ожидание данных...",
            FontSize = 12,
            Foreground = Brushes.Blue,
            Margin = new Thickness(20, 5, 0, 0)
        };

        infoPanel.Children.Add(_lblTemp);
        infoPanel.Children.Add(_lblStatus);

        _canvas = new Canvas { Background = Brushes.White };

        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
        Grid.SetRow(infoPanel, 0);
        Grid.SetRow(_canvas, 1);
        grid.Children.Add(infoPanel);
        grid.Children.Add(_canvas);

        Content = grid;

        _canvas.SizeChanged += (_, _) => Redraw();
    }

    public void AddPoint(double temperature, string status)
    {
        Dispatcher.UIThread.Post(() =>
        {
            _temps.Add(temperature);
            _lblTemp.Text = $"Температура: {temperature:F1}°C";
            _lblStatus.Text = $"Статус: {status}";
            Redraw();
        });
    }

    private void Redraw()
    {
        _canvas.Children.Clear();
        if (_temps.Count < 2) return;

        double w = _canvas.Bounds.Width;
        double h = _canvas.Bounds.Height;
        if (w < 10 || h < 10) return;

        const double pad = 45;
        double chartW = w - pad * 2;
        double chartH = h - pad * 2;

        double min = _temps.Min() - 2;
        double max = _temps.Max() + 2;
        double range = max - min;

        // Горизонтальные линии сетки
        for (int i = 0; i <= 5; i++)
        {
            double y = pad + chartH * i / 5;
            _canvas.Children.Add(new Line
            {
                StartPoint = new Point(pad, y),
                EndPoint = new Point(w - pad, y),
                Stroke = new SolidColorBrush(Color.FromRgb(220, 220, 220)),
                StrokeThickness = 1
            });

            double temp = max - range * i / 5;
            var lbl = new TextBlock
            {
                Text = $"{temp:F0}°",
                FontSize = 10,
                Foreground = Brushes.Gray
            };
            Canvas.SetLeft(lbl, 2);
            Canvas.SetTop(lbl, y - 7);
            _canvas.Children.Add(lbl);
        }

        // Линия графика
        var points = new AvaloniaList<Point>();
        for (int i = 0; i < _temps.Count; i++)
        {
            double x = pad + chartW * i / Math.Max(_temps.Count - 1, 1);
            double y = pad + chartH * (1 - (_temps[i] - min) / range);
            points.Add(new Point(x, y));
        }

        _canvas.Children.Add(new Polyline
        {
            Points = points,
            Stroke = Brushes.OrangeRed,
            StrokeThickness = 2
        });

        // Точки и подписи
        for (int i = 0; i < points.Count; i++)
        {
            var p = points[i];
            _canvas.Children.Add(new Ellipse
            {
                Width = 7,
                Height = 7,
                Fill = Brushes.DarkRed
            });
            Canvas.SetLeft(_canvas.Children[^1], p.X - 3.5);
            Canvas.SetTop(_canvas.Children[^1], p.Y - 3.5);

            if (i % Math.Max(1, points.Count / 10) == 0 || i == points.Count - 1)
            {
                var pointLbl = new TextBlock
                {
                    Text = $"{_temps[i]:F1}°",
                    FontSize = 9,
                    Foreground = Brushes.DarkRed
                };
                Canvas.SetLeft(pointLbl, p.X - 10);
                Canvas.SetTop(pointLbl, p.Y - 18);
                _canvas.Children.Add(pointLbl);
            }
        }

        // Подпись оси X
        var xLbl = new TextBlock
        {
            Text = $"Показаний: {_temps.Count}",
            FontSize = 10,
            Foreground = Brushes.Gray
        };
        Canvas.SetLeft(xLbl, w / 2 - 30);
        Canvas.SetTop(xLbl, h - pad + 5);
        _canvas.Children.Add(xLbl);
    }
}
