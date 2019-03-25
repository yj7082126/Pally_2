using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    public GameObject unityC;

    [Range(2f, 6f)]
    public float maxDist = (float)(Mathf.Sqrt(8.0f));

    [Range(-1f, 1f)]
    public float maxAngle = 0.0f;

    public bool hasSaidHello;

    // Start is called before the first frame update
    void Start()
    {
        hasSaidHello = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 difference = unityC.transform.position - gameObject.transform.position;

        if (difference.magnitude < maxDist) {
            if (Vector3.Dot(unityC.transform.forward, difference.normalized) > maxAngle) {
                //if (unityC.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "REFLESH00") {
                if (unityC.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "female_greet")
                {
                    hasSaidHello = true;
                    Debug.Log("Hello");
                }
            }
        }

    }
}
