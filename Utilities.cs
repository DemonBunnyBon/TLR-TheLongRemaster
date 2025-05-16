using UnityEngine;
using Il2Cpp;
using MelonLoader;
using UnityEngine.AddressableAssets;
using System;
using System.Text.RegularExpressions;

namespace TheLongRemaster
{
    internal static class Utils
    {

        public static bool IsMainScene(string scene)
        {
            return !(string.IsNullOrEmpty(scene) || scene.Contains("MainMenu") || scene == "Boot" || scene == "Empty" || scene.Contains("_SANDBOX") || scene.Contains("_DLC01"));
        }

        public static bool CompareName(string name, string compare)
        {
            string nameSanitized = SanitizeObjectName(name);

            if (nameSanitized == compare || nameSanitized.ToLowerInvariant().Contains(compare.ToLowerInvariant()))
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public static Material MakeTLDMatWithTexture(string TLRTextureName)
        {
            Material mat = new(GearItem.LoadGearItemPrefab("GEAR_Stick").gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material);
            mat.name = TLRTextureName + "_mat";
            mat.mainTexture = TLRMelon.TLRBundle.LoadAsset<Texture2D>(TLRTextureName);
            return mat;
        }

        public static Material MakeTLDGLassMatWithTexture(string TLRTextureName)
        {
            Material mat = new(GearItem.LoadGearItemPrefab("GEAR_MagnifyingLens").gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().materials[1]);
            mat.name = TLRTextureName + "_Glass_mat";
            mat.mainTexture = TLRMelon.TLRBundle.LoadAsset<Texture2D>(TLRTextureName);
            return mat;
        }

        public static Mesh LoadTLRMesh(string TLRMeshName)
        {
            return TLRMelon.TLRBundle.LoadAsset<Mesh>(TLRMeshName);
        }

        public static string SanitizeObjectName(string s)
        {
            s = Regex.Replace(s, @"\s|\(.*?\)", ""); // remove spaces, digits and anything within ()
            s = s.Trim(); // remove leading and trailing spaces
            s = Regex.Replace(s, @"\d+$", ""); // remove trailing digits
            return s;
        }
        public static void LogObjSwap(string obj)
        {
            if (Settings.instance.DebugObjects)
            {
                MelonLogger.Msg("Swap object for remaster: " + obj);
            }
        }

        public static void LogObjCount(GameObject[] list)
        {
            if (Settings.instance.DebugObjects)
            {
                MelonLogger.Msg("Found objects in scene: " + list.Length);
            }

        }

        public static MeshFilter[] GrabObjChildrenFilters(GameObject obj)
        {
            return obj.GetComponentsInChildren<MeshFilter>();
        }
        public static MeshRenderer[] GrabObjChildrenRenderers(GameObject obj)
        {
            return obj.GetComponentsInChildren<MeshRenderer>();
        }
    }






}