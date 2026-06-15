using Il2CppInterop.Runtime;
using ImGuiNET;
using PaintTheTownRedMenu.Cheats;
using PaintTheTownRedMenu.Cheats.Core;
using PaintTheTownRedMenu.Utils;
using System.Collections.Generic;
using UnityEngine;
using GUI = SharpGUI.GUI;
using sVector4 = System.Numerics.Vector4;

namespace PaintTheTownRedMenu.Menu.Core
{
    public class Renderer
    {
        public Renderer()
        {
            Tabs.Add(new GeneralTab());
            Tabs.Add(new SelfTab());
            Tabs.Add(new VisualTab());
            Tabs.Add(new MiscTab());
            Tabs.Add(new EnemyTab());
            Tabs.Add(new SettingsTab());
            Tabs.Add(new DebugTab());
            GUI.Initialize();
            GUI.OnRender += delegate
            {
                IL2CPP.il2cpp_thread_attach(IL2CPP.il2cpp_domain_get()); 
                Render();
            };
        }

        public List<Tab> Tabs { get; } = [];
        private bool _initialized;
        private int _selectedTabIndex;

        public void Render()
        {
            if (!_initialized)
            {
                _initialized = true;
                PaintTheTownRedMenuMod.Instance.Initialize();
                ApplyStyle();
            }
            if (Cheat.Instance<Panic>() is { Enabled: true }) return;
            VisualUtil.DrawList = ImGui.GetBackgroundDrawList();
            ToggleMenu? toggleMenu = Cheat.Instance<ToggleMenu>();
            FPSCounter? fpsCounter = Cheat.Instance<FPSCounter>();
            VisualUtil.Text(new VisualUtil.TextOptions
            {
                Position = new Vector2(5f, 5f),
                Text = $"Paint The Town Red Menu {PaintTheTownRedMenuMod.Instance.Info.Version}{(fpsCounter is { Enabled: true } ? $" | FPS {fpsCounter.FPS}" : "")} | Menu Toggle {toggleMenu?.Keybind}",
                TextColor = toggleMenu?.ToggleMenuSettings.MenuTextColor ?? new Color(0.30f, 0.62f, 1.00f, 1.00f),
                Outline = true,
                OutlineColor = Color.black,
                OutlineThickness = 1
            });
            if (Cheat.Instance<DebugMode>() is { Enabled: true })
            {
                VisualUtil.Text(new VisualUtil.TextOptions
                {
                    Position = new Vector2(5f, 20f),
                    Text = "[Debug Mode]",
                    TextColor = Color.green,
                    Outline = true,
                    OutlineColor = Color.black,
                    OutlineThickness = 1
                });
            }
            ImGuiIOPtr imguiIO = ImGui.GetIO();
            imguiIO.ConfigFlags &= ~ImGuiConfigFlags.NavEnableKeyboard;
            imguiIO.ConfigFlags &= ~ImGuiConfigFlags.NavEnableGamepad;
            if (toggleMenu is { Enabled: true })
            {
                UIUtil.Area("Paint The Town Red Menu", () =>
                {
                    UIUtil.ChildArea("TabBar", () =>
                    {
                        UIUtil.TabBar("PaintTheTownRedMenuTabBar", () =>
                        {
                            for (int index = 0; index < Tabs.Count; index++)
                            {
                                Tab tab = Tabs[index];
                                if (!tab.Enabled) continue;
                                int selectedTabIndex = index;
                                UIUtil.TabBarItem(tab.Name, () => _selectedTabIndex = selectedTabIndex);
                            }
                        });
                    }, new Vector2(-1, 0),  ImGuiChildFlags.AutoResizeY, ImGuiWindowFlags.NoScrollbar);
                    UIUtil.ChildArea("TabContent", () =>
                    {
                        if (_selectedTabIndex >= 0 && _selectedTabIndex < Tabs.Count) Tabs[_selectedTabIndex].Render();
                    });
                }, new Vector2(800f, 600f), null, ImGuiWindowFlags.NoCollapse, ImGuiCond.FirstUseEver);
            }
            PaintTheTownRedMenuMod.Instance.ToggleCheats.ForEach(tc => tc.OnGUI());
        }

        private static void ApplyStyle()
        {
            ImGuiStylePtr imguiStyle = ImGui.GetStyle();
            imguiStyle.Alpha = 1.0f;
            imguiStyle.DisabledAlpha = 0.55f;
            imguiStyle.WindowPadding = new Vector2(14, 14).ToNumerics();
            imguiStyle.FramePadding = new Vector2(8, 6).ToNumerics();
            imguiStyle.CellPadding = new Vector2(8, 6).ToNumerics();
            imguiStyle.ItemSpacing = new Vector2(8, 8).ToNumerics();
            imguiStyle.ItemInnerSpacing = new Vector2(6, 4).ToNumerics();
            imguiStyle.TouchExtraPadding = Vector2.zero.ToNumerics();
            imguiStyle.IndentSpacing = 22;
            imguiStyle.ColumnsMinSpacing = 8;
            imguiStyle.ScrollbarSize = 12;
            imguiStyle.ScrollbarRounding = 8;
            imguiStyle.GrabMinSize = 10;
            imguiStyle.GrabRounding = 6;
            imguiStyle.WindowRounding = 10;
            imguiStyle.ChildRounding = 8;
            imguiStyle.FrameRounding = 6;
            imguiStyle.PopupRounding = 8;
            imguiStyle.TabRounding = 6;
            imguiStyle.WindowBorderSize = 1;
            imguiStyle.ChildBorderSize = 1;
            imguiStyle.PopupBorderSize = 1;
            imguiStyle.FrameBorderSize = 0;
            imguiStyle.TabBorderSize = 0;
            imguiStyle.WindowTitleAlign = new Vector2(0.5f, 0.5f).ToNumerics();
            imguiStyle.ButtonTextAlign = new Vector2(0.5f, 0.5f).ToNumerics();
            imguiStyle.SelectableTextAlign = new Vector2(0.0f, 0.5f).ToNumerics();
            imguiStyle.SeparatorTextAlign = new Vector2(0.5f, 0.5f).ToNumerics();
            imguiStyle.SeparatorTextBorderSize = 1.0f;
            imguiStyle.SeparatorTextPadding = new Vector2(20, 3).ToNumerics();
            imguiStyle.TabBarBorderSize = 0;
            imguiStyle.TabBarOverlineSize = 3;
            imguiStyle.AntiAliasedLines = true;
            imguiStyle.AntiAliasedLinesUseTex = true;
            imguiStyle.AntiAliasedFill = true;
            RangeAccessor<sVector4> colors = imguiStyle.Colors;
            colors[(int)ImGuiCol.Text] = new Vector4(0.95f, 0.96f, 0.98f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.50f, 0.53f, 0.58f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TextLink] = new Vector4(0.45f, 0.75f, 1.00f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.30f, 0.62f, 1.00f, 0.35f).ToNumerics();
            colors[(int)ImGuiCol.WindowBg] = new Vector4(0.07f, 0.08f, 0.10f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.ChildBg] = new Vector4(0.09f, 0.10f, 0.12f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.PopupBg] = new Vector4(0.10f, 0.11f, 0.14f, 0.98f).ToNumerics();
            colors[(int)ImGuiCol.DockingEmptyBg] = new Vector4(0.08f, 0.09f, 0.11f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.Border] = new Vector4(0.18f, 0.22f, 0.28f, 0.90f).ToNumerics();
            colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f).ToNumerics();
            colors[(int)ImGuiCol.Separator] = new Vector4(0.20f, 0.24f, 0.30f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.30f, 0.62f, 1.00f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.20f, 0.52f, 0.95f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.FrameBg] = new Vector4(0.12f, 0.14f, 0.18f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.17f, 0.22f, 0.32f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.20f, 0.27f, 0.40f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TitleBg] = new Vector4(0.06f, 0.07f, 0.09f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.08f, 0.09f, 0.11f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.06f, 0.07f, 0.09f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.09f, 0.10f, 0.12f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.08f, 0.09f, 0.11f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.20f, 0.24f, 0.31f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.26f, 0.34f, 0.48f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.32f, 0.42f, 0.60f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.Button] = new Vector4(0.13f, 0.16f, 0.21f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.22f, 0.44f, 0.82f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.18f, 0.36f, 0.68f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.Header] = new Vector4(0.14f, 0.18f, 0.24f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.22f, 0.44f, 0.82f, 0.90f).ToNumerics();
            colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.18f, 0.36f, 0.68f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.CheckMark] = new Vector4(0.30f, 0.62f, 1.00f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.30f, 0.62f, 1.00f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.20f, 0.52f, 0.95f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.30f, 0.62f, 1.00f, 0.25f).ToNumerics();
            colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.30f, 0.62f, 1.00f, 0.70f).ToNumerics();
            colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.30f, 0.62f, 1.00f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.Tab] = new Vector4(0.11f, 0.13f, 0.18f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TabHovered] = new Vector4(0.25f, 0.48f, 0.88f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TabSelected] = new Vector4(0.20f, 0.42f, 0.78f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TabSelectedOverline] = new Vector4(0.40f, 0.75f, 1.00f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TabDimmed] = new Vector4(0.08f, 0.10f, 0.12f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TabDimmedSelected] = new Vector4(0.15f, 0.18f, 0.24f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TabDimmedSelectedOverline] = new Vector4(0.30f, 0.62f, 1.00f, 0.60f).ToNumerics();
            colors[(int)ImGuiCol.DockingPreview] = new Vector4(0.30f, 0.62f, 1.00f, 0.70f).ToNumerics();
            colors[(int)ImGuiCol.PlotLines] = new Vector4(0.30f, 0.62f, 1.00f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(0.45f, 0.75f, 1.00f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.30f, 0.62f, 1.00f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(0.45f, 0.75f, 1.00f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TableHeaderBg] = new Vector4(0.10f, 0.12f, 0.16f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TableBorderStrong] = new Vector4(0.22f, 0.26f, 0.34f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TableBorderLight] = new Vector4(0.16f, 0.18f, 0.24f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f).ToNumerics();
            colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(1.00f, 1.00f, 1.00f, 0.02f).ToNumerics();
            colors[(int)ImGuiCol.DragDropTarget] = new Vector4(0.45f, 0.75f, 1.00f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.30f, 0.62f, 1.00f, 1.00f).ToNumerics();
            colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(1.00f, 1.00f, 1.00f, 0.70f).ToNumerics();
            colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.80f, 0.80f, 0.80f, 0.20f).ToNumerics();
            colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.10f, 0.10f, 0.10f, 0.50f).ToNumerics();
        }
    }
}