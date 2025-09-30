using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum HitGrade { Miss, Good, Perfect }

public class JudgmentService : MonoBehaviour
{
    public static JudgmentService instance;
    
    public AudioController audioCtrl;
    public float perfectMs = 60f, goodMs = 110f;
    public RectTransform judgeLineRect;
    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;
    public int scorePerPerfect = 100, scorePerGood = 50;

    public NoteSpawner spawner;
    public UIController ui;

    int combo = 0, score = 0;
    
    public int Score => score;
    public int Combo => combo;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    void Update()
    {
        if (!GameController.Instance.Started) return;
        
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick(Input.mousePosition);
        }
#endif
        
#if UNITY_IOS || UNITY_ANDROID
        for (int i = 0; i < Input.touchCount; i++)
        {
            var t = Input.GetTouch(i);
            if (t.phase == TouchPhase.Began)
                HandleClick(t.position);
        }
#endif
    }
    
    void HandleClick(Vector2 screenPos)
    {
        if (!uiRaycaster || !eventSystem) return;

        var data = new PointerEventData(eventSystem) { position = screenPos };
        var results = new List<RaycastResult>();
        uiRaycaster.Raycast(data, results);

        if (results.Count == 0) return;

        RhythmNote note = null;
        foreach (var r in results)
        {
            note = r.gameObject.GetComponentInParent<RhythmNote>();
            if (note != null) break;
        }
        
        if (note == null) return;
        
        var audioTime = audioCtrl.SongTimeMsDSP();
        double d = System.Math.Abs(note.TargetTimeMs - audioTime);
        
        // Debug.LogWarning($"{d} = |{note.TargetTimeMs} - {audioTime}|");
        
        if (d <= perfectMs)
        {
            score += scorePerPerfect;
            combo++;
            ui?.ShowJudge(HitGrade.Perfect, combo);
            note.SetClampStatus(true);
            ui?.UpdateScore(score);
        }
        else if (d <= goodMs)
        {
            score += scorePerGood;
            combo++;
            ui?.ShowJudge(HitGrade.Good, combo);
            note.SetClampStatus(true);
            ui?.UpdateScore(score);
        }
        else
        {
            combo = 0;
            ui?.ShowJudge(HitGrade.Miss, combo);
        }
        CheckBestResult();
    }

    private void CheckBestResult()
    {
        int idx = GameSession.HasSelection ? GameSession.SelectedIndex : 0;
        
        int currentBestScore = PlayerSave.GetBestScore(idx);
        int currentBestCombo = PlayerSave.GetBestCombo(idx);
        
        if (score > currentBestScore)
            PlayerSave.SetBestScore(idx, score);
        
        if (combo > currentBestCombo)
            PlayerSave.SetBestCombo(idx, combo);
    }
    
    public bool IsNotePerfect(RhythmNote note, double timeDiff)
    {
        if (note == null) return false;
        
        return timeDiff <= perfectMs;
    }
    
    public float GetHitYInLane(RectTransform lineRect, Transform laneParent)
    {
        Vector3 world = lineRect.TransformPoint(Vector3.zero);
        Vector3 local = laneParent.InverseTransformPoint(world);
        return local.y;
    }
}
