using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer videoPlayer;

    // Update is called once per frame
    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene("MainGame");
    }
}
