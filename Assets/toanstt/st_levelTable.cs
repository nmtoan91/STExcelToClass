//Author: toanstt 
//This file is generated, do not edit!
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
st_level t;
t = new st_level();
t.id=0;
t.row=4f;
t.col="4";
t.is_boss=true;
t.myarray= new int[]{23,1,1};
t.testfloat=true;
t.teststring="asd";
t.array= new string[]{"string1","strings2"};
t.testenum=TestEnum.ENUM1;
t.e= new TestEnum[]{TestEnum.ENUM3,TestEnum.ENUM1};
VALUE.Add(0, t);

t = new st_level();
t.id=1;
t.row=0f;
t.col="4";
t.is_boss=false;
t.myarray= new int[]{2,3};
t.testfloat=false;
t.teststring="asd";
t.array= new string[]{"string2","strings3"};
t.testenum=TestEnum.ENUM2;
t.e= new TestEnum[]{TestEnum.ENUM2,TestEnum.ENUM3};
VALUE.Add(1, t);

t = new st_level();
t.id=2;
t.row=0f;
t.col="4";
t.is_boss=false;
t.myarray= new int[]{2,3};
t.testfloat=false;
t.teststring="asd";
t.array= new string[]{"string3","strings4"};
t.testenum=TestEnum.ENUM2;
t.e= new TestEnum[]{TestEnum.ENUM2};
VALUE.Add(2, t);

t = new st_level();
t.id=3;
t.row=4f;
t.col="4";
t.is_boss=false;
t.myarray= new int[]{2,3};
t.testfloat=true;
t.teststring="df";
t.array= new string[]{"string4","strings5"};
t.testenum=TestEnum.ENUM1;
t.e= new TestEnum[]{TestEnum.ENUM1};
VALUE.Add(3, t);

t = new st_level();
t.id=4;
t.row=4f;
t.col="";
t.is_boss=true;
t.myarray= new int[]{2};
t.testfloat=true;
t.teststring="dsf";
t.array= new string[]{"string5","strings6"};
t.testenum=TestEnum.ENUM3;
t.e= new TestEnum[]{TestEnum.ENUM3};
VALUE.Add(4, t);
}
public static st_level getst_levelByID(int id){if(!I.VALUE.ContainsKey(id)) return null;return I.VALUE[id];}}
