
using UnityEngine;
using System.IO;
using Unity.Collections;

[System.Serializable]
public class MiniNote
{
    public int t;
    public int lane;
}

[System.Serializable]
public class MiniChart
{
    public int lanes;
    public int offsetMs;
    public MiniNote[] notes;
}

public class ChartLoader : MonoBehaviour
{
    public string chartFileName = "mini_map";
    public AudioController audioCtrl;

    [Space]
    public MiniChart Chart;
    
    void Awake()
    {
        if (GameSession.HasSelection)
        {
            var song = GameSession.SelectedSong;
            chartFileName = song.chartFileNameNoExt;
            
            if (audioCtrl && song.audioClip)
                audioCtrl.source.clip = song.audioClip;
        }

        var json = Resources.Load<TextAsset>($"Beatmaps/{chartFileName}");
        if (json == null)
        {
            Debug.LogError($"[ChartLoader] Chart not found: Resources/Beatmaps/{chartFileName}.json");
            return;
        }
        Chart = JsonUtility.FromJson<MiniChart>(json.text);
        
        if (Chart == null)
            Debug.LogError("[ChartLoader] Failed to parse chart JSON.");
    }
}
