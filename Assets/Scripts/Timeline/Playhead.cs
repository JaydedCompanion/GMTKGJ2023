using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timeline {

    [ExecuteInEditMode]
    public class Playhead : MonoBehaviour {

        public LayerMask timelineMarkerLayer;

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

            if (Timeline.instance)
                transform.localPosition = new Vector3(
                    Timeline.SnapToGrid((Timeline.instance.playback - 0.5f) * Timeline.instance.timelineScale * Timeline.instance.trackDuration),
                    transform.localPosition.y,
                    transform.localPosition.z
                );

            Debug.DrawRay(transform.position, Vector2.up * 3, new Color (255, 97, 181));

            if (!Application.isPlaying)
                return;

            //Fuck it, we're detecting markers on the timeline using a raycast cause I can't be bothered to do this in a smarter way.
            RaycastHit2D[] markers = Physics2D.RaycastAll(transform.position, Vector2.up, 3, timelineMarkerLayer);

            CharacterBehaviour.instance.moveDir = 0;

            foreach (RaycastHit2D m in markers) {

                TimelineMarker marker;
                try {
                    marker = m.collider.GetComponent<TimelineMarker>();
                } catch {
                    continue;
                }

                switch (marker.markerType) {
                    case MarkerType.MOVE_LEFT:
                        CharacterBehaviour.instance.moveDir--;
                        break;
                    case MarkerType.MOVE_RIGHT:
                        CharacterBehaviour.instance.moveDir++;
                        break;
                    case MarkerType.JUMP:
                        //Jump frames are one-shots. To avoid skipping them, they have very wide colliders, and once they are called they are flagged. Flagged events are ignored. This flag is reset upon reaching the end of the timeline.
                        if (marker.hasBeenExecuted)
                            break;
                        CharacterBehaviour.instance.doJumpFrame = true;
                        marker.hasBeenExecuted = true;
                        break;
                }

            }

        }

    }

}