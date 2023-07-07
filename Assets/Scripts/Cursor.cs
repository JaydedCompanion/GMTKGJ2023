using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private Camera mainCam;
    private IPlatforms currentSelected;

    // Start is called before the first frame update
    void Awake()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //left click
        if (Input.GetMouseButtonDown(0))
        {
            ClickObject();
        }
    }

    private void ClickObject()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hits2D = Physics2D.GetRayIntersection(ray);

        if (hits2D.collider != null)
        {
            IPlatforms platform = hits2D.collider.gameObject.GetComponent<IPlatforms>();
            if (currentSelected != null)
            {
                currentSelected.Deselect();
            }
            currentSelected = platform;
            if (platform != null)
            {
                platform.onClickEvent();
            }
            Debug.Log("hit" + hits2D.collider.gameObject.name);
        }
    }
}
