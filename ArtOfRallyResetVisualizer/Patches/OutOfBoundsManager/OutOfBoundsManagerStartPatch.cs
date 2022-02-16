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
    public static class Triggers
    {
        public static SphereCollider CurrentTrigger;
        public static Bounds CarBounds;
    }
    
    [HarmonyPatch(typeof(global::OutOfBoundsManager), "FixedUpdate")]
    public class FixedUpdatePatch
    {
        public static void Postfix()
        {
            if (Triggers.CurrentTrigger == null) return;

            var car = GameEntryPoint.EventManager.playerManager.playerRigidBody;
            var bounds = new Bounds(car.transform.position, Vector3.zero);
            foreach (var collider in car.GetComponentsInChildren<Collider>()) {
                bounds.Encapsulate(collider.bounds);
            }

            var bias = Main.Settings.HardResetDistanceCompensation switch
            {
                HardResetDistanceCompensation.Car => bounds.extents.x,
                HardResetDistanceCompensation.Static => Main.Settings.ManualDistanceCompensation,
                _ => 0,
            };

            var distance = Vector3.Distance(
                car.transform.position,
                Triggers.CurrentTrigger.transform.position);

            if (distance > Triggers.CurrentTrigger.radius + bias) return;

            Main.Logger.Log("Resetting Car for Corner Cutting");
            Triggers.CurrentTrigger = null;
            
            GameEntryPoint.EventManager.outOfBoundsManager.HardReset();
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
                var car = GameEntryPoint.EventManager.playerManager.playerRigidBody;
                Triggers.CarBounds = new Bounds(car.transform.position, Vector3.zero);
                foreach (var collider in car.GetComponentsInChildren<Collider>()) {
                    Triggers.CarBounds.Encapsulate(collider.bounds);
                }
                
                ResetVisualizer.ResetObjects?.ForEach(Object.Destroy);
                ResetVisualizer.ResetObjects = new List<GameObject>();
                ResetVisualizer.ResetActualObjects?.ForEach(Object.Destroy);
                ResetVisualizer.ResetActualObjects = new List<GameObject>();
                foreach (var obj in resets)
                {
                    var resetObj = ((Transform)obj).gameObject.GetComponent<SphereCollider>();
                    var resetVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    ResetVisualizer.ResetObjects.Add(resetVisualizer);

                    var position = resetObj.transform.position;
                    resetVisualizer.transform.position = position;

                    var diameter = resetObj.radius * 2f;
                    resetVisualizer.transform.localScale = new Vector3(diameter, diameter, diameter);

                    ((Collider)resetVisualizer.GetComponent(typeof(Collider))).isTrigger = true;
                    ResetVisualizer.SetTransparentColor(resetVisualizer);
                    // ---------------------
                    var actualVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    ResetVisualizer.ResetActualObjects.Add(actualVisualizer);

                    actualVisualizer.transform.position = position;

                    diameter = resetObj.radius * 2f + Main.Settings.HardResetDistanceCompensation switch
                    {
                        HardResetDistanceCompensation.Car => Triggers.CarBounds.extents.x,
                        HardResetDistanceCompensation.Static => Main.Settings.ManualDistanceCompensation,
                        _ => 0,
                    };
                    actualVisualizer.transform.localScale = new Vector3(diameter, diameter, diameter);

                    ((Collider)actualVisualizer.GetComponent(typeof(Collider))).isTrigger = true;
                    ResetVisualizer.SetTransparentColor(actualVisualizer);
                }
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