using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace Release_Status
{
    public class Main : MelonLoader.MelonMod
    {
        private static HarmonyMethod GetPatch(string name) => new HarmonyMethod(typeof(Main).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        public override void OnApplicationStart()
        {
            HarmonyInstance harmonyInstance = HarmonyInstance.Create("oga boga");
            harmonyInstance.Patch(typeof(PipelineManager).GetMethod("Start", BindingFlags.Public | BindingFlags.Instance),GetPatch("PipePatch"));
            harmonyInstance.Patch(typeof(UiAvatarList).GetMethods().Where(x => x.GetParameters().Length == 2 && x.GetParameters().First().ParameterType == typeof(VRCUiContentButton)).FirstOrDefault(), GetPatch("UIListPatch"));
        }
        private static void PipePatch(PipelineManager __instance)
        {
            try
            {
                if (__instance.contentType == PipelineManager.ContentType.avatar && __instance.blueprintId != "avtr_749445a8-d9bf-4d48-b077-d18b776f66f7")
                {
                    VRCPlayer ply = __instance.GetComponentInParent<VRCPlayer>();
                    if (!ply.transform.FindChild("Player Nameplate/Canvas/Nameplate/Contents/Main/Text Container/Release-Text"))
                    {
                        GameObject SubbyText = ply.transform.FindChild("Player Nameplate/Canvas/Nameplate/Contents/Main/Text Container/Sub-Text").gameObject;
                        GameObject ReleaseStatus = GameObject.Instantiate(SubbyText, SubbyText.transform.parent);
                        ReleaseStatus.name = "Release-Text";
                        ReleaseStatus.SetActive(true);
                    }
                    if (ply.prop_ApiAvatar_0.releaseStatus == "public")
                    {

                        TextMeshProUGUI textmesh = ply.transform.FindChild("Player Nameplate/Canvas/Nameplate/Contents/Main/Text Container/Release-Text").GetComponent<TextMeshProUGUI>();
                        textmesh.color = Color.green;
                        textmesh.text = ply.prop_ApiAvatar_0.releaseStatus;
                    }
                    else
                    {

                        TextMeshProUGUI textmesh = ply.transform.FindChild("Player Nameplate/Canvas/Nameplate/Contents/Main/Text Container/Release-Text").GetComponent<TextMeshProUGUI>();
                        textmesh.color = Color.red;
                        textmesh.text = ply.prop_ApiAvatar_0.releaseStatus;
                    }
                }
            }
            catch
            {
            }
        }
        private static bool UIListPatch(VRCUiContentButton __0, Il2CppSystem.Object __1)
        {
            if (__1.Cast<ApiAvatar>().releaseStatus == "public")
            {
                __0.transform.Find("TitleText").GetComponent<Text>().color = Color.green;
            }
            else
            {
                __0.transform.Find("TitleText").GetComponent<Text>().color = Color.red;
            }
            return true;
        }
    }
}
