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
using System.Windows.Threading;

namespace PermDynamics_Markova.Pages
{
	/// <summary>
	/// Логика взаимодействия для Chart.xaml
	/// </summary>
	public partial class Chart : Page
	{
		public MainWindow mainWindow;
		public double actualHeightCanvas = 0;
		public double maxValue = 0;
		public double averageValue = 0;
		public DispatcherTimer dispatcherTimer = new DispatcherTimer();
		public Chart(MainWindow mainWindow)
		{
			InitializeComponent();
			this.mainWindow = mainWindow;
			actualHeightCanvas = mainWindow.Height - 50d;
			dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
			dispatcherTimer.Tick += CreateNewValue;
			dispatcherTimer.Start();
			CreateChart();
			ColorChart();
		}

		private void CreateNewValue(object sender, EventArgs e)
		{
			if (MainWindow.pointsInfo.Count == 0) return;
			Random random = new Random();
			double value = MainWindow.pointsInfo[MainWindow.pointsInfo.Count - 1].value;
			double newValue = value * (random.NextDouble() + 0.5d);
			MainWindow.pointsInfo.Add(new Classes.PointInfo(newValue));
			ControlCreateChart();
		}

		public void CreateChart()
		{
			canvas.Children.Clear();
			if (MainWindow.pointsInfo.Count == 0) return;
			maxValue = 0;
			for(int i = 0; i < MainWindow.pointsInfo.Count; i++)
			{
				if (MainWindow.pointsInfo[i].value > maxValue)
					maxValue = MainWindow.pointsInfo[i].value;
			}
			if (maxValue == 0) maxValue = 1;
			for(int i = 0; i < MainWindow.pointsInfo.Count; i++)
			{
				Line line = new Line();
				line.X1 = i * 20;
				line.X2 = (i + 1) * 20;
				if(i == 0)
				{
					line.Y1 = actualHeightCanvas;
				}
				else
				{
					line.Y1 = actualHeightCanvas - ((MainWindow.pointsInfo[i - 1].value / maxValue) * actualHeightCanvas);
				}
				line.Y2 = actualHeightCanvas - ((MainWindow.pointsInfo[i].value / maxValue) * actualHeightCanvas);
				line.StrokeThickness = 2;
				line.Stroke = Brushes.Red;
				MainWindow.pointsInfo[i].line = line;
				canvas.Children.Add(line);
			}
		}
		public void ControlCreateChart()
		{
			if (MainWindow.pointsInfo.Count == 0) return;
			double value = MainWindow.pointsInfo[MainWindow.pointsInfo.Count - 1].value;
			if(value < maxValue)
			{
				CreatePoint();
			}
			else
			{
				CreateChart();
			}
			ColorChart();
		}

		public void ColorChart()
		{
			if (MainWindow.pointsInfo.Count == 0) return;
			double value = MainWindow.pointsInfo[MainWindow.pointsInfo.Count - 1].value;
			averageValue = 0;
			for(int i = 0; i < MainWindow.pointsInfo.Count; i++)
			{
				averageValue += MainWindow.pointsInfo[i].value;
			}
			averageValue = averageValue / MainWindow.pointsInfo.Count;
			for(int i = 0; i < MainWindow.pointsInfo.Count; i++)
			{
				if(value < averageValue)
				{
					MainWindow.pointsInfo[i].line.Stroke = Brushes.Red;
				}
				else
				{
					MainWindow.pointsInfo[i].line.Stroke = Brushes.Green;
				}
			}
			canvas.Width = MainWindow.pointsInfo.Count * 20 + 300;
			scroll.ScrollToHorizontalOffset(canvas.Width);
			current_value.Content = "Тек. знач.: " + Math.Round(value, 2);
			average_value.Content = "Сред. знач.: " + Math.Round(averageValue, 2);
		}

		public void CreatePoint()
		{
			if (MainWindow.pointsInfo.Count < 2) return;
			maxValue = 0;
			for (int i = 0; i < MainWindow.pointsInfo.Count; i++)
			{
				if (MainWindow.pointsInfo[i].value > maxValue)
					maxValue = MainWindow.pointsInfo[i].value;
			}
			if (maxValue == 0) maxValue = 1;
			actualHeightCanvas = canvas.ActualHeight;
			if (actualHeightCanvas == 0) actualHeightCanvas = 420;
			Line line = new Line();
			line.X1 = (MainWindow.pointsInfo.Count - 1) * 20;
			line.X2 = MainWindow.pointsInfo.Count * 20;
			line.Y1 = actualHeightCanvas - ((MainWindow.pointsInfo[(MainWindow.pointsInfo.Count - 2)].value / maxValue) * actualHeightCanvas);
			line.Y2 = actualHeightCanvas - ((MainWindow.pointsInfo[(MainWindow.pointsInfo.Count - 1)].value / maxValue) * actualHeightCanvas);
			line.StrokeThickness = 2;
			line.Stroke = Brushes.Red;
			MainWindow.pointsInfo[(MainWindow.pointsInfo.Count - 1)].line = line;
			canvas.Children.Add(line);
		}

		private void Page_SizeChanget(object sender, SizeChangedEventArgs e)
		{
			actualHeightCanvas = Application.Current.MainWindow.Height - 50d;
			CreateChart();
			ColorChart();
		}
	}
}
