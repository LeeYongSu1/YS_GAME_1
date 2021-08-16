using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManagerEx : MonoBehaviour
{
    public Define.Scene scene;
    
    private void Start()
    {
        
    }
    public void LoadScene(Define.Scene type)
    {
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        
    }

    /*IEnumerator LoadingScene(Define.Scene type)
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(GetSceneName(type));
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone) 
        { 
            yield return null;
            timer += Time.deltaTime;

            if (op.progress < 0.9f) 
            { 
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);

                if (progressBar.fillAmount >= op.progress)
                { 
                    timer = 0f;
                } 
            } 
            else 
            { 
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                if (progressBar.fillAmount == 1.0f) 
                { 
                    op.allowSceneActivation = true; yield break; 
                } 
            } 
        }
    }*/
}
