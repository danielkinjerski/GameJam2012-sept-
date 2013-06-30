using UnityEngine;
using System.Collections;
using System;

public class MaterialManager : MonoBehaviour {

    Action<float> OnFinishedTransition;
    public Material BlackMat, WhiteMat;
    public GameObject BlackWorld, WhiteWorld, CharBRender, CharWRender;

    public float fadeSpeed = .1f;

    public bool IsBusy
    {
        get{return processing;}
    }

    public bool processing = false;

    void Awake()
    {
        BlackMat.color = new Color(BlackMat.color.r, BlackMat.color.g, BlackMat.color.b, 1f);
        WhiteMat.color = new Color(WhiteMat.color.r, WhiteMat.color.g, WhiteMat.color.b, 1f);
    }

    public void BeginBlackMaterialChange(float target)
    {
        CharBRender.SetActive(true);
        CharWRender.SetActive(false);
        StartCoroutine(ChangeMaterial(BlackMat, target, fadeSpeed, BlackWorld));
    }

    public void BeginWhiteMaterialChange(float target)
    {
        CharBRender.SetActive(false);
        CharWRender.SetActive(true);
        StartCoroutine(ChangeMaterial(WhiteMat, target, fadeSpeed, WhiteWorld));
    }

    public void GrayMaterialSetup()
    {
        CharBRender.SetActive(true);
        CharWRender.SetActive(true);
    }

    IEnumerator ChangeMaterial(Material mat, float target, float speed, GameObject goToDisable)
    {
        while (processing)
        {
            yield return null; 
        }
        if (target > 0)
        {
            goToDisable.SetActive(true);
        }
        processing = true;
        while (Mathf.Abs(mat.color.a - target) > .01f)
        {
            mat.color = Color.Lerp(mat.color, new Color(mat.color.r, mat.color.g, mat.color.b, target),speed);
            yield return null; 
        }
        if (target == 0)
        {
            goToDisable.SetActive(false);
        }
        processing = false;
        yield break;

    }

    void OnApplicationQuit()
    {
        BlackMat.color = new Color(BlackMat.color.r, BlackMat.color.g, BlackMat.color.b, 1);
        WhiteMat.color = new Color(WhiteMat.color.r, WhiteMat.color.g, WhiteMat.color.b, 1);
    }
}
