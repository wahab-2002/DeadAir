using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildAll
{
    const string Scene = "Assets/_DeadAir/Scenes/DeadAir.unity";
    [MenuItem("Dead Air/Build All")]
    public static void Build()
    {
        string[] paths = { "Builds/Windows/DeadAir.exe", "Builds/Mac/DeadAir.app", "Builds/Linux/DeadAir.x86_64" };
        BuildTarget[] targets = { BuildTarget.StandaloneWindows64, BuildTarget.StandaloneOSX, BuildTarget.StandaloneLinux64 };
        var results = new StringBuilder();
        for (int i = 0; i < targets.Length; i++)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(paths[i]));
                BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
                    scenes = new[] { Scene }, locationPathName = paths[i], target = targets[i], options = BuildOptions.None });
                results.AppendLine($"{targets[i]}: {report.summary.result}; {report.summary.totalSize} bytes; {report.summary.totalWarnings} warnings; {report.summary.totalErrors} errors");
                foreach (BuildStep step in report.steps)
                    foreach (BuildStepMessage message in step.messages)
                        if (message.type == LogType.Warning || message.type == LogType.Error || message.type == LogType.Exception)
                            results.AppendLine(message.type + ": " + message.content);
            }
            catch (Exception error) { results.AppendLine(targets[i] + ": failed: " + error.Message); }
            File.WriteAllText("Builds/BuildResults.txt", results.ToString());
        }
        Debug.Log(results.ToString());
    }
}
