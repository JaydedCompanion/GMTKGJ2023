using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChange : MonoBehaviour
{
    [SerializeField]
    private Sprite pressedSprite;
    private Sprite defaultSprite;

    private bool isPressed = false;
    private bool isFreezed = false;

    void Start() {
        defaultSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        
    }

    void Update() {
        if(isPressed)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = pressedSprite;
        }else{
            this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;
        }
        if(isFreezed)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.2f,0.2f,0.2f,1f);
        }else{
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
        }
    }

    public void Pressed(){
        isPressed = true;
    }
    public void Released(){
        isPressed = false;
    }
    public void Freeze(){
        isFreezed = true;
    }
    public void UnFreeze(){
        isFreezed = false;
    }
}
