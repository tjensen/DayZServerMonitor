﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace DayZServerMonitorCore
{
    public class DynamicIcons : IDisposable
    {
        // Row positions
        private const int TOP = 1;
        private const int MIDDLE = 7;
        private const int BOTTOM = 13;

        // Column positions (narrow)
        private const int LEFT = 1;
        private const int RIGHT = 4;

        // Column adjustments for character width
        private const int NARROW = 0;
        private const int WIDE = 2;

        // Simulates a 7-segment display, where bits 0-6 represent whether to light the
        // corresponding segment
        private static readonly int[] DIGITS = {
            0x7D, 0x50, 0x37, 0x57, 0x5A, 0x4F, 0x6F, 0x51, 0x7F, 0x5F };
        private static readonly Tuple<Point, Point>[] SEGMENTS =
        {
            new Tuple<Point, Point>(new Point(LEFT, TOP), new Point(RIGHT, TOP)),
            new Tuple<Point, Point>(new Point(LEFT, MIDDLE), new Point(RIGHT, MIDDLE)),
            new Tuple<Point, Point>(new Point(LEFT, BOTTOM), new Point (RIGHT, BOTTOM)),
            new Tuple<Point, Point>(new Point(LEFT, TOP), new Point (LEFT, MIDDLE)),
            new Tuple<Point, Point>(new Point(RIGHT, TOP), new Point (RIGHT, MIDDLE)),
            new Tuple<Point, Point>(new Point(LEFT, MIDDLE), new Point (LEFT, BOTTOM)),
            new Tuple<Point, Point>(new Point(RIGHT, MIDDLE), new Point (RIGHT, BOTTOM))
        };

        private readonly struct NumberIconParams
        {
            public NumberIconParams(uint number, Color foreground, Color background)
            {
                Number = number;
                Foreground = foreground.ToArgb();
                Background = background.ToArgb();
            }
            public uint Number { get; }
            public int Foreground { get; }
            public int Background { get; }
        }

        private readonly struct UnknownIconParams
        {
            public UnknownIconParams(Color foreground, Color background)
            {
                Foreground = foreground.ToArgb();
                Background = background.ToArgb();
            }
            public int Foreground { get; }
            public int Background { get; }
        }

        private readonly Dictionary<NumberIconParams, Icon> numberIcons =
            new Dictionary<NumberIconParams, Icon>();
        private readonly Dictionary<UnknownIconParams, Icon> unknownIcons =
            new Dictionary<UnknownIconParams, Icon>();

        public Icon GetIconForNumber(uint value, Color fg, Color bg)
        {
            NumberIconParams iconParams = new NumberIconParams(value, fg, bg);
            if (!numberIcons.ContainsKey(iconParams))
            {
                using Bitmap bmp = new Bitmap(16, 16);
                using Brush fgBrush = new SolidBrush(fg);
                using Brush bgBrush = new SolidBrush(bg);
                using Graphics graph = Graphics.FromImage(bmp);
                graph.FillRectangle(bgBrush, 0, 0, 16, 16);
                DrawNumber(graph, value, fgBrush);
                numberIcons[iconParams] = Icon.FromHandle(bmp.GetHicon());
            }
            return numberIcons[iconParams];
        }

        public Icon GetIconForUnknown(Color fg, Color bg)
        {
            UnknownIconParams iconParams = new UnknownIconParams(fg, bg);
            if (!unknownIcons.ContainsKey(iconParams))
            {
                using Bitmap bmp = new Bitmap(16, 16);
                using Brush fgBrush = new SolidBrush(fg);
                using Brush bgBrush = new SolidBrush(bg);
                using (Graphics graph = Graphics.FromImage(bmp))
                {
                    graph.FillRectangle(bgBrush, 0, 0, 16, 16);
                    DrawX(graph, fgBrush);
                }
                unknownIcons[iconParams] = Icon.FromHandle(bmp.GetHicon());
            }
            return unknownIcons[iconParams];
        }

        public void Reset()
        {
            foreach (var entry in numberIcons)
            {
                entry.Value.Dispose();
            }
            numberIcons.Clear();

            foreach (var entry in unknownIcons)
            {
                entry.Value.Dispose();
            }
            unknownIcons.Clear();
        }

        private Point AdjustX(Point point, int widthAdjust, int left)
        {
            // Returns a new point after adjusting for the left column and the character width
            Point result = new Point(point.X, point.Y);
            result.Offset(left + ((result.X == RIGHT) ? widthAdjust : 0), 0);
            return result;
        }

        private void DrawDigit(Graphics graph, int left, uint digit, int widthAdjust, Pen pen)
        {
            // Draws a single digit to look like a 7-segment display
            for (int segment = 0; segment < 7; segment++)
            {
                if ((DIGITS[digit] & (1 << segment)) != 0)
                {
                    graph.DrawLine(
                        pen,
                        AdjustX(SEGMENTS[segment].Item1, widthAdjust, left),
                        AdjustX(SEGMENTS[segment].Item2, widthAdjust, left));
                }
            }
        }

        private void DrawNumber(Graphics graph, uint value, Brush brush)
        {
            using Pen pen = new Pen(brush);
            if (value < 10)
            {
                DrawDigit(graph, 4, value, WIDE, pen);
            }
            else if (value < 100)
            {
                DrawDigit(graph, 0, (value / 10) % 10, WIDE, pen);
                DrawDigit(graph, 8, value % 10, WIDE, pen);
            }
            else
            {
                // This really only works for 100...199 since the third digit only has enough
                // room for a one.
                DrawDigit(graph, -2, (value / 100) % 10, NARROW, pen);
                DrawDigit(graph, 4, (value / 10) % 10, NARROW, pen);
                DrawDigit(graph, 10, value % 10, NARROW, pen);
            }
        }

        private void DrawX(Graphics graph, Brush brush)
        {
            using Pen pen = new Pen(brush);
            const int left = 4;
            graph.DrawLine(
                new Pen(brush),
                AdjustX(new Point(LEFT, TOP), WIDE, left),
                AdjustX(new Point(RIGHT, BOTTOM), WIDE, left));
            graph.DrawLine(
                new Pen(brush),
                AdjustX(new Point(RIGHT, TOP), WIDE, left),
                AdjustX(new Point(LEFT, BOTTOM), WIDE, left));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var entry in numberIcons)
                    {
                        entry.Value.Dispose();
                    }
                    foreach (var entry in unknownIcons)
                    {
                        entry.Value.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
