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
            _leaderboardManager.LeaderboardUpdated += OnLeaderboardUpdated;
            Download();
        }
        private void OnDisable()
        {
            _leaderboardManager.LeaderboardUpdated -= OnLeaderboardUpdated;
        }
        public void ShowNextPage()
        {
            if(_isDownloading)
                return;
            
            _pageIndex = (_pageIndex += 1) % _pageAmount;

            _isDownloading = true;
            Download();
        }
        public void ShowPreviousPage()
        {
            if(_isDownloading)
                return;
            
            _pageIndex = (_pageIndex -= 1) % _pageAmount;
            if(_pageIndex < 0)
                _pageIndex = _pageAmount - 1;
            
            _isDownloading = true;
            Download();
        }

        private void Download()
        {
            _leaderboardManager.DownloadScores();
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
            LeaderboardDownloading?.Invoke();
            _isDownloading = true;
            StartCoroutine(Co_OnLeaderboardUpdated(_requestDelay, leaderboard));
        }
        private IEnumerator Co_OnLeaderboardUpdated(float seconds, DreamloLeaderboard leaderboard)
        {
            yield return new WaitForSeconds(seconds);
            
            LeaderboardDownloaded?.Invoke();
            _isDownloading = false;

            SetListPage(leaderboard);
            SetScoreReferences(leaderboard);
        }
    }
}
