#if UNITY_EDITOR
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonToScriptableConverter : EditorWindow
{
    private string jsonFilePaht = "";                    //JSON파일경로 문자열값
    private string outputFolder = "Assets/ScriptableObjects/Items";     //출력 SO 파일 경로 값
    private bool createDatabase = true;                     //데이터 베이스 활용 여부 체크값

    [MenuItem("Tools/JSON to Scriptable Objects")]
    public static void ShowWindow()
    {
        GetWindow<JsonToScriptableConverter>("JSON to Scriptable Objects");
    }

    void OnGUI()
    {
        GUILayout.Label("JSON to Scriptable object Converter", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if(GUILayout.Button("Select JSON File"))
        {
            jsonFilePaht = EditorUtility.OpenFilePanel("Select Json File", "", "json");
        }

        EditorGUILayout.LabelField("Selected File :", jsonFilePaht);
        EditorGUILayout.Space();
        outputFolder = EditorGUILayout.TextField("Output Folder :", outputFolder);
        createDatabase = EditorGUILayout.Toggle("Create Databse Asset", createDatabase);
        EditorGUILayout.Space();

        if(GUILayout.Button("Conver to Scriptable Ojbects"))
        {
            if(string.IsNullOrEmpty(jsonFilePaht))
            {
                EditorUtility.DisplayDialog("Error", "Pease Select a JSON file first", "OK");
                return;
            }
            ConvertJsonToScriptableObjects();
        }
    }

    private void ConvertJsonToScriptableObjects()                          //JSON 파일을 ScriptableObject 파일로 변환시켜주느 함수
    {
        //폴더 생성
        if(!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        //JSON 파일 읽기
        string jsonText = File.ReadAllText(jsonFilePaht);                   //JSON파일을 읽는다. 

        try
        {
            //JSON 파싱
            List<ItemData> itemDataList = JsonConvert.DeserializeObject<List<ItemData>>(jsonText) ;

            List<ItemSO> createdItems = new List<ItemSO>();           //ItemSO 리스트 생성
            foreach(ItemData itemData in itemDataList)
            {
                ItemSO itemSO = ScriptableObject.CreateInstance<ItemSO>();                                        //ItemSO 파일을 생성

                //실제 데이터 복사
                itemSO.id = itemData.id;
                itemSO.ItemName = itemData.ItemName;
                itemSO.nameEng = itemData.nameEng;
                itemSO.description = itemData.description;

                //열거형 변환
                if(System.Enum.TryParse(itemData.itemTypeString, out ItemType parsedType))
                {
                    itemSO.itemType = parsedType;
                }
                else
                {
                    Debug.LogWarning($"아이템 {itemData.ItemName}의 유효하지 않은 타임 : {itemData.itemTypeString}");
                }

                itemSO.price = itemData.price;
                itemSO.power = itemData.power;
                itemSO.level = itemData.level;
                itemSO.isStackable = itemData.isStackable;

                //아이콘 로드 (경로가 있는 경우)
                if(!string.IsNullOrEmpty(itemData.iconPath))                         //아이콘 경로가 있는지 확인한다.
                {
                    itemSO.icon = AssetDatabase.LoadAssetAtPath<Sprite>($"AssetsItem / Resources /{itemData.iconPath}.png");

                    if(itemSO.icon == null)
                    {
                        Debug.LogWarning($"dkdklxpa {itemData.nameEng}의 아이콘을 찾을수 없습니다. : {itemData.iconPath}");  
                    }
                }

                //스크립터블 오브젝트 저장 - ID를 4자리 숫자로 포맷팅
                string assetpath = $"{outputFolder}/Item_{itemData.id.ToString("D4")}_{itemData.nameEng}.asset";
                AssetDatabase.CreateAsset(itemSO, assetpath);

                //에셋 이름 지정
                itemSO.name = $"Item_{itemData.id.ToString("D4")} + {itemData.nameEng}";
                createdItems.Add(itemSO);

                EditorUtility.SetDirty(itemSO);

                
            }

            //데이터베이스
            if (createDatabase && createdItems.Count > 0)
            {
                ItemDatabaseSO database = ScriptableObject.CreateInstance<ItemDatabaseSO>();             //생성
                database.items = createdItems;

                AssetDatabase.CreateAsset(database, $"{outputFolder}/ItemDatabase.asset");
                EditorUtility.SetDirty(database);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Sucess", $"Created {createdItems.Count} scriptable objects!", "OK");

        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to Conert JSON : {e.Message}", "OK");
            Debug.LogError($"JSON 변환 오류 : {e}");
        }
    }
}

#endif