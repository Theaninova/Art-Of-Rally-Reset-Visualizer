using UnityEngine;
using UnityModManagerNet;

namespace ArtOfRallyResetVisualizer
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("Show Reset Zones")] public bool ShowResetZones = false;
        [Draw("Show Waypoints")] public bool ShowWaypoints = false;

        public void OnChange()
        {
            GameObject.Find(ResetVisualizer.NoGoVisualizersName)?.SetActive(ShowResetZones);
            GameObject.Find(ResetVisualizer.WaypointVisualizersName)?.SetActive(ShowWaypoints);
        }
    }
}