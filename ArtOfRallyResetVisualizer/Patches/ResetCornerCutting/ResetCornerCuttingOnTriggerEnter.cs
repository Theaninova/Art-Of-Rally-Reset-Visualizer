using System.Runtime.CompilerServices;
using ArtOfRallyResetVisualizer.Settings;
using HarmonyLib;

namespace ArtOfRallyResetVisualizer.Patches.ResetCornerCutting
{
    [HarmonyPatch(typeof(ResetCarCornerCutting), "OnTriggerEnter")]
    public class ResetCornerCuttingOnTriggerEnter
    {
        public static bool Prefix()
        {
            return ResetVisualizer.HardResetMode == HardResetMode.Intersect;
        }
    }
}