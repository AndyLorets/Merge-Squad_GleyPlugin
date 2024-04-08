using System;
using UnityEditor;
using UnityEngine;

namespace D2D.Utils
{
    public enum ModelImporterMode { Off, Human, NotAnimatedModel, AnimatedModel, AnimatedModelWithAvatar}
    
    [Serializable]
    public struct ToolsIcons
    {
        public Texture dice;
        public Texture advanced;
        public Texture endPoint;
        public Texture relative;
        // public Texture playMode;
        public Texture kill;
        public Texture pong;
        public Texture call;
        public Texture playMode;
        public Texture effects;
        public Texture data;
    }
    
    [Serializable]
    public class ToolsSettings
    {
        public bool IsImporterSupportsAvatars => importerMode == ModelImporterMode.Human ||
                                                 importerMode == ModelImporterMode.AnimatedModelWithAvatar;

        public bool IsImporterOn => importerMode != ModelImporterMode.Off;

        public bool IsImporterSupportsAnimations => importerMode != ModelImporterMode.Off &&
                                                    importerMode != ModelImporterMode.NotAnimatedModel;

        [Header("Palette")] 
            public Vector2 palettePosition = new Vector2(1200, 25);
            public Vector2 paletteSize = new Vector2(300, 835);
        
        [Header("Topbar")] 
            public bool showLeftTopBar = true;
            public bool showMiddleTopBar = true;
            public bool showRightTopBar = true;
        
        
        [Header("Hierarchy keeper")] 
            public bool isKeeperOn = true;
            public string separatorName = "--------------------";
            public bool hideInternalButtons;

        [Header("Shortcuts")]
            public bool focusAlsoOnGameObjects;

        [Header("Palette")]
            [Tooltip("If UI is not ok try to set it to 0.4 on 4k, 0.8 on 1080p")] 
            public float paletteUIWidthFactor = .4f;
            
            [Tooltip("If something wrong try to set it to 1")]
            public int rightButtonId = -1;
            
            public int paletteSpacing1 = 11;
            public int paletteSpace = 8;

        [Header("Animation Importer")] 
            public ModelImporterMode importerMode;
            
            [Tooltip("Avatar used only if is human mode")]
            public Avatar defaultImportAvatar;

        [Space]
        
        public ToolsIcons icons;
    }
}