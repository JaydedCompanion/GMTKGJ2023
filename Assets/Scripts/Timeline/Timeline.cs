using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timeline {

    [ExecuteInEditMode]
    public class Timeline : MonoBehaviour {

        public static readonly int pixelSnappingSubdivisions = 8;
        public static Timeline instance;

        [Header("Components")]
        public List<SpriteRenderer> timelineRenderers;
        public Transform playhead;

        [Header("Properties")]
        [Range(0, 1)]
        public float playback;
        [field: SerializeField]
        public float trackDuration { get; private set; }
        [field: SerializeField]
        public float timelineScale { get; private set; }
        [field: SerializeField]
        public Vector3 positionOffset { get; private set; }

        public UnityEngine.Events.UnityEvent onTimelineLoop;

        private UnityEngine.U2D.PixelPerfectCamera PPC;

        // Start is called before the first frame update
        void Awake() {
            instance = this;
            UpdateTrackUI();
            PPC = Camera.main.GetComponent<UnityEngine.U2D.PixelPerfectCamera>();
        }

        // Update is called once per frame
        void Update() {

            transform.position = Camera.main.ScreenToWorldPoint(Camera.main.pixelRect.size * new Vector2(0.5f, 1)) + positionOffset;
            if (!Application.isPlaying) {
                Debug.DrawRay(Camera.main.ScreenToWorldPoint(Camera.main.pixelRect.size * new Vector2(0.5f, 1)), -Vector2.one, Color.red);
                Debug.DrawRay(Camera.main.ScreenToWorldPoint(Camera.main.pixelRect.size * new Vector2(0.5f, 1)), Vector2.down, Color.red);
                UpdateTrackUI();
                return;
            }
            playback += Time.deltaTime / trackDuration;
            if (playback > 1) {
                playback -= 1;
                onTimelineLoop.Invoke();
            }

        }

        private void UpdateTrackUI() {
            foreach (SpriteRenderer renderer in timelineRenderers) {
                Vector2 size = renderer.size;
                Vector2 pos = renderer.transform.localPosition;
                size.x = SnapToGrid(trackDuration * timelineScale);
                pos.x = SnapToGrid(trackDuration * timelineScale * -0.5f);
                renderer.size = size;
                renderer.transform.localPosition = pos;
            }
        }

        public static float SnapToGrid(float value) { return SnapToGrid(value, 1); }
        public static float SnapToGrid(float value, float gridScale) {
            return Mathf.Round(value * pixelSnappingSubdivisions * gridScale) / (pixelSnappingSubdivisions * gridScale);
        }

    }

}