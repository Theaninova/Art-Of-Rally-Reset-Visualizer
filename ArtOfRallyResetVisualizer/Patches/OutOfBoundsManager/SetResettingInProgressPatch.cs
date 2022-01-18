using ArtOfRallyResetVisualizer.Settings;
using HarmonyLib;

namespace ArtOfRallyResetVisualizer.Patches.OutOfBoundsManager
{
    [HarmonyPatch(typeof(global::OutOfBoundsManager), nameof(global::OutOfBoundsManager.SetResettingInProgress))]
    public class SetResettingInProgressPatch
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(bool __0)
        {
            if (Main.ResetVisualizerSettings.RenderMode != RenderMode.OnHit) return;
            ResetVisualizer.UpdateAllComponents(__0);
        }
    }
}