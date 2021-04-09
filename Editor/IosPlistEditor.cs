#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
 
public class IosPlistEditor {
 
    [PostProcessBuild]
    public static void EditPlist(BuildTarget buildTarget, string pathToBuiltProject) {
        if (buildTarget == BuildTarget.iOS) {
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            
            PlistElementDict rootDict = plist.root;

            rootDict.SetBoolean("UIFileSharingEnabled", true);
            rootDict.SetString("NSPhotoLibraryUsageDescription", "Used for saving screenshots / map backgrounds / etc.");
            
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}
#endif