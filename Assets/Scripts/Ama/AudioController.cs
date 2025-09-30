
using System;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource source;
    
    [Range(-200,200)] public int latencyOffsetMs = 0;
    
    double dspStart; 
    int sampleRate;

    void Awake()
    {
        sampleRate = AudioSettings.outputSampleRate;
    }

    public void Play(float leadMs)
    {
        double leadSec = Mathf.Max(0f, leadMs) / 1000f;
        dspStart = AudioSettings.dspTime + leadSec;
        source.PlayScheduled(dspStart);
    }

    public void Stop()
    {
        source.Stop();
    }
    
    //update UI progression bar with song timeline
    private void Update()
    {
        if (!source.isPlaying) return;
        
        var timePlayedPercent = source.time / source.clip.length;
        UIController.OnUpdateProgressionBar?.Invoke(timePlayedPercent);
    }

    public double SongTimeMsDSP()
    {
        return (AudioSettings.dspTime - dspStart) * 1000f + latencyOffsetMs;
    }

    public double SongTimeMsFromSamples()
    {
        return (double)source.timeSamples / sampleRate * 1000f + latencyOffsetMs;
    }
}
