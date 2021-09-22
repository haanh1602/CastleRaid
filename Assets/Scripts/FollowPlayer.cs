using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject PlayerManage;
    public GameObject plane;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        List<Transform> childs = PlayerManage.GetComponent<PlayerManager>().playersTransform;
        if (childs.Count > 0)
        {
            Transform fastestChild = childs[0];
            foreach (Transform child in childs)
            {
                if (child.position.y > fastestChild.position.y)
                {
                    fastestChild = child;
                }
            }
            Vector3 to = new Vector3(0.0f, fastestChild.position.y, 0.0f) + offset;
            transform.position = Vector3.MoveTowards(transform.position, to, 10f);
        }
    }
}
