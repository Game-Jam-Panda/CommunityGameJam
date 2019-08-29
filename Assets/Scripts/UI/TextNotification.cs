using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CGJ.UI
{
    public class TextNotification : MonoBehaviour
    {
        [SerializeField] bool overrideText = false;
        [SerializeField] string notificationText;

        void Start()
        {
            // Update the text at the beginning
            GetComponent<Text>().text = notificationText;
        }

        public void DeleteNotification()
        {
            Destroy(gameObject);
        }
    }

}