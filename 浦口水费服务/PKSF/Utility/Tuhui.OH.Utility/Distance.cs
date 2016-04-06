using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.OH.Utility
{
    public static class Distance
    {
        private const double EARTH_RADIUS = 6378.137;//地球半径
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        public static double GetDistance(string node1, string node2)
        {
            var node1Arr = node1.Split(',');
            var node2Arr = node2.Split(',');
            var _x1 = double.Parse(node1Arr[0]);
            var _y1 = double.Parse(node1Arr[1]);
            var _z1 = double.Parse(node1Arr[2]);

            var _x2 = double.Parse(node2Arr[0]);
            var _y2 = double.Parse(node2Arr[1]);
            var _z2 = double.Parse(node2Arr[2]);

            var _dis1 = GetDistance(_y1, _x1, _y2, _x2);

            var _dis2 = _z1 - _z2;

            if (_dis2 == 0) return _dis1;

            return Math.Sqrt(_dis1 * _dis1 + _dis2 * _dis2);
        }

        private static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10;
            return s;
        }
    }
}
