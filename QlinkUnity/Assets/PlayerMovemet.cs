using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovemet : MonoBehaviour
{
    public string debugText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 beans = new Vector3(1.0f, 0.0f, 0.0f);
            transform.localScale += beans;
            Debug.Log(debugText);
        }
    }
}
