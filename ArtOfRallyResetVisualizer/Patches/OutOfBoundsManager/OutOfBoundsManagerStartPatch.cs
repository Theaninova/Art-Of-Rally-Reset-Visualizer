using System.Collections.Generic;
using ArtOfRallyResetVisualizer.Settings;
using HarmonyLib;
using UnityEngine;

namespace ArtOfRallyResetVisualizer.Patches.OutOfBoundsManager
{
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
                ResetVisualizer.ResetObjects = new List<GameObject>();
                foreach (var obj in resets)
                {
                    var resetObj = ((Transform)obj).gameObject.GetComponent<SphereCollider>();
                    var resetVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    ResetVisualizer.ResetObjects.Add(resetVisualizer);

                    resetVisualizer.transform.position = resetObj.transform.position;

                    var radius = resetObj.radius * 2f;
                    resetVisualizer.transform.localScale = new Vector3(radius, radius, radius);

                    ((Collider)resetVisualizer.GetComponent(typeof(Collider))).isTrigger = true;
                    ResetVisualizer.SetTransparentColor(resetVisualizer);
                }
            }

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