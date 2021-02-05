using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AudioControlApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                foreach (var process in Process.GetProcesses())
                {
                    int mixer;
                    int currentVol;
                    AudioMixer.MIXERCONTROL volCtrl = new AudioMixer.MIXERCONTROL();
                    AudioMixer.mixerOpen(out mixer, process.Id, 0, 0, 0);
                    int type = AudioMixer.MIXERCONTROL_CONTROLTYPE_VOLUME;
                    GetVolumeControl(mixer, AudioMixer.MIXERLINE_COMPONENTTYPE_DST_SPEAKERS, type, out volCtrl, out currentVol);
                    AudioMixer.mixerClose(mixer);

                    if (currentVol > 0)
                    {
                        Console.WriteLine("hell");
                        ComboAudios.Items.Add(process.Id);
                    }
                }
            };
        }

        private static bool GetVolumeControl(int hmixer, int componentType, int ctrlType, out AudioMixer.MIXERCONTROL mxc, out int vCurrentVol)
        {
            // This function attempts to obtain a mixer control. 
            // Returns True if successful. 
            AudioMixer.MIXERLINECONTROLS mxlc = new AudioMixer.MIXERLINECONTROLS();
            AudioMixer.MIXERLINE mxl = new AudioMixer.MIXERLINE();
            AudioMixer.MIXERCONTROLDETAILS pmxcd = new AudioMixer.MIXERCONTROLDETAILS();
            AudioMixer.MIXERCONTROLDETAILS_UNSIGNED du = new AudioMixer.MIXERCONTROLDETAILS_UNSIGNED();
            mxc = new AudioMixer.MIXERCONTROL();
            int rc;
            bool retValue;
            vCurrentVol = -1;

            mxl.cbStruct = Marshal.SizeOf(mxl);
            mxl.dwComponentType = componentType;

            rc = AudioMixer.mixerGetLineInfoA(hmixer, ref mxl, AudioMixer.MIXER_GETLINEINFOF_COMPONENTTYPE);

            if (AudioMixer.MMSYSERR_NOERROR == rc)
            {
                int sizeofMIXERCONTROL = 152;
                int ctrl = Marshal.SizeOf(typeof(AudioMixer.MIXERCONTROL));
                mxlc.pamxctrl = Marshal.AllocCoTaskMem(sizeofMIXERCONTROL);
                mxlc.cbStruct = Marshal.SizeOf(mxlc);
                mxlc.dwLineID = mxl.dwLineID;
                mxlc.dwControl = ctrlType;
                mxlc.cControls = 1;
                mxlc.cbmxctrl = sizeofMIXERCONTROL;

                // Allocate a buffer for the control 
                mxc.cbStruct = sizeofMIXERCONTROL;

                // Get the control 
                rc = AudioMixer.mixerGetLineControlsA(hmixer, ref mxlc, AudioMixer.MIXER_GETLINECONTROLSF_ONEBYTYPE);

                if (AudioMixer.MMSYSERR_NOERROR == rc)
                {
                    retValue = true;

                    // Copy the control into the destination structure 
                    mxc = (AudioMixer.MIXERCONTROL)Marshal.PtrToStructure(mxlc.pamxctrl, typeof(AudioMixer.MIXERCONTROL));
                }
                else
                {
                    retValue = false;
                }
                int sizeofMIXERCONTROLDETAILS = Marshal.SizeOf(typeof(AudioMixer.MIXERCONTROLDETAILS));
                int sizeofMIXERCONTROLDETAILS_UNSIGNED = Marshal.SizeOf(typeof(AudioMixer.MIXERCONTROLDETAILS_UNSIGNED));
                pmxcd.cbStruct = sizeofMIXERCONTROLDETAILS;
                pmxcd.dwControlID = mxc.dwControlID;
                pmxcd.paDetails = Marshal.AllocCoTaskMem(sizeofMIXERCONTROLDETAILS_UNSIGNED);
                pmxcd.cChannels = 1;
                pmxcd.item = 0;
                pmxcd.cbDetails = sizeofMIXERCONTROLDETAILS_UNSIGNED;

                rc = AudioMixer.mixerGetControlDetailsA(hmixer, ref pmxcd, AudioMixer.MIXER_GETCONTROLDETAILSF_VALUE);

                du = (AudioMixer.MIXERCONTROLDETAILS_UNSIGNED)Marshal.PtrToStructure(pmxcd.paDetails, typeof(AudioMixer.MIXERCONTROLDETAILS_UNSIGNED));

                vCurrentVol = du.dwValue;

                return retValue;
            }

            retValue = false;
            return retValue;
        }
    }
}
