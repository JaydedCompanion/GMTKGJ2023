using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timeline {

    public enum MarkerType {
        MOVE_LEFT,
        MOVE_RIGHT,
        JUMP
    }

    [ExecuteInEditMode]
    [RequireComponent (typeof (SpriteRenderer))]
    [RequireComponent (typeof (BoxCollider2D))]
    public class TimelineMarker : MonoBehaviour {

        [field: SerializeField]
        public MarkerType markerType { get; private set; }
        [SerializeField]
        [Tooltip ("Enter 0 for one-shot event (jumps).")]
        private int duration;

        private SpriteRenderer renderer;
        private BoxCollider2D collider2D;

        public bool hasBeenExecuted;

        // Start is called before the first frame update
        void Start() {

            renderer = GetComponent<SpriteRenderer>();
            collider2D = GetComponent<BoxCollider2D>();

            if (!Application.isPlaying)
                return;

            Timeline.instance.onTimelineLoop.AddListener(() => { hasBeenExecuted = false; });

        }

        // Update is called once per frame
        void Update() {

            //Snap this block to pixel grid
            transform.localPosition = ((Vector3)Vector3Int.RoundToInt (transform.localPosition * Timeline.pixelSnappingSubdivisions)) / Timeline.pixelSnappingSubdivisions;
            //Lock it to the same axis as its parent timeline
            transform.localPosition = Vector3.Scale(transform.localPosition, Vector3.right);
            //Shift the transform slightly closer to the camera to avoid z-fighting
            transform.localPosition += Vector3.forward * -0.1f;

            if (markerType == MarkerType.JUMP)
                duration = 0;
            else
                duration = Mathf.Max(duration, 1);
            if (duration != 0) {
                renderer.size = new Vector2((duration + 1) / (float)Timeline.pixelSnappingSubdivisions * 1.5f, renderer.size.y);
                collider2D.size = renderer.size;
                collider2D.offset = new Vector2(renderer.size.x / 2, 1 / 32f);
            } else {
                renderer.size = new Vector2(1.5f / Timeline.pixelSnappingSubdivisions, renderer.size.y);
                collider2D.size = new Vector2(1, renderer.size.y);
                collider2D.offset = new Vector2(0.5f, 1 / 32f);
            }

        }

    }

}