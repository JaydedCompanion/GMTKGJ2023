using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalP : MonoBehaviour, IPlatforms
{
    public bool selected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(selected){
            //player move this object if it is selected
        }
    }

    public void onClickEvent(){
        selected = true;
    }

    public void Deselect(){
        selected = false;
    }
}
