using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public static class Settings
{
    public static bool IsMusic;
    public static bool IsSounds;

    [Serializable]
    public struct SettingsValues
    {
        public bool IsMusic;
        public bool IsSounds;

        public SettingsValues(bool isMusic, bool isSounds)
        {
            IsMusic = isMusic;
            IsSounds = isSounds;
        }
    }

    public static void SaveSettings()
    {
        using (StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/Settings.json"))
        {
            var values = new SettingsValues(IsMusic, IsSounds);
            string SerializedSettings = JsonConvert.SerializeObject(values, Formatting.Indented);
            sw.Write(SerializedSettings);
        }
    }

    public static void LoadSettings()
    {
        try
        {
            using (StreamReader sr = new StreamReader(Application.persistentDataPath + "/Settings.json"))
            {
                var values = JsonConvert.DeserializeObject<SettingsValues>(sr.ReadToEnd());
                IsMusic = values.IsMusic;
                IsSounds = values.IsSounds;
            }
        }
        catch (System.IO.FileNotFoundException)
        {
            SetDefault();
        }
    }

    public static void SetDefault()
    {
        IsMusic = true;
        IsSounds = true;
    }
}
