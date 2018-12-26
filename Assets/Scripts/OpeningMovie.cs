using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class OpeningMovie : MonoBehaviour {

    [SerializeField] private VideoPlayer m_videoPlayer;
    FadePanel fadePanel;
    private bool playing = false;

    private void Start()
    {
        if (Input.GetKey(KeyCode.P))
        {
            SceneSequencer ss = FindObjectOfType<SceneSequencer>();
            ss.ChangeScene("WorldLand");
            return;
        }


        m_videoPlayer = GetComponent<VideoPlayer>();
        m_videoPlayer.isLooping = true;
        m_videoPlayer.Play();
        m_videoPlayer.loopPointReached += (VideoPlayer vp) =>
        {
            SceneSequencer ss = FindObjectOfType<SceneSequencer>();
            ss.ChangeScene("WorldLand");
        };

        fadePanel = GameObject.Find("FadePanel").GetComponent<FadePanel>();
    }

    private void Update()
    {
        if (!playing)
        {
            if (!m_videoPlayer.isPrepared)
            {
                fadePanel.IsFadeIn = false;
            }
            else
            {
                fadePanel.IsFadeIn = true;
                playing = true;
            }
        }
    }
}
