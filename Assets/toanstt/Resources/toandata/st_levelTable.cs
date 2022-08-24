//Author: toanstt 
//This file is generated, do not edit!
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable] public class st_levelTableListJSON{ public st_level[] list;}

public class st_levelTable  
{
private static st_levelTable _instance;
public st_level[] list;
public Dictionary<int, st_level> VALUE;
public st_levelTable()
{
	VALUE = new Dictionary<int, st_level>();
}
public static st_levelTable I
{
	get
	{
	if (_instance == null)
	       {
	           _instance = new st_levelTable();
	           _instance.load();
	       }
	       return _instance;
	}
}
public st_level Get(int id)
{
return VALUE[id];
}
public void load()
{
TextAsset jsonData = Resources.Load<TextAsset>("toandata/st_levelJSON");
st_levelTableListJSON lmyist = JsonUtility.FromJson<st_levelTableListJSON> (jsonData.text);
foreach(st_level i in lmyist.list) { VALUE.Add(i.id, i); }
}
public static st_level getst_levelByID(int id){if(!I.VALUE.ContainsKey(id)) return null;return I.VALUE[id];}}
