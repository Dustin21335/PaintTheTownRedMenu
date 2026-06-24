using Il2Cpp;
using ImGuiNET;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Color = System.Drawing.Color;
using Il2CppSystemType = Il2CppSystem.Type;
using Il2CppType = Il2CppInterop.Runtime.Il2CppType;
using Type = System.Type;
using UColor = UnityEngine.Color;
using UVector2 = UnityEngine.Vector2;
using UVector3 = UnityEngine.Vector3;
using UVector4 = UnityEngine.Vector4;
using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;
using Vector4 = System.Numerics.Vector4;

namespace PaintTheTownRedMenu
{
    public static class Extensions
    {
        extension(Color color)
        {
            public Vector4 ToVector4()
            {
                return new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
            }
        }
    
        extension(UColor color)
        {
            public Vector4 ToVector4()
            {
                return new Vector4(color.r, color.g, color.b, color.a);
            }
        }

        extension(UVector2 vector2)
        {
            public Vector2 ToNumerics()
            {
                return new Vector2(vector2.x, vector2.y);
            }
        }

        extension(Vector2 vector2)
        {
            public UVector2 ToUnity()
            {
                return new UVector2(vector2.X, vector2.Y);
            }
        }

        extension(Vector3 vector3)
        {
            public UVector3 ToUnity()
            {
                return new UVector3(vector3.X, vector3.Y, vector3.Z);
            }
        }

        extension(UVector4 vector4)
        {
            public Vector4 ToNumerics()
            {
                return new Vector4(vector4.x, vector4.y, vector4.z, vector4.w);
            }

            public UColor ToColor()
            {
                return new UColor(vector4.x, vector4.y, vector4.z, vector4.w);
            }
        }

        extension(Vector4 vector4)
        {
            public UVector4 ToUnity()
            {
                return new UVector4(vector4.X, vector4.Y, vector4.Z, vector4.W);
            }

            public UColor ToColor()
            {
                return new UColor(vector4.X, vector4.Y, vector4.Z, vector4.W);
            }
        }

        extension(ImGuiKey imGuiKey)
        {
            public KeyCode ToUnity()
            {
                if (imGuiKey is >= ImGuiKey._0 and <= ImGuiKey._9) return KeyCode.Alpha0 + (imGuiKey - ImGuiKey._0);
                return Enum.GetValues<KeyCode>().FirstOrDefault(kc => imGuiKey.ToString() == kc.ToString());
            }
        }

        extension(Type? type)
        {
            public Il2CppSystemType? ToIl2Cpp()
            {
                return type == null ? null : Il2CppType.From(type);
            }
        }

        extension(WeaponBase weaponBase)
        {
            public bool IsGunWeapon()
            {
                return weaponBase.isGun || weaponBase.GetComponent<WeaponGun>() != null || weaponBase.name.ToLower().Contains("gun");
            }

            public bool IsShield()
            {
                return weaponBase.isShield || weaponBase.GetComponent<Shield>() != null;
            }

            public bool IsMelee()
            {
                return !weaponBase.IsGun() && !weaponBase.IsShield();
            }

            public string GetName()
            {
                return Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(weaponBase.name, @"\(\s*\d+\s*\)", ""), @"\(Clone\)", ""), @"\d+", ""), "-", ""), @"(?<=[a-z])(?=[A-Z])", " ").Trim();
            }
        }

        extension(Enemy enemy)
        {
            public string GetName()
            {
                return Regex.Replace(enemy.eAppearance.ToString().Split('_').Last(), @"(?<!^)([A-Z0-9])", " $1");
            }
        }

        extension(EnemySettings enemySettings)
        {
            public string GetName()
            {
                return Regex.Replace(Regex.Replace(enemySettings.name.Replace("Settings", "").Split('_').Last().Replace("-", ""), @"(?<=[a-z0-9])(?=[A-Z])|(?<=\d)(?=[A-Za-z])|(?<=[A-Za-z])(?=\d)", " "), @"\b0+(\d+)\b", "$1").Trim();
            }
        }

        extension(WorldButton worldButton)
        {
            public string GetName()
            {
                return Regex.Replace(Regex.Replace(Regex.Replace(worldButton.name, @"\d+", ""), "_", ""), @"(?<=[a-z])(?=[A-Z])", " ").Trim();
            }
        }
    }
}