using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Numerics;
using DSPLib;

// DSPLib Examples to Show Usage for Various Scenarios
// 20Jun16 - Added Example 7
// 01Aug16 - Added Examples 8-10
// 11Aug16 - Added example 11


/*
Example 1 – A basic DFT Example that generates data and produces a spectrum output.
Example 2 – Same as example 1, but also includes adding window and scaling factors.
Example 3 – Same as example 2, but also includes adding zero padding.
Example 4 – Same as example 2, but final result is Log Magnitude dBV units.
Example 5 – FFT Example with windowing.
Example 6 – DFT with Noise + Windowing. Demonstrates the proper way to measure and average noise.
Example 7 - DFT with Signal + Noise + Windowing. Shows how to add noise to a signal.
Example 8 - DFT with signal and noise floor added (Simulates a real system).
Example 9 - DFT Phase extraction test.
Example 10 - FFT Phase extraction test with windowing, zero padding and automatic peak detection.
Example 11 - Basic DFT Example on a Thread w/BackgroundWorker.
*/

namespace DSPLib_Test
{
   public static class Examples
   {
      public static void Example1() //================[ Basic DFT Example ]================
      {
         // Generate a test signal,
         //      1 Vrms at 20,000 Hz
         //      Sampling Rate = 100,000 Hz
         //      DFT Length is 1000 Points
         double amplitude = 1.0;
         double frequency = 20000;
         UInt32 length = 2000;
         double samplingRate = 100000;

         double[] inputSignal = DSP.Generate.ToneSampling(amplitude, frequency, samplingRate, length);

         // Instantiate a new DFT
         DFT dft = new DFT();

         // Initialize the DFT
         // You only need to do this once or if you change any of the DFT parameters.
         dft.Initialize(length);

         // Call the DFT and get the scaled spectrum back
         Complex[] cSpectrum = dft.Execute(inputSignal);

         // Convert the complex spectrum to magnitude
         double[] lmSpectrum = DSP.ConvertComplex.ToMagnitude(cSpectrum);

         // Note: At this point lmSpectrum is a 501 byte array that 
         // contains a properly scaled Spectrum from 0 - 50,000 Hz

         // For plotting on an XY Scatter plot generate the X Axis frequency Span
         double[] freqSpan = dft.FrequencySpan(samplingRate);

         // At this point a XY Scatter plot can be generated from,
         // X axis => freqSpan
         // Y axis => lmSpectrum

         // In this example the maximum value of 1 Vrms is located at bin 200 (20,000 Hz)
      }


      public static void Example2() //================[ Basic DFT + Windowing Example ]================
      {
         // Same Input Signal as Example 1
         double amplitude = 1.0;
         double frequency = 20000;
         UInt32 length = 1000;
         double samplingRate = 100000;
         double[] inputSignal = DSP.Generate.ToneSampling(amplitude, frequency, samplingRate, length);

         // Apply window to the Input Data & calculate Scale Factor
         double[] wCoefs = DSP.Window.Coefficients(DSP.Window.Type.Hamming, length);
         double[] wInputData = DSP.Math.Multiply(inputSignal, wCoefs);
         double wScaleFactor = DSP.Window.ScaleFactor.Signal(wCoefs);

         // Instantiate & Initialize a new DFT
         DSPLib.DFT dft = new DSPLib.DFT();
         dft.Initialize(length);

         // Call the DFT and get the scaled spectrum back
         Complex[] cSpectrum = dft.Execute(wInputData);

         // Convert the complex spectrum to note: Magnitude Format
         double[] lmSpectrum = DSP.ConvertComplex.ToMagnitude(cSpectrum);

         // Properly scale the spectrum for the added window
         lmSpectrum = DSP.Math.Multiply(lmSpectrum, wScaleFactor);

         // For plotting on an XY Scatter plot generate the X Axis frequency Span
         double[] freqSpan = dft.FrequencySpan(samplingRate);

         // At this point a XY Scatter plot can be generated from,
         // X axis => freqSpan
         // Y axis => lmSpectrum
      }


      public static void Example3() //================[ Basic DFT + Windowing Example + Zero Padding ]================
      {
         // Same Input Signal as Example 1, except 5 Vrms amplitude
         double amplitude = 5.0;
         double frequency = 20000;
         UInt32 length = 1000;
         double samplingRate = 100000;
         double[] inputSignal = DSP.Generate.ToneSampling(amplitude, frequency, samplingRate, length);

         // Apply window to the Input Data & calculate Scale Factor
         double[] wCoefs = DSP.Window.Coefficients(DSP.Window.Type.FTHP, length);
         double[] wInputData = DSP.Math.Multiply(inputSignal, wCoefs);
         double wScaleFactor = DSP.Window.ScaleFactor.Signal(wCoefs);

         // Instantiate & Initialize a new DFT w/Zero Padding (5000 points)
         DSPLib.DFT dft = new DSPLib.DFT();
         dft.Initialize(length, 5000);

         // Call the DFT and get the scaled spectrum back
         Complex[] cSpectrum = dft.Execute(wInputData);

         // Convert the complex spectrum to note: Magnitude Format
         double[] lmSpectrum = DSP.ConvertComplex.ToMagnitude(cSpectrum);

         // Properly scale the spectrum for the added window
         lmSpectrum = DSP.Math.Multiply(lmSpectrum, wScaleFactor);

         // For plotting on an XY Scatter plot generate the X Axis frequency Span
         double[] freqSpan = dft.FrequencySpan(samplingRate);

         // At this point a XY Scatter plot can be generated from,
         // X axis => freqSpan
         // Y axis => lmSpectrum

         // For this example - the maximum value of 5 Vrms is at bin 1200 (20,000 Hz)
      }


      public static void Example4() //================[ Basic DFT + Windowing Example + Log Magnitude ]================
      {
         // Same Input Signal as Example 1, except amplitude is 5 Vrms.
         double amplitude = 5.0;
         double frequency = 20000;
         UInt32 length = 1000;
         double samplingRate = 100000;
         double[] inputSignal = DSP.Generate.ToneSampling(amplitude, frequency, samplingRate, length);

         // Apply window to the Input Data & calculate Scale Factor
         double[] wCoefs = DSP.Window.Coefficients(DSP.Window.Type.FTHP, length);
         double[] wInputData = DSP.Math.Multiply(inputSignal, wCoefs);
         double wScaleFactor = DSP.Window.ScaleFactor.Signal(wCoefs);

         // Instantiate & Initialize a new DFT
         DSPLib.DFT dft = new DSPLib.DFT();
         dft.Initialize(length);

         // Call the DFT and get the scaled spectrum back
         Complex[] cSpectrum = dft.Execute(wInputData);

         // Convert the complex spectrum to note: Magnitude Format
         double[] lmSpectrum = DSP.ConvertComplex.ToMagnitude(cSpectrum);

         // Properly scale the spectrum for the added window
         lmSpectrum = DSP.Math.Multiply(lmSpectrum, wScaleFactor);

         // Convert from linear magnitude to log magnitude format
         double[] logMagSpectrum = DSP.ConvertMagnitude.ToMagnitudeDBV(lmSpectrum);

         // For plotting on an XY Scatter plot generate the X Axis frequency Span
         double[] freqSpan = dft.FrequencySpan(samplingRate);

         // At this point a XY Scatter plot can be generated from,
         // X axis => freqSpan
         // Y axis => logMagSpectrum

         // In this example - maximum amplitude of 13.974 dBV is at bin 200 (20,000 Hz)
      }


      public static void Example5() //================[ Basic FFT + Windowing Example ]================
      {
         // Same Input Signal as Example 1, except everything is a power of two
         double amplitude = 1.0;
         double frequency = 32768;
         UInt32 length = 1024;
         double samplingRate = 131072;
         double[] inputSignal = DSP.Generate.ToneSampling(amplitude, frequency, samplingRate, length);

         // Apply window to the Input Data & calculate Scale Factor
         double[] wCoefs = DSP.Window.Coefficients(DSP.Window.Type.Hamming, length);
         double[] wInputData = DSP.Math.Multiply(inputSignal, wCoefs);
         double wScaleFactor = DSP.Window.ScaleFactor.Signal(wCoefs);

         // Instantiate & Initialize a new DFT
         DSPLib.FFT fft = new DSPLib.FFT();
         fft.Initialize(length);

         // Call the FFT and get the scaled spectrum back
         Complex[] cSpectrum = fft.Execute(wInputData);

         // Convert the complex spectrum to note: Magnitude Squared Format
         // See text for the reasons to use Mag^2 format.
         double[] lmSpectrum = DSP.ConvertComplex.ToMagnitude(cSpectrum);

         // Properly scale the spectrum for the added window
         lmSpectrum = DSP.Math.Multiply(lmSpectrum, wScaleFactor);

         // For plotting on an XY Scatter plot generate the X Axis frequency Span
         double[] freqSpan = fft.FrequencySpan(samplingRate);

         // At this point a XY Scatter plot can be generated from,
         // X axis => freqSpan
         // Y axis => lmSpectrum
      }


      public static void Example6() //================[ Basic DFT + Noise Analysis Example ]================
      {
         // Setup parameters to generate noise test signal of 5 nVrms / rt-Hz
         double amplitude = 5.0e-9;
         UInt32 length = 1000;
         double samplingRate = 2000;


         // Generate window & calculate Scale Factor for NOISE!
         double[] wCoefs = DSP.Window.Coefficients(DSP.Window.Type.Hamming, length);

         double wScaleFactor = DSP.Window.ScaleFactor.Noise(wCoefs, samplingRate);

         // Instantiate & Initialize a new DFT
         DSPLib.DFT dft = new DSPLib.DFT();
         dft.Initialize(length);

         // Average the noise 'N' times
         Int32 N = 1000;
         double[] noiseSum = new double[(length / 2) + 1];
         for (Int32 i = 0; i < N; i++)
         {
            // Generate the noise signal & apply window
            double[] inputSignal = DSP.Generate.NoisePsd(amplitude, samplingRate, length);
            inputSignal = DSP.Math.Multiply(inputSignal, wCoefs);

            // DFT the noise -> Convert -> Sum
            Complex[] cSpectrum = dft.Execute(inputSignal);
            double[] mag2 = DSP.ConvertComplex.ToMagnitudeSquared(cSpectrum);

            if (N == 0)
               noiseSum = DSP.Math.Add(noiseSum, 0);
            else
               noiseSum = DSP.Math.Add(noiseSum, mag2);
         }

         // Calculate Average, convert to magnitude format
         // See text for the reasons to use Mag^2 format.
         double[] averageNoise = DSP.Math.Divide(noiseSum, N);
         double[] lmSpectrum = DSP.ConvertMagnitudeSquared.ToMagnitude(averageNoise);

         // Properly scale the spectrum for the added window
         lmSpectrum = DSP.Math.Multiply(lmSpectrum, wScaleFactor);

         // For plotting on an XY Scatter plot generate the X Axis frequency Span
         double[] freqSpan = dft.FrequencySpan(samplingRate);

         // At this point a XY Scatter plot can be generated from,
         // X axis => freqSpan
         // Y axis => lmSpectrum as a Power Spectral Density Value

         // Extra Credit - Analyze Plot Data Ignoring the first and last 20 Bins
         // Average value should be what we generated = 5.0e-9 Vrms / rt-Hz
         double averageValue = DSP.Analyze.FindMean(lmSpectrum, 20, 20);
      }


      public static void Example7() //================[ Basic FFT + Windowing Example + ZeroPadding ]================
      {
         // Same Input Signal as Example 1, except everything is a power of two
         double amplitude = 1.0;
         double frequency = 32768;
         UInt32 length = 1024;
         double samplingRate = 131072;
         double[] inputSignal = DSP.Generate.ToneSampling(amplitude, frequency, samplingRate, length);

         // Apply window to the Input Data & calculate Scale Factor
         double[] wCoefs = DSP.Window.Coefficients(DSP.Window.Type.Hamming, length);
         double[] wInputData = DSP.Math.Multiply(inputSignal, wCoefs);
         double wScaleFactor = DSP.Window.ScaleFactor.Signal(wCoefs);

         // Instantiate & Initialize a new DFT
         DSPLib.FFT fft = new DSPLib.FFT();
         fft.Initialize(length, length * 3);           // Zero Padding = 1024 * 3

         // Call the FFT and get the scaled spectrum back
         Complex[] cSpectrum = fft.Execute(wInputData);

         // Convert the complex spectrum to note: Magnitude Squared Format
         // See text for the reasons to use Mag^2 format.
         double[] lmSpectrum = DSP.ConvertComplex.ToMagnitude(cSpectrum);

         // Properly scale the spectrum for the added window
         lmSpectrum = DSP.Math.Multiply(lmSpectrum, wScaleFactor);

         // For plotting on an XY Scatter plot generate the X Axis frequency Span
         double[] freqSpan = fft.FrequencySpan(samplingRate);

         // At this point a XY Scatter plot can be generated from,
         // X axis => freqSpan
         // Y axis => lmSpectrum
      }


      public static void Example8() //================[ Basic DFT + Signal & Noise + Log Magnitude ]================
      {
         // Same Input Signal as Example 1, except amplitude is 5 Vrms.
         double amplitude = 5.0;
         double frequency = 20000;
         UInt32 length = 1000;
         double samplingRate = 100000;
         double[] inputSignal = DSP.Generate.ToneSampling(amplitude, frequency, samplingRate, length);

         // Add noise that is about 80 dBc from signal level
         // 80 dBc is about the level of noise from a 14 bit ADC
         // 1/10,000 down from full scale
         double[] inputNoise = DSP.Generate.NoiseRms(amplitude / 10000.0, length);

         // Add noise to the signal
         double[] compositeInput = DSP.Math.Add(inputSignal, inputNoise);

         // Use the BH92 type window - this is a very "Spectrum Analyzer" like window.
         // Apply window to the Input Data & calculate Scale Factor
         double[] wCoefs = DSP.Window.Coefficients(DSP.Window.Type.BH92, length);
         double wScaleFactor = DSP.Window.ScaleFactor.Signal(wCoefs);
         double[] wInputData = DSP.Math.Multiply(compositeInput, wCoefs);

         // Instantiate & Initialize a new DFT
         DSPLib.DFT dft = new DSPLib.DFT();
         dft.Initialize(length);

         // Call the DFT and get the scaled spectrum back
         Complex[] cSpectrum = dft.Execute(wInputData);

         // Convert the complex spectrum to note: Magnitude Format
         double[] lmSpectrum = DSP.ConvertComplex.ToMagnitude(cSpectrum);

         // Properly scale the spectrum for the added window
         lmSpectrum = DSP.Math.Multiply(lmSpectrum, wScaleFactor);

         // Convert from linear magnitude to log magnitude format
         double[] logMagSpectrum = DSP.ConvertMagnitude.ToMagnitudeDBV(lmSpectrum);

         // For plotting on an XY Scatter plot generate the X Axis frequency Span
         double[] freqSpan = dft.FrequencySpan(samplingRate);

         // At this point a XY Scatter plot can be generated from,
         // X axis => freqSpan
         // Y axis => logMagSpectrum

         // In this example - maximum amplitude of 13.974 dBV is at bin 200 (20,000 Hz)
      }


      public static void Example9() //================[ Basic DFT Phase Test ]================
      {
         // Generate a Phase Ramp between two signals
         double[] resultPhase = new double[360];

         // Instantiate & Initialize a new DFT
         DSPLib.DFT dft = new DSPLib.DFT();
         dft.Initialize(2048);

         for (Int32 phase = 0; phase < 360; phase++)
         {
            double[] inputSignalRef = DSP.Generate.ToneCycles(7.0, 128, 2048);
            double[] inputSignalPhase = DSP.Generate.ToneCycles(7.0, 128, 2048, phaseDeg: phase);

            // Call the DFT and get the scaled spectrum back of a reference and a phase shifted signal.
            Complex[] cSpectrumRef = dft.Execute(inputSignalRef);
            Complex[] cSpectrumPhase = dft.Execute(inputSignalPhase);

            // Magnitude Format - Just as a test point
            //double[] lmSpectrumTest = DSPLib.DSP.ConvertComplex.ToMagnitude(cSpectrumRef);
            //double[] lmSpectrumTestPhase = DSPLib.DSP.ConvertComplex.ToMagnitude(cSpectrumPhase);

            // Extract the phase of bin 128
            double[] resultArrayRef = DSP.ConvertComplex.ToPhaseDegrees(cSpectrumRef);
            double[] resultArrayPhase = DSP.ConvertComplex.ToPhaseDegrees(cSpectrumPhase);
            resultPhase[phase] = resultArrayPhase[128] - resultArrayRef[128];
         }

         // resultPhase has a linear phase incrementing signal at this point.
         // Use UnwrapPhase() to remove phase jumps in output data.
      }


      public static void Example10() //================[ Basic FFT Phase + Phase UnWrapping + Windowing + Zero Padding Test ]================
      {
         // Generate a Phase Ramp between two signals
         double[] resultPhase = new double[600];
         double[] unwrapPhase = new double[600];

         UInt32 length = 2048;
         double[] wCoeff = DSP.Window.Coefficients(DSP.Window.Type.FTHP, length);

         // Instantiate & Initialize a new DFT
         DSPLib.FFT fft = new DSPLib.FFT();
         fft.Initialize(length, 3 * length);

         for (Int32 phase = 0; phase < 600; phase++)
         {
            double[] inputSignalRef = DSP.Generate.ToneCycles(7.0, 128, length, phaseDeg: 45.0);
            double[] inputSignalPhase = DSP.Generate.ToneCycles(7.0, 128, length, phaseDeg: phase);

            inputSignalRef = DSP.Math.Multiply(inputSignalRef, wCoeff);
            inputSignalPhase = DSP.Math.Multiply(inputSignalPhase, wCoeff);

            // Call the DFT and get the scaled spectrum back of a reference and a phase shifted signal.
            Complex[] cSpectrumRef = fft.Execute(inputSignalRef);
            Complex[] cSpectrumPhase = fft.Execute(inputSignalPhase);

            // Magnitude Format - Just as a test point
            double[] lmSpectrumTest = DSP.ConvertComplex.ToMagnitude(cSpectrumRef);
            UInt32 peakLocation = DSP.Analyze.FindMaxPosition(lmSpectrumTest);

            // Extract the phase of 'peak value' bin
            double[] resultArrayRef = DSP.ConvertComplex.ToPhaseDegrees(cSpectrumRef);
            double[] resultArrayPhase = DSP.ConvertComplex.ToPhaseDegrees(cSpectrumPhase);
            resultPhase[phase] = resultArrayPhase[peakLocation] - resultArrayRef[peakLocation];

         }
         unwrapPhase = DSP.Analyze.UnwrapPhaseDegrees(resultPhase);
      }


      public static void Example11() //================[ Basic DFT Example on a Thread w/BackgroundWorker ]================
      {
         // This is a very basic example that proves that the Task Parallel DFT can also run as a Background Worker Thread.

         // Setup the Background Worker
         BackgroundWorker dftWorker = new BackgroundWorker();
         dftWorker.DoWork += new DoWorkEventHandler(DftWorker_DoWork);

         // Launch thread
         dftWorker.RunWorkerAsync();

         // Wait till done
         while (dftWorker.IsBusy)
         { Application.DoEvents(); }
      }


      // For example 11
      private static void DftWorker_DoWork(object sender, DoWorkEventArgs e)
      {
         // DFT Length is 30000 Points, so it takes a very long time!
         double amplitude = 1.0;
         double frequency = 20000;
         UInt32 length = 30000;
         double samplingRate = 100000;

         double[] inputSignal = DSP.Generate.ToneSampling(amplitude, frequency, samplingRate, length);

         // Instantiate a new DFT
         DFT dft = new DFT();

         // Initialize the DFT
         // You only need to do this once or if you change any of the DFT parameters.
         dft.Initialize(length);

         // Call the DFT and get the scaled spectrum back
         Complex[] cSpectrum = dft.Execute(inputSignal);

         // Convert the complex spectrum to magnitude
         double[] lmSpectrum = DSP.ConvertComplex.ToMagnitude(cSpectrum);
      }


      public static void Example12() //================[ Basic DFT with IQ (Complex) Input Data + Windowing Example ]================
      {
         // Input Signal is I = 20kHz and Q is 21kHz
         double amplitude = 1.0;
         double frequency_I = 20000;
         double frequency_Q = 21000;
         UInt32 length = 1000;
         double samplingRate = 100000;
         double[] inputSignal_I = DSP.Generate.ToneSampling(amplitude, frequency_I, samplingRate, length);
         double[] inputSignal_Q = DSP.Generate.ToneSampling(amplitude, frequency_Q, samplingRate, length);

         // Make complex input signal with I and Q data from above
         Complex[] inputSignal_IQ = new Complex[length];
         for (int i = 0; i < length; i++)
         {
            inputSignal_IQ[i] = new Complex(inputSignal_I[i], inputSignal_Q[i]);
         }

         // Calculate Window and Scale Factor
         double[] wCoefs = DSP.Window.Coefficients(DSP.Window.Type.Hann, length);
         double wScaleFactor = DSP.Window.ScaleFactor.Signal(wCoefs);

         // Apply Window to complex input data using helper function to multiply a Complex[] array to a double[] array
         Complex[] wInputSignal_IQ = new Complex[length];
         wInputSignal_IQ = DSP.Math.Multiply(inputSignal_IQ, wCoefs);

         // Instantiate & Initialize a new DFT (Only need to do this once as long as length does not change)
         DSPLib.DFT dft = new DSPLib.DFT();
         dft.Initialize(length);

         // Call the DFT and get the scaled spectrum back
         Complex[] cSpectrum = dft.Execute(wInputSignal_IQ);

         // Convert the complex spectrum to Magnitude Format
         double[] lmSpectrum = DSP.ConvertComplex.ToMagnitude(cSpectrum);

         // Properly scale the spectrum for the added window
         lmSpectrum = DSP.Math.Multiply(lmSpectrum, wScaleFactor);

         // For plotting on an XY Scatter plot generate the X Axis frequency Span
         double[] freqSpan = dft.FrequencySpan(samplingRate);

         // At this point a XY Scatter plot can be generated from,
         // X axis => freqSpan
         // Y axis => lmSpectrum
      }
   }
}
