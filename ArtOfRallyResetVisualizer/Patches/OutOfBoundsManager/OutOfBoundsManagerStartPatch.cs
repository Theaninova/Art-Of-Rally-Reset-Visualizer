using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArtOfRallyResetVisualizer.Settings;
using HarmonyLib;
using UnityEngine;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ArtOfRallyResetVisualizer.Patches.OutOfBoundsManager
{
    public static class ResetVisualizerState
    {
        public static SphereCollider[] HardResets = { };

        public static readonly MethodInfo ResetCarOutOfBoundsAnimation =
            typeof(global::OutOfBoundsManager).GetMethod("ResetCarOutOfBoundsAnimation",
                BindingFlags.Instance | BindingFlags.NonPublic);
    }

    [HarmonyPatch(typeof(global::OutOfBoundsManager), "FixedUpdate")]
    public class FixedUpdatePatch
    {
        public static void Postfix(
            global::OutOfBoundsManager __instance,
            ref bool ___isShowingOutOfBoundsAnimation,
            Vector3 ___playerPosition)
        {
            if (ResetVisualizer.HardResetMode != HardResetMode.Distance) return;

            if (GameEntryPoint.EventManager.playerManager == null ||
                GameModeManager.GameMode == GameModeManager.GAME_MODES.FREEROAM ||
                GameEntryPoint.EventManager.status != EventStatusEnums.EventStatus.UNDERWAY ||
                GameEntryPoint.EventManager.outOfBoundsManager.IsResettingInProgress()) return;

            var collisions =
                from collider in ResetVisualizerState.HardResets
                where  Vector3.Distance(___playerPosition, collider.center) < collider.radius
                select collider;
            if (!collisions.Any()) return;

            ___isShowingOutOfBoundsAnimation = true;
            __instance.SetResettingInProgress(true);
            __instance.StartCoroutine(
                (IEnumerator)ResetVisualizerState.ResetCarOutOfBoundsAnimation.Invoke(__instance,
                    new object[] { TimeConstants.ResetCarPenalty }));
        }
    }

    [HarmonyPatch(typeof(global::OutOfBoundsManager), nameof(global::OutOfBoundsManager.Start))]
    public class OutOfBoundsManagerStartPatch
    {
        // ReSharper disable twice InconsistentNaming
        public static void Postfix(global::OutOfBoundsManager __instance, float ___RESET_DISTANCE)
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
                ResetVisualizer.ResetObjects?.ForEach(Object.Destroy);
                ResetVisualizer.ResetObjects = new List<GameObject>();
                var hardResets = new List<SphereCollider>();
                foreach (var obj in resets)
                {
                    var resetObj = ((Transform)obj).gameObject.GetComponent<SphereCollider>();
                    hardResets.Add(resetObj);
                    var resetVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    ResetVisualizer.ResetObjects.Add(resetVisualizer);

                    resetVisualizer.transform.position = resetObj.transform.position;

                    var diameter = resetObj.radius * 2f;
                    resetVisualizer.transform.localScale = new Vector3(diameter, diameter, diameter);

                    ((Collider)resetVisualizer.GetComponent(typeof(Collider))).isTrigger = true;
                    ResetVisualizer.SetTransparentColor(resetVisualizer);
                }

                ResetVisualizerState.HardResets = hardResets.ToArray();
            }

            ResetVisualizer.WaypointObjects?.ForEach(Object.Destroy);
            ResetVisualizer.WaypointObjects = new List<GameObject>();
            foreach (var transform in __instance.GetWaypointList())
            {
                var waypointVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                ResetVisualizer.WaypointObjects.Add(waypointVisualizer);

                waypointVisualizer.transform.position = transform;

                var radius = ___RESET_DISTANCE * 2f;
                waypointVisualizer.transform.localScale = new Vector3(radius, radius, radius);

                ((Collider)waypointVisualizer.GetComponent(typeof(Collider))).isTrigger = true;
                ResetVisualizer.SetTransparentColor(waypointVisualizer);
            }

            ResetVisualizer.UpdateAllComponents();
        }
    }
}