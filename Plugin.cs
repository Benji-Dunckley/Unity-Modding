using BepInEx;
using System;
using System.Text;
using UnityEngine;
using HarmonyLib;
using BepInEx.Logging;
using Minty.Patches;
using GameNetcodeStuff;

namespace Minty
{

    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class MintyMod : BaseUnityPlugin
    {
        private const string ModGUID = "Doelarity.MintyBrackern";
        private const string ModName = "Minty Brackern";
        private const string ModVersion = "1.0.0.0";

        private readonly Harmony harmony = new Harmony(ModGUID);

        private static MintyMod Instance;

        public AudioClip ass;

        public static ManualLogSource Lucario;

        public static AudioClip[] newSFX;

        public static AudioClip[] newSFX2;

        public static Texture[] newText;

        public static Material[] newMat;

        public static Vector3 Funny;

        public static PlayerControllerB Gay;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            Lucario = BepInEx.Logging.Logger.CreateLogSource(ModGUID);
            Lucario.LogInfo((object)">> Mod lodded.");
            string location = ((BaseUnityPlugin)Instance).Info.Location;
            string text = "Modding.dll";
            string text2 = location.TrimEnd(text.ToCharArray());
            string text3 = text2 + "hahaah";
            AssetBundle val = AssetBundle.LoadFromFile(text3);
            if (val == null)
            {
                Lucario.LogError("Failed to load Venom.");
                return;
            }
            else
            {
                Lucario.LogInfo("Loaded Venom!");
            }
            newSFX = val.LoadAssetWithSubAssets<AudioClip>("assets/hahaah.mp3");
            newSFX2 = val.LoadAssetWithSubAssets<AudioClip>("assets/Emo2.mp3");
            newMat = val.LoadAssetWithSubAssets<Material>("assets/Luna.mat");
            newText = val.LoadAssetWithSubAssets<Texture2D>("assets/luna.PNG");

           harmony.PatchAll();

        }

    }







}
