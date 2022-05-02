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
            //GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            //myString = EditorGUILayout.TextField("Text Field", textBox1);

            //UnityEngine.Object file = Selection.activeObject;
            //string rootPath;
            //rootPath = AssetDatabase.GetAssetPath(file);
            //string fullPath = Path.GetFullPath(AssetDatabase.GetAssetPath(file));




            //GUILayout.Label("Type Definition", EditorStyles.boldLabel);
            //textBox_floatdef = EditorGUILayout.TextField("Float definition", textBox_floatdef);
            //textBox_intdef = EditorGUILayout.TextField("Int Definition", textBox_intdef);

            //is_string_id = EditorGUILayout.Toggle("Is String Id", is_string_id);
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Export Class Only"))
            {
                button1_Click(null, null);

                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Export Class With JSon"))
            {
                button2_Click(null, null);
                AssetDatabase.Refresh();
            }
            GUILayout.EndHorizontal();

            GUILayout.Label("Source text from Excel", EditorStyles.boldLabel);
            textBox1 = EditorGUILayout.TextArea(textBox1);
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("Output Directory: ", EditorStyles.boldLabel);
            //textBox3_dir = EditorGUILayout.TextField("", textBox3_dir);
            GUILayout.Label(parameters.Path, EditorStyles.boldLabel);
            GUILayout.EndHorizontal();
            GUILayout.Label("------------------------STGAME---------------------------", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Insert sample data"))
            {
                string s = "st_level\tst_levelTable\t{\"IsStringId\":false,\"IsGenItemClass\":true,\"JSONName\":\"st_levelJSON\"}\n";
                s += "id\trow\tcol\tis_boss\tmyarray0\tmyarray1\tmyarray2\tstring:testforcestring\tteststring\tarray0\tarray1\n";
                s += "0\t4\t4\tTRUE\t23\t1\t1\t2.5\tasd\tstring1\tstrings2\n";
                s += "1\t\t4\tfalse\t2\t3\t\t\tasd\tstring2\tstrings3\n";
                s += "2\t\t4\t0\t2\t3\t\t\tasd\tstring3\tstrings4\n";
                s += "3\t4\t4\t0\t2\t3\t\t2.4\tdf\tstring4\tstrings5\n";
                s += "4\t4\t\t1\t2\t\t\t1.2\tdsf\tstring5\tstrings6\n";
                textBox1 = s;
                AssetDatabase.Refresh();
            }
            if (GUILayout.Button("Show all configs"))
            {
                string s =
                     "IsStringId            : Force id to string type (default=false)\n";
                s += "IsGenItemClass : Skip generate item class; generate proto file instead (default=true)\n";
                s += "JSONName        : Json filename (default=toanstt)\n";
                s += "DefaultFloat      : Default value of float (default=0)\n";
                s += "DefaultInt          : Default value of int (default=0)\n";
                s += "type:varname   : Force set variable type, type = {int,float,string,bool,enumName} \n";
                s += "IsGenEnum       : Is Generate all enums (default=true) \n";
                s += "Path                   : Path to save the dataset (default=STGAME/Data)\n";

                textBox1 = s;
                AssetDatabase.Refresh();
            }
            GUILayout.EndHorizontal();

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
        TYPE[] types;
        string[] typesNames;
        Dictionary<string, List<string>> typesDictionraries;
        Dictionary<string, Dictionary<string, string>> typesDictionrariesLevel2;


        string[] protoKeyworks = { "int32", "float", "string", "bool" };
        private void button1_Click(object sender, EventArgs e)
        {

            JSONCLASS();

            Debug.Log("Files are generated in " + parameters.Path);
        }
        public void JSONCLASS()
        {
            InitData();
            ReadNames();
            TryToParseArray();
            LoadFirstData();
            Gen_st_hero(str_name_class_object);
            //gen_st_json(str_json_name_file, str_name_class_object);
            Gen_st_hero_table(str_name_class_data, str_name_class_object);
            //textBox1 = "";

        }
        private void button2_Click(object sender, EventArgs e)
        {
            InitData();
            ReadNames();
            TryToParseArray();
            LoadFirstData();
            Gen_st_hero(str_name_class_object);
            gen_st_json(parameters.JSONName, str_name_class_object);
            Gen_st_hero_table(str_name_class_data, str_name_class_object, false);

            Debug.Log("Files are generated in " + parameters.Path);
            //textBox1 = "";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            InitData();
            ReadNames();

            TryToParseArray();
            LoadFirstData();
            Gen_st_hero(str_name_class_object);
            gen_st_json(parameters.JSONName, str_name_class_object);
            Gen_st_hero_table(str_name_class_data, str_name_class_object);
            //textBox1 = "";

            Debug.Log("Files are generated in " + parameters.Path);
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
        public void ReadNames()
        {
            //MessageBox.Show(String.Join("*", lines));
            LINE = lines[iStar].Split('\t');
            n = LINE.Length;

            for (int i = 0; i < LINE.Length; i++)
            {
                names.Add(LINE[i]);
            }

            str_name_class_object = lines[0].Split('\t')[0];
            str_name_class_data = lines[0].Split('\t')[1];
            string str_json_name_file2 = lines[0].Split('\t')[2];
            //parameters = JsonUtility.FromJson(str_json_name_file, STExcelParameters);
            try
            {
                parameters = JsonUtility.FromJson<STExcelParameters>(str_json_name_file2);
                //Debug.Log(parameters.IsStringId);
            }
            catch (Exception ex)
            {
                Debug.Log("Warning: Cannot parse json data, please use this format at cell (C:1): {\"IsStringId\":false,\"IsGenItemClass\":false,\"JSONName\":\"stLevelJSON\"}");
                parameters.JSONName = str_json_name_file2;
            }


            Debug.Log("parameters.Path:" + parameters.Path);
            if (lines[0].Split('\t').Length >= 4 && !string.IsNullOrEmpty(lines[0].Split('\t')[3]))
                parameters.Path = lines[0].Split('\t')[3];
            Debug.Log("parameters.Path:" + parameters.Path);
            str_json_path_folder = parameters.Path;
            trimames();

            str_json_path_folder = str_json_path_folder.Replace('\\', '/');

            string textBox3_dir = str_json_path_folder;
            Debug.Log("creating folder: " + textBox3_dir);
            if (textBox3_dir[textBox3_dir.Length - 1] == '/') textBox3_dir = textBox3_dir.Substring(0, textBox3_dir.Length - 1);
            if (!string.IsNullOrEmpty(textBox3_dir))
            {
                //Debug.Log(textBox3_dir);
                //Debug.Log(Path.GetDirectoryName(textBox3_dir));
                //Directory.CreateDirectory(Path.GetDirectoryName(textBox3_dir));

                string[] subFolders = textBox3_dir.Split('/');
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
        }

        int[] ARRAY_LENGTH;
        int[] MY_ARRAY_INDEX;
        int[] MY_INDEX_IN_MY_ARRAY;
        public void trimames()
        {
            if (names[names.Count - 1] == "")
            {
                names.RemoveAt(names.Count - 1);
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
            types = new TYPE[n];
            typesNames = new string[n];
            typesDictionraries = new Dictionary<string, List<string>>();
            typesDictionrariesLevel2 = new Dictionary<string, Dictionary<string, string>>();
            for (int i = 0; i < n; i++)
            {
                try
                {
                    int.Parse(LINE[i]);

                    types[i] = TYPE.INT;
                }
                catch (Exception e)
                {
                    try
                    {
                        float.Parse(LINE[i]);
                        types[i] = TYPE.FLOAT;
                    }
                    catch (Exception e1)
                    {
                        types[i] = TYPE.STRING;
                    }
                }
                if (names[i].IndexOf("is_") == 0) { types[i] = TYPE.BOOL; }
                if (names[i].IndexOf("can_") == 0) { types[i] = TYPE.BOOL; }
                if (names[i].IndexOf("Is") == 0) { types[i] = TYPE.BOOL; }
                if (names[i].IndexOf("Can") == 0) { types[i] = TYPE.BOOL; }

                string[] separateLabel = names[i].Split(':');
                if (separateLabel.Length >= 2)
                {
                    if (separateLabel.Length == 3) // put a dummy variableName on top
                    {
                        separateLabel[0] = separateLabel[1];
                        separateLabel[1] = separateLabel[2];
                    }

                    names[i] = separateLabel[1];
                    if (separateLabel[0].Equals("int") || separateLabel[1].Equals("Int")) types[i] = TYPE.INT;
                    else if (separateLabel[0].Equals("float") || separateLabel[0].Equals("Float")) types[i] = TYPE.FLOAT;
                    else if (separateLabel[0].Equals("bool") || separateLabel[0].Equals("Bool") || separateLabel[0].Equals("Boolean")) types[i] = TYPE.BOOL;
                    else if (separateLabel[0].Equals("string") || separateLabel[0].Equals("String")) types[i] = TYPE.STRING;
                    else
                    {
                        types[i] = TYPE.ENUM;
                        typesNames[i] = separateLabel[0];
                    }
                }

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



            if (is_gen_data == false)
            {
                s += "[System.Serializable] public class " + classname + "ListJSON{ public " + class1 + "[] list;}\n\n";
            }


            s += "public class " + classname + "  \n{\n";

            //s += "public class " + classname + "  \n{\n";



            //singleton
            s += "private static " + classname + " _instance;\n";

            //new for JSON parser
            //s += "public List<" + class1 + "> list;\n";
            s += "public " + class1 + "[] list;\n";
            //end new for JSON parser

            if (parameters.IsStringId)
                s += "public Dictionary<string, " + class1 + "> VALUE;\n";
            else
                s += "public Dictionary<int, " + class1 + "> VALUE;\n";

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
            s += "\t           _instance.load();\n";
            s += "\t       }\n";
            s += "\t       return _instance;\n";
            s += "\t}\n";
            s += "}\n";

            //Get

            if (parameters.IsStringId)
            {
                s += "public " + class1 + " Get(string id)\n";
                s += "{\n";
                s += "return VALUE[id];\n";
                s += "}\n";
            }
            else
            {
                s += "public " + class1 + " Get(int id)\n";
                s += "{\n";
                s += "return VALUE[id];\n";
                s += "}\n";
            }

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
                            s += "t." + names[i] + "= new " + gettext(types[i], i) + "[]{";
                            int index = 0; int k;
                            for (k = i; k < i + ARRAY_LENGTH[i]; k++)
                            {
                                index = k - i;
                                if (LINE[k] == null || LINE[k].Length == 0)
                                    break; // break for short array
                                switch (types[i])
                                {
                                    case TYPE.FLOAT:
                                        if (LINE[k] == null || LINE[k].Length == 0)
                                            s += parameters.DefaultFloat + "f,";
                                        else
                                            s += LINE[k] + "f,";
                                        break;
                                    case TYPE.INT:
                                        if (LINE[k] == null || LINE[k].Length == 0)
                                            s += parameters.DefaultInt + ",";
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
                                        string enumInt = "";
                                        string[] enumVales = LINE[k].Split(':');
                                        if (enumVales.Length == 2) { LINE[k] = enumVales[0]; enumInt = enumVales[1]; }
                                        s += typesNames[i] + "." + LINE[k] + ",";
                                        if (!string.IsNullOrEmpty(typesNames[i]))
                                        {
                                            if (!typesDictionraries.ContainsKey(typesNames[i])) typesDictionraries.Add(typesNames[i], new List<string>());
                                            if (!typesDictionrariesLevel2.ContainsKey(typesNames[i])) typesDictionrariesLevel2.Add(typesNames[i], new Dictionary<string, string>());
                                            if (!typesDictionraries[typesNames[i]].Contains(LINE[k])) typesDictionraries[typesNames[i]].Add(LINE[k]);
                                            if (!string.IsNullOrEmpty(enumInt))
                                            {
                                                if (!typesDictionrariesLevel2[typesNames[i]].ContainsKey(LINE[k])) typesDictionrariesLevel2[typesNames[i]].Add(LINE[k], enumInt);
                                            }
                                        }
                                        break;

                                }
                            }
                            if (k > i)
                                s = s.Substring(0, s.Length - 1);
                            s += "};\n";
                            i += ARRAY_LENGTH[i] - 1;
                        }
                        else
                        {
                            if (names[i] == "id")
                            { current_id = LINE[i]; is_have_id = true; }
                            switch (types[i])
                            {
                                case TYPE.FLOAT:
                                    if (LINE[i] == null || LINE[i].Length == 0)
                                        s += "t." + names[i] + "=" + parameters.DefaultFloat + "f;\n";
                                    else
                                        s += "t." + names[i] + "=" + LINE[i] + "f;\n";
                                    break;
                                case TYPE.INT:
                                    if (LINE[i] == null || LINE[i].Length == 0)
                                        s += "t." + names[i] + "=" + parameters.DefaultInt + ";\n";
                                    else
                                        s += "t." + names[i] + "=" + LINE[i] + ";\n";
                                    break;
                                case TYPE.STRING:
                                    s += "t." + names[i] + "=\"" + LINE[i] + "\";\n";
                                    break;
                                case TYPE.BOOL:
                                    if (LINE[i] == "1") s += "t." + names[i] + "=true;\n";
                                    else if (LINE[i] == "0") s += "t." + names[i] + "=false;\n";
                                    else if (LINE[i] == "true") s += "t." + names[i] + "=true;\n";
                                    else if (LINE[i] == "false") s += "t." + names[i] + "=false;\n";
                                    else if (LINE[i] == "TRUE") s += "t." + names[i] + "=true;\n";
                                    else if (LINE[i] == "FALSE") s += "t." + names[i] + "=false;\n";
                                    else if (LINE[i] == "") s += "t." + names[i] + "=false;\n";
                                    else if (LINE[i] != "") s += "t." + names[i] + "=true;\n";
                                    else
                                    {
                                        MessageBox.Show("Khi dùng \"is_..\" thì giá trị phải 1 hoặc 0");

                                    }
                                    break;
                                case TYPE.ENUM:
                                    string enumInt = "";
                                    if (LINE[i] == null || LINE[i].Length == 0) ;
                                    else
                                    {
                                        string[] enumVales = LINE[i].Split(':');
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

                string resourceDir = parameters.Path.Replace("Resources/", "");
                resourceDir = resourceDir.Replace("Resources/", "");
                resourceDir += "/" + parameters.JSONName;
                resourceDir = resourceDir.Replace(".json", "");
                s += "TextAsset jsonData = Resources.Load<TextAsset>(\"" + resourceDir + "\");\n";
                s += classname + "ListJSON lmyist = JsonUtility.FromJson<" + classname + "ListJSON> (jsonData.text);\n";
                s += "foreach(" + class1 + " i in lmyist.list) { VALUE.Add(i.id, i); }\n";

            }
            s += "}\n";
            //function


            if (parameters.IsStringId)
            {
                s += "public static " + class1 + " get" + class1 + "ByID(string id)";
                s += "{";
                s += "if(!I.VALUE.ContainsKey(id)) return null;";
                s += "return I.VALUE[id];";
                s += "}";
            }
            else
            {
                s += "public static " + class1 + " get" + class1 + "ByID(int id)";
                s += "{";
                s += "if(!I.VALUE.ContainsKey(id)) return null;";
                s += "return I.VALUE[id];";
                s += "}";
            }



            //end
            s += "}\n";
            outputText = s;
            string dr = parameters.Path + "/" + classname + ".cs";
            if (parameters.Path == null || parameters.Path == "") dr = classname + ".cs";
            File.WriteAllText(Path.Combine(Application.dataPath, dr), s);


            //Generate all enum types
            if (parameters.IsGenEnum)
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
                case TYPE.FLOAT:
                    return "float";
                    break;
                case TYPE.INT:
                    return "int";
                    break;
                case TYPE.STRING:
                    return "string";
                    break;
                case TYPE.BOOL:
                    return "bool";
                    break;
            }
            if (index >= 0) return typesNames[index];
            return "flllloat";
        }

        string getStringValueByType(TYPE type, string value)
        {
            if (type == TYPE.FLOAT || type == TYPE.INT)
            {
                return value;
            }
            else return "\"" + value + "\"";
        }

        string getStringToJSONAsType(TYPE type)
        {
            if (type == TYPE.INT) return ".AsInt";
            else if (type == TYPE.FLOAT) return ".AsFloat";
            else return "";
        }


        public void gen_st_json(string namefile, string class1)
        {
            StringBuilder s = new StringBuilder();
            s.Append("{\n");
            s.Append("\"list\": \n");
            s.Append("[\n");

            string current_id = "null";
            bool is_have_id = false;
            for (int j = iStar + 1; j <= size; j++)
            {
                is_have_id = false;
                current_id = "null";
                string sss = lines[j].Replace("\n", "");
                LINE = sss.Split('\t');
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
                                break; // break for short array
                            //core here
                            switch (types[k])
                            {
                                case TYPE.FLOAT:
                                    if (LINE[k] == null || LINE[k].Length == 0)
                                        s.Append(parameters.DefaultFloat + ",");
                                    else
                                        s.Append(LINE[k] + ",");
                                    break;
                                case TYPE.INT:
                                    if (LINE[k] == null || LINE[k].Length == 0)
                                        s.Append(parameters.DefaultInt + ",");
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
                                    string enumInt = "";
                                    string[] enumVales = LINE[k].Split(':');
                                    if (enumVales.Length == 2) { LINE[k] = enumVales[0]; enumInt = enumVales[1]; }
                                    if (!typesDictionraries.ContainsKey(typesNames[i])) typesDictionraries.Add(typesNames[i], new List<string>());
                                    if (!typesDictionrariesLevel2.ContainsKey(typesNames[i])) typesDictionrariesLevel2.Add(typesNames[i], new Dictionary<string, string>());
                                    if (!typesDictionraries[typesNames[i]].Contains(LINE[k])) typesDictionraries[typesNames[i]].Add(LINE[k]);
                                    if (!string.IsNullOrEmpty(enumInt))
                                    {
                                        if (!typesDictionrariesLevel2[typesNames[i]].ContainsKey(LINE[k])) typesDictionrariesLevel2[typesNames[i]].Add(LINE[k], enumInt);
                                    }
                                    if (typesDictionrariesLevel2[typesNames[i]].ContainsKey(LINE[k]))
                                        s.Append(typesDictionrariesLevel2[typesNames[i]][LINE[k]] + ",");
                                    else s.Append(typesDictionraries[typesNames[i]].IndexOf(LINE[k]) + ",");

                                    break;
                            }
                        }
                        s.Replace(",", "", s.Length - 1, 1);
                        i += ARRAY_LENGTH[i] - 1;
                        s.Append("],\n");
                    }
                    else
                    {
                        if (names[i] == "id")
                        { current_id = LINE[i]; is_have_id = true; }
                        switch (types[i])
                        {
                            case TYPE.FLOAT:
                                if (LINE[i] == null || LINE[i].Length == 0)
                                    s.Append("\"" + names[i] + "\":" + parameters.DefaultFloat + ",\n");
                                else
                                    s.Append("\"" + names[i] + "\":" + LINE[i] + ",\n");
                                break;
                            case TYPE.INT:
                                if (LINE[i] == null || LINE[i].Length == 0)
                                    s.Append("\"" + names[i] + "\":" + parameters.DefaultInt + ",\n");
                                else
                                    s.Append("\"" + names[i] + "\":" + LINE[i] + ",\n");
                                break;
                            case TYPE.STRING:
                                s.Append("\"" + names[i] + "\":\"" + LINE[i] + "\",\n");
                                break;
                            case TYPE.BOOL:
                                if (LINE[i] == "1") s.Append("\"" + names[i] + "\":true" + ",\n");
                                else if (LINE[i] == "0") s.Append("\"" + names[i] + "\":false" + ",\n");
                                else if (LINE[i] == "true") s.Append("\"" + names[i] + "\":true" + ",\n");
                                else if (LINE[i] == "false") s.Append("\"" + names[i] + "\":false" + ",\n");
                                else if (LINE[i] == "TRUE") s.Append("\"" + names[i] + "\":true" + ",\n");
                                else if (LINE[i] == "FALSE") s.Append("\"" + names[i] + "\":false" + ",\n");
                                else if (LINE[i] == "") s.Append("\"" + names[i] + "\":false" + ",\n");
                                else if (LINE[i] != "") s.Append("\"" + names[i] + "\":true" + ",\n");
                                else
                                {
                                    MessageBox.Show("Khi dùng \"is_..\" thì giá trị phải 1 hoặc 0" + ",\n");
                                }
                                break;
                            case TYPE.ENUM:
                                string enumInt = "";
                                string[] enumVales = LINE[i].Split(':');
                                if (enumVales.Length == 2) { LINE[i] = enumVales[0]; enumInt = enumVales[1]; }
                                if (!typesDictionraries.ContainsKey(typesNames[i])) typesDictionraries.Add(typesNames[i], new List<string>());
                                if (!typesDictionrariesLevel2.ContainsKey(typesNames[i])) typesDictionrariesLevel2.Add(typesNames[i], new Dictionary<string, string>());
                                if (!typesDictionraries[typesNames[i]].Contains(LINE[i])) typesDictionraries[typesNames[i]].Add(LINE[i]);
                                if (!string.IsNullOrEmpty(enumInt))
                                {
                                    if (!typesDictionrariesLevel2[typesNames[i]].ContainsKey(LINE[i])) typesDictionrariesLevel2[typesNames[i]].Add(LINE[i], enumInt);
                                }
                                if (typesDictionrariesLevel2[typesNames[i]].ContainsKey(LINE[i]))
                                    s.Append("\"" + names[i] + "\":" + typesDictionrariesLevel2[typesNames[i]][LINE[i]] + ",\n");
                                else s.Append("\"" + names[i] + "\":" + typesDictionraries[typesNames[i]].IndexOf(LINE[i]) + ",\n");
                                break;
                        }
                    }

                    if (i == n - 1)
                        s.Replace(",\n", "\n", s.Length - 2, 2);
                }
                if (j == size) s.Append("}\n"); else s.Append("},\n");
                if (is_have_id == false)
                {
                    MessageBox.Show("ban ko ton tai id");
                }

                //if (is_string_id)
                //    s += "VALUE.Add(\"" + current_id + "\", t);\n";
                //else
                //    s += "VALUE.Add(" + current_id + ", t);\n";
            }





            s.Append("]\n");
            s.Append("}");

            string dr = parameters.Path + "/" + namefile + ".json";
            if (parameters.Path == null || parameters.Path == "") dr = namefile + ".json";
            File.WriteAllText(Path.Combine(Application.dataPath, dr), s.ToString());
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


            s.Append("\tpublic static " + class_object_name + " get" + class_object_name + "ByID(int " + field_id + "){\n");
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

        private void Form1_Load(object sender, EventArgs e)
        {

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
                s += "public enum " + item.Key + "\n{\n";
                for (int i = 0; i < item.Value.Count; i++)
                {
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
    }

    enum TYPE
    {
        INT = 0,
        FLOAT = 1,
        STRING = 2,
        BOOL = 3,
        ENUM = 4
    }


    [Serializable]
    class STExcelParameters
    {
        public bool IsStringId = false;
        public bool IsGenItemClass = true;
        public string JSONName = "toanstt";
        public float DefaultFloat = 0;
        public int DefaultInt = 0;
        public bool IsGenEnum = true;
        public string Path = "STGAME/Data";
    }
}
#endif