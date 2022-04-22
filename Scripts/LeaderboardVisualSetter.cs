using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CyberBuggy.Leaderboards
{
    public class LeaderboardVisualSetter : MonoBehaviour
    {
        [SerializeField] private LeaderboardRequestManager _leaderboardManager;
        [SerializeField] private List<ScoreReferences> _scoreReferences;
        [SerializeField] private TMP_Text _pageListText;

        private const float _requestDelay = 1f;
        private int _pageIndex;
        private int _pageAmount;
        private bool _isDownloading;
        public UnityEvent LeaderboardDownloading;
        public UnityEvent LeaderboardDownloaded;


        private void OnEnable()
        {
            Download();
        }

        public void ShowNextPage()
        {
            if(_isDownloading)
                return;
            
            _pageIndex = (_pageIndex += 1) % _pageAmount;
            Download();
        }
        public void ShowPreviousPage()
        {
            if(_isDownloading)
                return;
            
            _pageIndex = (_pageIndex -= 1) % _pageAmount;
            if(_pageIndex < 0)
                _pageIndex = _pageAmount - 1;
            
            Download();
        }

        private void Download()
        {
            LeaderboardDownloading?.Invoke();
            _isDownloading = true;

            _leaderboardManager.DownloadScores();
            _leaderboardManager.LeaderboardUpdated += OnLeaderboardUpdated;
        }
        private void SetScoreReferences(DreamloLeaderboard leaderboard)
        {
            ClearScoreReferences();
            var entries = leaderboard.Entries;

            if(entries == null || entries.Length == 0)
                return;

            for (int i = 0; i < _scoreReferences.Count; i++)
            {
                var entryPageIndex = i + (_pageIndex * _scoreReferences.Count);
                if (entryPageIndex >= entries.Length)
                    break;
                
                var entry = entries[entryPageIndex];
                var scoreReference = _scoreReferences[i];

                scoreReference.positionText.SetText($"{(entryPageIndex + 1).ToString()}.");
                scoreReference.nameText.SetText(entry.name);
                scoreReference.scoreText.SetText(entry.score);
            }
        }

        private void ClearScoreReferences()
        {
            for (int i = 0; i < _scoreReferences.Count; i++)
            {
                var scoreReference = _scoreReferences[i];

                scoreReference.positionText.SetText(string.Empty);
                scoreReference.nameText.SetText(string.Empty);
                scoreReference.scoreText.SetText(string.Empty);
            }
        }

        private void SetListPage(DreamloLeaderboard leaderboard)
        {
            var entries = leaderboard.Entries;
            if(entries == null || entries.Length == 0)
                _pageAmount = 1;
            else
                _pageAmount = Mathf.CeilToInt((float)entries.Length / _scoreReferences.Count);

            _pageListText.SetText($"{(_pageIndex + 1).ToString()}/{_pageAmount.ToString()}");
        }

        private void OnLeaderboardUpdated(DreamloLeaderboard leaderboard)
        {
            StartCoroutine(Co_OnLeaderboardUpdated(_requestDelay, leaderboard));
        }
        private IEnumerator Co_OnLeaderboardUpdated(float seconds, DreamloLeaderboard leaderboard)
        {
            yield return new WaitForSeconds(seconds);
            
            LeaderboardDownloaded?.Invoke();
            _isDownloading = false;
            _leaderboardManager.LeaderboardUpdated -= OnLeaderboardUpdated;

            SetListPage(leaderboard);
            SetScoreReferences(leaderboard);
        }
    }
}
