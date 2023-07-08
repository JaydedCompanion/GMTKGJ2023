using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChange : MonoBehaviour
{
    [SerializeField]
    private Sprite pressedSprite;

    private Sprite defaultSprite;

    private HorizontalP p;

    void Start() {
        defaultSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        p = this.gameObject.GetComponent<HorizontalP>();
    }

    void Update() {
        bool isPressed = p.selected;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = isPressed ? pressedSprite : defaultSprite;
    }
}
