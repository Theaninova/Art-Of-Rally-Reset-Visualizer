using System.Collections.Generic;
using ArtOfRallyResetVisualizer.Settings;
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

        public static bool IsLeaderboardDisabled;
        public static HardResetMode HardResetMode;

        public static List<GameObject> ResetObjects;
        public static List<GameObject> WaypointObjects;

        public static void SetTransparentColor(GameObject gameObject)
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
            material.EnableKeyword("_SpecularHighlights");
            material.EnableKeyword("_GlossyReflections");
            material.SetFloat(SpecularHighlights, 0f);
            material.SetFloat(GlossyReflections, 0f);
        }

        private static void UpdateComponent(List<GameObject> objects, bool active, Color color)
        {
            if (objects == null) return;

            foreach (var reset in objects)
            {
                reset.SetActive(active);
                reset.GetComponent<MeshRenderer>().material.color = color;
            }
        }

        public static void UpdateAllComponents(bool onHitShow = false)
        {
            var settings = Main.ResetVisualizerSettings;
            var alwaysShow = settings.RenderMode == RenderMode.Always && IsLeaderboardDisabled;

            UpdateComponent(
                ResetObjects,
                settings.ShowResetZones && (alwaysShow || onHitShow),
                settings.ResetColor
            );
            UpdateComponent(
                WaypointObjects,
                settings.ShowWaypoints && (alwaysShow || onHitShow),
                settings.WaypointColor
            );
        }
    }
}