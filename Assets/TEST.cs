using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        st_level aa = st_levelTable.getst_levelByID(2);
        Debug.Log(aa.col);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
