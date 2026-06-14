using System;
using UnityEngine;
using System.Collections;
using RobloxFW.WorldEventSystem.Data;
using RobloxFW.WorldEventSystem.Manager;
using TMPro;

namespace RobloxFW.WorldEventSystem.UI
{
    public class WorldEventUIController : MonoBehaviour
    {
        [Header("--- REFERENCES ---")]
        [SerializeField] private WorldEventManager eventManager;
        [SerializeField] private WorldEventScheduler eventScheduler;
        
        [Header("--- UI ElEMENTS ---")]
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private TextMeshProUGUI notificationText;
        
        [Header("--- SETTINGS ---")]
        [SerializeField] private float notificationDisplayTime = 5f;

        private string informationString;

        private void OnEnable()
        {
            if (eventManager != null)
            {
                eventManager.OnWorldEventStarted += HandleEventStarted;
                eventManager.OnWorldEventEnded += HandleEventEnded;
            }
        }

        private void OnDisable()
        {
            if (eventManager != null)
            {
                eventManager.OnWorldEventStarted -= HandleEventStarted;
                eventManager.OnWorldEventEnded -= HandleEventEnded;
            }
        }

        private void Update()
        {
            if (eventScheduler != null && countdownText != null)
            {
                if (!countdownText.gameObject.activeSelf)
                {
                    countdownText.gameObject.SetActive(true);
                }

                informationString = String.Empty;

                if (eventScheduler.IsEventActive)
                {
                    informationString += $"<color=green>ACTIVE:</color> {eventScheduler.ActiveEventId} - {Mathf.CeilToInt(eventScheduler.CurrentEventTimer)}s remaining";
                }
                else if (eventScheduler.IsWaitingForCooldown)
                {
                    informationString += $"<color=yellow>NEXT EVENT IN:</color> {Mathf.CeilToInt(eventScheduler.CurrentCooldownTimer)}s";
                }

                if (eventScheduler.UpcomingEvents != null && eventScheduler.UpcomingEvents.Count > 0)
                {
                    for (int i = 0; i < eventScheduler.UpcomingEvents.Count; i++)
                    {
                        var upcomingEvent = eventScheduler.UpcomingEvents[i];
                        float timeUntilStart = eventScheduler.GetTimeUntilEventStarts(i);
                        informationString += $"\n{upcomingEvent.EventId} (Starts in {Mathf.CeilToInt(timeUntilStart)}s)";
                    }
                }

                countdownText.text = informationString;
            }
        }

        private void HandleEventStarted(WorldEventEnum eventId)
        {
            if (notificationText != null)
            {
                StopAllCoroutines();
                StartCoroutine(ShowNotificationCoroutine($"Event Started: {eventId}!"));
            }
        }

        private void HandleEventEnded(WorldEventEnum eventId)
        {
            if (notificationText != null)
            {
                StopAllCoroutines();
                StartCoroutine(ShowNotificationCoroutine($"Event Ended: {eventId}!"));
            }
        }

        private IEnumerator ShowNotificationCoroutine(string message)
        {
            notificationText.text = message;
            notificationText.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(notificationDisplayTime);
            
            notificationText.gameObject.SetActive(false);
        }
    }
}
