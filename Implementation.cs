using MelonLoader;
using UnityEngine;
using Il2CppInterop;
using Il2CppInterop.Runtime.Injection; 
using System.Collections;
using Il2CppTLD.Gear;
using Il2Cpp;
using Il2CppTLD.IntBackedUnit;
using Il2CppVLB;
using Il2CppSystem;
using ModData;
using Il2CppSWS;
using Harmony;
using System.Text.Json;
using System.Text.Json.Serialization;
using Action = System.Action;
using Unity.VisualScripting;


namespace TheLongRemaster
{
	public class TLRMelon : MelonMod
	{

        private static AssetBundle? assetBundle;

        internal static AssetBundle TLRBundle
        {
            get => assetBundle ?? throw new System.NullReferenceException(nameof(assetBundle));
        }


        public override void OnInitializeMelon()
		{
            assetBundle = LoadAssetBundle("TLR.tlrbundle");
            Settings.instance.AddToModSettings("TLR");
            uConsole.RegisterCommand("countGameObjects", new Action(Console_countItems));
            uConsole.RegisterCommand("swapObjectsTLR", new Action(ExecuteMeshSwap));
            if(Settings.instance.DebugObjects)
            {
                MelonLogger.Msg("Registered TLR commands.");
            }

        }


        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
		{
            bool done = false;

            if (GameManager.IsMainMenuActive())
            {

            }


            if (Utils.IsMainScene(sceneName) && !done)
			{
                ExecuteMeshSwap();
                done = true;
            }
		}

        private static AssetBundle LoadAssetBundle(string path)
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            MemoryStream memoryStream = new MemoryStream((int)stream.Length);
            stream.CopyTo(memoryStream);

            return memoryStream.Length != 0
                ? AssetBundle.LoadFromMemory(memoryStream.ToArray())
                : throw new System.Exception("No data loaded!");
        }

        public static void Console_countItems()
        {
            GameObject[] SceneObjects = GameManager.FindObjectsOfType<GameObject>();
            Utils.LogObjCount(SceneObjects);
            uConsole.Log("Found objects in scene: " + SceneObjects.Length);
        }

        public static void ExecuteMeshSwap()
        {
            Material? mat;
            MeshFilter[]? filters;
            MeshRenderer[]? renderers;

            GameObject[] SceneObjects = GameManager.FindObjectsOfType<GameObject>();
            Utils.LogObjCount(SceneObjects);

            // Metal Frame Bed

            foreach (GameObject obj in SceneObjects)
            {
                if (Utils.CompareName(obj.name, "INTERACTIVE_TwinBedB")) 
                {
                    if(obj.GetComponent<MeshFilter>() != null)
                    {
                        Utils.LogObjSwap(obj.name);
                        mat = Utils.MakeTLDMatWithTexture("T_MetalBedFrameTLR_D");
                        obj.GetComponent<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_MetalBedFrameSingleTLR");
                        obj.GetComponent<MeshRenderer>().material = mat;
                        filters = obj.GetComponentsInChildren<MeshFilter>();
                        foreach (MeshFilter f in filters)
                        {
                            if (f.gameObject.name.Contains("OBJ_MetalBedMatressSingle"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_MetalBedFrameMattressSingleTLR");
                            }
                        }
                        renderers = obj.GetComponentsInChildren<MeshRenderer>();

                        foreach (MeshRenderer r in renderers)
                        {
                            if (r.gameObject.name.Contains("OBJ_MetalBedMatressSingle"))
                            {
                                r.material = mat;
                            }
                        }
                    }
                }

                //Blue Armchair

                if (Utils.CompareName(obj.name, "Obj_ChairA_Prefab")) 
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_ChairATLR_D");
                    filters = Utils.GrabObjChildrenFilters(obj);
                    renderers = Utils.GrabObjChildrenRenderers(obj);
                    foreach(MeshRenderer r in renderers)
                    {
                        if(r.gameObject.name.Contains("ChairA"))
                        {
                            r.material = mat;
                        }
                    }
                    foreach(MeshFilter f in filters)
                    {
                        if(f.gameObject.name.Contains("ChairA"))
                        {
                            f.mesh = Utils.LoadTLRMesh("SM_ChairATLR");
                        }
                    }
                }


                if(Utils.CompareName(obj.name,"MetalLockerC")) // Metal Locker Triple
                {
                    Utils.LogObjSwap(obj.name);
                    if(obj.GetComponent<MeshRenderer>() != null) 
                    {
                        mat = Utils.MakeTLDMatWithTexture("T_LockersTLR_D");
                        Material mat2 = Utils.MakeTLDMatWithTexture("T_LockersBTLR_D");
                        obj.GetComponent<MeshRenderer>().material = mat;
                        obj.GetComponent<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_LockerTripleTLR");
                        filters = Utils.GrabObjChildrenFilters(obj);
                        renderers = Utils.GrabObjChildrenRenderers(obj);
                        foreach(MeshFilter f in filters)
                        {
                            if (f.gameObject.name.Contains("DoorC1"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_LockerDoorATLR");
                            }
                            if (f.gameObject.name.Contains("DoorC2"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_LockerDoorBTLR");
                            }
                            if (f.gameObject.name.Contains("DoorC3"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_LockerDoorCTLR");
                            }
                        }
                        foreach(MeshRenderer r in renderers)
                        {

                            if(r.gameObject.name.Contains("DoorC2"))
                            {
                                r.material = mat2;
                            }
                            else
                            {
                                r.material = mat;
                            }
                        }
                    }
                }
                if (Utils.CompareName(obj.name, "MetalLockerB")) // Metal Locker Double
                {
                    Utils.LogObjSwap(obj.name);
                    if (obj.transform.childCount > 0)
                    {
                        mat = Utils.MakeTLDMatWithTexture("T_LockersTLR_D");
                        Material mat2 = Utils.MakeTLDMatWithTexture("T_LockersBTLR_D");
                        filters = Utils.GrabObjChildrenFilters(obj);
                        renderers = Utils.GrabObjChildrenRenderers(obj);
                        foreach(MeshFilter f in filters)
                        {
                            if(f.gameObject.name.Contains("MetalLockerB"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_LockerDoubleTLR");
                            }
                            if(f.gameObject.name.Contains("DoorB1"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_LockerDoorATLR");
                            }
                            if(f.gameObject.name.Contains("DoorB2"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_LockerDoorCTLR");
                            }
                        }
                        foreach(MeshRenderer r in renderers)
                        {
                            if(r.gameObject.name.Contains("DoorB2"))
                            {
                                r.material = mat2;
                            }
                            else
                            {
                                r.material = mat;
                            }
                        }


                    }
                }
                if(Utils.CompareName(obj.name,"OBJ_MetalLockerDoorA1_Prefab") || Utils.CompareName(obj.name,"OBJ_MetalLockerDoorA_Prefab")) // Metal Locker Door (breakable)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_LockersBTLR_D");
                    filters = Utils.GrabObjChildrenFilters(obj);
                    renderers = Utils.GrabObjChildrenRenderers(obj);
                    foreach (MeshFilter f in filters)
                    {
                        f.mesh = Utils.LoadTLRMesh("SM_LockerDoorATLR");
                    }
                    foreach (MeshRenderer r in renderers)
                    {
                        r.material = mat;
                    }
                }
                if(Utils.CompareName(obj.name,"CONTAINER_MetalLockerA")) // Metal Locker Single
                {
                    if(obj.transform.childCount > 1)
                    {
                        Utils.LogObjSwap(obj.name);
                        mat = Utils.MakeTLDMatWithTexture("T_LockersTLR_D");
                        filters = Utils.GrabObjChildrenFilters(obj);
                        renderers = Utils.GrabObjChildrenRenderers(obj);
                        foreach(MeshFilter f in filters) 
                        {
                            if(f.gameObject.name.Contains("Door"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_LockerDoorCTLR");
                            }
                            if(f.gameObject.name.Contains("MetalLockerA"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_LockerSingleTLR");
                            }
                        }
                        foreach(MeshRenderer r in renderers)
                        {
                                r.material = mat;
                        }
                    }
                }
                if (Utils.CompareName(obj.name, "OBJ_MetalLockerA_Prefab"))  // Metal Locker (breakable)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_LockersTLR_D");
                    filters = Utils.GrabObjChildrenFilters(obj);
                    renderers = Utils.GrabObjChildrenRenderers(obj);
                    foreach (MeshFilter f in filters)
                    {
                        f.mesh = Utils.LoadTLRMesh("SM_LockerSingleTLR");
                    }
                    foreach (MeshRenderer r in renderers)
                    {
                        r.material = mat;
                    }
                }
                if (Utils.CompareName(obj.name,"INTERACTIVE_BunkBed") && !obj.name.Contains("LOD")) // Bunk Bed Wooden
                {
                    mat = Utils.MakeTLDMatWithTexture("T_BunkBedTLR_D");
                    filters = Utils.GrabObjChildrenFilters(obj);
                    renderers = Utils.GrabObjChildrenRenderers(obj);
                    foreach (MeshFilter f in filters)
                    {
                        if(f.gameObject.name.Contains("BedFrame"))
                        {
                            f.mesh = Utils.LoadTLRMesh("SM_BunkBed_MattressTLR");
                        }
                        if(f.gameObject.name.Contains("BedBedding"))
                        {
                            f.mesh = Utils.LoadTLRMesh("SM_BunkBed_BeddingTLR");
                        }
                        if(f.gameObject.name.Contains("INTERACTIVE_BunkBed_LOD0"))
                        {
                            f.mesh = Utils.LoadTLRMesh("SM_BunkBed_FrameTLR");
                        }
                    }
                    foreach (MeshRenderer r in renderers)
                    {
                        r.material = mat;
                    }
                }
                if(Utils.CompareName(obj.name,"CONTAINER_MetalFileCabinetA")) //Filing Cabinet
                {
                    if (obj.transform.childCount > 1)
                    {
                        Utils.LogObjSwap(obj.name);
                        mat = Utils.MakeTLDMatWithTexture("T_FileCabinetATLR_D");
                        filters = Utils.GrabObjChildrenFilters(obj);
                        renderers = Utils.GrabObjChildrenRenderers(obj);
                        foreach (MeshFilter f in filters)
                        {
                            if (f.gameObject.name.Contains("DrawerA1"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_MetalDrawerATLR");
                            }
                            if (f.gameObject.name.Contains("DrawerA2"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_MetalDrawerBTLR");
                            }
                            if (f.gameObject.name.Contains("DrawerA3"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_MetalDrawerCTLR");
                            }
                            if (f.gameObject.name.Contains("DrawerA4")) 
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_MetalDrawerDTLR");
                            }
                            if(f.gameObject.name.Contains("FileCabinetA"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_FileCabinetTLR");
                            }
                        }
                        foreach (MeshRenderer r in renderers)
                        {
                            r.material = mat;
                        }
                    }
                }
                if (Utils.CompareName(obj.name, "OBJ_MetalFileCabinetB_Prefab") || Utils.CompareName(obj.name, "OBJ_MetalCabinetA_Prefab")) //Broken cabinets
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_FileCabinetATLR_D");
                    filters = Utils.GrabObjChildrenFilters(obj);
                    renderers = Utils.GrabObjChildrenRenderers(obj);
                    foreach(MeshFilter f in filters)
                    {
                        f.mesh = Utils.LoadTLRMesh("SM_FileCabinetTLR");
                    }
                    foreach(MeshRenderer r in renderers)
                    {
                        r.material = mat;
                    }
                }
                if (Utils.CompareName(obj.name, "OBJ_MetalFileCabinetDrawerA2_Prefab")) //Broken cabinet drawers
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_FileCabinetATLR_D");
                    filters = Utils.GrabObjChildrenFilters(obj);
                    renderers = Utils.GrabObjChildrenRenderers(obj);
                    foreach (MeshFilter f in filters)
                    {
                        f.mesh = Utils.LoadTLRMesh("SM_MetalDrawerATLR");
                    }
                    foreach (MeshRenderer r in renderers)
                    {
                        r.material = mat;
                    }
                }
                if(Utils.CompareName(obj.name,"OBJ_MetalFileCabinet_Prefab")) // Broken cabinet (full)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_FileCabinetATLR_D");
                    filters = Utils.GrabObjChildrenFilters(obj);
                    renderers = Utils.GrabObjChildrenRenderers(obj);
                    foreach (MeshFilter f in filters)
                    {
                        if(f.gameObject.name.Contains("MetalFileCabinetDrawerA1"))
                        {
                            f.mesh = Utils.LoadTLRMesh("SM_MetalDrawerATLR");
                        }
                        if (f.gameObject.name.Contains("MetalFileCabinetDrawerA2"))
                        {
                            f.mesh = Utils.LoadTLRMesh("SM_MetalDrawerBTLR");
                        }
                        if (f.gameObject.name.Contains("MetalFileCabinetDrawerA3"))
                        {
                            f.mesh = Utils.LoadTLRMesh("SM_MetalDrawerCTLR");
                        }
                        if (f.gameObject.name.Contains("MetalFileCabinetDrawerA4"))
                        {
                            f.mesh = Utils.LoadTLRMesh("SM_MetalDrawerDTLR");
                        }
                        if(f.gameObject.name.Contains("MetalFileCabinet_LOD0"))
                        {
                            f.mesh = Utils.LoadTLRMesh("SM_FileCabinetTLR");
                        }
                    }
                    foreach(MeshRenderer r in renderers)
                    {
                        r.material = mat;
                    }
                }

                //Dishes

                if(Utils.CompareName(obj.name,"OBJ_CupA_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDGLassMatWithTexture("T_CupA_GlassTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_GlassATLR");
                }
                if(Utils.CompareName(obj.name,"BowlC_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishBowlCTLR");
                }
                if(Utils.CompareName(obj.name, "BowlB_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishBowlBTLR");
                }
                if (Utils.CompareName(obj.name, "BowlBBroken_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishBowlBBrokenTLR");
                }
                if (Utils.CompareName(obj.name, "BowlA_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null || Utils.CompareName(obj.name,"BowlA1_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishBowlATLR");
                }
                if(Utils.CompareName(obj.name,"BowlA2_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishBowlBTLR");
                }
                if(Utils.CompareName(obj.name,"OBJ_DishPlateA_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null || Utils.CompareName(obj.name,"PlateA1_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishPlateATLR");
                }
                if(Utils.CompareName(obj.name,"PlateB1_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishPlateB1TLR");
                }
                if (Utils.CompareName(obj.name, "PlateB2_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishPlateB2TLR");
                }
                if (Utils.CompareName(obj.name, "PlateB3_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishPlateB3TLR");
                }
                if (Utils.CompareName(obj.name,"PlateABroken_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null || Utils.CompareName(obj.name, "PlateA1Broken_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishPlateABrokenTLR");
                }
                if(Utils.CompareName(obj.name, "PlateB1Broken_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishPlateB1BrokenTLR");
                }
                if(Utils.CompareName(obj.name, "PlateB2Broken_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishPlateB2BrokenTLR");
                }
                if (Utils.CompareName(obj.name, "PlateB3Broken_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishPlateB3BrokenTLR");
                }
                if(Utils.CompareName(obj.name,"DishServeB1_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishServeB1TLR");
                }
                if (Utils.CompareName(obj.name, "DishServeB2_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                        obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishServeB2TLR");
                }
                if (Utils.CompareName(obj.name, "DishServeB3_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_DishesTLR_D");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                        obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_DishServeB3TLR");

                }

                // Pilow 1

                if (Utils.CompareName(obj.name,"WhalingShipBedPillow_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null) 
                {
                        Utils.LogObjSwap(obj.name);
                        mat = Utils.MakeTLDMatWithTexture("T_PillowCaseTLR_D");
                        obj.GetComponent<MeshRenderer>().material = mat;
                        obj.GetComponent<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_PillowTLROffset");
                }

                // Curtains 1

                if (Utils.CompareName(obj.name, "CurtainA2_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_CurtainsATLR_D");
                        obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_CurtainATLR");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                }
                if (Utils.CompareName(obj.name,"CurtainB2_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_CurtainsATLR_D");
                        obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_CurtainBTLR");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                }
                if (Utils.CompareName(obj.name, "CurtainC2_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_CurtainsATLR_D");
                        obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_CurtainCTLR");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                }
                if (Utils.CompareName(obj.name, "CurtainD2_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_CurtainsATLR_D");
                        obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_CurtainDTLR");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                }
                if (Utils.CompareName(obj.name, "CurtainE2_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_CurtainsATLR_D");
                        obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_CurtainETLR");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                }
                if (Utils.CompareName(obj.name, "CurtainF2_Prefab") && obj.GetComponentInChildren<MeshFilter>() != null)
                {
                    Utils.LogObjSwap(obj.name);
                    mat = Utils.MakeTLDMatWithTexture("T_CurtainsATLR_D");

                      obj.GetComponentInChildren<MeshFilter>().mesh = Utils.LoadTLRMesh("SM_CurtainFTLR");
                        obj.GetComponentInChildren<MeshRenderer>().material = mat;
                    
                }
                // Metal Desk
                if(Utils.CompareName(obj.name, "CONTAINER_MetalDesk"))
                {
                    if(obj.transform.GetChild(0).name.Contains("CONTAINER_MetalDesk")) // Find child object with same name as parent
                    {
                        Utils.LogObjSwap(obj.name);
                        mat = Utils.MakeTLDMatWithTexture("T_MetalDeskTLR_D");
                        filters = Utils.GrabObjChildrenFilters(obj);
                        renderers = Utils.GrabObjChildrenRenderers(obj);
                        foreach(MeshFilter f in filters)
                        {
                            if(f.gameObject.name.Contains("CONTAINER_MetalDesk"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_MetalDeskTLR");
                            }
                            if (f.gameObject.name.Contains("OBJ_MetalDeskDrawer1"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_MetalDeskDrawer1TLR");
                            }
                            if (f.gameObject.name.Contains("OBJ_MetalDeskDrawer2"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_MetalDeskDrawer2TLR");
                            }
                            if (f.gameObject.name.Contains("OBJ_MetalDeskDrawer3"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_MetalDeskDrawer3TLR");
                            }
                            if (f.gameObject.name.Contains("OBJ_MetalDeskDrawer4"))
                            {
                                f.mesh = Utils.LoadTLRMesh("SM_MetalDeskDrawer4TLR");
                            }
                        }
                        foreach(MeshRenderer r in renderers)
                        {
                            r.material = mat;
                        }
                    }
                }



            }
        }

    }
}