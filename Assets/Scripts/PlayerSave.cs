using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSave
{
    private static string ScoreKey(int index) => $"BestScore_{index}";
    private static string ComboKey(int index) => $"BestCombo_{index}";

    public static int GetBestScore(int index) => PlayerPrefs.GetInt(ScoreKey(index), 0);
    public static int GetBestCombo(int index) => PlayerPrefs.GetInt(ComboKey(index), 0);

    public static void SetBestScore(int index, int value)
    {
        PlayerPrefs.SetInt(ScoreKey(index), value);
    }

    public static void SetBestCombo(int index, int value)
    {
        PlayerPrefs.SetInt(ComboKey(index), value);
    }
}
