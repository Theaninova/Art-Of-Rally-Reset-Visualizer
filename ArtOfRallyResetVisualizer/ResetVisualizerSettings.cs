using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityModManagerNet;

namespace ArtOfRallyResetVisualizer
{
    public enum RenderMode { Off, OnHit, Always }
    
    public class ResetVisualizerSettings : UnityModManager.ModSettings, IDrawable
    {
        [Header("Modes")] [Draw(DrawType.ToggleGroup)]
        public RenderMode RenderMode = RenderMode.Off;

        [Header("Visualizers")] [Draw("Reset Zones")]
        public bool ShowResetZones = true;

        [Draw("Waypoints")] public bool ShowWaypoints = true;

        [Header("Colors")] [Draw("Reset Color")]
        public Color ResetColor = new Color(1f, 0f, 0f, 0.7f);

        [Draw("Waypoint Color")] public Color WaypointColor = new Color(1f, 0.8f, 0.8f, 0.1f);

        public void OnChange()
        {
            UpdateComponent(ResetVisualizer.NoGoVisualizersName, ShowResetZones, ResetColor);
            UpdateComponent(ResetVisualizer.WaypointVisualizersName, ShowWaypoints, WaypointColor);
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        private static void UpdateComponent(string name, bool active, Color color)
        {
            var root = GameObject.Find(name);
            if (root == null) return;

            var resets = root.transform.GetChild(0);
            resets.gameObject.SetActive(active);
            foreach (var reset in resets)
            {
                var renderer = ((Transform)reset).gameObject.GetComponent<MeshRenderer>();
                renderer.material.color = color;
            }
        }
    }
}