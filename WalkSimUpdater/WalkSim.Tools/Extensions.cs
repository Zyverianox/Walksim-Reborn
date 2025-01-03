using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace WalkSim.Tools;

public static class Extensions
{
    private static Random rng = new Random();

    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        return component != null ? component : obj.AddComponent<T>();
    }

    public static void Obliterate(this GameObject self)
    {
        Object.Destroy(self);
    }

    public static void Obliterate(this Component self)
    {
        Object.Destroy(self);
    }

    public static float Distance(this Vector3 self, Vector3 other)
    {
        return Vector3.Distance(self, other);
    }

    public static int Wrap(int x, int min, int max)
    {
        int num = max - min;
        int num2 = (x - min) % num;
        if (num2 < 0)
        {
            num2 += num;
        }
        return num2 + min;
    }

    public static float Map(float x, float a1, float a2, float b1, float b2)
    {
        float num = a2 - a1;
        float num2 = b2 - b1;
        float num3 = (x - a1) / num;
        return b1 + num3 * num2;
    }

    public static byte[] StringToBytes(this string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    public static string BytesToString(this byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int num = list.Count;
        while (num > 1)
        {
            num--;
            int index = rng.Next(num + 1);
            T value = list[index];
            list[index] = list[num];
            list[num] = value;
        }
    }
}
