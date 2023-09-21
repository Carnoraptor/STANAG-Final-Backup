/*using System.IO;
using System.Xml;
using UnityEditor;

public class XMLCreation : Editor
{
    [MenuItem("Assets/XML Creation/BulletML Profile")]
    public static void CreatDefaultOne()
    {
        string tFile;

        if (Selection.activeObject != null)
        {
            tFile = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (Directory.Exists(tFile))
                tFile = Path.Combine(tFile, "DefaultBulletML.xml");
            else if (File.Exists(tFile))
                tFile = Path.Combine(Path.GetDirectoryName(tFile), "DefaultBulletML.xml");
        }
        else
            tFile = "Assets/DefaultBulletML.xml";

        tFile = AssetDatabase.GenerateUniqueAssetPath(tFile);

        XmlWriter tWriter = XmlWriter.Create(tFile);
        tWriter.WriteElementString("DefaultBulletML", "");
        tWriter.Close();

        AssetDatabase.ImportAsset(tFile);
    }
}*/