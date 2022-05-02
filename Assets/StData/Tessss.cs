using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tessss : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //TextAsset s = Resources.Load<TextAsset>("subFolder/st_levelJSON");
        //MyLevel lmyist = JsonUtility.FromJson<MyLevel>(s.text);
        //Debug.Log(lmyist.list.Length);
        //Debug.Log(JsonUtility.ToJson(lmyist.list[0]));
        //Debug.Log(JsonUtility.ToJson(lmyist.list[1]));

        //Debug.Log(lmyist.list[0].testenum);

        Debug.Log(JsonUtility.ToJson(st_levelTable.getst_levelByID(0)));
        Debug.Log(JsonUtility.ToJson(st_levelTable.getst_levelByID(1)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public class MyLevel
{
    public st_level[] list;
}
