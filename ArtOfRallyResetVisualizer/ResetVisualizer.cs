using HarmonyLib;
using UnityEngine;

namespace ArtOfRallyResetVisualizer
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ResetVisualizer
    {
        private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");
        private static readonly int Mode = Shader.PropertyToID("_Mode");
        private static readonly int SpecularHighlights = Shader.PropertyToID("_SpecularHighlights");
        private static readonly int GlossyReflections = Shader.PropertyToID("_GlossyReflections");

        public const string NoGoVisualizersName = "NoGoVisualizers";
        public const string WaypointVisualizersName = "WaypointVisualizersName";

        private static void SetTransparentColor(GameObject gameObject, Color color)
        {
            var meshRenderer = gameObject.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
            if (meshRenderer == null) return;
            meshRenderer.receiveShadows = false;
            var material = meshRenderer.material;
            material.SetInt(SrcBlend, 1);
            material.SetInt(DstBlend, 10);
            material.SetInt(ZWrite, 0);
            // ReSharper disable once StringLiteralTypo
            material.DisableKeyword("_ALPHATEST_ON");
            // ReSharper disable once StringLiteralTypo
            material.DisableKeyword("_ALPHABLEND_ON");
            // ReSharper disable once StringLiteralTypo
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
            material.SetFloat(Mode, 3f);
            material.color = color;
            material.EnableKeyword("_SpecularHighlights");
            material.EnableKeyword("_GlossyReflections");
            material.SetFloat(SpecularHighlights, 0f);
            material.SetFloat(GlossyReflections, 0f);
        }

        // ReSharper disable twice InconsistentNaming
        [HarmonyPatch(typeof(OutOfBoundsManager), nameof(OutOfBoundsManager.Start))]
        [HarmonyPostfix]
        public static void Start(OutOfBoundsManager __instance, float ___RESET_DISTANCE)
        {
            Transform resets = null;
            try
            {
                resets = GameObject.Find("ResetZones").transform;
            }
            catch
            {
                // ignored
            }

            if (resets != null)
            {
                var noGoParent = new GameObject(NoGoVisualizersName);
                noGoParent.SetActive(Main.Settings.ShowResetZones);
                Object.Instantiate(noGoParent);
                foreach (var obj in resets)
                {
                    var resetObj = ((Transform)obj).gameObject.GetComponent<SphereCollider>();
                    var noGoVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                    noGoVisualizer.transform.SetParent(noGoParent.transform);
                    noGoVisualizer.transform.position = resetObj.transform.position;

                    var radius = resetObj.radius * 2f;
                    noGoVisualizer.transform.localScale = new Vector3(radius, radius, radius);

                    ((Collider)noGoVisualizer.GetComponent(typeof(Collider))).isTrigger = true;
                    SetTransparentColor(noGoVisualizer, new Color(1f, 0f, 0f, 0.7f));
                }
            }

            var waypointParent = new GameObject(WaypointVisualizersName);
            waypointParent.SetActive(Main.Settings.ShowWaypoints);
            Object.Instantiate(waypointParent);
            foreach (var transform in __instance.GetWaypointList())
            {
                var waypointVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                waypointVisualizer.transform.SetParent(waypointParent.transform);
                waypointVisualizer.transform.position = transform;

                var radius = ___RESET_DISTANCE * 2f;
                waypointVisualizer.transform.localScale = new Vector3(radius, radius, radius);

                ((Collider)waypointVisualizer.GetComponent(typeof(Collider))).isTrigger = true;
                SetTransparentColor(waypointVisualizer, new Color(1f, 0.8f, 0.8f, 0.1f));
            }
        }
    }
}