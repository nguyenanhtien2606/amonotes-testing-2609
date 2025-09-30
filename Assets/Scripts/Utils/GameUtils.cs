using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUtils
{
    public static IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    public static void UpdateNumEffects(TextMeshProUGUI txt, int num)
    {
        DOTween.To(() => 0, x => txt.text = x.ToString(), num, 0.5f)
            .SetEase(Ease.OutQuad);
    }
}
