//Author : toanstt
//Git: https://github.com/nmtoan91
//UI: Hoang Anh Nguyen
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.Collections;

namespace STGAME.STExcelToClass
{

    public class STExcelToClass : EditorWindow
    {
        //string textBox_floatdef;
        //string textBox_intdef;
        //bool is_string_id = false;
        STExcelParameters parameters = new STExcelParameters();

        [MenuItem("STGame/Excel to Class")]
        static void Init()
        {
            STExcelToClass window = (STExcelToClass)EditorWindow.GetWindow(typeof(STExcelToClass));
            window.Show();
        }

        void OnGUI()
        {

            GUILayout.BeginHorizontal("box");

            if (GUILayout.Button("Import Data (Auto)"))
            {
                OnClickGen(null, null);
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Import Data (Class)"))
            {
                OnClickGenClassData(null, null);
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Import Data (JSON)"))
            {
                OnClickGenJSONData(null, null);
                AssetDatabase.Refresh();
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Insert sample data"))
            {
                string s = "st_level\tst_levelTable\t{\"Path\":\"StData/Data\"}\n";
                s += "id\trow\tcol\tis_boss\tmyarray0\tmyarray1\tmyarray2\ttestforcestring:string\tteststring\tarray0\tarray1\n";
                s += "0\t4\t4\tTRUE\t23\t1\t1\t2.5\tasd\tstring1\tstrings2\n";
                s += "1\t\t4\tfalse\t2\t3\t\t\tasd\tstring2\tstrings3\n";
                s += "2\t\t4\t0\t2\t3\t\t\tasd\tstring3\tstrings4\n";
                s += "3\t4\t4\t0\t2\t3\t\t2.4\tdf\tstring4\tstrings5\n";
                s += "4\t4\t\t1\t2\t\t\t1.2\tdsf\tstring5\tstrings6\n";
                textBox1 = s;
                EditorGUILayout.TextArea(textBox1);
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(this);
                GUIUtility.keyboardControl = 0;
                GUIUtility.hotControl = 0;
            }
            if (GUILayout.Button("Show all configs"))
            {
                string s = "Parameters:\n";
                //s += "\tIsStringId\t\t: Force id to string type (default=false)\n";
                s += "\tPath\t\t\t: Path to save the dataset (default=StData/Data)\n";
                s += "\tIsGenItemClass\t\t: Skip generate item class; generate proto file instead (default=true)\n";

                //s += "\tDefaultFloat\t\t: Default value of float (default=0)\n";
                //s += "\tDefaultInt\t\t: Default value of int (default=0)\n";

                //s += "\tSkipGenEnums\t\t: Array of skiped enums \n";
                s += "\tIsGenJSON\t\t: Force to gen JSON with click Extract \n";
                s += "\tJSONName\t\t: Json filename (default=StData/Data)\n";
                s += "\tIsSeparatedJSON\t: Gen json data in separated files \n";
                s += "\tIsGenSingleLineJSON\t: Gen single line json for putting on code   \n";
                s += "\tPathJSON\t\t: Path to save the json file (default=#Path)\n";
                s += "\tIsSkipZeroValue\t\t: Is generate zero values (default=true)\n";


                s += "\nProperty naming rules:\n";
                s += "\tvarname:type\t\t: Force set variable type, type = {int,float,string,bool,enumName} \n";
                s += "\tExamples\t\t: aaa\ta:int\tb:string\tc:enumName \n";



                s += "\nEnum rules:\n";
                s += "\tDefine\t\t\t: <enum_value>=<enum_int_value> \n";
                s += "\tInit enum example\t: variableName:MyEnum{A=0,B=11,C}:  \n";
                s += "\tSkip generate an enum\t: variableName:MyEnum{false}  \n";

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
            GUILayout.BeginHorizontal("box");
            //GUILayout.Label("Output Directory: ", EditorStyles.boldLabel);
            //textBox3_dir = EditorGUILayout.TextField("", textBox3_dir);
            //GUILayout.Label(parameters.Path, EditorStyles.boldLabel);
            GUILayout.EndHorizontal();


            //GUILayout.Label("------------------------STGAME---------------------------", EditorStyles.boldLabel);

            //GUILayout.BeginHorizontal("box");
            //GUILayout.Label("Config", EditorStyles.boldLabel);
            //GUILayout.Label("Desciption", EditorStyles.boldLabel);
            //GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal("box");
            //GUILayout.Label("Path");
            //GUILayout.Label("Path to save the dataset (default=STGAME/Data)");
            //GUILayout.EndHorizontal();
            //GUILayout.BeginHorizontal("box");
            //GUILayout.Label("PathJSON");
            //GUILayout.Label("Path to save the json file (default=#Path)");
            //GUILayout.EndHorizontal();
            //GUILayout.BeginHorizontal("box");
            //GUILayout.Label("IsGenJSON");
            //GUILayout.Label("Force to gen JSON with auto gen button");
            //GUILayout.EndHorizontal();

        }
        [Multiline]
        public string textBox1;
        public string outputText;
        //public string textBox3_dir;

        //public Form1()
        //{
        //    InitializeComponent();
        //    outputText = "mới: thêm chức năng tự động tạo mảng, (az0 az1 az2) ->  az[3]";
        //    DateTime today = DateTime.Today;
        //    DateTime day_last = new DateTime(2022, 03, 01);
        //    if (today > day_last)
        //    {
        //        MessageBox.Show("Update required, just contact toanstt!");
        //        System.Environment.Exit(1);
        //    }

        //}
        //st_test	st_testTable	st_test_JSON	StData/					
        //id	ab0	ab1	ab2	b0	t0	t1	b1	
        //0	4	5	4	2	dfg	ghjk fg	rty 	
        //1	1	6	4	5	hjghj	dfg 	rty 	
        //33	4	1	6	2	dfg fg	dfg 	 gh	
        //3	3	3	4	9	hjghj	fgger	fgh 	
        //12	2	2	5	2	hjk	rty	fgh 	

        List<string> names = new List<string>();
        Dictionary<int, string> IDs = new Dictionary<int, string>();

        string str_name_class_object;
        string str_name_class_data;
        //string str_json_name_file;
        string str_json_path_folder;

        int iStar = 1;

        int n;//so cot
        int size;//so dong
        string str;
        string[] lines;
        string[] LINE;
        List<TYPE> types;
        List<string> typesNames;

        Dictionary<string, List<string>> typesDictionraries;
        List<string> typesSkipedForGenerate;
        Dictionary<string, Dictionary<string, string>> typesDictionrariesLevel2;


        string[] protoKeyworks = { "int32", "float", "string", "bool" };
        List<string> allDataIds = new List<string>();

        private void OnClickGen(object sender, EventArgs e)
        {
            bool IsGenJSON = false;
            if (textBox1.IndexOf("\"IsGenJSON\":true") >= 0)
                IsGenJSON = true;
            else if (textBox1.IndexOf("\"IsGenJSON\":1") >= 0)
                IsGenJSON = true;

            if (IsGenJSON)
            {
                Debug.Log("Generating JSON");
                OnClickGenJSONData(sender, e);
            }
            else
            {
                Debug.Log("Generating Classes with data");
                OnClickGenClassData(sender, e);
            }
        }
        private void OnClickGenClassData(object sender, EventArgs e)
        {
            //parameters.IsGenJSON = false;
            if (typesDictionraries != null) typesDictionraries.Clear();
            if (typesDictionrariesLevel2 != null) typesDictionrariesLevel2.Clear();

            allDataIds.Clear();
            InitData();
            ReadNames(false);
            parameters.IsSeparatedJSON = false;
            TryToParseArray();
            LoadFirstData();
            Gen_st_hero(str_name_class_object);
            Gen_st_hero_table(str_name_class_data, str_name_class_object);
            Debug.Log("Files are generated in " + parameters.Path);
        }
        private void OnClickGenJSONData(object sender, EventArgs e)
        {
            //parameters.IsGenJSON = true;
            allDataIds.Clear();
            InitData();
            ReadNames(true);

            TryToParseArray();
            LoadFirstData();
            Gen_st_hero(str_name_class_object);
            gen_st_json(parameters.JSONName, str_name_class_object);
            Gen_st_hero_table(str_name_class_data, str_name_class_object, false);

            Debug.Log("Files are generated in " + parameters.Path);
            //textBox1 = "";
        }

        void InitData()
        {
            str = textBox1;
            names = new List<string>();
            IDs = new Dictionary<int, string>();
            str = str.Trim('\r');

            while (str[str.Length - 1] == '\n')
            {
                str = str.Substring(0, str.Length - 1);
            }

            lines = str.Split('\n');
            size = lines.Length - 1;
            for (int i = 0; i <= size; i++)
            {
                while (lines[i][lines[i].Length - 1] == '\n')
                {
                    lines[i] = lines[i].Substring(0, lines[i].Length - 1);
                }
                while (lines[i][lines[i].Length - 1] == '\r')
                {
                    lines[i] = lines[i].Substring(0, lines[i].Length - 1);
                }
            }

        }
        public void ReadNames(bool isJSON)
        {
            //MessageBox.Show(String.Join("*", lines));
            typesSkipedForGenerate = new List<string>();
            LINE = lines[iStar].Split('\t');
            n = LINE.Length;
            types = new List<TYPE>();
            typesNames = new List<string>();
            for (int i = 0; i < n; i++)
            {
                types.Add(TYPE.NOT_DECICED_YET);
                typesNames.Add(null);
            }


            for (int i = 0; i < LINE.Length; i++)
            {
                if (LINE[i].Split(':').Length >= 2)
                {
                    string[] substrings = LINE[i].Split(':');
                    string presetType = substrings[1];
                    if (presetType.Equals("int") || presetType.Equals("Int") || presetType.Equals("I") || presetType.Equals("i")) types[i] = TYPE.INT;
                    else if (presetType.Equals("long") || presetType.Equals("Long") || presetType.Equals("L") ) types[i] = TYPE.LONG;
                    else if (presetType.Equals("float") || presetType.Equals("Float") || presetType.Equals("F") || presetType.Equals("f")) types[i] = TYPE.FLOAT;
                    else if (presetType.Equals("double") || presetType.Equals("Double") || presetType.Equals("D") ) types[i] = TYPE.DOUBLE;
                    else if (presetType.Equals("bool") || presetType.Equals("Bool") || presetType.Equals("Boolean") || presetType.Equals("B") || presetType.Equals("b")) types[i] = TYPE.BOOL;
                    else if (presetType.Equals("string") || presetType.Equals("String") || presetType.Equals("S") || presetType.Equals("s")) types[i] = TYPE.STRING;
                    else
                    {
                        types[i] = TYPE.ENUM;
                        typesNames[i] = ExtractEnumNameAndProperties(presetType, true);
                    }
                    LINE[i] = substrings[0];
                }

                names.Add(LINE[i]);
            }

            str_name_class_object = lines[0].Split('\t')[0];
            str_name_class_data = lines[0].Split('\t')[1];

            if (lines[0].Split('\t').Length > 2 && !string.IsNullOrEmpty(lines[0].Split('\t')[2]))
            {
                string str_json_name_file2 = lines[0].Split('\t')[2];
                try
                {
                    parameters = JsonUtility.FromJson<STExcelParameters>(str_json_name_file2);
                    if (string.IsNullOrEmpty(parameters.JSONName))
                        parameters.JSONName = str_name_class_data + "_JSON";
                    if (string.IsNullOrEmpty(parameters.Path))
                        parameters.Path = "StData/Data";
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                    Debug.Log("Warning: Cannot parse json data, please use this format at cell (C:1): {\"IsStringId\":false,\"IsGenItemClass\":false,\"JSONName\":\"stLevelJSON\"}");
                    parameters.JSONName = str_json_name_file2;
                }
            }
            parameters.IsGenJSON = isJSON;

            if (lines[0].Split('\t').Length >= 4 && !string.IsNullOrEmpty(lines[0].Split('\t')[3]))
                parameters.Path = lines[0].Split('\t')[3];
            str_json_path_folder = parameters.Path;
            trimames();

            str_json_path_folder = str_json_path_folder.Replace('\\', '/');

            string textBox3_dir = str_json_path_folder;
            Debug.Log("creating folder: " + textBox3_dir);
            if (textBox3_dir[textBox3_dir.Length - 1] == '/') textBox3_dir = textBox3_dir.Substring(0, textBox3_dir.Length - 1);
            if (!string.IsNullOrEmpty(textBox3_dir))
            {
                CreateFolderIfNotExist(textBox3_dir);
            }

            if (string.IsNullOrEmpty(parameters.PathJSON))
                parameters.PathJSON = parameters.Path;
            if (parameters.PathJSON.IndexOf("Resources") < 0)
            {
                parameters.PathJSON = "Resources/" + parameters.PathJSON;
                if (isJSON)
                {
                    Debug.LogError("JSON file must be placed in Resources folder; JSON file is storaged in " + parameters.PathJSON);
                }
            }
            CreateFolderIfNotExist(parameters.PathJSON);
        }
        void CreateFolderIfNotExist(string dir)
        {
            string[] subFolders = dir.Split('/');
            string root = "Assets";
            foreach (string subFolder in subFolders)
            {
                if (!AssetDatabase.IsValidFolder(root + "/" + subFolder))
                {
                    string guid = AssetDatabase.CreateFolder(root, subFolder);
                }
                root += "/" + subFolder;
            }
        }

        int[] ARRAY_LENGTH;
        int[] MY_ARRAY_INDEX;
        int[] MY_INDEX_IN_MY_ARRAY;
        public void trimames()
        {
            if (names[names.Count - 1] == "")
            {
                names.RemoveAt(names.Count - 1);
                types.RemoveAt(names.Count - 1);
                typesNames.RemoveAt(names.Count - 1);
                n--;
            }
        }
        public void TryToParseArray()
        {
            ARRAY_LENGTH = new int[names.Count];
            MY_ARRAY_INDEX = new int[names.Count];
            MY_INDEX_IN_MY_ARRAY = new int[names.Count];

            for (int i = 0; i < names.Count; i++)
            {
                if (names[i][names[i].Length - 1] == '0')
                {
                    string name = names[i].Substring(0, names[i].Length - 1);
                    names[i] = name;
                    int I = i;
                    ARRAY_LENGTH[i] = 1;
                    MY_ARRAY_INDEX[i] = i;
                    for (int j = i + 1; j < names.Count; j++)
                    {
                        if (names[j].Length > name.Length && names[j].Substring(0, name.Length) == name)
                        {
                            i++;
                            ARRAY_LENGTH[I] = j - I + 1;
                            MY_ARRAY_INDEX[j] = I;
                            MY_INDEX_IN_MY_ARRAY[j] = j - I;
                        }
                        else break;
                    }

                }
                else
                {
                    ARRAY_LENGTH[i] = -1;
                    MY_ARRAY_INDEX[i] = -1;
                    MY_INDEX_IN_MY_ARRAY[i] = -1;
                }
            }

        }
        public void LoadFirstData()
        {
            LINE = lines[iStar + 1].Split('\t');

            if (typesDictionraries == null) typesDictionraries = new Dictionary<string, List<string>>();
            if (typesDictionrariesLevel2 == null) typesDictionrariesLevel2 = new Dictionary<string, Dictionary<string, string>>();


            for (int i = 0; i < n; i++)
            {
                if (types[i] != TYPE.NOT_DECICED_YET) continue;
                try
                {
                    int.Parse(LINE[i]);

                    types[i] = TYPE.INT;
                }
                catch //(Exception e)
                {
                    try
                    {
                        float.Parse(LINE[i]);
                        types[i] = TYPE.FLOAT;
                    }
                    catch //(Exception e1)
                    {
                        //Debug.Log(e1.)
                        types[i] = TYPE.STRING;
                    }
                }
                if (names[i].IndexOf("is_") == 0) { types[i] = TYPE.BOOL; }
                if (names[i].IndexOf("can_") == 0) { types[i] = TYPE.BOOL; }
                if (names[i].IndexOf("Is") == 0) { types[i] = TYPE.BOOL; }
                if (names[i].IndexOf("Can") == 0) { types[i] = TYPE.BOOL; }
            }
            if (types[0] == TYPE.STRING)
            {
                parameters.IsStringId = true;
                Debug.Log("Force set id to string because they are strings");
            }
            for (int i = 0; i < n; i++)
            {
                switch (types[i])
                {
                    case TYPE.INT: typesNames[i] = "int"; break;
                    case TYPE.FLOAT: typesNames[i] = "float"; break;
                    case TYPE.STRING: typesNames[i] = "string"; break;
                    case TYPE.BOOL: typesNames[i] = "bool"; break;
                    case TYPE.LONG: typesNames[i] = "long"; break;
                    case TYPE.DOUBLE: typesNames[i] = "double"; break;
                        
                }
            }
        }
        public string Gen_st_hero(string classname)
        {
            if (!parameters.IsGenItemClass)
            {
                return GenProto(classname);
            }

            string s = "//Author: toanstt \n//This file is generated, do not edit!\n";
            s += "using UnityEngine;\n";
            s += "using System.Collections;\n";
            s += "using System.Collections.Generic;\n";
            s += "[System.Serializable]";
            s += "public class " + classname + "  \n{\n";
            for (int i = 0; i < n; i++)
            {
                if (ARRAY_LENGTH[i] > 0)
                {
                    s += "public " + gettext(types[i], i) + "[] " + names[i] + ";// length=" + ARRAY_LENGTH[i] + "\n";
                    i += ARRAY_LENGTH[i] - 1;
                }
                else
                    s += "public " + gettext(types[i], i) + " " + names[i] + ";\n";
            }

            s += "}\n";
            outputText = s;
            //MessageBox.Show(s);
            string dr = parameters.Path + "/" + classname + ".cs";
            if (parameters.Path == null || parameters.Path == "") dr = classname + ".cs";
            //MessageBox.Show(Path.Combine(Application.dataPath, dr));
            File.WriteAllText(Path.Combine(Application.dataPath, dr), s);
            return s;
        }
        public string GenProto(string classname)
        {
            string s = "Message " + classname + " {\n";
            for (int i = 0; i < n; i++)
            {
                string ss = protoKeyworks[(int)types[i]] + " " + names[i] + " = " + (i + 1) + ";\n";
                if (ARRAY_LENGTH[i] > 0)
                    ss = "repeated " + ss;
                s += ss;
            }
            s += "}";
            Debug.Log(s);
            string dr = parameters.Path + "/" + classname + ".proto.txt";
            if (parameters.Path == null || parameters.Path == "") dr = classname + ".proto.txt";
            File.WriteAllText(Path.Combine(Application.dataPath, dr), s);
            return s;
        }
        public string Gen_st_hero_table(string classname, string class1, bool is_gen_data = true)
        {
            // MessageBox.Show(classname + " " + class1);
            string s = "//Author: toanstt \n//This file is generated, do not edit!\n";
            s += "using UnityEngine;\n";
            s += "using System.Collections;\n";
            s += "using System.Collections.Generic;\n";
            s += "using System;\n";
            s += "using System.Linq;\n";


            if (is_gen_data == false)
            {
                s += "[System.Serializable] class " + classname + "ListJSON{ public " + class1 + "[] list;}\n\n";
            }


            s += "public class " + classname + "  \n{\n";

            //s += "public class " + classname + "  \n{\n";



            //singleton
            s += "private static " + classname + " _instance;\n";

            //new for JSON parser
            //s += "public List<" + class1 + "> list;\n";
            s += "public " + class1 + "[] list;\n";
            //end new for JSON parser

            string scope = "public";
            if (parameters.IsSeparatedJSON) scope = "private";

            if (parameters.IsStringId)
                s += scope + " Dictionary<string, " + class1 + "> VALUE;\n";
            else
                s += scope + " Dictionary<int, " + class1 + "> VALUE;\n";
            if (parameters.IsSeparatedJSON)
            {
                s = s.Substring(0, s.Length - 1);
                s += "// VALUE must be private for separated JSON files, please use GetAllIds() function to get all ids\n";
            }

            s += "public " + classname + "()\n";
            s += "{\n";

            if (parameters.IsStringId)
                s += "\tVALUE = new Dictionary<string, " + class1 + ">();\n";
            else s += "\tVALUE = new Dictionary<int, " + class1 + ">();\n";
            //new for JSON parser
            //s += "\tlist = new List<" + class1 + ">();\n";
            //end new for JSON parser

            s += "}\n";

            s += "public static " + classname + " I\n";
            s += "{\n";
            s += "\tget\n";
            s += "\t{\n";
            s += "\tif (_instance == null)\n";
            s += "\t       {\n";
            s += "\t           _instance = new " + classname + "();\n";

            if (!parameters.IsGenJSON || !parameters.IsSeparatedJSON)
                s += "\t           _instance.load();\n";

            s += "\t       }\n";
            s += "\t       return _instance;\n";
            s += "\t}\n";
            s += "}\n";

            //Get

            //if (parameters.IsStringId)
            //{
            //    s += "public " + class1 + " Get(string id)\n";
            //    s += "{\n";
            //    s += "return VALUE[id];\n";
            //    s += "}\n";
            //}
            //else
            //{
            //    s += "public " + class1 + " Get(int id)\n";
            //    s += "{\n";
            //    s += "return VALUE[id];\n";
            //    s += "}\n";
            //}

            //define more variable
            s += "public void load()\n";
            s += "{\n";






            string current_id = "null";
            bool is_have_id = false;
            if (is_gen_data == true)
            {
                s += "" + class1 + " t;";
                for (int j = iStar + 1; j <= size; j++)
                {
                    is_have_id = false;
                    current_id = "null";
                    s += "\nt = new " + class1 + "();\n";
                    string sss = lines[j].Replace("\n", "");
                    LINE = sss.Split('\t');
                    for (int i = 0; i < n; i++)
                    {
                        if (ARRAY_LENGTH[i] > 0)
                        {
                            if (parameters.IsGenItemClass == true)
                                s += "t." + names[i] + "= new " + gettext(types[i], i) + "[]{";
                            else s += "t." + names[i] + ".AddRange(new " + gettext(types[i], i) + "[]{";

                            int index = 0; int k;
                            for (k = i; k < i + ARRAY_LENGTH[i]; k++)
                            {
                                index = k - i;
                                if (LINE[k] == null || LINE[k].Length == 0)
                                {
                                    for (int k2 = k; k2 < i + ARRAY_LENGTH[i]; k2++)
                                        if (!string.IsNullOrEmpty(LINE[k2]))
                                        {
                                            EditorUtility.DisplayDialog("STExcelToClass", string.Format("WARNING: There is a empty cell at row id:{0} col:{1}", current_id, k2), "OK");
                                            break;
                                        }
                                    break; // break for short array
                                }
                                switch (types[i])
                                {
                                    case TYPE.FLOAT:
                                        if (LINE[k] == null || LINE[k].Length == 0)
                                            s += "0f,";
                                        else
                                        {
                                            LINE[k] = LINE[k].Replace(",", "f,");
                                            s += LINE[k] + "f,";
                                        }
                                        break;
                                    case TYPE.DOUBLE:
                                        if (LINE[k] == null || LINE[k].Length == 0)
                                            s += "0,";
                                        else
                                        {
                                            LINE[k] = LINE[k].Replace(",", ",");
                                            s += LINE[k] + ",";
                                        }
                                        break;
                                    case TYPE.INT:
                                    case TYPE.LONG:
                                        if (LINE[k] == null || LINE[k].Length == 0)
                                            s += "0" + ",";
                                        else
                                            s += LINE[k] + ",";
                                        break;
                                    case TYPE.STRING:
                                        s += "\"" + LINE[k] + "\",";
                                        break;
                                    case TYPE.BOOL:
                                        if (LINE[k] == "1") s += "true,";
                                        else if (LINE[k] == "0") s += "false,";
                                        else if (LINE[k] == "true") s += "true,";
                                        else if (LINE[k] == "false") s += "false,";
                                        else if (LINE[k] == "TRUE") s += "true,";
                                        else if (LINE[k] == "FALSE") s += "false,";
                                        else if (LINE[k] == "") s += "false,";
                                        else if (LINE[k] != "") s += "true,";
                                        else
                                        {
                                            MessageBox.Show("Khi dùng \"is_..\" thì giá trị phải 1 hoặc 0");
                                        }
                                        break;
                                    case TYPE.ENUM:




                                        if (LINE[i] == "") break;
                                        string[] enumValues = LINE[k].Replace(" ", "").Split(',');
                                        foreach (string enumValue in enumValues)
                                        {
                                            string enumValue_ = enumValue;

                                            string enumInt = "";
                                            string[] enumKeyVales = enumValue.Split('=');
                                            if (enumKeyVales.Length == 2) { enumValue_ = enumKeyVales[0]; enumInt = enumKeyVales[1]; }



                                            if (enumValue_[0] >= '0' && enumValue_[0] <= '9')
                                                s += '(' + typesNames[i] + ')' + enumValue_ + ",";
                                            else
                                            {
                                                s += typesNames[i] + "." + enumValue_ + ",";

                                                if (!string.IsNullOrEmpty(typesNames[i]))
                                                {
                                                    if (!typesDictionraries.ContainsKey(typesNames[i])) typesDictionraries.Add(typesNames[i], new List<string>());
                                                    if (!typesDictionrariesLevel2.ContainsKey(typesNames[i])) typesDictionrariesLevel2.Add(typesNames[i], new Dictionary<string, string>());
                                                    if (!typesDictionraries[typesNames[i]].Contains(enumValue_)) typesDictionraries[typesNames[i]].Add(enumValue_);
                                                    if (!string.IsNullOrEmpty(enumInt))
                                                    {
                                                        if (!typesDictionrariesLevel2[typesNames[i]].ContainsKey(enumValue_)) typesDictionrariesLevel2[typesNames[i]].Add(enumValue_, enumInt);
                                                    }
                                                }
                                            }

                                        }






                                        break;

                                }

                            }
                            if (k > i)
                                s = s.Substring(0, s.Length - 1);
                            if (parameters.IsGenItemClass == true)
                                s += "};\n";
                            else s += "});\n";
                            i += ARRAY_LENGTH[i] - 1;
                        }
                        else
                        {
                            if (names[i] == "id" || names[i] == "Id")
                            { current_id = LINE[i]; is_have_id = true; }
                            switch (types[i])
                            {
                                case TYPE.FLOAT:
                                
                                    if (LINE[i] == null || LINE[i].Length == 0 || LINE[i].Equals("0") || LINE[i].Equals("0.0"))
                                    {
                                        if (!parameters.IsSkipZeroValue)
                                            s += "t." + names[i] + "=" + 0 + "f;\n";
                                    }
                                    else
                                        s += "t." + names[i] + "=" + LINE[i] + "f;\n";
                                    break;
                                case TYPE.DOUBLE:
                                    if (LINE[i] == null || LINE[i].Length == 0 || LINE[i].Equals("0") || LINE[i].Equals("0.0"))
                                    {
                                        if (!parameters.IsSkipZeroValue)
                                            s += "t." + names[i] + "=" + 0 + ";\n";
                                    }
                                    else
                                        s += "t." + names[i] + "=" + LINE[i] + ";\n";
                                    break;
                                case TYPE.INT:
                                case TYPE.LONG:
                                    if (LINE[i] == null || LINE[i].Length == 0 || LINE[i].Equals("0"))
                                    {
                                        if (!parameters.IsSkipZeroValue)
                                            s += "t." + names[i] + "=" + 0 + ";\n";
                                    }
                                    else
                                        s += "t." + names[i] + "=" + LINE[i] + ";\n";
                                    break;
                                case TYPE.STRING:
                                    if (parameters.IsSkipZeroValue && string.IsNullOrEmpty(LINE[i]))
                                        ;
                                    else s += "t." + names[i] + "=\"" + LINE[i] + "\";\n";
                                    break;
                                case TYPE.BOOL:
                                    if (LINE[i] == "1") s += "t." + names[i] + "=true;\n";
                                    else if (LINE[i] == "0" && !parameters.IsSkipZeroValue) s += "t." + names[i] + "=false;\n";
                                    else if (LINE[i] == "true") s += "t." + names[i] + "=true;\n";
                                    else if (LINE[i] == "false" && !parameters.IsSkipZeroValue) s += "t." + names[i] + "=false;\n";
                                    else if (LINE[i] == "TRUE") s += "t." + names[i] + "=true;\n";
                                    else if (LINE[i] == "FALSE" && !parameters.IsSkipZeroValue) s += "t." + names[i] + "=false;\n";
                                    else if (LINE[i] == "" && !parameters.IsSkipZeroValue) s += "t." + names[i] + "=false;\n";
                                    else if (LINE[i] != "") s += "t." + names[i] + "=true;\n";
                                    //else
                                    //{
                                    //    MessageBox.Show("Khi dùng \"is_..\" thì giá trị phải 1 hoặc 0");

                                    //}
                                    break;
                                case TYPE.ENUM:
                                    string enumInt = "";
                                    //Debug.Log("dddd " + LINE[i]);
                                    if (LINE[i] == null || LINE[i].Length == 0 || LINE[i] == "") ;
                                    else
                                    {
                                        string[] enumVales = LINE[i].Split('=');
                                        if (enumVales.Length == 2) { LINE[i] = enumVales[0]; enumInt = enumVales[1]; }
                                        s += "t." + names[i] + "=" + typesNames[i] + "." + LINE[i] + ";\n";
                                    }

                                    if (!string.IsNullOrEmpty(typesNames[i]))
                                    {
                                        if (!typesDictionraries.ContainsKey(typesNames[i])) typesDictionraries.Add(typesNames[i], new List<string>());
                                        if (!typesDictionrariesLevel2.ContainsKey(typesNames[i])) typesDictionrariesLevel2.Add(typesNames[i], new Dictionary<string, string>());
                                        if (!typesDictionraries[typesNames[i]].Contains(LINE[i])) typesDictionraries[typesNames[i]].Add(LINE[i]);
                                        if (!string.IsNullOrEmpty(enumInt))
                                        {

                                            if (!typesDictionrariesLevel2[typesNames[i]].ContainsKey(LINE[i])) typesDictionrariesLevel2[typesNames[i]].Add(LINE[i], enumInt);
                                        }
                                    }
                                    break;
                            }
                        }
                    }

                    if (allDataIds.Contains(current_id))
                    {
                        EditorUtility.DisplayDialog("STExcelToClass", string.Format("Duplicated id value: {0}", current_id), "OK");
                    }
                    else allDataIds.Add(current_id);

                    if (is_have_id == false)
                    {
                        MessageBox.Show("ban ko ton tai id");
                    }

                    if (parameters.IsStringId)
                        s += "VALUE.Add(\"" + current_id + "\", t);\n";
                    else
                        s += "VALUE.Add(" + current_id + ", t);\n";
                }
            }
            else
            {
                if (!(parameters.Path.IndexOf("Resources/") != 0 || parameters.Path.IndexOf("Resource\\") != 0))
                {
                    Debug.Log("WARNING: YOU MUST put JSON file to Resources to load it in build");
                }

                int ResourcesStringIndex = parameters.PathJSON.IndexOf("Resources") + 10;
                string resourceDir = parameters.PathJSON.Substring(ResourcesStringIndex, parameters.PathJSON.Length - ResourcesStringIndex);

                resourceDir += "/" + parameters.JSONName;
                resourceDir = resourceDir.Replace(".json", "");
                s += "TextAsset jsonData = Resources.Load<TextAsset>(\"" + resourceDir + "\");\n";
                s += classname + "ListJSON lmyist = JsonUtility.FromJson<" + classname + "ListJSON> (jsonData.text);\n";
                s += "foreach(" + class1 + " i in lmyist.list) { VALUE.Add(i." + names[0] + ", i); }\n";

            }
            s += "}\n";
            //function


            if (parameters.IsStringId)
            {
                s += "public static " + class1 + " get" + class1.Replace(".", "") + "ByID(string id)\n";
                s += "{\n";
                s += "if(!I.VALUE.ContainsKey(id))\n";
                if (parameters.IsSeparatedJSON)
                    s += "\treturn get" + class1.Replace(".", "") + "FromJSON(id);\n";
                else
                    s += "return null;\n";
                s += "return I.VALUE[id];\n";
                s += "}";
            }
            else
            {
                s += "public static " + class1 + " get" + class1.Replace(".", "") + "ByID(int id)\n";
                s += "{\n";
                s += "if(!I.VALUE.ContainsKey(id))\n";
                if (parameters.IsSeparatedJSON)
                    s += "\treturn get" + class1.Replace(".", "") + "FromJSON(id);\n";
                else s += "return null;\n";
                s += "return I.VALUE[id];\n";
                s += "}";
            }


            //load from sepraterd file 

            int ResourcesStringIndex2 = parameters.PathJSON.IndexOf("Resources") + 10;
            string resourceDir2 = parameters.PathJSON.Substring(ResourcesStringIndex2, parameters.PathJSON.Length - ResourcesStringIndex2);
            resourceDir2 += "/" + parameters.JSONName; resourceDir2 = resourceDir2.Replace(".json", "");
            s += "\n";
            s += "public static " + class1 + " get" + class1.Replace(".", "") + "FromJSON(" + (parameters.IsStringId ? "string" : "int") + " id)\n";
            s += "{\n";
            s += "\tTextAsset jsonData = Resources.Load<TextAsset>(\"" + resourceDir2 + "_\" + id );\n";
            s += "\tif (jsonData != null)\n";
            s += "\t\treturn JsonUtility.FromJson<" + class1 + ">(jsonData.text);\n";
            s += "\treturn null;\n";
            s += "}\n";


            //reading all ids from _ text file
            string stringOrInt = parameters.IsStringId ? "string" : "int";
            if (parameters.IsGenJSON)
            {
                s += $"static {stringOrInt}[] allIds = null;\n";
                //if (parameters.IsSeparatedJSON)
                //{

                s += $"public static {stringOrInt}[] GetAllIds()\n";
                s += "{\n";
                s += "if (allIds == null)\n";
                s += "{\n";
                s += $"TextAsset idsData = Resources.Load<TextAsset>(\"{resourceDir2}_\");\n";
                if (parameters.IsStringId)
                    s += "allIds = idsData.text.Split('\\t');\n";
                else
                    s += "allIds = Array.ConvertAll(idsData.text.Split('\\t'), int.Parse);\n";
                //
                s += "}\n";
                s += "return allIds;\n";
                s += "}\n";
                //s += "public static void CloseDataTable(){allIds = null;}\n";
                s += "public static void CloseDataTable(){allIds=null;_instance = null;}\n";
                //}
                //else 
                //{
                //    s += "public static void CloseDataTable(){allIds=null;_instance = null;}\n";
                //}
            }
            else
            {
                s += $"static {stringOrInt}[] allIds = null;\n";
                s += $"public static {stringOrInt}[] GetAllIds()\n";
                s += "{\n";
                s += "if (allIds == null)\n";
                s += "{\n";
                s += "allIds = I.VALUE.Keys.ToArray();\n";
                s += "}\n";
                s += "return allIds;\n";
                s += "}\n";
                s += "public static void CloseDataTable(){allIds=null;_instance = null;}\n";
            }

            //THE END
            s += "}\n";
            outputText = s;
            string dr = parameters.Path + "/" + classname + ".cs";
            if (parameters.Path == null || parameters.Path == "") dr = classname + ".cs";
            File.WriteAllText(Path.Combine(Application.dataPath, dr), s);


            //Generate all enum types
            //if (parameters.IsGenEnum)
            {
                dr = parameters.Path + "/" + class1 + ".cs";
                if (parameters.Path == null || parameters.Path == "") dr = class1 + ".cs";
                File.AppendAllText(Path.Combine(Application.dataPath, dr), GenerateEnums());
            }
            return s;
        }
        string gettext(TYPE type, int index)
        {
            switch (type)
            {
                case TYPE.LONG: return "long";
                case TYPE.DOUBLE: return "double";  
                case TYPE.FLOAT:
                    return "float";
                case TYPE.INT:
                    return "int";
                case TYPE.STRING:
                    return "string";
                case TYPE.BOOL:
                    return "bool";
            }
            if (index >= 0) return typesNames[index];
            return "flllloat";
        }

        string getStringValueByType(TYPE type, string value)
        {
            if (type == TYPE.FLOAT || type == TYPE.INT || type == TYPE.LONG || type == TYPE.DOUBLE)
            {
                return value;
            }
            else return "\"" + value + "\"";
        }

        string getStringToJSONAsType(TYPE type)
        {
            
            if (type == TYPE.INT) return ".AsInt";
            else if (type == TYPE.FLOAT) return ".AsFloat";
            else if (type == TYPE.LONG) return ".AsLong";
            else if (type == TYPE.DOUBLE) return ".AsDouble";
            else return "";
        }


        public void gen_st_json(string namefile, string class1)
        {
            StringBuilder S = new StringBuilder();
            S.Append("{\n");
            S.Append("\"list\": \n");
            S.Append("[\n");

            string current_id = "null";
            bool is_have_id = false;
            for (int j = iStar + 1; j <= size; j++)
            {
                is_have_id = false;
                current_id = "null";
                string sss = lines[j].Replace("\n", "");
                LINE = sss.Split('\t');
                StringBuilder s = new StringBuilder();
                s.Append("{\n");
                for (int i = 0; i < n; i++)
                {

                    if (ARRAY_LENGTH[i] > 0)
                    {
                        s.Append("\"" + names[i] + "\":[");
                        //s += "t." + names[i] + "= new " + gettext(types[i]) + "[" + ARRAY_LENGTH[i] + "];\n";
                        int index = 0;
                        for (int k = i; k < i + ARRAY_LENGTH[i]; k++)
                        {
                            index = k - i;
                            if (LINE[k] == null || LINE[k].Length == 0)
                            {
                                for (int k2 = k; k2 < i + ARRAY_LENGTH[i]; k2++)
                                    if (!string.IsNullOrEmpty(LINE[k2]))
                                    {
                                        EditorUtility.DisplayDialog("STExcelToClass", string.Format("WARNING: There is a empty cell at row id:{0} col:{1}", current_id, k2), "OK");
                                        break;
                                    }
                                break; // break for short array
                            }
                            //core here
                            switch (types[i])
                            {
                                case TYPE.FLOAT:
                                case TYPE.DOUBLE:
                                    if (LINE[k] == null || LINE[k].Length == 0)
                                        s.Append("0" + ",");
                                    else
                                        s.Append(LINE[k] + ",");
                                    break;
                                case TYPE.INT:
                                case TYPE.LONG:
                                    if (LINE[k] == null || LINE[k].Length == 0)
                                        s.Append("0" + ",");
                                    else
                                        s.Append(LINE[k] + ",");
                                    break;
                                case TYPE.STRING:
                                    s.Append("\"" + LINE[k] + "\",");
                                    break;
                                case TYPE.BOOL:
                                    if (LINE[k] == "1") s.Append("true" + ",");
                                    else if (LINE[k] == "0") s.Append("false" + ",");
                                    else if (LINE[k] == "true") s.Append("true" + ",");
                                    else if (LINE[k] == "false") s.Append("false" + ",");
                                    else if (LINE[k] == "TRUE") s.Append("true" + ",");
                                    else if (LINE[k] == "FALSE") s.Append("false" + ",");
                                    else if (LINE[k] == "") s.Append("false" + ",");
                                    else if (LINE[k] != "") s.Append("true" + ",");
                                    else
                                    {
                                        MessageBox.Show("Khi dùng \"is_..\" thì giá trị phải 1 hoặc 0" + ",\n");
                                    }
                                    break;
                                case TYPE.ENUM:
                                    if (LINE[i] == "") break;
                                    string enumInt = "";
                                    string[] enumVales = LINE[k].Split('=');
                                    if (enumVales.Length == 2) { LINE[k] = enumVales[0]; enumInt = enumVales[1]; }
                                    if (!typesDictionraries.ContainsKey(typesNames[i])) typesDictionraries.Add(typesNames[i], new List<string>());
                                    if (!typesDictionrariesLevel2.ContainsKey(typesNames[i])) typesDictionrariesLevel2.Add(typesNames[i], new Dictionary<string, string>());
                                    if (!typesDictionraries[typesNames[i]].Contains(LINE[k])) typesDictionraries[typesNames[i]].Add(LINE[k]);
                                    if (!string.IsNullOrEmpty(enumInt))
                                    {
                                        if (!typesDictionrariesLevel2[typesNames[i]].ContainsKey(LINE[k])) typesDictionrariesLevel2[typesNames[i]].Add(LINE[k], enumInt);
                                    }
                                    if (parameters.IsGenStringEnumValue)
                                    {
                                        s.Append("\"" + LINE[k] + "\",");
                                    }
                                    else
                                    {
                                        if (typesDictionrariesLevel2[typesNames[i]].ContainsKey(LINE[k]))
                                            s.Append(typesDictionrariesLevel2[typesNames[i]][LINE[k]] + ",");
                                        else s.Append(typesDictionraries[typesNames[i]].IndexOf(LINE[k]) + ",");
                                    }

                                    break;
                            }
                        }
                        s.Replace(",", "", s.Length - 1, 1);
                        i += ARRAY_LENGTH[i] - 1;
                        s.Append("],\n");
                    }
                    else
                    {
                        if (names[i] == "id" || names[i] == "Id")
                        { current_id = LINE[i]; is_have_id = true; }
                        switch (types[i])
                        {
                            case TYPE.FLOAT:
                                if (LINE[i] == null || LINE[i].Length == 0 || LINE[i].Equals("0") || LINE[i].Equals("0.0"))
                                {
                                    if (!parameters.IsSkipZeroValue)
                                        s.Append("\"" + names[i] + "\":" + "0" + ",\n");
                                }
                                else
                                    s.Append("\"" + names[i] + "\":" + LINE[i] + ",\n");
                                break;
                            case TYPE.INT:
                                if (LINE[i] == null || LINE[i].Length == 0 || LINE[i].Equals("0"))
                                {
                                    if (!parameters.IsSkipZeroValue)
                                        s.Append("\"" + names[i] + "\":" + "0" + ",\n");
                                }
                                else
                                    s.Append("\"" + names[i] + "\":" + LINE[i] + ",\n");
                                break;
                            case TYPE.STRING:
                                if (parameters.IsSkipZeroValue && string.IsNullOrEmpty(LINE[i]))
                                    ;
                                else s.Append("\"" + names[i] + "\":\"" + LINE[i] + "\",\n");

                                break;
                            case TYPE.BOOL:
                                if (LINE[i] == "1") s.Append("\"" + names[i] + "\":true" + ",\n");
                                else if (LINE[i] == "0" && !parameters.IsSkipZeroValue) s.Append("\"" + names[i] + "\":false" + ",\n");
                                else if (LINE[i] == "true") s.Append("\"" + names[i] + "\":true" + ",\n");
                                else if (LINE[i] == "false" && !parameters.IsSkipZeroValue) s.Append("\"" + names[i] + "\":false" + ",\n");
                                else if (LINE[i] == "TRUE") s.Append("\"" + names[i] + "\":true" + ",\n");
                                else if (LINE[i] == "FALSE" && !parameters.IsSkipZeroValue) s.Append("\"" + names[i] + "\":false" + ",\n");
                                else if (LINE[i] == "" && !parameters.IsSkipZeroValue) s.Append("\"" + names[i] + "\":false" + ",\n");
                                else if (LINE[i] != "") s.Append("\"" + names[i] + "\":true" + ",\n");
                                //else
                                //{
                                //    MessageBox.Show("Khi dùng \"is_..\" thì giá trị phải 1 hoặc 0" + ",\n");
                                //}
                                break;
                            case TYPE.ENUM:
                                if (LINE[i] == "") break;
                                string enumInt = "";
                                string[] enumVales = LINE[i].Split('=');
                                if (enumVales.Length == 2) { LINE[i] = enumVales[0]; enumInt = enumVales[1]; }
                                if (!typesDictionraries.ContainsKey(typesNames[i])) typesDictionraries.Add(typesNames[i], new List<string>());
                                if (!typesDictionrariesLevel2.ContainsKey(typesNames[i])) typesDictionrariesLevel2.Add(typesNames[i], new Dictionary<string, string>());
                                if (!typesDictionraries[typesNames[i]].Contains(LINE[i])) typesDictionraries[typesNames[i]].Add(LINE[i]);
                                if (!string.IsNullOrEmpty(enumInt))
                                {
                                    if (!typesDictionrariesLevel2[typesNames[i]].ContainsKey(LINE[i])) typesDictionrariesLevel2[typesNames[i]].Add(LINE[i], enumInt);
                                }

                                if (parameters.IsGenStringEnumValue)
                                {
                                    s.Append("\"" + names[i] + "\":\"" + LINE[i] + "\",\n");
                                }
                                else
                                {
                                    if (typesDictionrariesLevel2[typesNames[i]].ContainsKey(LINE[i]))
                                        s.Append("\"" + names[i] + "\":" + typesDictionrariesLevel2[typesNames[i]][LINE[i]] + ",\n");
                                    else s.Append("\"" + names[i] + "\":" + typesDictionraries[typesNames[i]].IndexOf(LINE[i]) + ",\n");
                                }

                                break;
                        }
                    }


                    if (i == n - 1)
                        s.Replace(",\n", "\n", s.Length - 2, 2);
                }

                s.Append("}");
                //finish a row here
                if (parameters.IsSeparatedJSON)
                {
                    string d = parameters.PathJSON + "/" + namefile + "_" + current_id + ".json";
                    if (parameters.PathJSON == null || parameters.PathJSON == "") d = namefile + ".json";
                    File.WriteAllText(Path.Combine(Application.dataPath, d), s.ToString());
                }

                if (j < size) s.Append(","); s.Append("\n");

                S.Append(s);
                if (is_have_id == false)
                {
                    MessageBox.Show("ban ko ton tai id");
                }
                if (allDataIds.Contains(current_id))
                {
                    EditorUtility.DisplayDialog("STExcelToClass", string.Format("Duplicated id value: {0}", current_id), "OK");
                }
                else allDataIds.Add(current_id);
                //if (is_string_id)
                //    s += "VALUE.Add(\"" + current_id + "\", t);\n";
                //else
                //    s += "VALUE.Add(" + current_id + ", t);\n";
            }





            S.Append("]\n");
            S.Append("}");

            if (parameters.IsGenSingleLineJSON)
            {
                S = S.Replace("\n", "");
                S = S.Replace("\"", "\\\"");
            }

            string dr = parameters.PathJSON + "/" + namefile + ".json";
            if (parameters.PathJSON == null || parameters.PathJSON == "") dr = namefile + ".json";
            File.WriteAllText(Path.Combine(Application.dataPath, dr), S.ToString());

            //generate all id into an index file
            if (parameters.IsGenJSON)
            {
                string idsText = "";
                for (int i = 0; i < allDataIds.Count; i++)
                {
                    idsText += allDataIds[i];
                    if (i < allDataIds.Count - 1) idsText += "\t";
                }
                string dr2 = parameters.PathJSON + "/" + namefile + "_.txt";
                File.WriteAllText(Path.Combine(Application.dataPath, dr2), idsText);
            }
        }

        public void gen_st_read_json(string class_data_name, string class_object_name, string json_file_name, string json_folder, string field_id)
        {
            StringBuilder s = new StringBuilder();
            s.Append("using UnityEngine;\n");
            s.Append("using System.Collections;\n");
            s.Append("using System.Collections.Generic;\n");
            s.Append("public class " + class_data_name + "  \n{\n");

            s.Append("\tpublic static string PATH_" + json_file_name + " = \"" + json_folder + json_file_name + "\";\n");
            s.Append("\tpublic static Dictionary<int," + class_object_name + "> lst" + class_object_name + " = new Dictionary<int," + class_object_name + ">();\n");

            s.Append("\t#region Load" + class_object_name + "\n");

            s.Append("\tpublic static void LoadData" + class_object_name + "FromJSON(string path){\n");
            s.Append("\t\tTextAsset bindata = Resources.Load(path) as TextAsset;\n");
            s.Append("\t\tJSONNode data = JSON.Parse(bindata.text);\n");
            s.Append("\t\tJSONArray list = data[\"list\"].AsArray;\n");
            s.Append("\t\tfor (int i = 0; i < list.Count; i++){\n");
            s.Append("\t\t\tJSONNode node" + class_object_name + " = list[i];\n");
            s.Append("\t\t\t" + class_object_name + " obj = new " + class_object_name + "();\n");

            for (int j = 0; j < n; j++)
            {
                s.Append("\t\t\tobj." + names[j] + " = node" + class_object_name + "[\"" + names[j] + "\"]" + getStringToJSONAsType(types[j]) + ";\n");

            }
            s.Append("\t\t\tlst" + class_object_name + ".Add(obj.id,obj);\n\t\t}\n\t}\n");


            s.Append("\tpublic static " + class_object_name + " get" + class_object_name.Replace(".", "") + "ByID(int " + field_id + "){\n");
            //s.Append("\t\tfor (int i = 0; i < lst" + class_object_name + ".Count; i++){\n");
            //s.Append("\t\t\tif(lst" + class_object_name + "[i]." + field_id + " == " + field_id + ") return lst" + class_object_name + "[i];\n}");
            //s.Append("\t\t\treturn null;\n\t}\n");
            s.Append("return lstst_hero[id];\n}");
            s.Append("\t\n#endregion\n");

            s.Append("\tstatic " + class_data_name + "(){\n");
            s.Append("\t\tLoadData" + class_object_name + "FromJSON(PATH_" + json_file_name + ");\n");
            s.Append("\t}\n");

            s.Append("}");

            string dr = parameters.Path + "/" + class_data_name + ".cs";
            if (parameters.Path == null || parameters.Path == "") dr = class_data_name + ".cs";
            File.WriteAllText(Path.Combine(Application.dataPath, dr), s.ToString());
        }

        


        private static class MessageBox
        {
            public static void Show(string message)
            {
                EditorUtility.DisplayDialog("", message, "ok");
            }
        }

        string GenerateEnums()
        {
            string s = "\n";
            foreach (KeyValuePair<string, List<string>> item in typesDictionraries)
            {

                if (typesSkipedForGenerate.Contains(item.Key)) continue;
                s += "public enum " + item.Key + "\n{\n";
                for (int i = 0; i < item.Value.Count; i++)
                {
                    if (string.IsNullOrEmpty(item.Value[i])) continue;

                    s += item.Value[i];
                    if (typesDictionrariesLevel2.ContainsKey(item.Key) && typesDictionrariesLevel2[item.Key].ContainsKey(item.Value[i]))
                    {
                        s += "=" + typesDictionrariesLevel2[item.Key][item.Value[i]];
                    }
                    if (i < item.Value.Count - 1) s += ",\n";
                }
                s += "\n}\n";
            }
            return s;
        }
        string ExtractEnumNameAndProperties(string s, bool isAddToSkipList)
        {
            int i = s.IndexOf("{");
            int j = s.IndexOf("}");

            string enumName = i >= 0 ? s.Substring(0, i) : s;
            if (j > i)
            {
                if (j != s.Length - 1) Debug.LogError("Invalid format; please use \"EnumName{false}\"");

                //Debug.Log(i + " " + j + " " + (j - i - 1));
                string val = s.Substring(i + 1, j - i - 1);
                s = s.Substring(0, i);
                if (isAddToSkipList && val.Equals("") || val.Equals("0") || val.Equals("FALSE") || val.Equals("False") || val.Equals("false"))
                {
                    if (!typesSkipedForGenerate.Contains(s))
                        typesSkipedForGenerate.Add(s);
                }
                else if (!(val.Equals("1") || val.Equals("TRUE") || val.Equals("True") || val.Equals("true")))
                {

                    string[] values = val.Split(',');
                    if (typesDictionraries == null) typesDictionraries = new Dictionary<string, List<string>>();
                    if (typesDictionrariesLevel2 == null) typesDictionrariesLevel2 = new Dictionary<string, Dictionary<string, string>>();

                    if (!typesDictionraries.ContainsKey(enumName)) typesDictionraries.Add(enumName, new List<string>());
                    if (!typesDictionrariesLevel2.ContainsKey(enumName)) typesDictionrariesLevel2.Add(enumName, new Dictionary<string, string>());
                    for (int iv = 0; iv < values.Length; iv++)
                    {
                        string idString = values[iv];
                        string valString = iv.ToString();
                        if (idString.Split('=').Length == 2)
                        {
                            valString = idString.Split('=')[1];
                            idString = idString.Split('=')[0];

                            if (!typesDictionrariesLevel2[enumName].ContainsKey(idString))
                                typesDictionrariesLevel2[enumName].Add(idString, valString);
                        }

                        if (!typesDictionraries[enumName].Contains(idString))
                            typesDictionraries[enumName].Add(idString);

                    }
                }
            }
            else if (j <= i && i >= 0) Debug.LogError("Invalid format; please use \"EnumName{false}\"");
            return s;
        }
    }


    enum TYPE
    {
        INT = 0,
        FLOAT = 1,
        STRING = 2,
        BOOL = 3,
        ENUM = 4,
        LONG=5,
        DOUBLE=6,
        NOT_DECICED_YET = 10
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
}
#endif