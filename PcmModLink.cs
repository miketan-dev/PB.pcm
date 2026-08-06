using System;
using PhantomBrigade.Mods;
using UnityEngine;

namespace PB.pcm
{
    public class PcmModLink : ModLink
    {
        private static int modIndex;
        private static string modId;
        private static string modPath;
        private static string modVersion;

        public override void OnLoadStart()
        {
            modIndex = modIndexPreload;
            modPath = metadata.path;
            modId = modID;

            modVersion = metadata.gameVersionMin;

            try
            {
                // Trigger when the mod is not loaded
                if (modIndex == -1 || modId == null || modPath == null || modVersion == null)
                {
                    throw new Exception("Mod index is -1");
                }
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("[PCM] - MOD NOT LOADED: {0}", e.Message);
            }

            Debug.LogFormat("[PCM] - MOD LOADED. modIndex: " + modIndex + " | " + " modId: " + modId + " | " +
                            "modPath: " + modPath + " | " + " modVersion: " + modVersion + " ");

            EnableHarmonyFileLog(); // USE ONLY WHEN DEBUGGING - DON'T LEAVE IT ENABLED IN RELEASE.
        }
    }
}