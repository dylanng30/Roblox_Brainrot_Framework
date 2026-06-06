using System;
using System.Collections.Generic;

namespace RobloxFW.InteractionSystem
{
    public class InteractionData
    {
        public InteractionType Type;
        public List<string> DisplayTexts;
        public Action OnInteractAction;

        public InteractionData()
        {
            DisplayTexts = new List<string>();
        }
    }
}