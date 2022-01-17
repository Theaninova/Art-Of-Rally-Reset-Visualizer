using UnityEngine;
using UnityModManagerNet;

namespace ArtOfRallyResetVisualizer
{
    public class ResetVisualizerSettings : UnityModManager.ModSettings, IDrawable
    {
        [Header("Visualizers")] [Draw("Reset Zones")]
        public bool ShowResetZones = false;

        [Draw("Waypoints")] public bool ShowWaypoints = false;

        public void OnChange()
        {
            GameObject.Find(ResetVisualizer.NoGoVisualizersName)
                ?.transform.GetChild(0)?.gameObject.SetActive(ShowResetZones);
            GameObject.Find(ResetVisualizer.WaypointVisualizersName)
                ?.transform.GetChild(0)?.gameObject.SetActive(ShowWaypoints);
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}