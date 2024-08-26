using UnityEngine;
using System.Collections;

public class DarkToLight : MonoBehaviour
{
    public Transform player;
    public Transform targetCube;
    public float triggerDistance = 5f;
    public Material blackSkyboxMaterial;
    public Material skyboxMaterial;
    public float transitionSpeed = 1f;

    private bool isTransitioning = false;
    private float initialIntensity;
    private Color initialAmbientColor;

    private Material initialSkyboxMaterial;
    private Light directionalLight;

    void Start()
    {
        initialIntensity = RenderSettings.ambientIntensity;
        initialAmbientColor = RenderSettings.ambientLight;
        initialSkyboxMaterial = RenderSettings.skybox;

        // Use Object.FindAnyObjectByType for efficiency
        directionalLight = Object.FindAnyObjectByType<Light>();

        RenderSettings.skybox = blackSkyboxMaterial;
        RenderSettings.ambientIntensity = 0f;
        RenderSettings.ambientLight = Color.black;

        if (directionalLight != null)
        {
            directionalLight.intensity = 0f;
        }
    }

    void Update()
    {
        float distanceToCube = Vector3.Distance(player.position, targetCube.position);

        if (distanceToCube <= triggerDistance)
        {
            if (!isTransitioning)
            {
                isTransitioning = true;
                StopAllCoroutines();
                StartCoroutine(LightTransition());
            }
        }
        else if (isTransitioning)
        {
            isTransitioning = false;
            StopAllCoroutines();
            StartCoroutine(ResetLighting());
        }
    }

    private IEnumerator LightTransition()
    {
        float transitionProgress = 0f;

        while (transitionProgress < 1f)
        {
            transitionProgress += Time.deltaTime * transitionSpeed;

            RenderSettings.ambientIntensity = Mathf.Lerp(0f, initialIntensity, transitionProgress);
            RenderSettings.ambientLight = Color.Lerp(Color.black, initialAmbientColor, transitionProgress);

            if (skyboxMaterial != null)
            {
                RenderSettings.skybox = skyboxMaterial;
                DynamicGI.UpdateEnvironment(); // Update global illumination
            }

            if (directionalLight != null)
            {
                directionalLight.intensity = Mathf.Lerp(0f, 1f, transitionProgress);
            }

            yield return null;
        }

        RenderSettings.ambientIntensity = initialIntensity;
        RenderSettings.ambientLight = initialAmbientColor;
        RenderSettings.skybox = skyboxMaterial;
    }

    private IEnumerator ResetLighting()
    {
        float transitionProgress = 0f;

        while (transitionProgress < 1f)
        {
            transitionProgress += Time.deltaTime * transitionSpeed;

            RenderSettings.ambientIntensity = Mathf.Lerp(initialIntensity, 0f, transitionProgress);
            RenderSettings.ambientLight = Color.Lerp(initialAmbientColor, Color.black, transitionProgress);

            RenderSettings.skybox = blackSkyboxMaterial; // Reset to black skybox
            DynamicGI.UpdateEnvironment(); // Ensure environment updates

            if (directionalLight != null)
            {
                directionalLight.intensity = Mathf.Lerp(1f, 0f, transitionProgress);
            }

            yield return null;
        }

        RenderSettings.ambientIntensity = 0f;
        RenderSettings.ambientLight = Color.black;
        RenderSettings.skybox = blackSkyboxMaterial;
    }
}




// using UnityEngine;
// using System.Collections;

// public class DarkToLight : MonoBehaviour
// {
//     public Transform player;
//     public Transform targetCube;
//     public float triggerDistance = 5f;
//     public Material skyboxMaterial;
//     public Material blackSkyboxMaterial; // Black skybox material
//     public float transitionSpeed = 1f;

//     private bool isTransitioning = false;
//     private float initialIntensity;
//     private Color initialAmbientColor;
//     private Material initialSkyboxMaterial;
//     private Light directionalLight;

//     void Start()
//     {
//         initialIntensity = RenderSettings.ambientIntensity;
//         initialAmbientColor = RenderSettings.ambientLight;
//         initialSkyboxMaterial = RenderSettings.skybox;

//         // Use Object.FindAnyObjectByType for efficiency
//         directionalLight = Object.FindAnyObjectByType<Light>();

//         // Set the initial environment to black
//         RenderSettings.ambientIntensity = 0f;
//         RenderSettings.ambientLight = Color.black;
//         RenderSettings.skybox = blackSkyboxMaterial;

//         if (directionalLight != null)
//         {
//             directionalLight.intensity = 0f;
//         }
//     }

//     void Update()
//     {
//         float distanceToCube = Vector3.Distance(player.position, targetCube.position);

//         if (distanceToCube <= triggerDistance)
//         {
//             if (!isTransitioning)
//             {
//                 isTransitioning = true;
//                 StopAllCoroutines();
//                 StartCoroutine(LightTransition());
//             }
//         }
//         else if (isTransitioning)
//         {
//             isTransitioning = false;
//             StopAllCoroutines();
//             StartCoroutine(ResetLighting());
//         }
//     }

//     private IEnumerator LightTransition()
//     {
//         float transitionProgress = 0f;

//         while (transitionProgress < 1f)
//         {
//             transitionProgress += Time.deltaTime * transitionSpeed;

//             RenderSettings.ambientIntensity = Mathf.Lerp(0f, initialIntensity, transitionProgress);
//             RenderSettings.ambientLight = Color.Lerp(Color.black, initialAmbientColor, transitionProgress);

//             if (skyboxMaterial != null)
//             {
//                 RenderSettings.skybox = skyboxMaterial;
//                 DynamicGI.UpdateEnvironment(); // Update global illumination
//             }

//             if (directionalLight != null)
//             {
//                 directionalLight.intensity = Mathf.Lerp(0f, 1f, transitionProgress);
//             }

//             yield return null;
//         }

//         RenderSettings.ambientIntensity = initialIntensity;
//         RenderSettings.ambientLight = initialAmbientColor;
//         RenderSettings.skybox = skyboxMaterial;
//     }

//     private IEnumerator ResetLighting()
//     {
//         float transitionProgress = 0f;

//         while (transitionProgress < 1f)
//         {
//             transitionProgress += Time.deltaTime * transitionSpeed;

//             RenderSettings.ambientIntensity = Mathf.Lerp(initialIntensity, 0f, transitionProgress);
//             RenderSettings.ambientLight = Color.Lerp(initialAmbientColor, Color.black, transitionProgress);

//             RenderSettings.skybox = blackSkyboxMaterial;
//             DynamicGI.UpdateEnvironment(); // Update global illumination

//             if (directionalLight != null)
//             {
//                 directionalLight.intensity = Mathf.Lerp(1f, 0f, transitionProgress);
//             }

//             yield return null;
//         }

//         RenderSettings.ambientIntensity = 0f;
//         RenderSettings.ambientLight = Color.black;
//         RenderSettings.skybox =

