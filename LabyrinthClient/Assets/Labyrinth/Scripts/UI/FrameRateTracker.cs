using System.Collections.Generic;

namespace Labyrinth
{
    public class FrameRateTracker
    {
        private List<float> _samples = new List<float>();
        private double _sum = 0.0f;
        private int _maxSamples;

        public FrameRateTracker(int avgWindowSize)
        {
            _maxSamples = avgWindowSize;
        }

        public void AddSample(float sample)
        {
            // Add the next sample
            _samples.Add(sample);
            _sum += sample;

            // Remove the oldest sample.
            if (_samples.Count > _maxSamples)
            {
                _sum -= _samples[0];
                _samples.RemoveAt(0);
            }
        }

        public float GetAvg()
        {
            return _samples.Count > 0 ? (float)_sum/_samples.Count : 0.0f;
        }

    }
}