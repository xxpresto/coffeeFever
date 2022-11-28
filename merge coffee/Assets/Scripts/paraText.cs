using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class paraText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 1.6f);

        transform.DOLocalRotate(new Vector3(0, -90, 0), 0);


    }

    float moveTimer;
    void Update()
    {
        moveTimer += Time.deltaTime;
        if (moveTimer > 0.01f)
        {
            moveTimer = 0;
            transform.position += new Vector3(0, 0.0018f, 0);
        }
        
    }
}
