using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    
    public AudioController audioCtrl;
    public NoteSpawner spawner;
    public JudgmentService judge;
    public UIController ui;

    bool started, ended;
    
    public bool Started => started;
    public bool Ended => ended;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        ui?.ShowResult(false);
        ui?.SetLoadingPanel(false);
    }
    
    public void OnStartGameClicked(Action callback = null)
    {
        if (started) return;
        audioCtrl.Play(spawner.spawnLeadTimeMs);
        started = true;
        callback?.Invoke();
    }

    void Update()
    {
        if (!started || ended) return;
        
        if (spawner.FinishedSpawning && !spawner.HasLiveNotes)
        {
            GameOver();
        }
    }
    
    public void Home()
    {
        ui?.SetLoadingPanel(true);
        StartCoroutine(GameUtils.LoadSceneAsync("Menu"));
    }
    
    public void ReloadGame()
    {
        ui?.SetLoadingPanel(true);
        StartCoroutine(GameUtils.LoadSceneAsync("Main"));
    }

    public void GameOver()
    {
        if (!started || ended) return;
        
        ended = true;
        started = false;
        
        var score = judge.Score;
        var combo = judge.Combo;
        
        audioCtrl.Stop();
        spawner.DeactiveAllNotes();
        
        StartCoroutine(CoDelayEndGame(0.8f, score));
        
        Debug.Log("GameOver");
        
        PlayerPrefs.Save();
    }
    
    IEnumerator CoDelayEndGame(float delay, int score)
    {
        yield return new WaitForSeconds(delay);
        ui?.ShowFinal(score);
        ui?.ShowResult(true);
    }
}