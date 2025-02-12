using BepInEx;
using BepInEx.Bootstrap;
using RoR2.UI;
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

      On.RoR2.UI.GameEndReportPanelController.SetDisplayData += RunReportOnGenerate;
    }

    private void OnDestroy()
    {
      On.RoR2.UI.GameEndReportPanelController.SetDisplayData -= RunReportOnGenerate;
    }

    private void RunReportOnGenerate(On.RoR2.UI.GameEndReportPanelController.orig_SetDisplayData orig, GameEndReportPanelController self, GameEndReportPanelController.DisplayData newDisplayData)
    {
      if (!ConsoleWindow.instance)
      {
        Instantiate(Resources.Load<GameObject>("Prefabs/UI/ConsoleWindow")).GetComponent<ConsoleWindow>();
      }

      Debug.Log("Loaded Plugins:");
      foreach (var plugin in Chainloader.PluginInfos)
      {
        Debug.Log(plugin.Value.ToString());
      }

      orig(self, newDisplayData);
    }
  }
}
