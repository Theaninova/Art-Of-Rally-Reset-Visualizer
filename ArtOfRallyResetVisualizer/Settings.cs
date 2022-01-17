using UnityEngine;
using UnityModManagerNet;

namespace ArtOfRallyResetVisualizer
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Header("Visualizers")]
        [Draw("Reset Zones")] public bool ShowResetZones = false;
        [Draw("Waypoints")] public bool ShowWaypoints = false;

        public void OnChange()
        {
            GameObject.Find(ResetVisualizer.NoGoVisualizersName)?.SetActive(ShowResetZones);
            GameObject.Find(ResetVisualizer.WaypointVisualizersName)?.SetActive(ShowWaypoints);
        }
    }
}