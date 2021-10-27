using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class First_UI : UI_Base
{
    [SerializeField] private Text loading_Text;
    [SerializeField] private string loading_String = "Loading";

    [SerializeField] private Slider progress_Bar;
    [SerializeField] private Text progress_Text;

    public override void On_Open()
    {
        base.On_Open();

        loading_Text.text = loading_String;
        
        StartCoroutine("loading_Anim");
    }
    
    public override void On_Close()
    {
        base.On_Close();
    }

    IEnumerator loading_Anim()
    {
        yield return new WaitForSeconds(1F);
        var ao = SceneManager.LoadSceneAsync("Lobby");
        ao.allowSceneActivation = false;
        
        string dot_String = ""; 
        float progress = 0f;
        while (!ao.isDone)
        {
            yield return new WaitForSeconds(0.3F);
            
            if (ao.progress < 0.9F)
            {
                progress = Mathf.Clamp(progress+Time.deltaTime*4F, 0F, ao.progress);
            }
            else
            {
                progress = Mathf.Clamp(progress+Time.deltaTime*4F, 0F, 1F);
                if (progress >= 1F)
                {
                    ao.allowSceneActivation = false;
                    this.Remove();
                    UI_Manager.Instance.CreateUI<Login_UI>();    
                }
            }

            progress_Bar.value = progress;
            progress_Text.text = Mathf.Ceil(progress * 100) + "%";

            if (dot_String.Length >= 3)
            {
                dot_String = "";
            }
            dot_String = dot_String + ".";

            loading_Text.text = loading_String + " " + dot_String;
        }
    }
}
