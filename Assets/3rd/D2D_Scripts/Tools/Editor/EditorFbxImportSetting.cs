using D2D.Utilities;
using D2D.Utils;
using UnityEditor;
using UnityEngine;

namespace D2D
{
    public class EditorFbxImportSetting : AssetPostprocessor 
    {
        private void OnPreprocessModel() 
        {
            var tools = CoreSettings.Instance.tools;
            ModelImporter importer = assetImporter as ModelImporter;
            if (importer == null || !tools.IsImporterOn)
                return;

            string assetName = importer.assetPath.ToLower();

            // We dont needed by default lights and cameras
            importer.importCameras = false;
            importer.importLights = false;

            // If name consists "t-name" => make it self avatar and remember avatar
            if (IsTPose(assetName))
            {
                if (tools.IsImporterSupportsAvatars)
                {
                    importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
                    tools.defaultImportAvatar = importer.sourceAvatar;
                }
            }
            else
            {
                if (tools.IsImporterSupportsAnimations)
                {
                    importer.importAnimation = true;
                    
                    // Animation type (human / generic)
                    importer.animationType = tools.importerMode == ModelImporterMode.Human ?
                        ModelImporterAnimationType.Human : 
                        ModelImporterAnimationType.Generic;

                    // Install avatar from t-pose
                    if (tools.defaultImportAvatar != null && importer.animationType == ModelImporterAnimationType.Human)
                    {
                        importer.avatarSetup = ModelImporterAvatarSetup.CopyFromOther;
                        importer.sourceAvatar = tools.defaultImportAvatar;
                    }
                }
            }

            importer.SaveAndReimport();
        }

        private void OnPostprocessModel (GameObject g)
        {
            var tools = CoreSettings.Instance.tools;
            ModelImporter importer = assetImporter as ModelImporter;
            string name = importer.assetPath.ToLower();
            if (importer == null || tools.IsImporterOn == false || 
                IsTPose(name) || tools.IsImporterSupportsAnimations == false)
            {
                return;
            }

            var clips = new ModelImporterClipAnimation[importer.defaultClipAnimations.Length];
     
            for (int i = 0; i < importer.defaultClipAnimations.Length; i++)
            {
                clips[i] = new ModelImporterClipAnimation
                {
                    cycleOffset = importer.defaultClipAnimations[i].cycleOffset,
                    events = importer.defaultClipAnimations[i].events,
                    heightFromFeet = importer.defaultClipAnimations[i].heightFromFeet,
                    heightOffset = importer.defaultClipAnimations[i].heightOffset,
                    keepOriginalOrientation = importer.defaultClipAnimations[i].keepOriginalOrientation,
                    keepOriginalPositionXZ = importer.defaultClipAnimations[i].keepOriginalPositionXZ,
                    keepOriginalPositionY = importer.defaultClipAnimations[i].keepOriginalPositionY,
                    lockRootHeightY = importer.defaultClipAnimations[i].lockRootHeightY,
                    lockRootPositionXZ = importer.defaultClipAnimations[i].lockRootPositionXZ,
                    lockRootRotation = importer.defaultClipAnimations[i].lockRootRotation,
                    loopPose = importer.defaultClipAnimations[i].loopPose,
                    maskSource = importer.defaultClipAnimations[i].maskSource,
                    maskType = importer.defaultClipAnimations[i].maskType,
                    mirror = importer.defaultClipAnimations[i].mirror,
                    rotationOffset = importer.defaultClipAnimations[i].rotationOffset,
                    takeName = importer.defaultClipAnimations[i].takeName,
                    curves = importer.defaultClipAnimations[i].curves,
                    name = importer.defaultClipAnimations[i].name,
                    firstFrame = importer.defaultClipAnimations[i].firstFrame,
                    lastFrame = importer.defaultClipAnimations[i].lastFrame,
                    loop = true,
                    loopTime = true,
                    wrapMode = WrapMode.Loop
                };
            }
     
            importer.clipAnimations = clips;
            Debug.Log("Updated " + importer.defaultClipAnimations.Length + " animations");
        }

        private static bool IsTPose(string name) =>
            name.StartsWith("_tpose") || 
            name.StartsWith("_t_pose") || 
            name.StartsWith("t_pose") || 
            name.StartsWith("tpose");
    }
}