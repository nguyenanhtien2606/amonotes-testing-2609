using System.Collections.Generic;
using UnityEngine;

public class NotePool : MonoBehaviour
{
    [SerializeField] private RhythmNote prefab;
    [SerializeField] private int prewarm = 30;
    [SerializeField] private Transform poolParent;
    
    readonly Queue<RhythmNote> pool = new Queue<RhythmNote>();

    public void Warm()
    {
        for (int i = 0; i < prewarm; i++)
        {
            var n = Instantiate(prefab, poolParent);
            n.gameObject.SetActive(false);
            pool.Enqueue(n);
        }
    }

    public RhythmNote Get(Transform parent)
    {
        RhythmNote n = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab, parent);
        n.gameObject.transform.SetParent(parent, false);
        
        n.gameObject.SetActive(true);
        return n;
    }

    public void Recycle(RhythmNote n)
    {
        n.Deactivate();
        n.transform.SetParent(poolParent);
        n.gameObject.SetActive(false);
        pool.Enqueue(n);
    }
}