using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.sceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
           
        }
       public void FadeImmediate()
        {
            canvasGroup.alpha = 1;
        }
       public IEnumerator FadeOut(float time)
        {
        //    print("fade out");
            while (canvasGroup.alpha<1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }
       public IEnumerator FadeIn(float time)
        {
         //   print("fade in");
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }

}
