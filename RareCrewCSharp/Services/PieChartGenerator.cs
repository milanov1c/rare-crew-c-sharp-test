using RareCrewCSharp.Models;
using System.Drawing;
using System.Drawing.Imaging;

namespace RareCrewCSharp.Services
{
    public class PieChartGenerator
    {

        public void GenerateChart(List <Employee> employees, string savePath)
        {
            int width = 600, height = 400;
            using var bitmap = new Bitmap(width, height);
            using var g= Graphics.FromImage(bitmap);
            g.Clear(Color.White);

            double total=employees.Sum(e=>e.TotalHours);

            var colors = new[] {Color.Red, Color.Green, Color.Blue, Color.Orange, Color.Coral, Color.Purple, Color.Yellow, Color.Pink};

            float startAngle = 0;
            int colorIndex = 0;

            foreach(var emp in employees)
            {
                float sweep=(float)(emp.TotalHours/total*360);
                var color=colors[colorIndex%colors.Length];

                using var brush = new SolidBrush(color);
                g.FillPie(brush, 50, 50, 300, 300, startAngle, sweep);

                g.DrawString($"{emp.EmployeeName}: {emp.TotalHours:F1}h", 
                    new Font("Arial", 10), Brushes.Black, 370, 50+colorIndex*20);

                startAngle += sweep;
                colorIndex++;
            }

            bitmap.Save(savePath, ImageFormat.Png);
            
        }
    }
}
