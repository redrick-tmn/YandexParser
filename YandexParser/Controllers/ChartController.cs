using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using YandexParser.Models;

namespace YandexParser.Controllers
{
    public class ChartController : Controller
    {
        //
        // GET: /Chart/

        private readonly QueryModel mQueryModel = new QueryModel();

        [NonAction]
        public Legend CreateLegend(string siteName)
        {
            return new Legend
                {
                Name = siteName,
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                BackColor = Color.Transparent,
                Font = new Font(new FontFamily("Trebuchet MS"), 9),
                LegendStyle = LegendStyle.Row,
            };
        }

        [NonAction]
        public ChartArea CreateChartArea()
        {
            var chartArea = new ChartArea
            {
                Name = "Position Chart",
                BackColor = Color.Transparent,
                AxisX = {IsLabelAutoFit = false},
                AxisY = {IsLabelAutoFit = false},
            };
            chartArea.AxisX.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.Interval = 1;
            chartArea.AxisY.Interval = 1;
            chartArea.AxisY.Title = "Position";
            return chartArea;
        }

       
        public FileResult Index(int siteId)
        {
            var queryResults = mQueryModel.QueryResults.Where(x => x.SiteId == siteId).Join(mQueryModel.Queries, result => result.QueryId, query => query.Id, (result, query) =>
                new
                {
                    result.Position,
                    query.Date,
                });
            var t = from p in queryResults
                    group p by p.Date into grps
                    select new
                    {
                        grps.Key,
                        Value = grps.Select(x => x.Position).Max()
                    };  
            var siteName = mQueryModel.Sites.First(x => x.Id == siteId).Url;
            var chart = new Chart
                {
                    Width = 700,
                    Height = 500,
                    BackColor = Color.FromArgb(211, 223, 240),
                    BorderlineDashStyle = ChartDashStyle.Solid,
                    BackSecondaryColor = Color.White,
                    BackGradientStyle = GradientStyle.TopBottom,
                    BorderlineWidth = 1,
                    Palette = ChartColorPalette.BrightPastel,
                    BorderlineColor = Color.FromArgb(26, 59, 105),
                    RenderType = RenderType.BinaryStreaming,
                    AntiAliasing = AntiAliasingStyles.All,
                    TextAntiAliasingQuality = TextAntiAliasingQuality.Normal,
                    BorderSkin = {SkinStyle = BorderSkinStyle.Emboss},
                };
            chart.Titles.Add(CreateTitle());
            chart.Legends.Add(CreateLegend(siteName));
            chart.Series.Add(CreateSeries(t, siteName));
            chart.ChartAreas.Add(CreateChartArea());

            var ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        [NonAction]
        public Title CreateTitle()
        {
            return new Title
            {
                Text = "Position Chart",
                ShadowColor = Color.FromArgb(32, 0, 0, 0),
                Font = new Font("Trebuchet MS", 14F, FontStyle.Bold),
                ShadowOffset = 3,
                ForeColor = Color.FromArgb(26, 59, 105),
            };
        }

        [NonAction]
        public Series CreateSeries(IEnumerable<dynamic> results, string siteName)
        {
            var seriesDetail = new Series
                {
                Name = siteName,
                IsValueShownAsLabel = false,
                Color = Color.FromArgb(198, 99, 99),
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                MarkerSize = 9,
                MarkerStyle = MarkerStyle.Circle,
                ToolTip = "#VALX: #VAL"
            };

            foreach (var result in results)
            {
                var point = new DataPoint();
                point.AxisLabel = result.Key.ToString();
                point.YValues = new double[] { result.Value };
                seriesDetail.Points.Add(point);
            }
            seriesDetail.ChartArea = "Position Chart";
            return seriesDetail;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mQueryModel.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
