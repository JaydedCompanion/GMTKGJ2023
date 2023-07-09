using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {

    public float gravRadius;
    public float gravForce;
    public float reachDistance;
    public Vector3 originOffset;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate() {

        CharacterBehaviour ch = CharacterBehaviour.instance;
        float chDist = Vector2.Distance(transform.position + originOffset, ch.transform.position);

        Vector2 gravVector = (transform.position + originOffset - ch.transform.position).normalized;
        gravVector *= gravForce / (transform.position + originOffset - ch.transform.position).sqrMagnitude;

        if (chDist < gravRadius)
            CharacterBehaviour.instance.rb.AddForce(gravVector * Time.fixedDeltaTime);

        if (chDist < reachDistance)
            PlayerReached();

    }

    private void PlayerReached() {

        CharacterBehaviour.instance.gameObject.SetActive(false);
        StartCoroutine("LevelEndCoroutine");

    }

    public IEnumerator LevelEndCoroutine() {

        yield return new WaitForSeconds(0.5f);

        ScreenTransition.ShowTransition();

        yield return new WaitForSeconds(0.5f);

        ScreenTransition.ShowText("Complete!");

        yield return new WaitForSeconds(2);

        ScreenTransition.levelTransition = true;

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);

    }

}