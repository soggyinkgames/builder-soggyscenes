using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;


public class SoggyStarterPackage : EditorWindow
{
  string packageName = "com.soggyinkgames.package-[]";
  string displayName = "Soggy Package";
  string description = "A custom Soggy Ink Games package.";
  string version = "1.0.0";

  string sampleOne = "SoggySampleOne";
  string sampleTwo = "SoggySampleTwo";
  bool includeRuntime = true;
  bool includeEditor = true;
  bool includeTests = false;

  [MenuItem("Tools/Create Soggy Starter Package")]
  public static void ShowWindow()
  {
    GetWindow<SoggyStarterPackage>("Soggy Starter Package Generator");
  }

  void OnGUI()
  {
    GUILayout.Label("Soggy Starter Package Info", EditorStyles.boldLabel);

    packageName = EditorGUILayout.TextField("Package Name", packageName);
    displayName = EditorGUILayout.TextField("Display Name", displayName);
    description = EditorGUILayout.TextField("Description", description);
    version = EditorGUILayout.TextField("Version", version);
    sampleOne = EditorGUILayout.TextField("Soggy Sample One", sampleOne);
    sampleTwo = EditorGUILayout.TextField("Sample Two Name", sampleTwo);

    includeRuntime = EditorGUILayout.Toggle("Include Runtime Folder", includeRuntime);
    includeEditor = EditorGUILayout.Toggle("Include Editor Folder", includeEditor);
    includeTests = EditorGUILayout.Toggle("Include Tests Folder", includeTests);

    if (GUILayout.Button("Generate Package"))
    {
      string folderPath = EditorUtility.SaveFolderPanel("Choose Package Location", "Assets", "");
      if (!string.IsNullOrEmpty(folderPath))
        GeneratePackage(folderPath);
    }
  }

  // Namespaced template script content
  private string GetRuntimeTemplate() =>
$@"namespace SoggyInkGames.{displayName.Replace(" ", "")}
{{
    using UnityEngine;

    public class #SCRIPTNAME# : MonoBehaviour
    {{
        // Your runtime logic here
    }}
}}";

  private string GetEditorTemplate() =>
  $@"namespace SoggyInkGames.{displayName.Replace(" ", "")}.Editor
{{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(YourScriptName))]
    public class #SCRIPTNAME#Editor : Editor
    {{
        public override void OnInspectorGUI()
        {{
            base.OnInspectorGUI();

            YourScriptName script = (YourScriptName)target;

            if (GUILayout.Button(""Do Something in Editor""))
            {{
                Debug.Log(""Editor button pressed on "" + script.gameObject.name);
            }}
        }}
    }}
}}";

  private string GetTestTemplate() =>
$@"namespace SoggyInkGames.{displayName.Replace(" ", "")}.Tests
{{
    using NUnit.Framework;
    using UnityEngine;

    public class #SCRIPTNAME#Tests
    {{
        [Test]
        public void #SCRIPTNAME#SimplePasses()
        {{
            // Use the Assert class to test conditions
            Assert.IsTrue(true);
        }}

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode, it will simply run to completion.
        // [UnityTest]
        // public IEnumerator #SCRIPTNAME#WithEnumeratorPasses()
        // {{
        //     // Use the Assert class to test conditions.
        //     // Use yield to skip a frame.
        //     yield return null;
        // }}
    }}
}}";

  void GeneratePackage(string basePath)
  {
    string fullPath = Path.Combine(basePath, packageName);
    Directory.CreateDirectory(fullPath);

    string sanitizedDisplayName = displayName.Replace(" ", "");
    string runtimeAssemblyName = $"{packageName.Replace("[]", sanitizedDisplayName)}.runtime";
    string editorAssemblyName = $"{packageName.Replace("[]", sanitizedDisplayName)}.editor";
    string testsAssemblyName = $"{packageName.Replace("[]", sanitizedDisplayName)}.tests";

    // Create package.json
    File.WriteAllText(Path.Combine(fullPath, "package.json"), GetPackageJson());
    // Create README.md
    File.WriteAllText(Path.Combine(fullPath, "README.md"), $"# {displayName}\n\n{description}");
    // Create LICENSE.md
    string licensePathInProject = "Assets/Editor/SoggyPackageBuilder/LICENSE.txt";
    string licenseText = File.ReadAllText(licensePathInProject);
    File.WriteAllText(Path.Combine(fullPath, "LICENSE.md"), licenseText);
    // Create .gitignore
    string gitignorePathInProject = "Assets/Editor/SoggyPackageBuilder/gitignore.txt";
    string gitignore = File.ReadAllText(gitignorePathInProject);
    File.WriteAllText(Path.Combine(fullPath, ".gitignore"), gitignore);
    // Create .gitattributes
    string gitattributesPathInProject = "Assets/Editor/SoggyPackageBuilder/gitattributes.txt";
    string gitattributes = File.ReadAllText(gitattributesPathInProject);
    File.WriteAllText(Path.Combine(fullPath, ".gitattributes"), gitattributes);
    // Create CHANGELOG
    File.WriteAllText(Path.Combine(fullPath, "CHANGELOG.md"), "## 1.0.0\n- Initial release");



    if (includeRuntime)
    {
      string runtimePath = Path.Combine(fullPath, "Runtime");
      Directory.CreateDirectory(runtimePath);
      string runtimeAsmDefPath = Path.Combine(runtimePath, $"{runtimeAssemblyName}.asmdef");
      File.WriteAllText(runtimeAsmDefPath, GetAsmDef(runtimeAssemblyName));
      File.WriteAllText(Path.Combine(runtimePath, $"{sanitizedDisplayName}Runtime.cs"), GetRuntimeTemplate().Replace("#SCRIPTNAME#", $"{sanitizedDisplayName}Runtime"));
    }

    if (includeEditor)
    {
      string editorPath = Path.Combine(fullPath, "Editor");
      Directory.CreateDirectory(editorPath);
      string editorAsmDefPath = Path.Combine(editorPath, $"{editorAssemblyName}.asmdef");
      string[] editorReferences = includeRuntime ? new string[] { runtimeAssemblyName } : new string[0];
      editorReferences = editorReferences.Concat(new string[] { "UnityEditor.CoreModule", "UnityEditor.UIModule" }).ToArray();
      File.WriteAllText(editorAsmDefPath, GetAsmDef(editorAssemblyName, true, false, editorReferences));
      File.WriteAllText(Path.Combine(editorPath, $"{sanitizedDisplayName}Editor.cs"), GetEditorTemplate()
          .Replace("#SCRIPTNAME#", $"{sanitizedDisplayName}"));
    }

    if (includeTests)
    {
      string testsPath = Path.Combine(fullPath, "Tests");
      Directory.CreateDirectory(testsPath);
      string testsAsmDefPath = Path.Combine(testsPath, $"{testsAssemblyName}.asmdef");
      string[] testReferences = includeRuntime ? new string[] { runtimeAssemblyName } : new string[0];
      testReferences = testReferences.Concat(new string[] { "UnityEngine.TestRunner", "UnityEditor.TestRunner" }).ToArray();
      File.WriteAllText(testsAsmDefPath, GetAsmDef(testsAssemblyName, false, true, testReferences));
      File.WriteAllText(Path.Combine(testsPath, $"{sanitizedDisplayName}Test.cs"), GetTestTemplate().Replace("#SCRIPTNAME#", $"{sanitizedDisplayName}Test"));
    }

    string samplesPath = Path.Combine(fullPath, "Samples~");
    Directory.CreateDirectory(samplesPath);

    Directory.CreateDirectory(Path.Combine(samplesPath, sampleOne));
    Directory.CreateDirectory(Path.Combine(samplesPath, sampleTwo)); // NEW
    AssetDatabase.Refresh();
    Debug.Log($"✅ Package '{packageName}' created at: {fullPath}");
  }

  string GetUnityMajorMinor()
  {
    Match match = Regex.Match(Application.unityVersion, @"^(\d+\.\d+)");
    return match.Success ? match.Groups[1].Value : "2020.3";
  }

  string GetPackageJson() =>
$@"{{
  ""name"": ""{packageName}"",
  ""version"": ""{version}"",
  ""displayName"": ""{displayName}"",
  ""description"": ""{description}"",
  ""unity"": ""{GetUnityMajorMinor()}"",
  ""dependencies"": {{
    ""com.unity.[CHANGETHISPACKAGENAME]"": ""[version.number.here]"",
    ""com.unity.[CHANGETHISPACKAGENAME]"": ""[version.number.here]"",
    ""com.unity.[CHANGETHISPACKAGENAME]"": ""[version.number.here]""
  }},
  ""documentationUrl"": ""https://example.com/"",
  ""changelogUrl"": ""https://example.com/changelog.html/"",
  ""licensesUrl"": ""https://example.com/licensing.html/"",
  ""author"": {{
    ""name"": ""SOGGY INK GAMES"",
    ""email"": ""soggyinkgames@gmail.com"",
    ""url"": ""https://www.soggyinkgames.com/form""
  }},
  ""samples"":  [
    {{
      ""displayName"": ""{sampleOne}"",
      ""description"": ""{description}"",
      ""path"": ""Samples~/{sampleOne}""
    }},
    {{
      ""displayName"": ""{sampleTwo}"",
      ""description"": ""{description}"",
      ""path"": ""Samples~/{sampleTwo}""
    }}
  ]
}}";

  string GetAsmDef(string name, bool isEditor = false, bool isTest = false, string[] references = null)
  {
    string referencesJson = "";
    if (references != null && references.Length > 0)
    {
      referencesJson = $@",
    ""references"": [{string.Join(", ", references.Select(r => $"\"{r}\""))}]";
    }

    string json = $@"{{
    ""name"": ""{name}"",
    ""rootNamespace"": """",
    ""includePlatforms"": {GetIncludePlatforms(isEditor, isTest)},
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": false,
    ""overrideReferences"": false,
    ""precompiledReferences"": [],
    ""autoReferenced"": true,
    ""defineConstraints"": [],
    ""versionDefines"": [],
    ""noEngineReferences"": false
    {referencesJson}
}}";
    return json;
  }


  string GetIncludePlatforms(bool isEditor, bool isTest)
  {
    if (isEditor) return "[\"Editor\"]";
    if (isTest) return "[]"; // Tests run in the editor and in play mode
    return "[]"; // Runtime
  }
  string Sanitize(string name)
  {
    return name.Replace(" ", "").Replace("-", "").Replace(".", "");
  }
}
