using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AudioControlApp
{
    public static class AudioMixer
    {
        public const int MMSYSERR_NOERROR = 0;
        public const int MAXPNAMELEN = 32;
        public const int MIXER_LONG_NAME_CHARS = 64;
        public const int MIXER_SHORT_NAME_CHARS = 16;
        public const int MIXER_GETLINEINFOF_COMPONENTTYPE = 0x3;
        public const int MIXER_GETCONTROLDETAILSF_VALUE = 0x0;
        public const int MIXER_GETLINECONTROLSF_ONEBYTYPE = 0x2;
        public const int MIXER_SETCONTROLDETAILSF_VALUE = 0x0;

        public const int MIXERLINE_COMPONENTTYPE_DST_FIRST = 0x0;
        public const int MIXERLINE_COMPONENTTYPE_SRC_FIRST = 0x1000;
        public const int MIXERLINE_COMPONENTTYPE_DST_SPEAKERS = (MIXERLINE_COMPONENTTYPE_DST_FIRST + 4);
        public const int MIXERLINE_COMPONENTTYPE_SRC_MICROPHONE = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 3);
        public const int MIXERLINE_COMPONENTTYPE_SRC_LINE = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 2);

        public const int MIXERCONTROL_CT_CLASS_FADER = 0x50000000;
        public const int MIXERCONTROL_CT_UNITS_UNSIGNED = 0x30000;
        public const int MIXERCONTROL_CONTROLTYPE_FADER = (MIXERCONTROL_CT_CLASS_FADER | MIXERCONTROL_CT_UNITS_UNSIGNED);
        public const int MIXERCONTROL_CONTROLTYPE_VOLUME = (MIXERCONTROL_CONTROLTYPE_FADER + 1);

        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        public static extern int mixerClose(int hmx);

        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        public static extern int mixerGetControlDetailsA(int hmxobj, ref MIXERCONTROLDETAILS pmxcd, int fdwDetails);

        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        public static extern int mixerGetDevCapsA(int uMxId, MIXERCAPS pmxcaps, int cbmxcaps);

        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        public static extern int mixerGetID(int hmxobj, int pumxID, int fdwId);

        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        public static extern int mixerGetLineControlsA(int hmxobj, ref MIXERLINECONTROLS pmxlc, int fdwControls);

        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        public static extern int mixerGetLineInfoA(int hmxobj, ref MIXERLINE pmxl, int fdwInfo);

        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        public static extern int mixerGetNumDevs();

        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        public static extern int mixerMessage(int hmx, int uMsg, int dwParam1, int dwParam2);

        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        public static extern int mixerOpen(out int phmx, int uMxId, int dwCallback, int dwInstance, int fdwOpen);

        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        public static extern int mixerSetControlDetails(int hmxobj, ref MIXERCONTROLDETAILS pmxcd, int fdwDetails);

        [DllImport("winmm.dll", EntryPoint = "sndPlaySoundA")]
        public static extern int sndPlaySound(string lpszSoundName, int uFlags);

        [Flags]
        public enum SND : uint
        {
            SND_SYNC = 0x0000,/* play synchronously (default) */
            SND_ASYNC = 0x0001, /* play asynchronously */
            SND_NODEFAULT = 0x0002, /* silence (!default) if sound not found */
            SND_MEMORY = 0x0004, /* pszSound points to a memory file */
            SND_LOOP = 0x0008, /* loop the sound until next sndPlaySound */
            SND_NOSTOP = 0x0010, /* don't stop any currently playing sound */
            SND_NOWAIT = 0x00002000, /* don't wait if the driver is busy */
            SND_ALIAS = 0x00010000,/* name is a registry alias */
            SND_ALIAS_ID = 0x00110000, /* alias is a pre d ID */
            SND_FILENAME = 0x00020000, /* name is file name */
            SND_RESOURCE = 0x00040004, /* name is resource name or atom */
            SND_PURGE = 0x0040,  /* purge non-static events for task */
            SND_APPLICATION = 0x0080  /* look for application specific association */
        }

        public struct MIXERCAPS
        {
            public int wMid;
            public int wPid;
            public int vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
            public string szPname;
            public int fdwSupport;
            public int cDestinations;
        }

        public struct MIXERCONTROL
        {
            public int cbStruct;
            public int dwControlID;
            public int dwControlType;
            public int fdwControl;
            public int cMultipleItems;
            [MarshalAs(UnmanagedType.ByValTStr,
                 SizeConst = MIXER_SHORT_NAME_CHARS)]
            public string szShortName;
            [MarshalAs(UnmanagedType.ByValTStr,
                 SizeConst = MIXER_LONG_NAME_CHARS)]
            public string szName;
            public int lMinimum;
            public int lMaximum;
            [MarshalAs(UnmanagedType.U4, SizeConst = 10)]
            public int reserved;
        }

        public struct MIXERCONTROLDETAILS
        {
            public int cbStruct;
            public int dwControlID;
            public int cChannels;
            public int item;
            public int cbDetails;
            public IntPtr paDetails;
        }

        public struct MIXERCONTROLDETAILS_UNSIGNED
        {
            public int dwValue;
        }

        public struct MIXERLINE
        {
            public int cbStruct;
            public int dwDestination;
            public int dwSource;
            public int dwLineID;
            public int fdwLine;
            public int dwUser;
            public int dwComponentType;
            public int cChannels;
            public int cConnections;
            public int cControls;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MIXER_SHORT_NAME_CHARS)]
            public string szShortName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MIXER_LONG_NAME_CHARS)]
            public string szName;
            public int dwType;
            public int dwDeviceID;
            public int wMid;
            public int wPid;
            public int vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
            public string szPname;
        }

        public struct MIXERLINECONTROLS
        {
            public int cbStruct;
            public int dwLineID;

            public int dwControl;
            public int cControls;
            public int cbmxctrl;
            public IntPtr pamxctrl;
        }
    }

    public static class AudioDevice
    {
        [DllImport("Kernel32")]
        public static extern uint GetLastError();

        [DllImport("Kernel32")]
        public extern static int CloseHandle(IntPtr handle);

        [DllImport("winmm.dll")]
        public static extern Int32 mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        [DllImport("winmm.dll")]
        public static extern Int32 mciSendString(string command, String buffer, int bufferSize, int hwndCallback);

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint mciSendCommand(int deviceId, int command, int flags, ref MCI_OPEN_PARMS param);

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint mciSendCommand(IntPtr deviceId, int command, int flags, ref MCI_RECORD_PARMS param);

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint mciSendCommand(int deviceId, int command, int flags, ref MCI_RECORD_PARMS param);

        [DllImport("winmm.dll")]
        public static extern bool mciGetErrorString(uint fdwError, StringBuilder lpszErrorText, uint cchErrorText);

        [DllImport("winmm.dll", SetLastError = true)]
        public static extern uint waveOutGetNumDevs();

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveOutGetDevCaps(IntPtr hwo, ref WAVEOUTCAPS pwoc, uint cbwoc);

        [DllImport("winmm.dll")]
        public static extern uint waveOutSetPlaybackRate(IntPtr hwo, uint pdwRate);

        [DllImport("winmm.dll")]
        public static extern uint waveOutGetPlaybackRate(IntPtr hwo, ref uint pdwRate);

        [DllImport("winmm.dll")]
        public static extern uint waveOutGetPosition(IntPtr hWaveOut, ref MMTIME lpInfo, int uSize);

        [DllImport("winmm.dll")]
        public static extern uint waveOutPause(IntPtr hwo);

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveOutOpen(ref IntPtr hWaveOut, uint uDeviceID, ref WAVEFORMATEX lpFormat, IntPtr dwCallback, uint dwInstance, uint dwFlags);

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveOutOpen(ref IntPtr hWaveOut, IntPtr uDeviceID, ref WAVEFORMATEX lpFormat, IntPtr dwCallback, IntPtr dwInstance, uint dwFlags);

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveOutWrite(IntPtr hwo, ref WAVEHDR pwh, uint cbwh);

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveOutReset(IntPtr hwo);

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveOutRestart(IntPtr hwo);

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveOutPrepareHeader(IntPtr hWaveOut, ref WAVEHDR lpWaveOutHdr, uint uSize);

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveOutUnprepareHeader(IntPtr hwo, ref WAVEHDR pwh, uint cbwh);

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveOutGetDevCaps(uint hwo, ref WAVEOUTCAPS pwoc, uint cbwoc);

        [DllImport("winmm.dll", EntryPoint = "waveOutClose", SetLastError = true)]
        public static extern uint waveOutClose(IntPtr hwo);

        [DllImport("winmm.dll")]
        public static extern uint waveInGetDevCaps(IntPtr deviceId, out WAVEINCAPS caps, int capsSize);

        [DllImport("winmm.dll", ExactSpelling = true)]
        public static extern uint waveInGetNumDevs();

        [DllImport("winmm.dll", SetLastError = true)]
        public static extern uint waveInReset(IntPtr hwi);

        [DllImport("winmm.dll")]
        public static extern uint waveInOpen(ref IntPtr hWaveIn, uint deviceId, ref WAVEFORMATEX wfx, IntPtr dwCallBack, uint dwInstance, uint dwFlags);

        [DllImport("winmm.dll", SetLastError = true)]
        public static extern uint waveInStart(IntPtr hwi);

        [DllImport("winmm.dll", SetLastError = true)]
        public static extern uint waveInStop(IntPtr hwi);

        [DllImport("winmm.dll", SetLastError = true)]
        public static extern uint waveInUnprepareHeader(IntPtr hwi, ref WAVEHDR pwh, uint cbwh);

        [DllImport("winmm.dll", SetLastError = true)]
        public static extern uint waveInPrepareHeader(IntPtr hwi, ref WAVEHDR pwh, uint cbwh);

        [DllImport("winmm.dll", EntryPoint = "waveInAddBuffer", SetLastError = true)]
        public static extern uint waveInAddBuffer(IntPtr hwi, ref WAVEHDR pwh, uint cbwh);

        [DllImport("winmm.dll", SetLastError = true)]
        public static extern uint waveInClose(IntPtr hwi);

        public struct MinMax
        {
            public short Min, Max;
            public override string ToString()
            {
                return ("Min " + Min.ToString() + " : Max " + Max.ToString());
            }
        }

        [Flags]
        public enum WaveOutOpenFlags : uint
        {
            WAVE_FORMAT_QUERY = 0x0001,
            WAVE_ALLOWSYNC = 0x0002,
            WAVE_MAPPED = 0x0004,
            WAVE_FORMAT_DIRECT = 0x0008,
            CALLBACK_WINDOW = 0x000100000,
            CALLBACK_TASK = 0x000200000,
            CALLBACK_FUNCTION = 0x00030000
        };

        [Flags]
        public enum WaveInOpenFlags : uint
        {
            CALLBACK_NULL = 0,
            CALLBACK_FUNCTION = 0x30000,
            CALLBACK_EVENT = 0x50000,
            CALLBACK_WINDOW = 0x10000,
            CALLBACK_THREAD = 0x20000,
            WAVE_FORMAT_QUERY = 1,
            WAVE_MAPPED = 4,
            WAVE_FORMAT_DIRECT = 8
        }

        [Flags]
        public enum WaveHdrFlags : uint
        {
            WHDR_DONE = 1,
            WHDR_PREPARED = 2,
            WHDR_BEGINLOOP = 4,
            WHDR_ENDLOOP = 8,
            WHDR_INQUEUE = 16
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WAVEHDR
        {
            public IntPtr lpData;
            public uint dwBufferLength;
            public uint dwBytesRecorded;
            public IntPtr dwUser;
            public WaveHdrFlags dwFlags;
            public uint dwLoops;
            public IntPtr lpNext;
            public IntPtr reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WAVEFORMATEX
        {
            public ushort wFormatTag;
            public ushort nChannels;
            public uint nSamplesPerSec;
            public uint nAvgBytesPerSec;
            public ushort nBlockAlign;
            public ushort wBitsPerSample;
            public ushort cbSize;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WAVEOUTCAPS
        {
            public ushort wMid;
            public ushort wPid;
            public uint vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 164)]
            public string szPname;
            public uint dwFormats;
            public ushort wChannels;
            public ushort wReserved1;
            public uint dwSupport;
        }

        public struct WAVEINCAPS
        {
            public short ManufacturerId, ProductId;
            public uint DriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string Name;
            public uint Formats;
            public short Channels;
            public ushort Reserved;
            public Guid ManufacturerGuid, ProductGuid, NameGuid;
        }

        public struct MCI_RECORD_PARMS
        {
            public uint dwCallback;
            public uint dwFrom;
            public uint dwTo;
        }

        public struct MCI_OPEN_PARMS
        {
            public int dwCallback;
            public int wDeviceID;
            public string lpstrDeviceType;
            public string lpstrElementName;
            public string lpstrAlias;
        }

        public struct MCI_STATUS_PARMS
        {
            public int dwCallback;
            public int dwReturn;
            public int dwItem;
            public short dwTrack;
        }

        public struct MCI_INFO_PARMS
        {
            public int dwCallback;
            public string lpstrReturn;
            public int dwRetSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MMTIME
        {
            public int wType;
            public int u;
            public int x;
        }

        public const int MAX_ITEM_COUNT = 100;

        public const int MCI_OPEN = 0x803;
        public const int MCI_OPEN_ALIAS = 0x400;
        public const int MCI_OPEN_ELEMENT = 0x200;
        public const int MCI_OPEN_ELEMENT_ID = 0x800;
        public const int MCI_OPEN_SHAREABLE = 0x100;
        public const int MCI_OPEN_TYPE = 0x2000;
        public const int MCI_OPEN_TYPE_ID = 0x1000;


        public const int MCI_RECORD = 0x80F;
        public const int MCI_NOTIFY = 1;
        public const int MCI_WAIT = 2;
        public const int MCI_FROM = 4;
        public const int MCI_TO = 8;

        public const int MMSYSERR_NOERROR = 0;

        public const int MM_WOM_OPEN = 0x3BB;
        public const int MM_WOM_CLOSE = 0x3BC;
        public const int MM_WOM_DONE = 0x3BD;

        public const int MM_WIM_OPEN = 0x3BE;
        public const int MM_WIM_CLOSE = 0x3BF;
        public const int MM_WIM_DATA = 0x3C0;
        public const int MM_WIM_DONE = 0x3bd;

        public const int CALLBACK_FUNCTION = 0x00030000;

        public const int TIME_MS = 0x0001;
        public const int TIME_SAMPLES = 0x0002;
        public const int TIME_BYTES = 0x0004;

        public const int WAVE_INVALIDFORMAT = 0;
        public const int WAVE_FORMAT_1M08 = 1;
        public const int WAVE_FORMAT_1S08 = 2;
        public const int WAVE_FORMAT_1M16 = 4;
        public const int WAVE_FORMAT_1S16 = 8;
        public const int WAVE_FORMAT_2M08 = 16;
        public const int WAVE_FORMAT_2S08 = 32;
        public const int WAVE_FORMAT_2M16 = 64;
        public const int WAVE_FORMAT_2S16 = 128;
        public const int WAVE_FORMAT_4M08 = 256;
        public const int WAVE_FORMAT_4S08 = 512;
        public const int WAVE_FORMAT_4M16 = 1024;
        public const int WAVE_FORMAT_4S16 = 2048;
        public const int WAVE_FORMAT_PCM = 1;
    }
}
