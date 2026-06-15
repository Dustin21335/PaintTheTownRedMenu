using ImGuiNET;
using System;
using UnityEngine;
// ReSharper disable MemberHidesStaticFromOuterClass

namespace PaintTheTownRedMenu.Utils
{
    public static unsafe class VisualUtil
    {
        public static ImDrawListPtr DrawList { get; set; }

        public struct TextOptions
        {
            public Vector2 Position;
            public string Text;
            public Color TextColor;
            public bool Centered;
            public bool Outline;
            public Color OutlineColor;
            public int OutlineThickness;
        }

        public static void Text(TextOptions options)
        {
            if ((IntPtr)DrawList.NativePtr == IntPtr.Zero) return;
            Vector2 position = options.Position;
            if (options.Centered)
            {
                position.x -= options.Text.Length * 3f;
                position.y -= 6f;
            }
            if (options.Outline)
            {
                uint outlineColor = ImGui.ColorConvertFloat4ToU32(options.OutlineColor.ToVector4());
                int outlineThickness = options.OutlineThickness;
                for (int x = -outlineThickness; x <= outlineThickness; x++)
                {
                    for (int y = -outlineThickness; y <= outlineThickness; y++)
                    {
                        if (x == 0 && y == 0) continue;
                        DrawList.AddText((position + new Vector2(x, y)).ToNumerics(), outlineColor, options.Text);
                    }
                }
            }
            DrawList.AddText(position.ToNumerics(), ImGui.ColorConvertFloat4ToU32(options.TextColor.ToVector4()), options.Text);
        }

        public struct CircleOptions
        {
            public Vector2 Position;
            public float Radius;
            public Color Color;
            public bool Filled;
            public float Thickness;
        }

        public static void Circle(CircleOptions options)
        {
            if ((IntPtr)DrawList.NativePtr == IntPtr.Zero) return;
            uint color = ImGui.ColorConvertFloat4ToU32(options.Color.ToVector4());
            if (options.Filled) DrawList.AddCircleFilled(options.Position.ToNumerics(), options.Radius, color);
            else DrawList.AddCircle(options.Position.ToNumerics(), options.Radius, color, 0, options.Thickness);
        }

        public struct SquareOptions
        {
            public Vector2 Position;
            public Vector2 Size;
            public Color Color;
            public bool Filled;
            public float Thickness;
        }

        public static void Square(SquareOptions options)
        {
            if ((IntPtr)DrawList.NativePtr == IntPtr.Zero) return;
            uint color = ImGui.ColorConvertFloat4ToU32(options.Color.ToVector4());
            Vector2 halfSize = options.Size / 2f;
            Vector2 topLeft = options.Position - halfSize;
            Vector2 bottomRight = options.Position + halfSize;
            if (options.Filled) DrawList.AddRectFilled(topLeft.ToNumerics(), bottomRight.ToNumerics(), color);
            else DrawList.AddRect(topLeft.ToNumerics(), bottomRight.ToNumerics(), color, 0f, ImDrawFlags.None, options.Thickness);
        }
    }
}