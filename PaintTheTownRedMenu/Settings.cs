using Newtonsoft.Json.Linq;
using PaintTheTownRedMenu.Cheats.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PaintTheTownRedMenu
{
    public static class Settings
    {
        private const string SettingsFile = "Settings.json";
        private const string DefaultSettingsFile = "Default.Settings.json";

        public static bool SettingsFileExists()
        {
            return File.Exists(SettingsFile);
        }

        public static void SaveSettings()
        {
            SaveSettings(SettingsFile);
        }

        public static void SaveDefaultSettings()
        {
            SaveSettings(DefaultSettingsFile);
        }

        private static void SaveSettings(string file)
        {
            JObject json = new JObject();
            JObject toggleCheats = new JObject();
            JObject executableCheats = new JObject();
            foreach (Cheat cheat in PaintTheTownRedMenuMod.Instance.Cheats.Where(c => !c.Hidden))
            {
                Type type = cheat.GetType();
                Type? settingsType = type.GetNestedType("Settings", BindingFlags.Public | BindingFlags.NonPublic);
                JObject? settingsJson = null;
                if (settingsType != null)
                {
                    FieldInfo? settingsField = type.GetFields(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(f => f.FieldType == settingsType);
                    if (settingsField != null)
                    {
                        object? settingsObj = settingsField.GetValue(cheat);
                        if (settingsObj == null) return;
                        settingsJson = (JObject)SerializeObject(settingsObj);
                    }
                }
                switch (cheat)
                {
                    case ToggleCheat toggleCheat:
                        {
                            JObject obj = new JObject
                            {
                                ["Enabled"] = toggleCheat.Enabled,
                                ["Keybind"] = toggleCheat.Keybind.ToString()
                            };
                            if (settingsJson != null) obj["Settings"] = settingsJson;
                            toggleCheats[toggleCheat.GetType().Name] = obj;
                            break;
                        }
                    case ExecutableCheat executableCheat:
                        {
                            JObject obj = new JObject
                            {
                                ["Keybind"] = executableCheat.Keybind.ToString()
                            };
                            if (settingsJson != null) obj["Settings"] = settingsJson;
                            executableCheats[executableCheat.GetType().Name] = obj;
                            break;
                        }
                }
            }
            json["ToggleCheats"] = toggleCheats;
            json["ExecutableCheats"] = executableCheats;
            File.WriteAllText(file, json.ToString());
        }

        public static void LoadSettings()
        {
            if (!SettingsFileExists()) SaveSettings();
            JObject json = JObject.Parse(File.ReadAllText(SettingsFile));
            JObject? toggleCheatsJson = json["ToggleCheats"] as JObject;
            JObject? executableCheatsJson = json["ExecutableCheats"] as JObject;
            foreach (Cheat cheat in PaintTheTownRedMenuMod.Instance.Cheats.Where(c => !c.Hidden))
            {
                Type type = cheat.GetType();
                Type? settingsType = type.GetNestedType("Settings", BindingFlags.Public | BindingFlags.NonPublic);
                FieldInfo? settingsField = settingsType != null ? type.GetFields(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(f => f.FieldType == settingsType) : null;
                JObject? cheatJson = null;
                switch (cheat)
                {
                    case ToggleCheat toggleCheat:
                        {
                            if (toggleCheatsJson != null) cheatJson = toggleCheatsJson[type.Name] as JObject;
                            if (cheatJson == null) continue;
                            toggleCheat.Enabled = cheatJson["Enabled"]?.Value<bool>() ?? false;
                            if (Enum.TryParse(cheatJson["Keybind"]?.ToString(), out KeyCode key)) toggleCheat.Keybind = key;
                            break;
                        }
                    case ExecutableCheat executableCheat:
                        {
                            if (executableCheatsJson != null) cheatJson = executableCheatsJson[type.Name] as JObject;
                            if (cheatJson == null) continue;
                            if (Enum.TryParse(cheatJson["Keybind"]?.ToString(), out KeyCode keybind)) executableCheat.Keybind = keybind;
                            break;
                        }
                }
                if (cheatJson == null || settingsType == null || settingsField == null) continue;
                if (cheatJson["Settings"] is not JObject settingsJson) continue;
                settingsField.SetValue(cheat, DeserializeObject(settingsJson, settingsType));
            }
        }

        public static void ResetSettings()
        {
            if (!File.Exists(DefaultSettingsFile)) return;
            File.Copy(DefaultSettingsFile, SettingsFile, true);
            LoadSettings();
        }

        public static void OpenSettings()
        {
            Process.Start("explorer.exe", SettingsFile);
        }

        private static JToken SerializeObject(object? obj)
        {
            if (obj == null) return JValue.CreateNull();
            Type type = obj.GetType();
            if (type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(float) || type == typeof(double) || type == typeof(int) || type == typeof(bool)) return JToken.FromObject(obj);
            JObject jObject = new JObject();
            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                object? value = field.GetValue(obj);
                jObject[field.Name] = SerializeObject(value);
            }
            return jObject;
        }

        private static object? DeserializeObject(JToken? token, Type type)
        {
            if (token == null || token.Type == JTokenType.Null) return Activator.CreateInstance(type);
            if (type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(float) || type == typeof(double) || type == typeof(int) || type == typeof(bool)) return token.ToObject(type);
            object? obj = Activator.CreateInstance(type);
            if (obj == null) return null;
            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                JToken? child = token[field.Name];
                if (child == null) continue;
                object? value = DeserializeObject(child, field.FieldType);
                if (value == null) continue;
                field.SetValue(obj, value);
            }
            return obj;
        }
    }
}