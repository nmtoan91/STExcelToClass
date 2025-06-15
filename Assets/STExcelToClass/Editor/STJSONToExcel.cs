//Author : toanstt
//Git: https://github.com/nmtoan91
//UI: Hoang Anh Nguyen
#if UNITY_EDITOR
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Unity.Plastic.Antlr3.Runtime;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using Unity.Plastic.Newtonsoft.Json.Linq;
using JetBrains.Annotations;
using System.Linq;

namespace STGAME.STExcelToClass
{

    public class STJSONToExcel : EditorWindow
    {
        //string textBox_floatdef;
        //string textBox_intdef;
        //bool is_string_id = false;
        STJSONToExcelParameters parameters = new STJSONToExcelParameters();

        [MenuItem("STGame/Json To Excel")]
        static void Init()
        {
            STJSONToExcel window = (STJSONToExcel)EditorWindow.GetWindow(typeof(STJSONToExcel));
            window.Show();
        }
        [Multiline]
        public string textBox1;
        public string outputText;
        public string textConfigs;
        void OnGUI()
        {

            GUILayout.BeginHorizontal("box");

            if (GUILayout.Button("Import Data (Auto)"))
            {
                OnClickGen();
                AssetDatabase.Refresh();
            }


            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Insert sample data"))
            {
                string s = "{\"list\": [{\"row\":4,\"col\":\"4\",\"is_boss\":true,\"myarray\":[23,1,1],\"testfloat\":2.5,\"teststring\":\"asd\",\"array\":[\"string1\",\"strings2\"],\"e\":[\"ENUM\",\"ENUM3\",\"ENUM1\",\"ENUM7\"],\"dsd\":\"ssdf\",\"testDou\":[2.89375E+22,2.89375E+22]},{\"id\":1,\"col\":\"4\",\"is_boss\":true,\"myarray\":[2,3],\"teststring\":\"asd\",\"array\":[\"string2\",\"strings3\"],\"e\":[\"ENUM2\",\"ENUM2\",\"ENUM3\",\"ENUM8\"],\"dsd\":\"ffg\",\"testDou\":[1,2]},{\"id\":2,\"col\":\"4\",\"is_boss\":true,\"myarray\":[2,3],\"teststring\":\"asd\",\"array\":[\"string3\",\"strings4\"],\"e\":[\"ENUM2\",\"ENUM2\"],\"dsd\":\"dff\",\"testDou\":[3]},{\"id\":3,\"row\":4,\"col\":\"4\",\"is_boss\":true,\"myarray\":[2,3],\"testfloat\":2.4,\"teststring\":\"df\",\"array\":[\"string4\",\"strings5\"],\"e\":[\"ENUM1\",\"ENUM1\"],\"dsd\":\"sdf\",\"testDou\":[]},{\"id\":4,\"row\":4,\"is_boss\":true,\"myarray\":[2],\"testfloat\":1.2,\"teststring\":\"dsf\",\"array\":[\"string5\",\"strings6\"],\"e\":[\"ENUM3\",\"ENUM3\"],\"dsd\":\"sdf\",\"testDou\":[]}]}";
                textBox1 = s;

                //textConfigs = "{\t\\\"ItemClass\\\": \\\"ItemClass\\\",\t\\\"TableClass\\\": \\\"TableClass\\\",\t\\\"Parameters\\\": {\\\"IsStringId\\\":false,\\\"IsGenItemClass\\\":true,\\\"JSONName\\\":\\\"st_levelJSON\\\",\\\"IsGenEnum\\\":true,\\\"Path\\\":\\\"toanstt\\\"},\t\\\"Maps\\\": \t{\t  \\\"e\\\":\\\"TestEnum\\\",\t\t\\\"testLong\\\":\\\"long\\\",\t\t\\\"testfloat\\\":\\\"float\\\"\t}}";
                textConfigs = "{\t\"ItemClass\": \"ItemClass\",\t\"TableClass\": \"TableClass\",\t\"Parameters\": {\"IsStringId\":false,\"IsGenItemClass\":true,\"JSONName\":\"st_levelJSON\",\"IsGenEnum\":true,\"Path\":\"toanstt\"},\t\"Maps\": \t[\t  \"e:TestEnum\",\t\t\"testLong:long\",\t\t\"testfloat:float\"\t]}";
                EditorGUILayout.TextArea(textBox1);
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(this);
                GUIUtility.keyboardControl = 0;
                GUIUtility.hotControl = 0;
            }
            if (GUILayout.Button("Show all configs"))
            {
                string s = "Parameters:\n";
                s += "\nContact:\n\ttoan_stt@yahoo.com   \n";
                s += "\thttps://github.com/nmtoan91   \n";

                textBox1 = s;
                EditorGUILayout.TextArea(textBox1);
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(this);
                GUIUtility.keyboardControl = 0;
                GUIUtility.hotControl = 0;
            }

            GUILayout.EndHorizontal();



            GUILayout.Label("Paste your excel table here", EditorStyles.boldLabel);
            textBox1 = EditorGUILayout.TextArea(textBox1);
            GUILayout.Label("Configs", EditorStyles.boldLabel);
            textConfigs = EditorGUILayout.TextArea(textConfigs);

            GUILayout.Label("Output (Excel)", EditorStyles.boldLabel);
            outputText = EditorGUILayout.TextArea(outputText);
            GUILayout.BeginHorizontal("box");
            
            GUILayout.EndHorizontal();


           

        }
        void OnClickGen()
        {
            if(!string.IsNullOrEmpty(textConfigs)) 
                parameters = JsonUtility.FromJson<STJSONToExcelParameters>(textConfigs);

            parameters.Process();
            S = "";
            columnInfomations.Clear();
            JObject json = JObject.Parse(textBox1);
            
            var list = json["list"];
            GetAllColumns(list);
            CheckMaxLengths(list);
            GenerateColumns();
            GenerateData(list);
            outputText =  parameters.ItemClass  + "\t"
                + parameters.TableClass + "\t"
                + JsonUtility.ToJson(parameters.Parameters) + "\n" +
                S;
            int c = 12;
        }
        void GenerateData(JToken list)
        {
            JArray listObject = (JArray)list;
            for (int i = 0; i < listObject.Count; i++)
            {
                JToken jToken = list[i];
                JObject jOject = (JObject)jToken;

                foreach(string propertySingle in columnsSingle)
                {
                    string colName = propertySingle;
                    int n = columnInfomations[colName].maxLength; 
                    if( n <=0)
                    {
                        //if ((colName == "Id" || colName == "id"))
                        //{
                        //    Debug.Log("here");
                        //}
                        if ((colName=="Id" || colName == "id") && string.IsNullOrEmpty((string)jOject[colName]) )
                            S +=   "0\t";
                        else
                            S += jOject[colName] + "\t";

                    }
                    else
                    {
                        var node = jOject[colName];
                        JArray nodeArray = (JArray)node;
                        for(int j =0; j < n; j++)
                        {
                            if (j < nodeArray.Count) S += nodeArray[j] + "\t";
                            else S += "\t";
                        }
                    }
                }
                if (S[S.Length - 1] == '\t') S.Remove(S.Length - 1, 1);
                S += "\r\n";
            }
            Debug.Log(S);
        }
        void GenerateColumns()
        {
            columnsSingle.Clear();
            List<string> columnsBreakdown = new List<string>();
            foreach (var i in columnInfomations)
            {
                columnsSingle.Add(i.Value.name);
                string currentName = i.Value.name;
                string type = null;
                if (parameters.Maps_Dict.ContainsKey(currentName)) type = parameters.Maps_Dict[currentName];
                if (i.Value.maxLength <= 0)
                {
                    if (type != null)
                        columnsBreakdown.Add(currentName + ":" + type);
                    else
                        columnsBreakdown.Add(currentName);
                }
                else
                {
                    for (int j = 0; j < i.Value.maxLength; j++)
                    {
                        if (j == 0 && type != null)
                            columnsBreakdown.Add(i.Value.name + j + ":" + type);
                        else
                            columnsBreakdown.Add(i.Value.name + j);

                    }
                }
            }
            

            if (columnsSingle.Contains("Id") &&  columnsSingle[0] != "Id")
            {
                columnsSingle.Remove("Id");
                columnsSingle.Insert(0, "Id");
                columnsBreakdown.Remove("Id");
                columnsBreakdown.Insert(0, "Id");
            }
            if (columnsSingle.Contains("id") && columnsSingle[0] != "id")
            {
                columnsSingle.Remove("id");
                columnsSingle.Insert(0, "id");
                columnsBreakdown.Remove("id");
                columnsBreakdown.Insert(0, "id");
            }

            for (int i = 0; i < columnsBreakdown.Count; i++)
            {
                S += columnsBreakdown[i];
                if (i < columnsBreakdown.Count - 1) S += "\t";
                else S += "\r\n";
            }
            //Debug.Log(S);
        }
        void CheckMaxLengths(JToken list)
        {
            int c = 12;
            
            JArray listObject = (JArray)list;
            //IList<JToken> list = json;
            for (int i = 0; i < listObject.Count; i++)
            {
                JToken jToken = list[i];
                JObject jOject = (JObject)jToken;
                foreach (var property in jOject.Properties())
                {
                    if (!columnInfomations.ContainsKey(property.Name))
                        Debug.LogError($"Cannot find {property.Name} in columnInfomations");

                    if ((int)columnInfomations[property.Name].type < 100) continue;
                    var node = jOject[property.Name];
                    JArray nodeArray = (JArray)node;
                    if (nodeArray.Count > columnInfomations[property.Name].maxLength)
                        columnInfomations[property.Name].maxLength = nodeArray.Count;
                }
            }
        }
        void GetAllColumns(JToken list)
        {
            JArray listObject = (JArray)list;
            //IList<JToken> list = json;
            for (int i = 0; i < listObject.Count; i++)
            {
                JToken jToken = list[i];
                JObject jOject = (JObject)jToken;
                int c = 12;
                foreach (var property in jOject.Properties())
                {
                    if (columnInfomations.ContainsKey(property.Name)) continue;

                    Debug.Log("Key: \"" + property.Name + "\" , Value: " + property.Value);
                    STJSONToExcel_ColumnType type = TryToGetType(property.Name, jOject[property.Name]);
                    ColumnInformation ci = new ColumnInformation();
                    ci.name = property.Name;
                    ci.type = type;
                    columnInfomations.Add(property.Name, ci);
                }
            }
            
        }
        STJSONToExcel_ColumnType TryToGetType(string key, JToken jToken)
            {
                //string sub2 = key.Substring(0, 2);
                //string sub3 = key.Substring(0, 3);
                //if (sub2.Equals("is") || sub2.Equals("Is") || sub3 == "can" || sub3 == "Can")
                //    return STJSONToExcel_ColumnType.BOOL;

                if (jToken.Type == JTokenType.Integer)
                    return STJSONToExcel_ColumnType.INT;
                if (jToken.Type == JTokenType.String)
                    return STJSONToExcel_ColumnType.STRING;
                if (jToken.Type == JTokenType.Boolean)
                    return STJSONToExcel_ColumnType.BOOL;
                if (jToken.Type == JTokenType.Float)
                    return STJSONToExcel_ColumnType.FLOAT;
                if (jToken.Type == JTokenType.Array)
                {
                    JToken jTokenItem = jToken[0];
                    if (jTokenItem.Type == JTokenType.Integer)
                        return STJSONToExcel_ColumnType.ARRAY_INT;
                    if (jTokenItem.Type == JTokenType.String)
                        return STJSONToExcel_ColumnType.ARRAY_STRING;
                    if (jTokenItem.Type == JTokenType.Boolean)
                        return STJSONToExcel_ColumnType.ARRAY_BOOL;
                    if (jTokenItem.Type == JTokenType.Float)
                        return STJSONToExcel_ColumnType.ARRAY_FLOAT;
                    Debug.LogError("ERROR");
                }



                return STJSONToExcel_ColumnType.UNKNOWN;
            
        }
        class ColumnInformation
        {
            public string name;
            public STJSONToExcel_ColumnType type=STJSONToExcel_ColumnType.UNKNOWN;
            public int maxLength = -1;
        }
        List<string> columnsSingle = new List<string>();
        
        Dictionary<string, ColumnInformation> columnInfomations = new Dictionary<string, ColumnInformation>();
        string S=null;
        enum STJSONToExcel_ColumnType
        {
            UNKNOWN,
            INT,
            FLOAT,
            LONG,
            DOUBLE,
            STRING,
            BOOL,

            ARRAY_INT=100,
            ARRAY_FLOAT = 101,
            ARRAY_LONG = 102,
            ARRAY_DOUBLE = 103,
            ARRAY_STRING = 104,
            ARRAY_BOOL = 105,



        }
        [Serializable]
        class STExcelParameters
        {
            public bool IsStringId = false;
            public bool IsGenItemClass = true;
            public string JSONName = "";
            //public float DefaultFloat = 0;
            //public int DefaultInt = 0;
            //public string[] SkipGenEnums = new string[0];
            public string Path = "StData/Data";
            public string PathJSON = "";
            public bool IsGenSingleLineJSON = false;
            public bool IsSeparatedJSON = false;
            public bool IsGenJSON = false;
            public bool IsSkipZeroValue = true;
            public bool IsGenStringEnumValue = true;
        }
        [Serializable]
        class STJSONToExcelParameters
        {
            public string ItemClass = "ItemClass";
            public string TableClass = "TableClass";
            public STExcelParameters Parameters = null;
            public List<string> Maps = new List<string>();
            public Dictionary<string, string> Maps_Dict = new Dictionary<string, string>();
            public void Process()
            {
                
                Maps_Dict = new Dictionary<string, string>();
                foreach(var i in Maps)
                {
                    string[] aa = i.Split(":");
                    Maps_Dict.Add(aa[0], aa[1]);    
                }
            }
        }
    }
}
#endif
