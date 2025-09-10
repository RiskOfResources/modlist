using BepInEx;
using BepInEx.Bootstrap;
using RoR2.UI;
using UnityEngine;

namespace ModList
{
  [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
  public class Plugin : BaseUnityPlugin
  {
    private void Awake()
    {
      // Plugin startup logic
      Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

      On.RoR2.UI.GameEndReportPanelController.SetDisplayData += RunReportOnGenerate;
    }

    private void OnDestroy()
    {
      On.RoR2.UI.GameEndReportPanelController.SetDisplayData -= RunReportOnGenerate;
    }

    private void RunReportOnGenerate(On.RoR2.UI.GameEndReportPanelController.orig_SetDisplayData orig, GameEndReportPanelController self, GameEndReportPanelController.DisplayData newDisplayData) {
      LogPlugins();
      orig(self, newDisplayData);
    }

    private static void LogPlugins() {
      if (!ConsoleWindow.instance)
      {
        Instantiate(Resources.Load<GameObject>("Prefabs/UI/ConsoleWindow")).GetComponent<ConsoleWindow>();
      }

      var prevMaxMessages = RoR2.Console.maxMessages.value;
      RoR2.Console.maxMessages.value = Chainloader.PluginInfos.Count + prevMaxMessages;
      Debug.Log("Loaded Plugins:");
      foreach (var plugin in Chainloader.PluginInfos)
      {
        Debug.Log(plugin.Value.ToString());
      }

      RoR2.Console.maxMessages.value = prevMaxMessages;
    }
  }
}
