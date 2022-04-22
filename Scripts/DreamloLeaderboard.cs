using UnityEngine;
using System;

namespace CyberBuggy.Leaderboards
{
    [Serializable]
    public class DreamloLeaderboard
    {
        [SerializeField] private Dreamlo dreamlo;
        public Entry[] Entries { get => dreamlo.leaderboard.entry; set => dreamlo.leaderboard.entry = value; }
        
        [Serializable]
        public class Dreamlo
        {
            public Leaderboard leaderboard;
        }
        [Serializable]
        public class Leaderboard
        {
            public Entry[] entry;
        }
        [Serializable]
        public class Entry
        {
            public string name;
            public string score;
            public string seconds;
            public string text;
            public string date;

        }
    }
    
    
    
}
