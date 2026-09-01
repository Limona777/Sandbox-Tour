using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamera : MonoBehaviour
{
    [Header("Transparent")]
    public float transparentAlpha = 0.3f;
    public float fadeSpeed = 5f;

    private Transform[] childs;
    private Transform player;
    private Renderer[] childRenderers;
    private Collider[] childColliders;
    private Material[][] originalMaterials;
    private float[] targetAlphas;

    void Start()
    {
        int count = transform.childCount;
        childs = new Transform[count];
        childRenderers = new Renderer[count];
        childColliders = new Collider[count];
        originalMaterials = new Material[count][];
        targetAlphas = new float[count];

        for (int i = 0; i < count; i++)
        {
            childs[i] = transform.GetChild(i);
            childRenderers[i] = childs[i].GetComponent<Renderer>();
            childColliders[i] = childs[i].GetComponent<Collider>();

            if (childRenderers[i] != null)
            {
                originalMaterials[i] = childRenderers[i].materials;
                targetAlphas[i] = 1f;
            }
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
            Debug.LogWarning("No Player");
    }

    void Update()
    {
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].rotation = Camera.main.transform.rotation;
        }

        if (player == null || childRenderers == null) return;

        for (int i = 0; i < childs.Length; i++)
        {
            if (childRenderers[i] == null || childColliders[i] == null) continue;

            bool isOccluding = IsChildOccludingPlayer(childs[i], childColliders[i]);

            targetAlphas[i] = isOccluding ? transparentAlpha : 1f;

            float currentAlpha = childRenderers[i].material.color.a;
            float newAlpha = Mathf.MoveTowards(currentAlpha, targetAlphas[i], fadeSpeed * Time.deltaTime);

            foreach (Material mat in originalMaterials[i])
            {
                Color color = mat.color;
                color.a = newAlpha;
                mat.color = color;

                if (mat.shader.name.Contains("Standard"))
                {
                    mat.SetFloat("_Mode", 2);
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.DisableKeyword("_ALPHATEST_ON");
                    mat.EnableKeyword("_ALPHABLEND_ON");
                    mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mat.renderQueue = 3000;
                }
            }
        }
    }

    private bool IsChildOccludingPlayer(Transform child, Collider col)
    {
        if (Camera.main == null || player == null) return false;

        Vector3 camPos = Camera.main.transform.position;
        Vector3 playerPos = player.position;
        Vector3 direction = playerPos - camPos;
        float distance = direction.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(camPos, direction.normalized, out hit, distance))
        {
            if (hit.collider == col)
            {
                return true;
            }
        }
        return false;
    }
}