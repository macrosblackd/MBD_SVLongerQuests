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
        public const string pluginVersion = "1.1.0";

        static ManualLogSource logger = new ManualLogSource("(MBD) LongerQuests");
        
        private static ConfigEntry<int> durationMultiplier;
        private static ConfigEntry<bool> noDeliveryDuration;

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

            noDeliveryDuration = Config.Bind<bool>("General", "NoDeliveryDuration", false,
                "If true, delivery quests will not have a time limit. If false, the multiplier will be applied to the default delivery time.");
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
                    if (noDeliveryDuration.Value)
                    {
                        stationQuest.quest.deliveryTime = 0;
                        logger.LogInfo("New Delivery time: 0 (no delivery duration)");
                        continue;
                    } else
                    {
                        stationQuest.quest.deliveryTime *= durationMultiplier.Value;
                        logger.LogInfo($"New Delivery time: {stationQuest.quest.deliveryTime}");
                    }
                    stationQuest.quest.par1 = -55;

                }
            }
        }
    }
}
