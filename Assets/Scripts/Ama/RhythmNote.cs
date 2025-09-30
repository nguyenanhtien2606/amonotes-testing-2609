using UnityEngine;
using UnityEngine.UI;

public class RhythmNote : MonoBehaviour
{
    public Image visual;

    public Animation anim;
    public AnimationClip redAlertClip;
    
    [Space]
    public Color activeColor;
    public Color clamedColor;
    
    [Header("Debug")]
    public bool debugPerfect;
    public Color debugPrefectColor;

    AudioController audio;
    private double targetTimeMs;
    private double spawnMs;
    private float spawnPos = 6f;
    private float hitLineY = 0;
    private float endY;
    private float leadMs = 1500f;
    private float velocity;
    
    private bool active;
    private bool clamed;

    public double TargetTimeMs => targetTimeMs;
    
    void Awake()
    {
        audio = FindObjectOfType<AudioController>();
    }

    public void Activate(double targetMs, double spawnMs, float spawnPos, float hitLineY, float leadMs)
    {
        targetTimeMs = targetMs;
        this.spawnMs = spawnMs;
        this.spawnPos = spawnPos;
        this.hitLineY = hitLineY;
        this.leadMs = leadMs;
        
        velocity = spawnPos / Mathf.Max(1f, leadMs); // pixels per ms
        endY = hitLineY - 800f;
        
        active = true;
        transform.localPosition = new Vector3(0, spawnPos, 0);

        SetClampStatus(false);
    }
    
    public void SetClampStatus(bool isClamed)
    {
        clamed = isClamed;
        visual.raycastTarget = !isClamed;
        visual.color = isClamed ? clamedColor : activeColor;
    }

    public void Deactivate()
    {
        active = false;
    }
    
    public void SetDebugPerfect(bool isPerfect)
    {
        visual.color = isPerfect ? debugPrefectColor : (clamed ? clamedColor : activeColor);
    }

    void Update()
    {
        if (!active || !GameController.Instance.Started) return;
        
        double timeNow = audio.SongTimeMsDSP();
        float y = hitLineY + velocity * (float)(targetTimeMs - timeNow);
        
        var pos = transform.localPosition;
        pos.y = y;
        transform.localPosition = pos;

        if (debugPerfect)
        {
            SetDebugPerfect(JudgmentService.instance.IsNotePerfect(this, System.Math.Abs(targetTimeMs - timeNow)));
        }

        // despawn if miss
        if (y < endY)
        {
            if (clamed)
            {
                NoteSpawner.OnDespawnNote(this);
            }
            else
            {
                //call end game
                anim.Play(redAlertClip.name);
                GameController.Instance.GameOver();
            }
        }
    }
}