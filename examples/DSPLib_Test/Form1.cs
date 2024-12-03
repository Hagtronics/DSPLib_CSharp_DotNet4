using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Numerics;
using DSPLib;
using PlotWrapper;



/*
* Released under the MIT License
*
* DSP Library for C# / Demonstration Program & Examples
* 
* Copyright(c) 2016 Steven C. Hageman.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to
* deal in the Software without restriction, including without limitation the
* rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
* sell copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
* FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
* IN THE SOFTWARE.
*/




namespace DSPLib_Test
{
   public partial class Form1 : Form
   {
      public Form1()
      {
         InitializeComponent();
      }

      private void Form1_Load(object sender, EventArgs e)
      {
         // Load window combo box with the Window Names (from ENUMS)
         cmbWindow.DataSource = Enum.GetNames(typeof(DSPLib.DSP.Window.Type));

         lblCached.Text = "DFT Not Run Yet.";
      }

      // ===== FFT ============================================================================
      private void button1_Click(object sender, EventArgs e)
      {
         UInt32 N = Convert.ToUInt32(txtN.Text);
         UInt32 zeros = Convert.ToUInt32(txtZp.Text);
         double samplingRateHz = Convert.ToDouble(txtFs.Text);

         txtPoints.Text = (N + zeros).ToString();

         string selectedWindowName = cmbWindow.SelectedValue.ToString();
         DSPLib.DSP.Window.Type windowToApply = (DSPLib.DSP.Window.Type)Enum.Parse(typeof(DSPLib.DSP.Window.Type), selectedWindowName);

         // Update the output window
         txtPoints.Text = (N + zeros).ToString();

         // Make time series data
         double[] timeSeries = GenerateTimeSeriesData(N);

         // Apply window to the time series data
         double[] wc = DSP.Window.Coefficients(windowToApply, N);

         double windowScaleFactor = DSP.Window.ScaleFactor.Signal(wc);
         double[] windowedTimeSeries = DSP.Math.Multiply(timeSeries, wc);

         // Plot Time Series data
         Plot fig1 = new Plot("Figure 1 - FFT Time Series Input", "Sample", "Volts");
         fig1.PlotData(windowedTimeSeries);
         fig1.Show();

         // Instantiate & Initialize the FFT class
         DSPLib.FFT fft = new DSPLib.FFT();
         fft.Initialize(N, zeros);

         // Start a Stopwatch
         Stopwatch stopwatch = new Stopwatch();
         stopwatch.Start();

         // Perform a DFT
         Complex[] cpxResult = fft.Execute(windowedTimeSeries);

         // Calculate the elapsed time
         stopwatch.Stop();
         txtTime.Text = Convert.ToString(stopwatch.ElapsedMilliseconds / 1.0);

         // Convert the complex result to a scalar magnitude 
         double[] magResult = DSP.ConvertComplex.ToMagnitude(cpxResult);
         magResult = DSP.Math.Multiply(magResult, windowScaleFactor);

         // Plot the DFT Magnitude
         Plot fig2 = new Plot("Figure 2 - FFT Magnitude", "FFT Bin", "Mag (Vrms)");
         fig2.PlotData(magResult);
         fig2.Show();

         // Calculate the frequency span
         double[] fSpan = fft.FrequencySpan(samplingRateHz);

         // Convert and Plot Log Magnitude
         double[] mag = DSP.ConvertComplex.ToMagnitude(cpxResult);
         mag = DSP.Math.Multiply(mag, windowScaleFactor);
         double[] magLog = DSP.ConvertMagnitude.ToMagnitudeDBV(mag);
         Plot fig3 = new Plot("Figure 3 - FFT Log Magnitude ", "Frequency (Hz)", "Mag (dBV)");
         fig3.PlotData(fSpan, magLog);
         fig3.Show();
      }





      // ===== DFT ============================================================================
      private void button3_Click(object sender, EventArgs e)
      {
         // Get the setup data off the form
         UInt32 N = Convert.ToUInt32(txtN.Text);
         UInt32 zeros = Convert.ToUInt32(txtZp.Text);
         double samplingRateHz = Convert.ToDouble(txtFs.Text);

         string selectedWindowName = cmbWindow.SelectedValue.ToString();
         DSPLib.DSP.Window.Type windowToApply = (DSPLib.DSP.Window.Type)Enum.Parse(typeof(DSPLib.DSP.Window.Type), selectedWindowName);

         // Update the output window
         txtPoints.Text = (N + zeros).ToString();

         // Make time series data
         double[] timeSeries = GenerateTimeSeriesData(N);

         // Apply window to the time series data
         double[] wc = DSP.Window.Coefficients(windowToApply, N);

         double windowScaleFactor = DSP.Window.ScaleFactor.Signal(wc);
         double[] windowedTimeSeries = DSP.Math.Multiply(timeSeries, wc);

         // Plot Time Series data
         Plot fig1 = new Plot("Figure 1 - DFT Time Series Input", "Sample", "Volts");
         fig1.PlotData(windowedTimeSeries);
         fig1.Show();

         // Instantiate & Initialize the DFT class
         DSPLib.DFT dft = new DSPLib.DFT();
         dft.Initialize(N, zeros);

         // Update Label
         if (dft.IsUsingCached == true)
            lblCached.Text = "IsUsingCached = true";
         else
            lblCached.Text = "IsUsingCached = false";


         // Start a Stopwatch
         Stopwatch stopwatch = new Stopwatch();
         stopwatch.Start();

         // Perform a DFT
         Complex[] cpxResult = dft.Execute(windowedTimeSeries);

         // Calculate the elapsed time
         stopwatch.Stop();
         txtTime.Text = Convert.ToString(stopwatch.ElapsedMilliseconds / 1.0);

         // Convert the complex result to a scalar magnitude 
         double[] magResult = DSP.ConvertComplex.ToMagnitude(cpxResult);
         magResult = DSP.Math.Multiply(magResult, windowScaleFactor);


         // Plot the DFT Magnitude
         Plot fig2 = new Plot("Figure 2 - DFT Magnitude", "DFT Bin", "Mag (Vrms)");
         fig2.PlotData(magResult);
         fig2.Show();

         // Calculate the frequency span
         double[] fSpan = dft.FrequencySpan(samplingRateHz);

         // Convert and Plot Log Magnitude
         double[] mag = DSP.ConvertComplex.ToMagnitude(cpxResult);
         mag = DSP.Math.Multiply(mag, windowScaleFactor);
         double[] magLog = DSP.ConvertMagnitude.ToMagnitudeDBV(mag);
         Plot fig3 = new Plot("Figure 3 - DFT Log Magnitude ", "Frequency (Hz)", "Mag (dBV)");
         fig3.PlotData(fSpan, magLog);
         fig3.Show();
      }


      // Quit
      private void button2_Click(object sender, EventArgs e)
      {
         Application.Exit();
      }


      // Note: Since this is a test application - there is no exception handling on invalid inputs
      private double[] GenerateTimeSeriesData(UInt32 N)
      {
         // Generate some time series test data, based on form input
         double freqIn = Convert.ToDouble(txtFreqIn.Text);
         double ampRMS = Convert.ToDouble(txtAmplitude.Text);
         double freqSampling = Convert.ToDouble(txtFs.Text);
         double ampDC = Convert.ToDouble(txtDcOffset.Text);

         double[] timeSeries = DSP.Generate.ToneSampling(ampRMS, freqIn, freqSampling, N, ampDC);

         // Add another tone for testing
         //double[] timeSeries2 = DSP.Generate.ToneCycles(ampRMS / 10.0, 400, N, ampDC);
         //timeSeries = DSP.Vector.Add(timeSeries, timeSeries2);

         return timeSeries;
      }



      //=====[ Code Snippet Examples Using DSPLib ]==========================

      // A simple test function for testing the code samples 
      // See the example file ExampleCode.cs
      private void cmdTestFunction_Click(object sender, EventArgs e)
      {

         // Uncomment one of the examples below to run it.
         // Then stick a Debug Breakpoint at the end of the example to stop the program
         // and then you can examine the results with Watch Statements.
         Examples.Example1();
         //Examples.Example2();
         //Examples.Example3();
         //Examples.Example4();
         //Examples.Example5();
         //Examples.Example6();
         //Examples.Example7();
         //Examples.Example8();
         //Examples.Example9();
         //Examples.Example10();
         //Examples.Example11();
         //Examples.Example12();
      }
   }
}
