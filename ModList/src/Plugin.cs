using System;
using System.Globalization;
using System.Text;
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
      var newMaxMessageValue = RoR2.Console.maxMessages.value = 2 * (Chainloader.PluginInfos.Count + 1) + prevMaxMessages;
      On.RoR2.Console.SaveArchiveConVars += ConsoleOnSaveArchiveConVars;

      StringBuilder sb = new();
      sb.AppendLine();
      var now = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);
      sb.AppendLine($"Mod List: {now}");
      sb.AppendLine("----------------");
      foreach (var plugin in Chainloader.PluginInfos) {
        sb.AppendLine(plugin.Value.ToString());
      }
 
      Debug.Log(sb.ToString());
      void ConsoleOnSaveArchiveConVars(On.RoR2.Console.orig_SaveArchiveConVars orig, RoR2.Console self) {
        // restore original value
        if (RoR2.Console.maxMessages.value == newMaxMessageValue) {
          RoR2.Console.maxMessages.value = prevMaxMessages;
        }
 
        orig(self);
        On.RoR2.Console.SaveArchiveConVars -= ConsoleOnSaveArchiveConVars;
      }
    }
  }
}
