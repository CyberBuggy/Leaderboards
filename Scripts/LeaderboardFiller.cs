using UnityEngine;

namespace CyberBuggy.Leaderboards
{
    public class LeaderboardFiller : MonoBehaviour
    {
        [SerializeField] private LeaderboardRequestManager _leaderboardManager;

        [SerializeField] private string _userName;
        [SerializeField] private int _score;

        [SerializeField] private int _randomNameGenerationCount = 100;

        [SerializeField] private int _randomScoreMaxThreshold = int.MaxValue;

        private string[] _randomNameList = new string[50]{"Anne","Lamb","Erin","Long","Marty","Collier","Ethel","Higgins","Hattie","Ray","Don","Kelley","Barbara","Garcia","Franklin","Schwartz","Chester","Guzman","Wesley","Ortiz","Al","Cunningham","Marcella","Rivera","Pamela","Rogers","Joyce","Barnett","Horace","Barrett","Larry","Copeland","Jermaine","Robertson","Constance","Lopez","Cornelius","Hoffman","Ana", "Stokes","Clint", "Graham","Jessie","Chambers","Lorenzo", "Castillo","Rex","Cook","Pam","Lucas"};

        [ContextMenu("Add Current Score")]
        public void AddScore()
        {
            _leaderboardManager.AddScore(_userName, _score);
        }
        [ContextMenu("Get Scores")]
        public void GetScores()
        {
            _leaderboardManager.DownloadScores();
            // Debug.Log();
        }
        [ContextMenu("Clear Scores")]
        public void ClearScores()
        {
            _leaderboardManager.ClearScores();
        }

        [ContextMenu("Add Random Scores")]
        public void AddRandomScores()
        {
            for (int i = 0; i < _randomNameGenerationCount; i++)
            {
                _userName = _randomNameList[Random.Range(0, 50)] + _randomNameList[Random.Range(0, 50)];
                _score = Random.Range(0, _randomScoreMaxThreshold);
                AddScore();
            }
        }
    }
}
