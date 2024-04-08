using UnityEngine;

namespace D2D.Utilities
{
    /// <summary>
    /// Useful to put store lots of object in folder (to avoid scene flood)
    /// For such objects like enemies, bullets, particles, etc.
    /// </summary>
    public static class TransformFolders
    {
        // Example of transform folder usage
        public static Transform Projectiles => Get(nameof(Projectiles));

        private static Transform Get(string name)
        {
            Transform folder = GameObject.Find(name)?.transform;
            
            // Create folder if not exists
            if (folder == null)
                folder = new GameObject(name + " [created at runtime]").transform;
            
            return folder;
        }
    }
}