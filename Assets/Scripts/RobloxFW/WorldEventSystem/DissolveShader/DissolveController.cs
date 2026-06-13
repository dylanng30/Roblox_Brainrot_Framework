using System.Collections;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    [Header("Settings")]
    public float morphDuration = 2f;
    public Texture2D[] textureList;

    private Renderer targetRenderer;
    private MaterialPropertyBlock propBlock;

    // Trạng thái vòng lặp
    private int currentIndex = 0;
    private bool isMorphing = false;

    // Cache ID của các properties trong Shader Graph để tối ưu Garbage Collection
    private static readonly int TextureA_ID = Shader.PropertyToID("_TextureA");
    private static readonly int TextureB_ID = Shader.PropertyToID("_TextureB");
    private static readonly int Cutoff_ID = Shader.PropertyToID("_CutoffHeight");

    void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();

        if (textureList.Length > 0)
        {
            // Set trạng thái ban đầu: Cutoff 1.5f sẽ hiển thị hoàn toàn TextureA
            SetMaterialProperties(textureList[0], textureList[0], 1.5f);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMorphing && textureList.Length > 1)
        {
            StartCoroutine(MorphToNextTexture());
        }
    }

    private IEnumerator MorphToNextTexture()
    {
        isMorphing = true;

        int nextIndex = (currentIndex + 1) % textureList.Length;

        SetMaterialProperties(textureList[currentIndex], textureList[nextIndex], 1.5f);

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

        SetMaterialProperties(textureList[currentIndex], textureList[nextIndex], -1f);

        currentIndex = nextIndex;
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