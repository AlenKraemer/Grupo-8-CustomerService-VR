using System;

namespace Score
{
    [Serializable]
    public class QuestState
    {
        public string questId;
        public bool completed;
        public bool processed;
        public PaperworkType paperworkType;

        public QuestState(string id, PaperworkType paperworkType, bool isCompleted, bool isProcessed = false)
        {
            questId = id;
            this.paperworkType = paperworkType;
            completed = isCompleted;
            processed = isProcessed;
        }
    }
}