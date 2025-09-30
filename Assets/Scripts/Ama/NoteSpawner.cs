
using System;
using UnityEngine; using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour
{
    public NotePool pool;
    public Transform[] laneParents;
    public float spawnLeadTimeMs = 1500f;
    public float spawnPos = 1500f;
    public AudioController audioCtrl;
    public ChartLoader chart;
    public JudgmentService judgmentService;
    
    int nextIndex = 0;
    List<RhythmNote> live = new List<RhythmNote>();

    public bool HasLiveNotes => live.Count > 0;
    public bool FinishedSpawning => nextIndex >= chart.Chart.notes.Length;
    
    public static Action<RhythmNote> OnDespawnNote;
    
    void Start()
    {
        if (pool == null) pool = gameObject.AddComponent<NotePool>();
        pool.Warm();
    }

    private void OnEnable()
    {
        OnDespawnNote += Despawn;
    }
    
    private void OnDisable()
    {
        OnDespawnNote -= Despawn;
    }

    void Update()
    {
        if (!GameController.Instance.Started) return;
        
        double ms = audioCtrl.SongTimeMsDSP();
        while (nextIndex < chart.Chart.notes.Length)
        {
            var n = chart.Chart.notes[nextIndex];
            if (n.t - ms <= spawnLeadTimeMs)
            {
                var lane = Mathf.Clamp(n.lane, 0, laneParents.Length - 1);
                var note = pool.Get(laneParents[lane]);
                
                float hitY = judgmentService.GetHitYInLane(judgmentService.judgeLineRect, laneParents[lane]);
                
                note.Activate(n.t, ms, spawnPos, hitY, spawnLeadTimeMs);
                live.Add(note);
                nextIndex++;
            }
            else break;
        }

        for (int i = live.Count - 1; i >= 0; i--)
        {
            if (live[i] == null) live.RemoveAt(i);
        }
    }

    /// <summary>
    /// Despawns the specified note, returning it to the pool and removing it from the live list.
    /// </summary>
    public void Despawn(RhythmNote n)
    {
        n.Deactivate();
        pool.Recycle(n);
        live.Remove(n);
    }
    
    public void DeactiveAllNotes()
    {
        for (int i = live.Count - 1; i >= 0; i--)
        {
            if (live[i] != null)
            {
                live[i].Deactivate();
            }
        }
        live.Clear();
    }
}
