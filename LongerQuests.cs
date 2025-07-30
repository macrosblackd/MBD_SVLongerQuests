using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

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
                UpdateQuestTimeout(ref stationQuest.quest);
            }
        }

        [HarmonyPatch(typeof(QuestControl), nameof(QuestControl.VerifyQuestTimeout))]
        public static void PreFix_QuestControl_VerifyQuestTimeout(QuestControl __instance, Transform playerTrans)
        {
            for (int currentQuestIndex = 0; currentQuestIndex < PChar.Char.activeQuests.Count; currentQuestIndex++)
            {
                Quest quest = PChar.Char.activeQuests[currentQuestIndex];
                UpdateQuestTimeout(ref quest);
            }
        }

        private static void UpdateQuestTimeout(ref Quest quest)
        {
            if (quest.deliveryTime > 0 && quest.par1 != -55)
            {
                logger.LogInfo($"Found unchanged active quest with delivery time: {quest.deliveryTime}");
                quest.deliveryTime = noDeliveryDuration.Value ? 0 : quest.deliveryTime * durationMultiplier.Value;
                quest.par1 = -55;
                logger.LogInfo($"Updated active quest delivery time: {quest.deliveryTime}");
            }
        }
    }
}
