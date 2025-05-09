## Soggy Starter Package Generator - Usage Guide

This tool simplifies the creation of a standardized Unity package structure, complete with essential files and optional Runtime, Editor, and Test folders with pre-configured assembly definitions and template scripts.

**Prerequisites:**

* **Unity Editor:** This tool is designed to run within the Unity Editor.
* **LICENSE.txt and gitignore.txt:** Ensure you have two text files named `LICENSE.txt` and `gitignore.txt` located in your Unity project at `Assets/Editor/SoggyPackageBuilder/`. These files will be copied into your generated package. You can customize these files with your desired license and Git ignore patterns.

**How to Use:**

1.  **Open the Tool:**
    * In your Unity Editor, navigate to the top menu bar.
    * Click on `Tools`.
    * Select `Create Soggy Starter Package`.
    * This will open the "Soggy Starter Package Generator" window.

2.  **Configure Package Information:**
    * **Package Name:** Enter the unique identifier for your package. It's recommended to follow the format `com.yourcompany.yourpackage-feature`. The `[]` placeholder will be automatically replaced with a sanitized version of your "Display Name".
    * **Display Name:** Enter a human-readable name for your package (e.g., "Soggy Awesome Features"). This name will be used in the `package.json` and as the basis for namespaces and assembly names.
    * **Description:** Provide a brief description of your package.
    * **Version:** Set the initial version of your package (e.g., "1.0.0").
    * **Soggy Sample One / Sample Two Name:** Enter names for the example folders that will be created in the `Samples~` directory. These are optional but provide a good starting point for demonstrating how to use your package.

3.  **Select Optional Folders:**
    * **Include Runtime Folder:** Check this box to create a `Runtime` folder. This folder will contain your core package scripts. An assembly definition file (`.asmdef`) and a basic namespaced MonoBehaviour script will be generated here.
    * **Include Editor Folder:** Check this box to create an `Editor` folder. This folder will contain any custom editor scripts for your package. An assembly definition file (`.asmdef`) that references the Runtime assembly (if included) and a basic namespaced Editor script with a `CustomEditor` attribute will be generated here.
    * **Include Tests Folder:** Check this box to create a `Tests` folder. This folder will contain your unit and integration tests. An assembly definition file (`.asmdef`) that references the Runtime assembly (if included) and a basic namespaced NUnit test script will be generated here.

4.  **Generate the Package:**
    * Once you have configured all the desired settings, click the `Generate Package` button.
    * A file selection dialog will appear, prompting you to choose the location where you want to save your new package folder.
    * Select your desired location and click "Save" or "Choose".

5.  **Package Contents:**
    * A new folder with the name you specified in the "Package Name" field will be created at the chosen location.
    * This folder will contain the following:
        * `package.json`: A manifest file containing essential information about your package (name, version, display name, description, dependencies, etc.).
        * `README.md`: A basic README file with the package's display name and description.
        * `LICENSE.md`: A copy of the `LICENSE.txt` file from `Assets/Editor/SoggyPackageBuilder/`.
        * `.gitignore`: A copy of the `gitignore.txt` file from `Assets/Editor/SoggyPackageBuilder/`.
        * `.gitattributes`: A basic `.gitattributes` file (often used for line ending normalization in Git).
        * `CHANGELOG.md`: An initial changelog file.
        * **`Runtime/` (Optional):**
            * `YourPackageName.runtime.asmdef`: Assembly definition file for the Runtime scripts.
            * `YourPackageNameRuntime.cs`: A basic MonoBehaviour script within the `SoggyInkGames.YourPackageName` namespace.
        * **`Editor/` (Optional):**
            * `YourPackageName.editor.asmdef`: Assembly definition file for the Editor scripts, referencing the Runtime assembly.
            * `YourPackageNameEditor.cs`: A basic Editor script with a `CustomEditor` attribute targeting a `YourPackageName` script in the Runtime. It resides in the `SoggyInkGames.YourPackageName.Editor` namespace.
        * **`Tests/` (Optional):**
            * `YourPackageName.tests.asmdef`: Assembly definition file for the Test scripts, referencing the Runtime assembly and the TestRunner assemblies.
            * `YourPackageNameTest.cs`: A basic NUnit test script within the `SoggyInkGames.YourPackageName.Tests` namespace.
        * **`Samples~/`:**
            * `SoggySampleOne/`: An empty folder for your first sample.
            * `SampleTwoName/`: An empty folder for your second sample.

**Next Steps:**

* **Import into Unity:** You can now either keep this package as a local package within your project (by placing it in the `Packages` folder or adding it via the Package Manager) or prepare it for distribution.
* **Add Your Code:** Start adding your custom scripts and assets into the generated `Runtime`, `Editor`, and `Tests` folders.
* **Write Tests:** Implement unit and integration tests in the `Tests` folder to ensure the quality of your package.
* **Create Samples:** Add example scenes and scripts to the `Samples~` folders to demonstrate how to use your package.
* **Version Control:** Initialize a Git repository in the package folder to manage your changes.
* **Documentation:** Expand the `README.md` file with detailed instructions and API documentation for your package.
* **Distribution:** Consider using tools like `upm` (Unity Package Manager command-line tool) to further manage and potentially distribute your package.

This tool provides a solid foundation for developing reusable Unity packages. Remember to customize the generated files and folders to fit the specific needs of your project.