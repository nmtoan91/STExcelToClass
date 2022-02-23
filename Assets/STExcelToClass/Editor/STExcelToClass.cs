//Author : toanstt 
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
        string textBox_floatdef;
        string textBox_intdef;
        bool is_string_id = false;

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

            
            GUILayout.Label("Source text from Excel", EditorStyles.boldLabel);
            textBox1 = EditorGUILayout.TextArea(textBox1);

            GUILayout.Label("Output Directory", EditorStyles.boldLabel);
            //textBox3_dir = EditorGUILayout.TextField("", textBox3_dir);
            GUILayout.Label(textBox3_dir, EditorStyles.boldLabel);

            GUILayout.Label("Type Definition", EditorStyles.boldLabel);
            textBox_floatdef = EditorGUILayout.TextField("Float definition", textBox_floatdef);
            textBox_intdef = EditorGUILayout.TextField("Int Definition", textBox_intdef);

            is_string_id = EditorGUILayout.Toggle("Is String Id", is_string_id);

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
        }
        [Multiline]
        public string textBox1;
        public string outputText;
        public string textBox3_dir;

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
        string str_json_name_file;
        string str_json_path_folder;

        int iStar = 1;

        int n;//so cot
        int size;//so dong
        string str;
        string[] lines;
        string[] LINE;
        TYPE[] types;
        private void button1_Click(object sender, EventArgs e)
        {
            
            JSONCLASS();

            Debug.Log("Files are generated in " + textBox3_dir);
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
            gen_st_json(str_json_name_file, str_name_class_object);
            Gen_st_hero_table(str_name_class_data, str_name_class_object, false);

            Debug.Log("Files are generated in " + textBox3_dir);
            //textBox1 = "";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            InitData();
            ReadNames();




            TryToParseArray();
            LoadFirstData();
            Gen_st_hero(str_name_class_object);
            gen_st_json(str_json_name_file, str_name_class_object);
            Gen_st_hero_table(str_name_class_data, str_name_class_object);
            //textBox1 = "";

            Debug.Log("Files are generated in " + textBox3_dir);
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
            str_json_name_file = lines[0].Split('\t')[2];
            str_json_path_folder = lines[0].Split('\t')[3];

            trimames();

            str_json_path_folder = str_json_path_folder.Replace('\\', '/');

             textBox3_dir = str_json_path_folder;
            if(textBox3_dir[textBox3_dir.Length-1] == '/') textBox3_dir = textBox3_dir.Substring(0, textBox3_dir.Length - 1);
            if (!string.IsNullOrEmpty(textBox3_dir))
            {
                //Debug.Log(textBox3_dir);
                //Debug.Log(Path.GetDirectoryName(textBox3_dir));
                //Directory.CreateDirectory(Path.GetDirectoryName(textBox3_dir));

                string[] subFolders = textBox3_dir.Split('/');
                string root = "Assets";
                foreach (string subFolder in subFolders)
                {
                    if (!AssetDatabase.IsValidFolder(root +"/"+ subFolder))
                    {
                        string guid = AssetDatabase.CreateFolder(root, subFolder);
                    }
                    root += "/"+subFolder ;
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
            }

        }
        public string Gen_st_hero(string classname)
        {
            string s = "//Author: toanstt \n//This file is generated, do not edit!\n";
            s += "using UnityEngine;\n";
            s += "using System.Collections;\n";
            s += "using System.Collections.Generic;\n";
            s += "[System.Serializable]";
            s += "public class " + classname + "  \n{\n";
            //define variables here
            //s += "private static " + classname + " _instance;";
            //define more variable


            ////for (int i = 0; i < n; i++)
            ////{
            ////    s += "public " + gettext(types[i]) + " " + names[i] + ";\n";
            ////}
            for (int i = 0; i < n; i++)
            {
                if (ARRAY_LENGTH[i] > 0)
                {
                    s += "public " + gettext(types[i]) + "[] " + names[i] + ";// lenght=" + ARRAY_LENGTH[i] + "\n";
                    i += ARRAY_LENGTH[i] - 1;
                }
                else
                    s += "public " + gettext(types[i]) + " " + names[i] + ";\n";
            }


            //function

            //end
            s += "}\n";
            outputText = s;
            //MessageBox.Show(s);
            string dr = textBox3_dir + "/" + classname + ".cs";
            if (textBox3_dir == null || textBox3_dir == "") dr = classname + ".cs";
            //MessageBox.Show(Path.Combine(Application.dataPath, dr));
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
            s += "public class " + classname + "  \n{\n";

            //s += "public class " + classname + "  \n{\n";



            //singleton
            s += "private static " + classname + " _instance;\n";

            //new for JSON parser
            //s += "public List<" + class1 + "> list;\n";
            s += "public " + class1 + "[] list;\n";
            //end new for JSON parser

            if (is_string_id)
                s += "public Dictionary<string, " + class1 + "> VALUE;\n";
            else
                s += "public Dictionary<int, " + class1 + "> VALUE;\n";

            s += "public " + classname + "()\n";
            s += "{\n";

            if (is_string_id)
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

            if (is_string_id)
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
            s += "" + class1 + " t;";





            string current_id = "null";
            bool is_have_id = false;
            if (is_gen_data == true)
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
                            s += "t." + names[i] + "= new " + gettext(types[i]) + "[]{";
                            int index = 0;
                            for (int k = i; k < i + ARRAY_LENGTH[i]; k++)
                            {
                                index = k - i;
                                if (LINE[k] == null || LINE[k].Length == 0)
                                    break; // break for short array
                                switch (types[i])
                                {
                                    case TYPE.FLOAT:
                                        if (LINE[k] == null || LINE[k].Length == 0)
                                            s += textBox_floatdef + "f,";
                                        else
                                            s += LINE[k] + "f,";
                                        break;
                                    case TYPE.INT:
                                        if (LINE[k] == null || LINE[k].Length == 0)
                                            s += textBox_intdef + ",";
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
                                }
                            }
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
                                        s += "t." + names[i] + "=" + textBox_floatdef + "f;\n";
                                    else
                                        s += "t." + names[i] + "=" + LINE[i] + "f;\n";
                                    break;
                                case TYPE.INT:
                                    if (LINE[i] == null || LINE[i].Length == 0)
                                        s += "t." + names[i] + "=" + textBox_intdef + ";\n";
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
                            }
                        }
                    }

                    if (is_have_id == false)
                    {
                        MessageBox.Show("ban ko ton tai id");
                    }

                    if (is_string_id)
                        s += "VALUE.Add(\"" + current_id + "\", t);\n";
                    else
                        s += "VALUE.Add(" + current_id + ", t);\n";
                }
            s += "}\n";
            //function


            if (is_string_id)
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
            string dr = textBox3_dir + "/" + classname + ".cs";
            if (textBox3_dir == null || textBox3_dir == "") dr = classname + ".cs";
            File.WriteAllText(Path.Combine(Application.dataPath, dr), s);

            return s;
        }
        public string gettext(TYPE type)
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
            return "flllloat";
        }

        public string getStringValueByType(TYPE type, string value)
        {
            if (type == TYPE.FLOAT || type == TYPE.INT)
            {
                return value;
            }
            else return "\"" + value + "\"";
        }

        public string getStringToJSONAsType(TYPE type)
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
                                        s.Append(textBox_floatdef + ",");
                                    else
                                        s.Append(LINE[k] + ",");
                                    break;
                                case TYPE.INT:
                                    if (LINE[k] == null || LINE[k].Length == 0)
                                        s.Append(textBox_intdef + ",");
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
                                    s.Append("\"" + names[i] + "\":" + textBox_floatdef + ",\n");
                                else
                                    s.Append("\"" + names[i] + "\":" + LINE[i] + ",\n");
                                break;
                            case TYPE.INT:
                                if (LINE[i] == null || LINE[i].Length == 0)
                                    s.Append("\"" + names[i] + "\":" + textBox_intdef + ",\n");
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

            string dr = textBox3_dir + "/" + namefile + ".json";
            if (textBox3_dir == null || textBox3_dir == "") dr = namefile + ".json";
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

            string dr = textBox3_dir + "/" + class_data_name + ".cs";
            if (textBox3_dir == null || textBox3_dir == "") dr = class_data_name + ".cs";
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

    }

    public enum TYPE
    {
        INT = 0,
        FLOAT = 1,
        STRING = 2,
        BOOL = 3
    }
}
#endif