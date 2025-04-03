using System.Collections;
using UnityEngine;
using TMPro;

public class TextAnimate : MonoBehaviour
{
    public TMP_Text tmpText;
    public float waveFrequency = 2f; // Speed of the wave
    public float waveAmplitude = 5f; // Height of the wave
    public float waveSpeed = 2f; // Controls how fast the wave moves

    private Mesh mesh;
    private Vector3[] vertices;

    void Start()
    {
        if (tmpText == null)
        {
            tmpText = GetComponent<TMP_Text>();
        }
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        while (true)
        {
            tmpText.ForceMeshUpdate(); // Refresh the TMP mesh
            mesh = tmpText.mesh;
            vertices = mesh.vertices;

            TMP_TextInfo textInfo = tmpText.textInfo;
            float time = Time.time * waveSpeed;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible) continue;

                int vertexIndex = charInfo.vertexIndex;

                for (int j = 0; j < 4; j++) // Loop through the 4 vertices of each character
                {
                    Vector3 offset = new Vector3(0, Mathf.Sin(time + charInfo.origin * waveFrequency) * waveAmplitude, 0);
                    vertices[vertexIndex + j] += offset;
                }
            }

            mesh.vertices = vertices;
            tmpText.canvasRenderer.SetMesh(mesh);

            yield return null;
        }
    }
}
