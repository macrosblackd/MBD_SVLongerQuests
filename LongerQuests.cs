using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace MBD_SVLongerQuests
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class LongerQuests : BaseUnityPlugin
    {
        public const string pluginGuid = "macrosblackd.starvalormods.longerquests";
        public const string pluginName = "Longer Quest Duration";
        public const string pluginVersion = "1.0.0";

        static ManualLogSource logger = new ManualLogSource("(MBD) LongerQuests");
        
        private static ConfigEntry<int> durationMultiplier;

        private void Awake()
        {
            BepInEx.Logging.Logger.Sources.Add(logger);
            LoadConfig();
            Harmony.CreateAndPatchAll(typeof(LongerQuests));
            logger.Log(LogLevel.Info, $"Loaded {pluginName} v{pluginVersion}");
        }

        private void LoadConfig()
        {
            durationMultiplier = Config.Bind<int>("General", "QuestDurationMultiplier", 3,
                "The multiplier for how much longer you want delivery quests to last. Affects local & regional delivery quests.");
        }

        [HarmonyPatch(typeof(SM_MissionBoard), nameof(SM_MissionBoard.GenerateQuests))]
        [HarmonyPostfix()]
        public static void PostFix(SM_MissionBoard __instance)
        {
            foreach (var stationQuest in __instance.quests)
            {
                if (stationQuest.quest.deliveryTime > 0 && stationQuest.quest.par1 != -55)
                {
                    logger.LogInfo($"Found quest with deliver time: {stationQuest.quest.deliveryTime}");
                    stationQuest.quest.deliveryTime *= durationMultiplier.Value;
                    stationQuest.quest.par1 = -55;
                    logger.LogInfo($"New Delivery time: {stationQuest.quest.deliveryTime}");

                }
            }
        }
    }
}
