//Author: toanstt 
//This file is generated, do not edit!
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
public class st_mapTable  
{
private static st_mapTable _instance;
public st_map[] list;
public Dictionary<int, st_map> VALUE;
public st_mapTable()
{
	VALUE = new Dictionary<int, st_map>();
}
public static st_mapTable I
{
	get
	{
	if (_instance == null)
	       {
	           _instance = new st_mapTable();
	           _instance.load();
	       }
	       return _instance;
	}
}
public void load()
{
st_map t;
t = new st_map();
t.map="a10101010101010100";
t.tesbo=true;
t.a= new int[]{6};
t.b= new float[]{4f};
t.c= new bool[]{true,true};
t.s= new string[]{"dfdf","dfdf"};
VALUE.Add(0, t);

t = new st_map();
t.id=1;
t.tesbo=true;
t.a= new int[]{4,5,6};
t.b= new float[]{7.6f,0f,0f};
t.c= new bool[]{true,false};
t.s= new string[]{"dfdf"};
VALUE.Add(1, t);

t = new st_map();
t.id=2;
t.a= new int[]{0,0,0};
t.b= new float[]{7f};
t.c= new bool[]{};
t.s= new string[]{};
VALUE.Add(2, t);

t = new st_map();
t.id=3;
t.test=1.5f;
t.tesbo=true;
t.a= new int[]{4};
t.b= new float[]{2f,4f};
t.c= new bool[]{false,false,false};
t.s= new string[]{"dfdf","dfdf","df"};
VALUE.Add(3, t);

t = new st_map();
t.id=4;
t.row=4;
t.col=4;
t.map="a10101010101010100";
t.tesbo=true;
t.a= new int[]{4,0,6};
t.b= new float[]{5f,6f,7f};
t.c= new bool[]{false};
t.s= new string[]{"dfdf","dfdf","df"};
VALUE.Add(4, t);

t = new st_map();
t.id=5;
t.row=4;
t.col=4;
t.map="a10101010101010100";
t.tesbo=true;
t.a= new int[]{5,6};
t.b= new float[]{};
t.c= new bool[]{false};
t.s= new string[]{"dfdf","dfdf"};
VALUE.Add(5, t);
}
public static st_map getst_mapByID(int id)
{
if(!I.VALUE.ContainsKey(id))
return null;
return I.VALUE[id];
}
public static st_map getst_mapFromJSON(int id)
{
	TextAsset jsonData = Resources.Load<TextAsset>("StData/Data/st_mapJSON_" + id );
	if (jsonData != null)
		return JsonUtility.FromJson<st_map>(jsonData.text);
	return null;
}
static int[] allIds = null;
public static int[] GetAllIds()
{
if (allIds == null)
{
allIds = I.VALUE.Keys.ToArray();
}
return allIds;
}
public static void CloseDataTable(){allIds=null;_instance = null;}
}
