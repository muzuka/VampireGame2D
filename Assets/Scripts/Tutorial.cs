using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Tutorial controller
 */
public class Tutorial : MonoBehaviour
{
    public List<TutorialEvent> events;

    public GameObject MessagePanel;

    public float MessageTimer = 2.0f;
    float timeConsumed = 0.0f;
    
    Player _playerObject;

    void Start()
    {
        _playerObject = FindObjectOfType<Player>();
        events.ForEach((x) =>
        {
            if (x.Type == EventType.START && !x.Triggered)
            {
                OpenMessage(x);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        events.ForEach((x) =>
        {
            if (x.Type == EventType.COLLISION && !x.Triggered)
            {
                if (x.ColliderA.IsTouching(x.ColliderB))
                {
                    OpenMessage(x);
                }
            }
        });

        if (MessagePanel.activeSelf)
        {
            Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(_playerObject.transform.position);
            MessagePanel.transform.position = new Vector3(playerScreenPoint.x, 
                playerScreenPoint.y + 120f, 0);
            timeConsumed += Time.deltaTime;
            
            if (timeConsumed > MessageTimer)
            {
                CloseMessage();
                timeConsumed = 0.0f;
            }
        }
    }

    void OpenMessage(TutorialEvent e)
    {
        MessagePanel.SetActive(true);
        MessagePanel.GetComponentInChildren<Text>().text = e.Message;
        e.Triggered = true;
    }

    void CloseMessage()
    {
        MessagePanel.SetActive(false);
    }
}
