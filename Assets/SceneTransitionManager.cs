using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public FadeScreen fadeScreen;

    public void GoToScene(int sceneIndex)
    {
        StartCoroutine(GoToSceneCorutine(sceneIndex));
    }
    IEnumerator GoToSceneCorutine(int sceneIndex)
    {
        fadeScreen.FadeOut();

        AsyncOperation operation=SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;
        float timer = 0;
        while (timer<= fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        operation.allowSceneActivation = true;
    }
    IEnumerator EndGoToSceneCorutine(int sceneIndex)
    {
        fadeScreen.SpecialFadeOut();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;
        float timer = 0;
        while (timer <= fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        operation.allowSceneActivation = true;
    }

    public void WinGame()
    {
        StartCoroutine(EndGoToSceneCorutine(0));
    }


}
