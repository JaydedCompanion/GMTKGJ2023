using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour {

    public static ScreenTransition instance;
    public static bool levelTransition = true;

    public AnimationClip transitions;

    private Animator anim;
    private AnimatorOverrideController aoc;
    private TMPro.TextMeshPro textMesh;

    // Start is called before the first frame update
    void Start() {

        instance = this;
        anim = GetComponent<Animator>();
        textMesh = GetComponentInChildren<TMPro.TextMeshPro>();
        textMesh.enabled = false;

        if (levelTransition)
            StartCoroutine("LevelStart");
        else
            HideTransition();

    }

    // Update is called once per frame
    void Update() {

    }

    public IEnumerator LevelStart() {

        Time.timeScale = 0;

        ShowText(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(3);

        HideText();

        HideTransition();

        Time.timeScale = 1;

        levelTransition = false;

    }

    public static void ShowTransition () {
        instance.anim.SetBool("Show", true);
    }

    public static void HideTransition() {
        instance.anim.SetBool("Show", false);
    }

    public static void ShowText(string text) {
        instance.textMesh.text = text;
        instance.textMesh.enabled = true;
    }

    public static void HideText() {
        instance.textMesh.enabled = false;
    }

}