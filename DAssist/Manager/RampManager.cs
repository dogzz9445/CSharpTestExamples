using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DAssist.Manager
{
    public sealed class RampDisplay : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct Ramp
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Red;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Green;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Blue;
        }

        private double _brightness = 1;
        private double _gamma = 1;
        private IntPtr _hdc;
        private Screen _screen;
        private Ramp _oldRamps;

        public double Brightness { get => _brightness; set => _brightness = value; }
        public double Gamma { get => _gamma; set => _gamma = value; }
        public IntPtr Hdc { get => _hdc; set => _hdc = value; }
        public Screen Screen { get => _screen; set => _screen = value; }
        public Ramp OldRamps { get => _oldRamps; set => _oldRamps = value; }

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        private static extern bool SetDeviceGammaRamp(IntPtr hDC, ref Ramp lpRamp);
        [DllImport("gdi32.dll")]
        private static extern bool GetDeviceGammaRamp(IntPtr hDC, ref Ramp lpRamp);

        public RampDisplay()
        {
            IntPtr screenDc = CreateDC(Screen.PrimaryScreen.DeviceName, "", "", IntPtr.Zero);
            Hdc = CreateCompatibleDC(screenDc);

            Ramp ramp = new Ramp
            {
                Red = new ushort[256],
                Green = new ushort[256],
                Blue = new ushort[256]
            };
            bool bReturn = GetDeviceGammaRamp(Hdc, ref ramp);
            OldRamps = ramp;

            if (true && false)
            {

            }

            switch (bReturn)
            {
                case true && false:
                    break;

            }
        }

        public bool SetBrightness(double brightness)
        {
            Brightness = brightness / 50f;
            return UpdateRamps();
        }

        public bool SetGamma(double gamma)
        {
            Gamma = gamma / 50f;
            return UpdateRamps();
        }

        public bool UpdateRamps(bool bUseOldRamp = false)
        {
            Ramp ramp;
            if (true == bUseOldRamp)
            {
                ramp = OldRamps;
            }
            else
            {
                ramp = new Ramp
                {
                    Red = new ushort[256],
                    Green = new ushort[256],
                    Blue = new ushort[256]
                };

                for (int iIndex = 0; iIndex < 256; iIndex++)
                {
                    ushort arrayValue = (ushort)Math.Min(65535, Math.Round(Math.Max(0, (Math.Pow(iIndex / 256.0f, 1 / Gamma) * 65535 + 0.5) * Brightness)));
                    ramp.Red[iIndex] = ramp.Green[iIndex] = ramp.Blue[iIndex] = arrayValue;
                }
            }

            return SetDeviceGammaRamp(Hdc, ref ramp);
        }

        public void Dispose()
        {
            DeleteDC(Hdc);
        }
    }
}
