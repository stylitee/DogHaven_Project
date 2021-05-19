﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace doghavenCapstone.Model
{
    public static class PolylineHelper
    {
        public static IEnumerable<Position> Decode(string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                throw new ArgumentException("encodedPoints");
            char[] polylineChars = encodedPoints.ToCharArray();
            int index = 0;
            int currentLat = 0, currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            while(index < polylineChars.Length)
            {
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                    break;
                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5bits >= 32)
                    break;
                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                yield return new Position(Convert.ToDouble(currentLat) / 1E5, Convert.ToDouble(currentLng) / 1E5);
            }
        }

        public static string Encode(IEnumerable<Position> points)
        {
            var str = new StringBuilder();
            var encodedDiff = (Action<int>)(diff =>
            {
                int shifted = diff << 1;
                if (diff < 0)
                    shifted = ~shifted;
                int rem = shifted;
                while(rem >= 0x20)
                {
                    str.Append((char)((0x20) | (rem & 0x1f) + 63));
                    rem >>= 5;
                }
                str.Append((char)(rem + 63));
            });

            int lastLat = 0;
            int lastLng = 0;

            foreach(var point in points)
            {
                int lat = (int)Math.Round(point.Latitude * 1E5);
                int lng = (int)Math.Round(point.Longitude * 1E5);

                encodedDiff(lat - lastLat);
                encodedDiff(lng - lastLng);

                lastLat = lat;
                lastLng = lng;
            }
            return str.ToString();
        }
    }
}
