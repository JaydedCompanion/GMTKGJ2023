using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cursor : MonoBehaviour
{
    private Camera mainCam;
    private IPlatforms currentSelected;

    private bool paused = false;

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
        if (Input.GetKeyDown("r"))
        {
            StartCoroutine("RestartCoroutine");
            enabled = false;
        }
        if (Input.GetButtonDown("Cancel")) {
            paused = !paused;
            if (paused)
                ScreenTransition.ShowText("Paused");
            else
                ScreenTransition.HideText();
            Time.timeScale = paused ? 0 : 1;
        }
    }

    public IEnumerator RestartCoroutine() {

        ScreenTransition.ShowTransition();

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

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
