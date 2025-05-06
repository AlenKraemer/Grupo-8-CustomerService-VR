using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Score
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private Text tramitesCompletadosText;
        [SerializeField] private float completionChance = 0.5f;

        private Queue<string> questQueue = new();
        [SerializeField] private List<string> questList = new();
        [SerializeField] private List<QuestState> questStates = new();

        private void Start()
        {
            scoreManager ??= FindObjectOfType<ScoreManager>();
            if (scoreManager == null) Debug.LogError("No ScoreManager found!");

            UpdateTramitesCompletadosText();

            foreach (string quest in questList) questQueue.Enqueue(quest);
            if (questQueue.Count == 0) AddTestQuests();
        }

        public void AddQuestToQueue(string questId)
        {
            questQueue.Enqueue(questId);
            questList.Add(questId);
            if (questStates.All(q => q.questId != questId))
                questStates.Add(new QuestState(questId, false, false));
        }

        [ContextMenu("Add Test Quest")]
        public void AddTestQuest() => AddQuestToQueue($"Form_{Random.Range(100, 999)}");

        [ContextMenu("Try Complete Current Quest")]
        public void TryCompleteQuest()
        {
            if (questQueue.Count == 0) { Debug.Log("No quests to complete"); return; }

            string questId = questQueue.Dequeue();
            questList.RemoveAt(0);
            bool success = Random.value < completionChance;

            CompleteQuest(questId, success);
            Debug.Log(success ? $"Completed {questId}" : $"Failed {questId}");
        }

        [ContextMenu("Force Complete Current Quest")]
        public void ForceCompleteQuest()
        {
            if (questQueue.Count == 0) { Debug.Log("No quests to complete"); return; }

            string questId = questQueue.Dequeue();
            questList.RemoveAt(0);
            CompleteQuest(questId, true);
            Debug.Log($"Force completed {questId}");
        }

        private void CompleteQuest(string questId, bool isCompleted)
        {
            var questState = questStates.FirstOrDefault(q => q.questId == questId);
            if (questState == null)
                questStates.Add(new QuestState(questId, isCompleted, true));
            else
            {
                questState.completed = isCompleted;
                questState.processed = true;
            }

            if (isCompleted) UpdateScore(questId);
        }

        private void UpdateScore(string questId)
        {
            var score = scoreManager?.TramitesCompletadosScore();
            if (score != null)
            {
                score.IncreaseScore(1);
                Debug.Log($"Form {questId} completed! Total: {score.CurrentScore}");
                UpdateTramitesCompletadosText();
            }
            else Debug.LogError("Score is null!");
        }

        private void UpdateTramitesCompletadosText()
        {
            var score = scoreManager?.TramitesCompletadosScore();
            if (tramitesCompletadosText != null && score != null)
                tramitesCompletadosText.text = $"{score.CurrentScore}";
        }

        public string GetQuestStatusInfo()
        {
            int completed = questStates.Count(q => q.completed);
            int processed = questStates.Count(q => q.processed);
            return $"Current: {(questQueue.Count > 0 ? questQueue.Peek() : "None")} | Queued: {questQueue.Count} | Completed: {completed} | Processed: {processed}";
        }

        private void AddTestQuests()
        {
            AddQuestToQueue("Form_001");
            AddQuestToQueue("Form_002");
            AddQuestToQueue("Form_003");
            AddQuestToQueue("Form_004");
        }
    }
}