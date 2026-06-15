using ImGuiNET;
using PaintTheTownRedMenu.Cheats.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using sVector4 = System.Numerics.Vector4;

namespace PaintTheTownRedMenu.Utils
{
    public static class UIUtil
    {
        public static void Area(string name, Action action, Vector2? size = null, Vector2? position = null, ImGuiWindowFlags windowFlags = ImGuiWindowFlags.None, ImGuiCond cond = ImGuiCond.None)
        {
            if (position.HasValue) ImGui.SetNextWindowPos(position.Value.ToNumerics(), cond);
            if (size.HasValue) ImGui.SetNextWindowSize(size.Value.ToNumerics(), cond);
            if (!ImGui.Begin(name, windowFlags)) return;
            action.Invoke();
            ImGui.End();
        }

        public static void ChildArea(string name, Action action, Vector2? size = null, ImGuiChildFlags childFlags = ImGuiChildFlags.None, ImGuiWindowFlags windowFlags = ImGuiWindowFlags.None)
        {
            if (!ImGui.BeginChild(name, size?.ToNumerics() ?? Vector2.zero.ToNumerics(), childFlags, windowFlags)) return;
            action.Invoke();
            ImGui.EndChild();
        }

        public static void TabBar(string id, Action action)
        {
            if (!ImGui.BeginTabBar(id)) return;
            action.Invoke();
            ImGui.EndTabBar();
        }

        public static void TabBarItem(string name, Action action)
        {
            if (!ImGui.BeginTabItem(name)) return;
            action.Invoke();
            ImGui.EndTabItem();
        }

        public static void Indent(Action action)
        {
            ImGui.Indent();
            action.Invoke();
            ImGui.Unindent();
        }

        public static void ID(string id, Action action)
        {
            ImGui.PushID(id);
            action.Invoke();
            ImGui.PopID();
        }

        public static void ID(Cheat? cheat, Action action)
        {
            if (cheat == null) throw new ArgumentNullException(nameof(cheat));
            ID(cheat.GetName(), action);
        }

        public static bool Checkbox(ToggleCheat? toggleCheat)
        {
            if (toggleCheat == null) throw new ArgumentNullException(nameof(toggleCheat));
            bool enabled = toggleCheat.Enabled;
            bool changed = Checkbox(toggleCheat.GetName(), ref enabled);
            toggleCheat.Enabled = enabled;
            return changed;
        }

        public static bool Checkbox(string name, ref bool value)
        {
            return ImGui.Checkbox(name, ref value);
        }

        public static void Text(string text, Color? color = null)
        {
            if (color.HasValue) ImGui.TextColored(color.Value.ToVector4(), text);
            else ImGui.Text(text);
        }

        public static bool Slider(string name, ref int value, int min, int max)
        {
            return ImGui.SliderInt(name, ref value, min, max);
        }

        public static bool Slider(string name, ref float value, float min, float max)
        {
            return ImGui.SliderFloat(name, ref value, min, max);
        }

        public static bool Input(string label, ref string value, uint maxLength)
        {
            return ImGui.InputText(label, ref value, maxLength);
        }

        public static void SameLine()
        {
            ImGui.SameLine();
        }

        public static void Separator()
        {
            ImGui.Separator();
        }

        public static void ColorPickerButton(string name, ref Color color)
        {
            string popupId = $"##{name}ColorPickerPopup";
            if (Button(name, color)) ImGui.OpenPopup(popupId);
            SameLine();
            Text(name);
            if (!ImGui.BeginPopup(popupId)) return;
            ColorPicker($"##{name}ColorPicker", ref color);
            ImGui.EndPopup();
        }

        public static void ColorPicker(string name, ref Color color)
        {
            sVector4 vector4Color = color.ToVector4();
            if (ImGui.ColorPicker4(name, ref vector4Color)) color = vector4Color.ToColor();
        }

        public static void Button(string name, Action action, Color? color = null)
        {
            if (Button(name, color)) action.Invoke();
        }

        public static void Button(ExecutableCheat? executableCheat)
        {
            if (executableCheat == null) throw new ArgumentNullException(nameof(executableCheat));
            Button(executableCheat.GetName(), executableCheat.Execute);
        }

        public static bool Button(string name, Color? color = null)
        {
            return color.HasValue ? ImGui.ColorButton(name, color.Value.ToVector4()) : ImGui.Button(name);
        }

        public static bool IsKeyPressed(ImGuiKey imGuiKey)
        {
            return ImGui.IsKeyPressed(imGuiKey);
        }

        public static void Combo(string name, string preview, Action action)
        {
            if (!ImGui.BeginCombo(name, preview)) return;
            action.Invoke();
            ImGui.EndCombo();
        }

        public static bool Selectable(string name, bool selected)
        {
            return ImGui.Selectable(name, selected);
        }

        public static void SetItemDefaultFocus()
        {
            ImGui.SetItemDefaultFocus();
        }

        public static bool Dropdown<T>(string name, IReadOnlyList<T> items, ref int selectedIndex, Func<T, string>? display = null)
        {
            if (items.Count == 0) return false;
            if (selectedIndex < 0 || selectedIndex >= items.Count) selectedIndex = 0;
            display ??= item => item?.ToString() ?? "None";
            bool changed = false;
            int index = selectedIndex;
            Combo(name, display(items[index]), () =>
            {
                for (int i = 0; i < items.Count; i++)
                {
                    bool selected = i == index;
                    if (Selectable(display(items[i]), selected))
                    {
                        index = i;
                        changed = true;
                    }

                    if (selected) SetItemDefaultFocus();
                }
            });
            selectedIndex = index;
            return changed;
        }

        public static bool Dropdown<T>(string name, ref T value) where T : Enum
        {
            T[] values = (T[])Enum.GetValues(typeof(T));
            int selectedIndex = Array.IndexOf(values, value);
            bool changed = Dropdown(name, values, ref selectedIndex, item => item.ToString());
            if (changed) value = values[selectedIndex];
            return changed;
        }

        private static string? _keyPicker;
        private static bool _previousWantCaptureKeyboard;
        private static bool _previousWantCaptureMouse;
        private static bool _previousWantTextInput;

        private static readonly List<KeyCode> BlacklistedKeyCodes =
        [
            KeyCode.W,
            KeyCode.A,
            KeyCode.S,
            KeyCode.D,
            KeyCode.LeftShift,
            KeyCode.Space
        ];

        public static bool KeyPicker(string name, ref KeyCode keybind, ref bool waitingForKeybind)
        {
            ImGuiIOPtr io = ImGui.GetIO();
            bool active = _keyPicker == name;
            bool changed = false;
            waitingForKeybind = active;
            ImGui.AlignTextToFramePadding(); 
            Text(name);
            SameLine();
            ImGui.PushID(name);
            if (_keyPicker != null && !active) ImGui.BeginDisabled();
            if (Button(active ? "..." : keybind.ToString()))
            {
                _keyPicker = name;
                waitingForKeybind = true;
            }
            SameLine();
            if (Button("Clear"))
            {
                keybind = KeyCode.None;
                changed = true;
            }
            if (_keyPicker != null && !active) ImGui.EndDisabled();
            ImGui.PopID();
            if (!active) return changed;
            if (!_previousWantCaptureKeyboard)
            {
                _previousWantCaptureKeyboard = io.WantCaptureKeyboard;
                _previousWantCaptureMouse = io.WantCaptureMouse;
                _previousWantTextInput = io.WantTextInput;
                io.WantCaptureKeyboard = true;
                io.WantCaptureMouse = true;
                io.WantTextInput = true;
            }
            if (IsKeyPressed(ImGuiKey.Escape)) _keyPicker = null;
            else
            {
                foreach (ImGuiKey imGuiKey in Enum.GetValues<ImGuiKey>())
                {
                    if (!IsKeyPressed(imGuiKey)) continue;
                    KeyCode keyCode = imGuiKey.ToUnity();
                    if (keyCode == KeyCode.None || BlacklistedKeyCodes.Contains(keyCode)) continue;
                    keybind = keyCode;
                    changed = true;
                    _keyPicker = null;
                    break;
                }
            }
            if (_keyPicker != null || !_previousWantCaptureKeyboard) return changed;
            io.WantCaptureKeyboard = _previousWantCaptureKeyboard;
            io.WantCaptureMouse = _previousWantCaptureMouse;
            io.WantTextInput = _previousWantTextInput;
            _previousWantCaptureKeyboard = false;
            _previousWantCaptureMouse = false;
            _previousWantTextInput = false;
            waitingForKeybind = false;
            return changed;
        }

        public static bool KeyPicker(Cheat cheat)
        {
            KeyCode keybind = cheat.Keybind;
            bool waitingForKeybind = cheat.WaitingForKeybind;
            bool changed = KeyPicker(cheat.GetName(), ref keybind, ref waitingForKeybind);
            cheat.Keybind = keybind;
            cheat.WaitingForKeybind = waitingForKeybind;
            return changed;
        }
    }
}