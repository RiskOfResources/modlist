using BepInEx;
using BepInEx.Bootstrap;
using RoR2;
using UnityEngine;

namespace ModList
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
 
            On.RoR2.RunReport.Generate += RunReportOnGenerate;
        }

        private void OnDestroy() {
            On.RoR2.RunReport.Generate -= RunReportOnGenerate;
        }

        private RunReport RunReportOnGenerate(On.RoR2.RunReport.orig_Generate orig, Run run, GameEndingDef gameending) {
            if (!RoR2.UI.ConsoleWindow.instance) {
                Instantiate(Resources.Load<GameObject>("Prefabs/UI/ConsoleWindow")).GetComponent<RoR2.UI.ConsoleWindow>();
            }
 
            Debug.Log("Loaded Plugins:");
            foreach (var plugin in Chainloader.PluginInfos)
            {
                Debug.Log(plugin.Value.ToString());
            }

            return orig(run, gameending);
        }
    }
}
