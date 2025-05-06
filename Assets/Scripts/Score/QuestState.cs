using System;

namespace Score
{
    [Serializable]
    public class QuestState
    {
        public string questId;
        public bool completed;
        public bool processed;

        public QuestState(string id, bool isCompleted, bool isProcessed = false)
        {
            questId = id;
            completed = isCompleted;
            processed = isProcessed;
        }
    }
}