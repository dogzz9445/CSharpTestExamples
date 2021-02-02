using System.Windows;
using System.Windows.Controls;

namespace Ramp
{
    public partial class RampWindow : Window
    {
        private RampDisplay displayRamp;

        public RampWindow()
        {
            InitializeComponent();

            displayRamp = new RampDisplay();
            sliderBrightness.ValueChanged += this.SliderBrightness_ValueChanged;
            sliderGamma.ValueChanged += this.SliderGamma_ValueChanged;
        }

        private void SliderBrightness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            displayRamp.SetBrightness((sender as Slider).Value);
        }

        private void SliderGamma_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            displayRamp.SetGamma((sender as Slider).Value);
        }

        private void ButtonApply_Clicked(object sender, RoutedEventArgs e)
        {
            displayRamp.SetBrightness(sliderBrightness.Value);
            displayRamp.SetGamma(sliderGamma.Value);
            this.Close();
        }

        private void ButtonCancel_Clicked(object sender, RoutedEventArgs e)
        {
            displayRamp.UpdateRamps(true);
            this.Close();
        }
    }
}
