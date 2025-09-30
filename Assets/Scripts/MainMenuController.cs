using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private SongData data;

    [Header("UI")] 
    [SerializeField] private GameObject loadingPanel;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        GameSession.Data = data;
    }

    private void Start()
    {
        loadingPanel.SetActive(false);
    }

    public void SelectSongByIndex(int index)
    {
        GameSession.SelectedIndex = index;
    }

    public void LoadGame()
    {
        if (!GameSession.HasSelection) GameSession.SelectedIndex = 0;
        
        //load scene async
        loadingPanel.SetActive(true);
        StartCoroutine(GameUtils.LoadSceneAsync("Main"));
    }
}
