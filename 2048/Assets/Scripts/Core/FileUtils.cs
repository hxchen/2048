using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using FullSerializer;

public static class FileUtils
{

    /// <summary>
    /// Saves the specified data to a json file at the specified path.
    /// </summary>
    /// <param name="path">The path to the json file.</param>
    /// <param name="data">The data to save.</param>
    /// <typeparam name="T">The type of the data to serialize to the file.</typeparam>
    public static void SaveJsonFile<T>(string path, T data) where T : class {
        fsData serializedData;
        var serializer = new fsSerializer();
        serializer.TrySerialize(data, out serializedData).AssertSuccessWithoutWarnings();
        var file = new StreamWriter(path);
        var json = fsJsonPrinter.PrettyJson(serializedData);
        file.WriteLine(json);
        file.Close();
    }

    /// <summary>
    /// Loads the specified json file.
    /// </summary>
    /// <param name="serializer">The FullSerializer serializer to use.</param>
    /// <param name="path">The json file path.</param>
    /// <typeparam name="T">The type of the data to load.</typeparam>
    /// <returns>The loaded json data.</returns>
    public static T LoadJsonFile<T>(fsSerializer serializer, string path) where T : class {
        var textAsset = Resources.Load<TextAsset>(path);
        Assert.IsNotNull((textAsset));
        var data = fsJsonParser.Parse(textAsset.text);
        object deserialized = null;
        serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
        return deserialized as T;
    }

    /// <summary>
    /// Returns true if the specified path exists and false otherwise.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>True if the specified path exists; false otherwise.</returns>
    public static bool FileExists(string path) {
        var textAsset = Resources.Load<TextAsset>(path);
        return textAsset != null;
    }
}
