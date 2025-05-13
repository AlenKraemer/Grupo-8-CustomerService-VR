using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Score
{
    public class QuestManager : MonoBehaviour
    {
        public Action onButtonPressed;
        public Action onButtonPressedCustomer;

        public bool isObjectiveCompleted;

        [SerializeField] private Text tramitesCompletadosText;
        private ScoreManager scoreManager;
        private int servicedCustomers = 0;

        private Queue<string> questQueue = new();
        [SerializeField] private List<string> questList = new();
        [SerializeField] private List<QuestState> questStates = new();

        private void Start()
        {
            scoreManager = GameManager.Instance.scoreManager;
            UpdateTramitesCompletadosText();

            foreach (string quest in questList) questQueue.Enqueue(quest);
            //if (questQueue.Count == 0) AddTestQuests();
        }

        public void AddQuestToQueue(string questId, PaperworkType paperworkType)
        {
            questQueue.Enqueue(questId);
            questList.Add(questId);
            //if (questStates.All(q => q.questId != questId))
            questStates.Add(new QuestState(questId, paperworkType, false, false));
        }

        //[ContextMenu("Add Test Quest")]
        //public void AddTestQuest() => AddQuestToQueue($"Form_{Random.Range(100, 999)}");

        [ContextMenu("Try Complete Current Quest")]
        public void TryCompleteQuest()
        {
            CompleteQuest();
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
        public QuestState GetQuestStatus()
        {
            return questStates[servicedCustomers];
        }



        public void CompleteQuest(string questId = "", bool isCompleted = false)
        {
            if (questId == "")
            {
                var currentId = GetQuestStatus();
                questId = currentId.questId;
            }
            var questState = questStates.FirstOrDefault(q => q.questId == questId);
            //if (questState == null)
            //    questStates.Add(new QuestState(questId, PaperworkType.signature,isCompleted, true));

            questState.completed = isCompleted;
            questState.processed = true;


            if (isObjectiveCompleted)
            {
                UpdateScore();
                onButtonPressed?.Invoke();
            }
            servicedCustomers++;
            onButtonPressedCustomer?.Invoke();
            isObjectiveCompleted = false;
            GameManager.Instance.customerSpawn.SpawnCustomer();

        }

        private void UpdateScore()
        {
            var score = scoreManager?.TramitesCompletadosScore();
            if (score != null)
            {
                score.IncreaseScore(1);
                UpdateTramitesCompletadosText();
            }
        }

        private void UpdateTramitesCompletadosText()
        {
            var score = scoreManager?.TramitesCompletadosScore();
            if (tramitesCompletadosText != null && score != null)
                tramitesCompletadosText.text = $"{score.CurrentScore}";
        }


        //private void AddTestQuests()
        //{
        //    AddQuestToQueue("Form_001");
        //    AddQuestToQueue("Form_002");
        //    AddQuestToQueue("Form_003");
        //    AddQuestToQueue("Form_004");
        //}
    }
}