using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EndingMovie : MonoBehaviour {


    private void Start()
    {
        VideoPlayer player= GetComponent<VideoPlayer>();
        player.isLooping = true;
        player.loopPointReached += (VideoPlayer vp) =>
        {
            SceneSequencer ss = FindObjectOfType<SceneSequencer>();
            ss.ChangeScene("EvolveDragonGameStart");
        };
    }
}
