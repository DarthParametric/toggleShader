using Kingmaker.Visual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.PostProcessing;

namespace NoFilmGrain
{
    class PostProcessing
    {
        public static void ToggleFilmGrain()
        {
            if (RenderingManager.Instance == null)
            {
                Main.Mod.Error("Cannot toggle film grain, RenderingManager is null");
                return;
            }
            if (RenderingManager.Instance.MainCamera != null)
            {
                var m_PostProcessingBehaviour = RenderingManager.Instance.MainCamera.GetComponent<PostProcessingBehaviour>();
                if (m_PostProcessingBehaviour == null)
                {
                    Main.Mod.Log($"m_PostProcessingBehaviour is null");
                } else
                {
                    var grainState = m_PostProcessingBehaviour.profile.grain.enabled ? "Enabled" : "Disabled";
                    Main.Mod.Log($"Toggling Film Grain in m_PostProcessingBehaviour {m_PostProcessingBehaviour.name} (previous setting {grainState})");
                    Main.Mod.Debug(Settings.disableFilmGrain);
                    m_PostProcessingBehaviour.profile.grain.enabled = !Settings.disableFilmGrain;
                }
            } else
            {
                Main.Mod.Log($"Main camera is null");
            }
            foreach(var setting in PostProcessSettings.Instances)
            {
                var grainState = setting.PostProcessingProfile.grain.enabled ? "Enabled" : "Disabled";
                Main.Mod.Log($"Toggling Film Grain in {setting.name}(previous setting {grainState})");
                setting.PostProcessingProfile.grain.enabled = !Settings.disableFilmGrain;
            }
            RenderingManager.Instance.ApplySettings();
        }
        [HarmonyLib.HarmonyPatch(typeof(PostProcessingBehaviour), "OnEnable")]
        static class PostProcessingBehaviour_OnEnable_Patch
        {
            public static void Postfix(PostProcessingBehaviour __instance)
            {
                try
                {
                    if (!Settings.disableFilmGrain) return;
                    if (__instance.profile == null) return;
                    var grainState = __instance.profile.grain.enabled ? "Enabled" : "Disabled";
                    Main.Mod.Log($"Disabling film grain in PostProcessingBehaviour {__instance.name} (previous setting {grainState})");
                    __instance.profile.grain.enabled = false;
                }
                catch (Exception ex)
                {
                    Main.Mod.Error(ex);
                }
            }
        }
        [HarmonyLib.HarmonyPatch(typeof(PostProcessSettings), "OnEnable")]
        static class PostProcessingSettings_OnEnable_Patch 
        {
            public static void Postfix(PostProcessSettings __instance)
            {
                try
                {
                    if (!Settings.disableFilmGrain) return;
                    var grainState = __instance.PostProcessingProfile.grain.enabled ? "Enabled" : "Disabled";
                    Main.Mod.Log($"Disabling film grain in PostProcessSettings {__instance.name} (previous setting {grainState})");
                    __instance.PostProcessingProfile.grain.enabled = false;
                } catch(Exception ex)
                {
                    Main.Mod.Error(ex);
                }
            }
        }
    }
}
