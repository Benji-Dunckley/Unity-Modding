using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using Unity.Netcode;
using GameNetcodeStuff;
using System.Reflection;
using BepInEx;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Minty.Patches
{
    [HarmonyPatch(typeof(Landmine))]
    public class LandminePatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void MinePatch(Landmine __instance)
        {
            __instance.mineDetonate = MintyMod.newSFX[0];
            __instance.mineDetonateFar = MintyMod.newSFX[0];
            __instance.mineTrigger = MintyMod.newSFX[0];
            __instance.minePress = MintyMod.newSFX[0];

        }

        [HarmonyPatch("Detonate")]
        [HarmonyPrefix]
        private static bool MineVolumeAdjust(Landmine __instance)
        {
            __instance.mineAudio.minDistance = 100f;
            __instance.mineAudio.maxDistance = 500f;
            __instance.mineFarAudio.minDistance = 500f;
            __instance.mineFarAudio.maxDistance = 2000f;
            __instance.mineAudio.PlayOneShot(__instance.mineDetonate, 20f);
            Landmine.SpawnExplosion(((Component)__instance).transform.position + Vector3.up, true, killRange: 1f, damageRange: 6.4f);
            Collider[] array = Physics.OverlapSphere(((Component)__instance).transform.position, 600f, 2621448, (QueryTriggerInteraction)2);
            PlayerControllerB val = null;
            for (int j = 0; j < array.Length; j++)
            {
                val = ((Component)array[j]).gameObject.GetComponent<PlayerControllerB>();
                Debug.Log("this is: " + val.playerUsername);
                if ((object)val != null)
                {
                    Landmine.SpawnExplosion(val.transform.position, true, 3f, 5f);
                }
            }



            return true;

        }

    }
    [HarmonyPatch(typeof(FlashlightItem))]
    public class lightPatch
    {
        public static Vector3 deathVector;

        public static Vector3 randoVector;

        public static Texture oldTexture;

        [HarmonyPatch("ItemActivate")]
        [HarmonyPostfix]
        private static void funny(bool used, FlashlightItem __instance)
        {
            randoVector = new Vector3(UnityEngine.Random.Range(1f, 20f), UnityEngine.Random.Range(1f, 20f), UnityEngine.Random.Range(1f, 20f));
            if (used)
            {
                oldTexture = __instance.flashlightMesh.material.mainTexture;
                __instance.flashlightMesh.material.mainTexture = MintyMod.newText[0];
                Debug.Log("Texture swapped.");
            }
            else
            {
                __instance.flashlightMesh.material.mainTexture = oldTexture;
                Debug.Log("Texture swapped back.");
            }
            Debug.Log("Vector ready");
            Debug.Log("Spawning location is:" + __instance.transform.position);
            Debug.Log("Player location is:" + GameNetworkManager.Instance.localPlayerController.transform.position);

            if (UnityEngine.Random.Range(0, 10) == 3)
            {
                __instance.flashlightAudio.PlayOneShot(MintyMod.newSFX[0], 5f);
                Landmine.SpawnExplosion((__instance.transform.position), true, 5f, 10f);
                //Collider[] array = Physics.OverlapSphere(__instance.transform.position, 10f, 2621448, (QueryTriggerInteraction)2);
                // Debug.Log("Sphere did thingy. List of Players: " + array.Length);
                //PlayerControllerB val = null;
                //for (int i = 0; i < array.Length; i++)

                {
                    // val = ((Component)array[i]).gameObject.GetComponent<PlayerControllerB>();
                    // if (val != null)
                    {
                        // deathVector = (val.transform.position - val.transform.position);
                        // Debug.Log("Playercontroller found. Object:" + val);
                        // val.KillPlayer(deathVector, true, (CauseOfDeath)0, 0);
                        // Landmine.SpawnExplosion(val.transform.position, true, 0f, 0f);
                        // Debug.Log("Player killed");
                    }
                    // else
                    {
                        // Debug.Log("Failed to find player >:/");

                    }


                }
            }
        }
    }

    [HarmonyPatch(typeof(TimeOfDay))]
    [HarmonyPatch("Awake")]
    public static class TimeOfDayAwakePatch
    {
        private static void Postfix(TimeOfDay __instance)
        {
            if (__instance.quotaVariables != null)
            {
                __instance.quotaVariables.startingCredits = 2000;
            }
        }
    }

    [HarmonyPatch(typeof(ItemDropship))]
    internal class SoundPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void thePatch(ItemDropship __instance)
        {
            Debug.Log("Texture for the item ship swapped.");
            AudioClip theSong = MintyMod.newSFX2[0];
            AudioSource component = ((Component)((Component)__instance).transform.Find("Music")).gameObject.GetComponent<AudioSource>();
            AudioSource component2 = ((Component)((Component)((Component)__instance).transform.Find("Music")).transform.Find("Music (1)")).gameObject.GetComponent<AudioSource>();
            component.clip = theSong;
            component2.clip = theSong;
        }
    }
    [HarmonyPatch(typeof(ExtensionLadderItem))]
    internal class LadderPatch
    {
        [HarmonyPatch("StartLadderAnimation")]
        [HarmonyPostfix]
        private static void Ladder(ExtensionLadderItem __instance)
        {
            Landmine.SpawnExplosion(__instance.transform.position, true, 0f, 10f);
        }

    }
    [HarmonyPatch(typeof(Shovel))]
    internal class ShovelFix
    {
        [HarmonyPatch("ItemActivate")]
        [HarmonyPrefix]
        private static void ItemActivate(bool used, bool buttonDown, Shovel __instance)
        {
            string username = __instance.playerHeldBy.playerUsername;
            if (username == "Doelarity")
            {
                __instance.shovelHitForce = 5;
                Debug.Log("Doelarity detected >>> Increasing SHF");
            }
            else if (username == "MaidMage")
            {
                Landmine.SpawnExplosion(__instance.transform.position, true, 0.5f, 5f);
                Debug.Log("Killing Albert");
            }
            else
            {
                __instance.shovelHitForce = 1;
                Debug.Log("Someone else detetced >> reseting SHF");
            }

        }



    } 
}
