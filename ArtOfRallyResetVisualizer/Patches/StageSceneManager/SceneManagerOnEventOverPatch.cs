using ArtOfRallyResetVisualizer.Settings;
using HarmonyLib;

namespace ArtOfRallyResetVisualizer.Patches.StageSceneManager
{
    [HarmonyPatch(typeof(global::StageSceneManager), nameof(global::StageSceneManager.OnEventOver))]
    public class SceneManagerOnEventOverPatch
    {
        public static void Prefix(ref bool __0)
        {
            if (ResetVisualizer.IsLeaderboardDisabled)
            {
                // apply terminal damage
                __0 = true;
            }
        }

        public static void Postfix()
        {
            ResetVisualizer.IsLeaderboardDisabled = Main.ResetVisualizerSettings.RenderMode == RenderMode.Always;
        }
    }
}