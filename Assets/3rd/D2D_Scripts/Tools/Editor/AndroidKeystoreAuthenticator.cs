using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

#if UNITY_EDITOR_OSX

/// <summary>
/// Unity clears Android Keystore and Key Alias passwords when it exits,
/// likely for security reasons.
/// 
/// This script uses the macOS Keychain to store the passwords securely
/// on your system. The passwords are stored per Keystore and Key Alias,
/// so you only have to enter them once accross all your Unity projects.
/// 
/// Enter the passwords once like normal in Unity's Player Settings and
/// make a build. On successive builds, the passwords are loaded from the
/// Keychain and you don't have to enter them again.
/// 
/// To update the passwords, enter the new ones in the Player Settings and
/// then make a build. Use Keychain Access to delete saved passwords, 
/// search for "Android Keystore" in the login Keychain.
/// </summary>
public class AndroidKeystoreAuthenticator : IPreprocessBuild, IPostprocessBuild
{
    const string KeychainServiceName = "Android Keystore";

    public int callbackOrder {
        get {
            return 0;
        }
    }

    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        var keystorePath = PlayerSettings.Android.keystoreName;
        if (string.IsNullOrEmpty(keystorePath) 
                || !File.Exists(keystorePath)
                || string.IsNullOrEmpty(PlayerSettings.Android.keyaliasName)) {
            // Unity will warn the user that the Keystore hasn't been configured yet
            return;
        }

        // Used to only warn once if both Keystore and Key Alias passwords are missing
        var hasWarned = false;

        var pwd = ManagePassword("Keystore", keystorePath, PlayerSettings.Android.keystorePass, ref hasWarned);
        if (pwd != null) {
            PlayerSettings.Android.keystorePass = pwd;
        }

        var keyName = keystorePath + "#" + PlayerSettings.Android.keyaliasName;
        pwd = ManagePassword("Key Alias", keyName, PlayerSettings.Android.keyaliasPass, ref hasWarned);
        if (pwd != null) {
            PlayerSettings.Android.keyaliasPass = pwd;
        }
    }

    public void OnPostprocessBuild(BuildTarget target, string path)
    {
        // Remove passwords after build
        // Note that this is not reliable as it won't be called if the build is cancelled
        // In this case Unity will clear the password when it exits
        PlayerSettings.Android.keystorePass = null;
        PlayerSettings.Android.keyaliasPass = null;
    }

    /// <summary>
    /// Manage a password, adding, updating and loading it from the macOS Keychain
    /// as necessary.
    /// </summary>
    /// <param name="name">Display name to identify password to the user</param>
    /// <param name="keychainName">Unique name to store the password in the Keychain</param>
    /// <param name="currentPassword">The current password (if any)</param>
    /// <returns>The loaded password or null if the password doesn't exist 
    /// in the Keychain or matches the current password</returns>
    string ManagePassword(string name, string keychainName, string currentPassword, ref bool hasWarned)
    {
        var command = string.Format(
            "find-generic-password -a '{0}' -s '{1}' -w", 
            keychainName, KeychainServiceName
        );
        string output, error;
        var code = Execute("security", command, out output, out error);
        if (code != 0) {
            if (string.IsNullOrEmpty(currentPassword)) {
                if (!hasWarned) {
                    // Password not saved or set, prompt the user to do so
                    EditorUtility.DisplayDialog(
                        "Android Keystore Authenticator",
                        "The " + name + " password could not found in the Keychain.\n\n"
                        + "Set it once in Unity's Player Settings and it will "
                        + "be remembered in the Keychain for successive builds.",
                        "Ok"
                    );
                    hasWarned = true;
                }
                return null;
            } else {
                // Password set but not yet saved, store it in Keychain
                Debug.Log("Adding " + name + " password to Keychain.");
                AddPassword(keychainName, currentPassword);
                return null;
            }
        } else if (currentPassword != output) {
            if (!string.IsNullOrEmpty(currentPassword)) {
                // Password in Keychain differs, update it
                Debug.Log("Updating " + name + " password in Keychain.");
                AddPassword(keychainName, currentPassword);
                return null;
            } else {
                // Set password from Keychain
                return output;
            }
        }

        // Keychain password and Player Settins password match
        return null;
    }

    /// <summary>
    /// Add a password to the Keychain, updating it if it already exists
    /// </summary>
    /// <param name="keychainName">Name of the password in the keychain (user account)</param>
    /// <param name="password">The password to save</param>
    static void AddPassword(string keychainName, string password)
    {
        // We use the interactive mode of security here that allows us to
        // pipe the command to stdin and thus avoid having the password
        // exposed in the process table.
        var command = string.Format(
            "add-generic-password -U -a '{0}' -s '{1}' -w '{2}'\n", 
            keychainName, KeychainServiceName, password
        );
        string output, error;
        var code = Execute("security", "-i", command, out output, out error);
        if (code != 0) {
            Debug.LogError("Failed to store password in Keychain: " + error);
        }
    }

    /// <summary>
    /// Call the security command line tool to set and get macOS Keychain passwords.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="arguments">Arguments passed to the command.</param>
    /// <param name="output">The standard output of the command.</param>
    /// <param name="error">The standard error of the command.</param>
    /// <returns>The exit code of the command.</returns>
    static int Execute(string command, string arguments, out string output, out string error)
    {
        var proc = new System.Diagnostics.Process();
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardError = true;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.FileName = command;
        proc.StartInfo.Arguments = arguments;

        proc.Start();
        proc.WaitForExit();

        output = proc.StandardOutput.ReadToEnd();
        error = proc.StandardError.ReadToEnd();
        return proc.ExitCode;
    }

    /// <summary>
    /// Call the security command line tool to set and get macOS Keychain passwords.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="arguments">Arguments passed to the command.</param>
    /// <param name="input">Data to write to the command's standard input.</param>
    /// <param name="output">The standard output of the command.</param>
    /// <param name="error">The standard error of the command.</param>
    /// <returns>The exit code of the command.</returns>
    static int Execute(string command, string arguments, string input, out string output, out string error)
    {
        var proc = new System.Diagnostics.Process();
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardInput = true;
        proc.StartInfo.RedirectStandardError = true;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.FileName = command;
        proc.StartInfo.Arguments = arguments;

        proc.Start();

        // Unity's old Mono runtime writes a BOM to the input stream,
        // tripping up the command. Ceate a new writer with an encoding
        // that has BOM disabled.
        var writer = new StreamWriter(proc.StandardInput.BaseStream, new System.Text.UTF8Encoding(false));
        writer.Write(input);
        writer.Close();

        proc.WaitForExit();

        output = proc.StandardOutput.ReadToEnd();
        error = proc.StandardError.ReadToEnd();
        return proc.ExitCode;
    }
}

#endif