namespace SRDebugger.UI
{
    using Services;
    using SRF;
    using UnityEngine;
    using UnityEngine.UI;

    public class ProfilerFPSLabel : SRMonoBehaviourEx
    {
        [SerializeField] private GameObject GO;
        [RequiredField] [SerializeField] private Text _text;
        
#if EZ_ENABLE_LOG
        private float _nextUpdate;
        [Import] private IProfilerService _profilerService;
        public float UpdateFrequency = 1f;
#endif

        protected override void Start()
        {
            GO.SetActive(false);
            
#if EZ_ENABLE_LOG
            base.Start();
            GO.SetActive(true);
#endif
        }
        
#if EZ_ENABLE_LOG
        protected override void Update()
        {

            base.Update();
            if (Time.realtimeSinceStartup > _nextUpdate)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            _text.text = "{0:0.00}".Fmt(1f / _profilerService.AverageFrameTime);
            _nextUpdate = Time.realtimeSinceStartup + UpdateFrequency;
        }
#endif
    }
}
