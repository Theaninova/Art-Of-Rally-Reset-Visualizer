using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace ArtOfRallyResetVisualizer
{
    public class ResetVisualizer
    {
        public class ResetVisualizerState
        {
            public List<GameObject> WaypointVisualizers = new List<GameObject>();
            public List<GameObject> NoGoVisualizers = new List<GameObject>();
        }

        static void SetTransparentColor(GameObject gameObject, Color color)
        {
			MeshRenderer meshRenderer = gameObject.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
			meshRenderer.receiveShadows = false;
			meshRenderer.material.SetInt("_SrcBlend", 1);
			meshRenderer.material.SetInt("_DstBlend", 10);
			meshRenderer.material.SetInt("_ZWrite", 0);
			meshRenderer.material.DisableKeyword("_ALPHATEST_ON");
			meshRenderer.material.DisableKeyword("_ALPHABLEND_ON");
			meshRenderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			meshRenderer.material.renderQueue = 3000;
			meshRenderer.material.SetFloat("_Mode", 3f);
			meshRenderer.material.color = color;
			meshRenderer.material.EnableKeyword("_SpecularHighlights");
			meshRenderer.material.EnableKeyword("_GlossyReflections");
			meshRenderer.material.SetFloat("_SpecularHighlights", 0f);
			meshRenderer.material.SetFloat("_GlossyReflections", 0f);
		}

        [HarmonyPatch(typeof(OutOfBoundsManager), nameof(OutOfBoundsManager.Start))]
        [HarmonyPostfix]
        public void Start(OutOfBoundsManager __instance, out ResetVisualizerState __state)
        {
            __state = new ResetVisualizerState();

            Transform resets = null;
            try
            {
                resets = GameObject.Find("ResetZones").transform;
            }
            catch
            {
            }

			if (resets != null)
			{
				foreach (object obj in resets)
				{
					SphereCollider resetObj = ((Transform)obj).gameObject.GetComponent<SphereCollider>();
					GameObject noGoVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					__state.NoGoVisualizers.Add(noGoVisualizer);
					noGoVisualizer.transform.position = resetObj.transform.position;
					noGoVisualizer.transform.localScale = new Vector3(resetObj.radius * 2f, resetObj.radius * 2f, resetObj.radius * 2f);
					((Collider)noGoVisualizer.GetComponent(typeof(Collider))).isTrigger = true;
					SetTransparentColor(noGoVisualizer, new Color(1f, 0f, 0f, 0.7f));
				}
			}

			foreach (Vector3 transform in __instance.GetWaypointList())
            {
				GameObject waypointVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				waypointVisualizer.SetActive(false);
				waypointVisualizer.transform.position = transform;
				waypointVisualizer.transform.localScale = new Vector3(34f, 34f, 34f);
				((Collider)waypointVisualizer.GetComponent(typeof(Collider))).isTrigger = true;
				SetTransparentColor(waypointVisualizer, new Color(1f, 0.8f, 0.8f, 0.1f));
			}
		}
    }
}
