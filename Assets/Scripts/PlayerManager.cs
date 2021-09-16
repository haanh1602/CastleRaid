using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Transform> playersTransform;

    public void UpdatePlayers()
    {
        //Debug.Log(transform.childCount);
        playersTransform.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            playersTransform.Add(transform.GetChild(i));
        }
    }

    private void Start()
    {
        UpdatePlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
