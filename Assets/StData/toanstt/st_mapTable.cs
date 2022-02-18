using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
public st_map Get(int id)
{
return VALUE[id];
}
public void load()
{
st_map t;
t = new st_map();
t.id=0;
t.row=4;
t.col=4;
t.map="a10101010101010100";
t.a= new int[]{23,1,1};
VALUE.Add(0, t);

t = new st_map();
t.id=1;
t.row=4;
t.col=4;
t.map="a10101010101010100";
t.a= new int[]{2,3,4};
VALUE.Add(1, t);

t = new st_map();
t.id=2;
t.row=4;
t.col=4;
t.map="a10101010101010100";
t.a= new int[]{2,3,4};
VALUE.Add(2, t);

t = new st_map();
t.id=3;
t.row=4;
t.col=4;
t.map="a10101010101010100";
t.a= new int[]{2,3,4};
VALUE.Add(3, t);

t = new st_map();
t.id=4;
t.row=4;
t.col=4;
t.map="a10101010101010100";
t.a= new int[]{2,3,4};
VALUE.Add(4, t);
}
public static st_map getst_mapByID(int id){if(!I.VALUE.ContainsKey(id)) return null;return I.VALUE[id];}}
