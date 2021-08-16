using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using CoreGame.Items;

namespace EditorTools
{
    public class ContentSegregateTools : Editor, ITools
    {
        private const string toolName = "Content Tool";
        
        #region OVERRIDE TOOLSBASE
        
        public string GetName => toolName;
        public void DoUpdate()
        {
            GUI();
        }

        #endregion

        private void GUI()
        {
            ContentGUI();
            InventoryJsonGUI();
            DataItemGUI();
            ItemCatalogGUI();
        }

        #region INIT CONTENT

        private DefaultAsset contentFolder = null;
        private void ContentGUI()
        {
            GUILayout.Space(10f);
            EditorGUILayout.HelpBox("This tools is for Segrating content and crate or update ItemData in ", MessageType.Info);
            GUILayout.Space(5f);
            GUILayout.Label("Drag & Drop Content Root Folder: Assets/Project/Art/Game/Content");
            
            contentFolder = (DefaultAsset)EditorGUILayout.ObjectField("Content Folder:", contentFolder, typeof(DefaultAsset), false);

            if(contentFolder != null)
            {
                GUILayout.Space(5f);
                if (GUILayout.Button("Initialize Content", GUILayout.Width(155)))
                {
                    InitContentFiles();
                }
            }
            else
            {
                // inventoryJson = "";
                if (searchedFileList != null && searchedFileList.Count > 0)
                {
                    searchedFileList = new List<SearchedFile>();
                }

                if (itemContentDataList != null && itemContentDataList.Count > 0)
                {
                    itemContentDataList = new List<ContentsItem.ItemData>();
                }
            }
        }

        private List<SearchedFile> searchedFileList = new List<SearchedFile>();
        private void InitContentFiles()
        {
            searchedFileList = new List<SearchedFile>();
            string folderPath = Path.GetFullPath(AssetDatabase.GetAssetPath(contentFolder));
            List<string> searchedFiles = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories).Where(arg => ContentExtension(arg)).ToList();

            Debug.Log($"DATAPATH: {Path.GetFullPath(Application.dataPath)} COUNT: {searchedFiles.Count}");

            if (searchedFiles != null && searchedFiles.Count > 0)
            {
                for (int i = 0; i < searchedFiles.Count; i++)
                {
                    string filePath = searchedFiles[i];
                    string fileNameWoExt = Path.GetFileNameWithoutExtension(filePath);
                    SearchedFile searchedFile = new SearchedFile();

                    if(fileNameWoExt.ToLower().Contains("thumb"))
                    {
                        fileNameWoExt = fileNameWoExt.Replace("thumb_", "");
                    }

                    searchedFile = searchedFileList.Find(arg => arg.fileName == fileNameWoExt);
                    if (searchedFile == null)
                    {
                        searchedFile = new SearchedFile();
                        searchedFile.fileName = fileNameWoExt;
                        searchedFileList.Add(searchedFile);
                    }

                    if (filePath.ToLower().Contains("thumb"))
                    {
                        searchedFile.thumbnailPath = filePath;
                    }
                    else
                    {
                        searchedFile.filePath = filePath;
                    }
                }
            }
        }

        private static bool ContentExtension(string fileName)
        {
            if (fileName.EndsWith(".jpg") || fileName.EndsWith(".png"))
                return true;

            return false;
        }

        #endregion INIT CONTENT

        #region INVENTORY
        
        private string inventoryJson;
        private List<ContentsItem.ItemData> itemContentDataList = new List<ContentsItem.ItemData>();

        private void InventoryJsonGUI()
        {
            if (searchedFileList != null && searchedFileList.Count > 0)
            {
                GUILayout.Space(10f);
                GUILayout.Label("Input Inventory from excel data in JSON format.");
                inventoryJson = EditorGUILayout.TextField("Inventory text", inventoryJson);

                if (string.IsNullOrEmpty(inventoryJson) == false)
                {
                    if (GUILayout.Button("Verify Json", GUILayout.Width(155)))
                    {
                        SimpleJSON.JSONNode jsonNode = SimpleJSON.JSON.Parse(inventoryJson);
                        if (jsonNode != null)
                        {
                            foreach (var node in jsonNode)
                            {
                                SimpleJSON.JSONNode nodeData = node;
                                string label = nodeData["Label"];
                                string status = nodeData["Status"];
                                string theme = nodeData["Category"];
                                string character = nodeData["Character"];

                                SearchedFile searchedFile = searchedFileList.Find(arg => arg.fileName == label);
                                if (searchedFile != null)
                                {
                                    if (string.IsNullOrEmpty(searchedFile.fileName) || string.IsNullOrEmpty(searchedFile.filePath) || string.IsNullOrEmpty(searchedFile.thumbnailPath))
                                    {
                                        Debug.LogError($"File is Empty: FILENAME = {searchedFile.fileName} :: FILEPATH = {searchedFile.filePath} :: THUMBNAIL = {searchedFile.thumbnailPath}");
                                        continue;
                                    }

                                    ContentsItem.ItemData itemData = new ContentsItem.ItemData(searchedFile.fileName, searchedFile.filePath);
                                    if (string.IsNullOrEmpty(searchedFile.thumbnailPath))
                                    {
                                        Debug.LogError($"No Thumbnail found for the Item: {searchedFile.fileName}");
                                    }
                                    else
                                    {
                                        itemData.UpdateData(thumbnailPath: searchedFile.thumbnailPath, characterId: character, theme: theme);
                                        itemContentDataList.Add(itemData);
                                    }
                                }

                            }
                        }

                        Debug.Log($"Verify Json: verification complete for JSON: {inventoryJson}");
                    }
                }
            }
        }

        #endregion INVENTORY

        #region CREATING ITEMDATA SO

        private DefaultAsset dataItemFolder = null;
        private void DataItemGUI()
        {
            if (itemContentDataList != null && itemContentDataList.Count > 0)
            {
                GUILayout.Space(10f);
                GUILayout.Label("Drag & Drop ItemData Root Folder: Assets/Project/Data/ItemDatas");
                dataItemFolder = (DefaultAsset)EditorGUILayout.ObjectField("Items Folder:", dataItemFolder, typeof(DefaultAsset), false);
                if (dataItemFolder != null)
                {
                    GUILayout.Space(5f);
                    if (GUILayout.Button("Update ItemData SO", GUILayout.Width(155)))
                    {
                        canUpdateCatalog = false;
                        string folderPath = Path.GetFullPath(AssetDatabase.GetAssetPath(dataItemFolder));
                        int count = itemContentDataList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            ContentsItem.ItemData itemData = itemContentDataList[i];
                            string themeType = itemData.themeType.ToString().ToLower();
                            themeType = char.ToUpperInvariant(themeType[0]) + themeType.Substring(1); //Keeping 1st Letter Capital
                            string categoryFolder = $"{folderPath}/{themeType}";

                            itemData.UpdateSoFilePath(categoryFolder);
                            
                            Debug.Log($"PATH 1: {categoryFolder} ");
                            if (Directory.Exists(categoryFolder) == false)
                            {
                                Directory.CreateDirectory(categoryFolder);
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();
                            }

                            if (itemData.categoryType != CoreGame.Items.Category.Type.None)
                            {
                                string categoryType = itemData.categoryType.ToString().ToLower();
                                categoryType = char.ToUpperInvariant(categoryType[0]) + categoryType.Substring(1); //Keeping 1st Letter Capital
                                categoryFolder = $"{categoryFolder}/{categoryType}";

                                itemData.UpdateSoFilePath(categoryFolder);

                                Debug.Log($"PATH 2: {categoryFolder} ");
                                if (Directory.Exists(categoryFolder) == false)
                                {
                                    Directory.CreateDirectory(categoryFolder);
                                    AssetDatabase.SaveAssets();
                                    AssetDatabase.Refresh();
                                }
                            }

                            CreateItemDataSO(itemData);
                        }
                        canUpdateCatalog = true;
                    }
                }
            }
        }

        private void CreateItemDataSO(ContentsItem.ItemData data)
        {
            CoreGame.Items.ItemData asset = null;
            if (File.Exists(data.itemSOFilePath))
            {
                asset = AssetDatabase.LoadAssetAtPath<CoreGame.Items.ItemData>(data.GetSoAssetPath);
            }
            else
            {
                asset = ScriptableObject.CreateInstance<CoreGame.Items.ItemData>();
                AssetDatabase.CreateAsset(asset, data.GetSoAssetPath);
                AssetDatabase.SaveAssets();
            }

            if (asset != null)
            {
                Sprite assetItemThumbnail = AssetDatabase.LoadAssetAtPath<Sprite>(data.GetThumbnailAssetPath);
                Sprite assetItem = AssetDatabase.LoadAssetAtPath<Sprite>(data.GetAssetPath);

                asset.UpdateData(data.itemName, data.character, data.themeType, data.categoryType);
                asset.UpdateSprite(assetItem, assetItemThumbnail);

                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        #endregion CREATING ITEMDATA SO

        #region UPDATE ITEM CATALOG
        
        private bool canUpdateCatalog;

        private void ItemCatalogGUI()
        {
            if(canUpdateCatalog)
            {
                GUILayout.Space(5f);
                if (GUILayout.Button("Update Item Catalog", GUILayout.Width(155)))
                {
                    var catalogConfig = (ItemCatalogCofig)AssetDatabase.LoadAssetAtPath(ItemCatalogCofig.AssetPath, typeof(ItemCatalogCofig));
                    if(catalogConfig == null)
                    {
                        Debug.Log($"Cannot find ItemCatalogCofig Createing new one in Path: {ItemCatalogCofig.AssetPath}");

                        var asset = ScriptableObject.CreateInstance<ItemCatalogCofig>();
                        ProjectWindowUtil.CreateAsset(asset, ItemCatalogCofig.AssetPath);
                        AssetDatabase.SaveAssets();
                    }

                    CacheItemCatalogData(catalogConfig);
                    catalogConfig.Items.Clear();

                    Debug.Log("Started catalog update...");

                    InitItemDatas(catalogConfig);
                }
            }
        }

        private static List<TempItemCatalog> tempItemCatalogList = new List<TempItemCatalog>();
        private void CacheItemCatalogData(ItemCatalogCofig catalogConfig)
        {
            int count = catalogConfig.Items.Count;
            if (count > 0)
            {
                tempItemCatalogList = new List<TempItemCatalog>();
                for (var i = 0; i < count; i++)
                {
                    int countItems = catalogConfig.Items[i].ItemCatalogDatas.Count;
                    for (int j = 0; j < countItems; j++)
                    {
                        ItemCatalogData data = catalogConfig.Items[i].ItemCatalogDatas[j];

                        TempItemCatalog tempItemCatalog = new TempItemCatalog();
                        tempItemCatalog.itemName = data.ItemName;
                        tempItemCatalog.labelName = data.labelName;
                        tempItemCatalog.isLocal = data.isLocal;
                        tempItemCatalogList.Add(tempItemCatalog);
                    }
                }
            }
        }

        private void InitItemDatas(ItemCatalogCofig catalogConfig)
        {
            string[] guids = AssetDatabase.FindAssets("", new[] { AssetDatabase.GetAssetPath(dataItemFolder) });
            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                {
                    ItemData asset = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);
                    if (asset != null)// && !asset.ForTesting)
                    {
                        try
                        {
                            catalogConfig.AddItemCatalogEntry(asset);

                            // catalogConfig.Items.Find(arg => arg.CategoryType)

                            // var newEntry = new ItemCatalogEntry(new AssetReference(guid), asset);

                            // TempItemCatalog tempItemCatalog = tempItemCatalogList.Find(arg => arg.itemName == asset.itemName);
                            // if (tempItemCatalog != null)
                            // {
                            //     newEntry.labelName = tempItemCatalog.labelName;
                            //     newEntry.isLocal = tempItemCatalog.isLocal;
                            // }

                            // catalog.items.Add(newEntry);
                            EditorUtility.SetDirty(asset);
                        }
                        catch (System.Exception ex)
                        {
                             // TODO
                        }
                    }
                }
            }
        }

        #endregion UPDATE ITEM CATALOG

        [System.Serializable]
        public class SearchedFile
        {
            public string fileName;
            public string filePath;
            public string thumbnailPath;
        }

        [System.Serializable]
        public class TempItemCatalog
        {
            public string itemName;
            public string labelName;
            public bool isLocal;
        }
    }
}
