using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ComboVisuals
{
    public GameObject go;
    public Animation anim;
}

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject progressionBar;
    [SerializeField] private Image progressionFill;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Animation scoreTxtAnim;
    
    [Space]
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] Animation comboGoAnim;
    
    [Space]
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI finalComboText;
    
    [Space]
    [SerializeField] ComboVisuals perfectCombo;
    [SerializeField] ComboVisuals goodCombo;
    [SerializeField] ComboVisuals missCombo;
    
    [Header("Features Buttons")]
    [SerializeField] private Button startGameBtn;
    
    [Header("Panels")]
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject resultPanel;
    
    public static Action<float> OnUpdateProgressionBar;

    private void Start()
    {
        startGameBtn.gameObject.SetActive(true);
        
        progressionBar.SetActive(false);
        progressionFill.fillAmount = 0f;
        
        scoreText.gameObject.SetActive(false);
        
        perfectCombo.go.SetActive(false);
        goodCombo.go.SetActive(false);
        missCombo.go.SetActive(false);
        
        comboText.gameObject.SetActive(false);
        
        ShowResult(false);
        
        startGameBtn.onClick.RemoveAllListeners();
        startGameBtn.onClick.AddListener(() =>
        {
            GameController.Instance.OnStartGameClicked(() =>
            {
                UpdateProgression(0f);
                startGameBtn.gameObject.SetActive(false);
            });
        });
    }
    
    private void OnEnable()
    {
        OnUpdateProgressionBar += UpdateProgression;
    }
    
    private void OnDisable()
    {
        OnUpdateProgressionBar -= UpdateProgression;
    }
    
    private void UpdateProgression(float p)
    {
        if (progressionBar.activeInHierarchy == false)
        {
            progressionBar.SetActive(true);
        }
        
        if (progressionFill)
        {
            progressionFill.fillAmount = p;
        }
    }

    public void UpdateScore(int score)
    {
        if (scoreText.gameObject.activeInHierarchy == false)
        {
            scoreText.gameObject.SetActive(true);
        }

        if (scoreTxtAnim)
        {
            scoreTxtAnim.Stop();
            scoreTxtAnim.Play();
        }

        if (scoreText)
        {
            scoreText.text = score.ToString();
        }
    }

    public void ShowJudge(HitGrade g, int combo)
    {
        perfectCombo.go.SetActive(g == HitGrade.Perfect);
        goodCombo.go.SetActive(g == HitGrade.Good);
        missCombo.go.SetActive(g == HitGrade.Miss);

        //Grade
        switch (g)
        {
            case HitGrade.Perfect:
                perfectCombo.anim.Stop();
                perfectCombo.anim.Play();
                break;
            case HitGrade.Good:
                goodCombo.anim.Stop();
                goodCombo.anim.Play();
                break;
            case HitGrade.Miss:
                missCombo.anim.Stop();
                missCombo.anim.Play();
                break;
        }

        //Combo
        if (combo >= 2)
        {
            if (comboText.gameObject.activeInHierarchy == false)
            {
                comboText.gameObject.SetActive(true);
            }
            comboText.text = $"x{combo}";
        }
        
        if (combo > 1 && comboGoAnim)
        {
            comboGoAnim.Stop();
            comboGoAnim.Play();
        }
    }

    public void ShowFinal(int score)
    {
        int idx = GameSession.HasSelection ? GameSession.SelectedIndex : 0;
        
        int currentBestCombo = PlayerSave.GetBestCombo(idx);

        if (finalScoreText)
        {
            DOTween.To(() => 0, x => finalScoreText.text = x.ToString(), score, 0.5f)
                .SetEase(Ease.OutQuad);
        }
        
        if (finalComboText)
        {
            DOTween.To(() => 0, x => finalComboText.text = $"Best Combo: x{x}", currentBestCombo, 0.5f)
                .SetEase(Ease.OutQuad);
        }
    }

    public void ShowResult(bool on)
    {
        if (resultPanel) resultPanel.SetActive(on);
    }

    public void SetLoadingPanel(bool isOn)
    {
        loadingPanel.SetActive(isOn);
    }
}
