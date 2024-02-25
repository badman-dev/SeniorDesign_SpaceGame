using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralBehavior : MonoBehaviour
{

    public GameObject dustCloud;
    public GameObject astOrigin;
    private GameObject mineCloud;
    // Start is called before the first frame update
    void Start()
    {
        // rotates mineral deposits towards center of asteroid
        Vector3 placement = transform.InverseTransformPoint(astOrigin.transform.position);
        float angle = Mathf.Atan2(placement.y, placement.x) * Mathf.Rad2Deg + 90;
        transform.Rotate(0, 0, angle);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "drill")
        {
            mineCloud = Instantiate(dustCloud, transform.position + new Vector3(0, 0, 2), new Quaternion(0, 0, 0, 0)) as GameObject;
            mineCloud.transform.parent = transform;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "drill")
        {
            Destroy(mineCloud);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


}
