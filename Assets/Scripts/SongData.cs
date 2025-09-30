using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SongEntry
{
    public string displayName;
    public string chartFileNameNoExt;
    public AudioClip audioClip;
}

[CreateAssetMenu(fileName = "SongData", menuName = "Rhythm/Song Data")]
public class SongData : ScriptableObject
{
    public SongEntry[] songs;
}
