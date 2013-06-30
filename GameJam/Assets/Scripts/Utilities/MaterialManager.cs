using UnityEngine;
using System.Collections;

public class MaterialManager : MonoBehaviour {

    public Material BlackMat, WhiteMat;

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
        processing = true;
        StartCoroutine(ChangeMaterial(BlackMat, target, fadeSpeed));
    }

    public void BeginWhiteMaterialChange(float target)
    {
        processing = true;
        StartCoroutine(ChangeMaterial(WhiteMat, target, fadeSpeed));
    }

    IEnumerator ChangeMaterial(Material mat, float target, float speed)
    {
        while (Mathf.Abs(mat.color.a - target) > .01f)
        {
            mat.color = Color.Lerp(mat.color, new Color(mat.color.r, mat.color.g, mat.color.b, target),speed);
            yield return null; 
        }
        processing = true;
        yield break;

    }

    void OnApplicationQuit()
    {
        BlackMat.color = new Color(BlackMat.color.r, BlackMat.color.g, BlackMat.color.b, 1);
        WhiteMat.color = new Color(WhiteMat.color.r, WhiteMat.color.g, WhiteMat.color.b, 1);
    }
}
