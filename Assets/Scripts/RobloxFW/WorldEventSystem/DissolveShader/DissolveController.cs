using System.Collections;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    [Header("Settings")]
    public float morphDuration = 2f;
    public Texture2D[] textureList;

    private Renderer targetRenderer;
    private MaterialPropertyBlock propBlock;
    
    private int currentIndex = 0;
    private bool isMorphing = false;
    
    private static readonly int TextureA_ID = Shader.PropertyToID("_TextureA");
    private static readonly int TextureB_ID = Shader.PropertyToID("_TextureB");
    private static readonly int Cutoff_ID = Shader.PropertyToID("_CutoffHeight");

    void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();

        if (textureList.Length > 0)
        {
            SetMaterialProperties(textureList[0], textureList[0], 1.5f);
        }
    }

    public void TriggerMorph(Texture2D newTexture)
    {
        if (textureList.Length > 0 && newTexture != null)
        {
            StopAllCoroutines();
            StartCoroutine(MorphToNextTexture(newTexture));
        }
    }

    private IEnumerator MorphToNextTexture(Texture2D nextTexture)
    {
        isMorphing = true;

        Texture2D currentTexture = textureList[currentIndex];
        
        SetMaterialProperties(currentTexture, nextTexture, 1.5f);

        float elapsedTime = 0f;
        while (elapsedTime < morphDuration)
        {
            elapsedTime += Time.deltaTime;

            float currentCutoff = Mathf.Lerp(1.5f, -1f, elapsedTime / morphDuration);

            targetRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat(Cutoff_ID, currentCutoff);
            targetRenderer.SetPropertyBlock(propBlock);

            yield return null;
        }

        SetMaterialProperties(currentTexture, nextTexture, -1f);
        
        textureList[currentIndex] = nextTexture; 

        isMorphing = false;
    }

    private void SetMaterialProperties(Texture2D texA, Texture2D texB, float cutoffValue)
    {
        targetRenderer.GetPropertyBlock(propBlock);

        propBlock.SetTexture(TextureA_ID, texA);
        propBlock.SetTexture(TextureB_ID, texB);
        propBlock.SetFloat(Cutoff_ID, cutoffValue);

        targetRenderer.SetPropertyBlock(propBlock);
    }
}