#pragma warning disable 618

using System;
using System.Collections;
using UnityEngine;

namespace CyberBuggy.Leaderboards
{
    public class LeaderboardRequestManager : MonoBehaviour
    {
        [SerializeField] private string _webUrl = "http://dreamlo.com/lb";
        [SerializeField] private string _privateCode = "";
        [SerializeField] private string _publicCode = "";

        public Action<DreamloLeaderboard> LeaderboardUpdated;

        public void AddScore(string name, int score)
        {
            var urlSegment = $"add/{WWW.EscapeURL(name)}/{score}";
            StartCoroutine(SendLeaderboardRequest(urlSegment));
        }
        public void ClearScores()
        {
            var urlSegment = $"clear";
            StartCoroutine(SendLeaderboardRequest(urlSegment));
        }
        public void DownloadScores()
        {
            StartCoroutine(DownloadLeaderboardRequest());
        }
        private IEnumerator SendLeaderboardRequest(string urlSegment)
        {
            var fullUrl = $"{_webUrl}/{_privateCode}/{urlSegment}";

            // var webRequest = new UnityWebRequest(fullUrl);

            var webRequest = new WWW(fullUrl);

            yield return webRequest;

            if(string.IsNullOrEmpty(webRequest.error))
            {
                Debug.Log($"Upload Successful at ({fullUrl})! \n Data: {webRequest.text}");
                DownloadScores();
            }
            else
                Debug.Log("Upload Error! \n" + webRequest.error);
        }
        private IEnumerator DownloadLeaderboardRequest()
        {
            var fullUrl = $"{_webUrl}/{_publicCode}/json";

            // var webRequest = new UnityWebRequest(fullUrl);

            var webRequest = new WWW(fullUrl);

            yield return webRequest; 
            
            if(string.IsNullOrEmpty(webRequest.error))
            {
                Debug.Log($"Download Successful at ({fullUrl})! \n Data: {webRequest.text}");
                SeparateScores(webRequest.text);
            }
            else
                Debug.Log("Download Error! \n" + webRequest.error);
        }

        private void SeparateScores(string text)
        {
            var dreamloLeaderboard = JsonUtility.FromJson<DreamloLeaderboard>(text);
            LeaderboardUpdated?.Invoke(dreamloLeaderboard);
        }
    }
}
