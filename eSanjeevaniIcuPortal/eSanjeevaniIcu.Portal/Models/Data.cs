using DotNet.Highcharts.Attributes;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Point = DotNet.Highcharts.Options.Point;

namespace iWay.Portal.Models
{
    public class Data
    {
        public Data(object[] data) { ArrayData = data; }

        public Data(object[,] data) { DoubleArrayData = data; }

        public Data(Point[] data) { Points = data; }

        public Data(SeriesData[] data) { SeriesData = data; }

        [Name("data")]
        public object[] ArrayData { get; private set; }

        [Name("data")]
        public object[,] DoubleArrayData { get; private set; }

        [Name("data")]
        public Point[] Points { get; private set; }

        [Name("data")]
        public SeriesData[] SeriesData { get; private set; }
    }
}
