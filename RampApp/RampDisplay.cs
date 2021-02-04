using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Ramp
{
    public static class RampDisplay
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct Ramp
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Red;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Green;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Blue;
        }

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

        public static bool UpdateRamp(double gamma, double brightness, string screen = "")
        {
            // 비어있을시 주모니터
            if (screen == "")
            {
                screen = Screen.PrimaryScreen.DeviceName;
            }
            IntPtr screenDc = CreateDC(screen, "", "", IntPtr.Zero);
            IntPtr hdc = CreateCompatibleDC(screenDc);

            Ramp ramp = new Ramp
            {
                Red = new ushort[256],
                Green = new ushort[256],
                Blue = new ushort[256]
            };

            for (int iIndex = 0; iIndex < 256; iIndex++)
            {
                ushort arrayValue = (ushort)Math.Min(65535, Math.Round(Math.Max(0, (Math.Pow(iIndex / 255.0f, 1 / gamma) * 65535 + 0.5) * brightness)));
                ramp.Red[iIndex] = ramp.Green[iIndex] = ramp.Blue[iIndex] = arrayValue;
            }

            bool bReturn = SetDeviceGammaRamp(hdc, ref ramp);

            DeleteDC(hdc);
            return bReturn;
        }
    }
}
