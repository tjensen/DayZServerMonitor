using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestDynamicIcons
    {
        private readonly Color FG = Color.Orange;
        private readonly Color BG = Color.Green;

        private ushort GetRowFromBitmap(Bitmap bmp, int row)
        {
            ushort result = 0;
            for (int column = 0; column < 16; column++)
            {
                Color pixel = bmp.GetPixel(column, row);
                if (pixel.ToArgb() == FG.ToArgb())
                {
                    result |= (ushort)(1 << column);
                }
                else if (pixel.ToArgb() != BG.ToArgb())
                {
                    Assert.Fail($"Invalid pixel color {pixel} at column {column}, row {row}");
                }
            }
            return result;
        }

        private void AssertIconHasPixels(ushort[] rows, Icon icon)
        {
            using Bitmap bmp = icon.ToBitmap();
            for (int row = 0; row < 16; row++)
            {
                ushort actual = GetRowFromBitmap(bmp, row);
                Assert.AreEqual(
                    rows[row],
                    actual,
                    $"Row {row} pixels (0x{actual:X4}) do not match expected (0x{rows[row]:X4})");
            }
        }

        [TestMethod]
        public void GetIconForNumberReturnsIconShowingSingleDigit()
        {
            using DynamicIcons icons = new DynamicIcons();
            Icon icon = icons.GetIconForNumber(8, FG, BG);

            Assert.AreEqual(16, icon.Width);
            Assert.AreEqual(16, icon.Height);
            AssertIconHasPixels(new ushort[16]
                {
                    0x0000, 0x07E0, 0x0420, 0x0420, 0x0420, 0x0420, 0x0420, 0x07E0,
                    0x0420, 0x0420, 0x0420, 0x0420, 0x0420, 0x07E0, 0x0000, 0x0000
                },
                icon);
        }

        [TestMethod]
        public void GetIconForNumberReturnsIconShowingTwoDigits()
        {
            using DynamicIcons icons = new DynamicIcons();
            Icon icon = icons.GetIconForNumber(88, FG, BG);

            Assert.AreEqual(16, icon.Width);
            Assert.AreEqual(16, icon.Height);
            AssertIconHasPixels(new ushort[16]
                {
                    0x0000, 0x7E7E, 0x4242, 0x4242, 0x4242, 0x4242, 0x4242, 0x7E7E,
                    0x4242, 0x4242, 0x4242, 0x4242, 0x4242, 0x7E7E, 0x0000, 0x0000
                },
                icon);
        }

        [TestMethod]
        public void GetIconForNumberReturnsIconShowingThreeDigits()
        {
            using DynamicIcons icons = new DynamicIcons();
            Icon icon = icons.GetIconForNumber(188, FG, BG);

            Assert.AreEqual(16, icon.Width);
            Assert.AreEqual(16, icon.Height);
            AssertIconHasPixels(new ushort[16]
                {
                    0x0000, 0x79E4, 0x4924, 0x4924, 0x4924, 0x4924, 0x4924, 0x79E4,
                    0x4924, 0x4924, 0x4924, 0x4924, 0x4924, 0x79E4, 0x0000, 0x0000
                },
                icon);
        }

        [TestMethod]
        public void GetIconForNumberReturnsSameIconForSameInput()
        {
            using DynamicIcons icons = new DynamicIcons();
            Icon icon1 = icons.GetIconForNumber(42, FG, BG);
            Icon icon2 = icons.GetIconForNumber(42, FG, BG);

            Assert.AreSame(icon1, icon2);
        }

        [TestMethod]
        public void GetIconForNumberReturnsDifferentIconsForDifferentInputs()
        {
            using DynamicIcons icons = new DynamicIcons();
            Icon icon1 = icons.GetIconForNumber(42, FG, BG);
            Icon icon2 = icons.GetIconForNumber(24, FG, BG);

            Assert.AreNotSame(icon1, icon2);
        }

        [TestMethod]
        public void GetIconForUnknownReturnsIconShowingSingleX()
        {
            using DynamicIcons icons = new DynamicIcons();
            Icon icon = icons.GetIconForUnknown(FG, BG);

            Assert.AreEqual(16, icon.Width);
            Assert.AreEqual(16, icon.Height);
            AssertIconHasPixels(new ushort[16]
                {
                        0x0000, 0x0420, 0x0420, 0x0240, 0x0240, 0x0180, 0x0180, 0x0180,
                        0x0180, 0x0180, 0x0240, 0x0240, 0x0420, 0x0420, 0x0000, 0x0000
                },
                icon);
        }

        [TestMethod]
        public void GetIconForUnknownReturnsSameIconEachTime()
        {
            using DynamicIcons icons = new DynamicIcons();
            Icon icon1 = icons.GetIconForUnknown(FG, BG);
            Icon icon2 = icons.GetIconForUnknown(FG, BG);

            Assert.AreSame(icon1, icon2);
        }

        [TestMethod]
        public void ResetClearsInternalCacheOfPreviouslyRenderedIcons()
        {
            using DynamicIcons icons = new DynamicIcons();
            Icon unknownIcon1 = icons.GetIconForUnknown(FG, BG);
            Icon numberIcon1 = icons.GetIconForNumber(42, FG, BG);

            icons.Reset();

            Icon unknownIcon2 = icons.GetIconForUnknown(FG, BG);
            Icon numberIcon2 = icons.GetIconForNumber(42, FG, BG);
            Assert.AreNotSame(unknownIcon1, unknownIcon2);
            Assert.AreNotSame(numberIcon1, numberIcon2);
        }
    }
}
