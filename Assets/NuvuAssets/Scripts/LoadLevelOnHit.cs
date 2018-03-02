using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadLevelOnHit : HitTarget
{
    public string levelToLoad = "";

    void Start()
    {
    }

    override public void Hit(TouchHitInfo hitInfo)
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
