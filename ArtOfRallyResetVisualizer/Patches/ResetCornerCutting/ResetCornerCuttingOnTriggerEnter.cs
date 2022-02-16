using System.Runtime.CompilerServices;
using ArtOfRallyResetVisualizer.Patches.OutOfBoundsManager;
using ArtOfRallyResetVisualizer.Settings;
using HarmonyLib;
using UnityEngine;

namespace ArtOfRallyResetVisualizer.Patches.ResetCornerCutting
{
    [HarmonyPatch(typeof(ResetCarCornerCutting), "OnTriggerEnter")]
    public class ResetCornerCuttingOnTriggerEnter
    {
        public static bool Prefix(Collider __0, ResetCarCornerCutting __instance)
        {
            if (ResetVisualizer.HardResetMode == HardResetMode.Intersect) return true;

            if (GameEntryPoint.EventManager?.playerManager?.playerRigidBody == null ||
                GameEntryPoint.EventManager.status != EventStatusEnums.EventStatus.UNDERWAY || !__0.CompareTag("Car") ||
                GameEntryPoint.EventManager.outOfBoundsManager.IsResettingInProgress()) return false;

            Triggers.CurrentTrigger = __instance.gameObject.GetComponent<SphereCollider>();
            if (Triggers.CurrentTrigger == null)
            {
                Main.Logger.Error("Hard Reset without sphere collider!");
            }

            return false;
        }
    }
}