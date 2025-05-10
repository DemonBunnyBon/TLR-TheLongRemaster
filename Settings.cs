using ModSettings;

namespace TheLongRemaster
{
    internal class Settings : JsonModSettings
    {
        internal static Settings instance = new Settings();

        [Section("Debug Logging")]
        [Name("Enable debug object logging.")]
        [Description("Enables debug messages in the Melon log related to objects with swapped models and textures. This should be left off unless you know what you're doing.")]
        public bool DebugObjects = false;

        [Section("Reset Settings")]
        [Name("Reset To Default")]
        [Description("Resets all settings to Default. (Confirm and Scene Transition/Reload required.)")]
        public bool ResetSettings = false;


        protected override void OnConfirm()
        {
            ApplyReset();
            instance.ResetSettings = false;
            base.OnConfirm();
            base.RefreshGUI();
        }

        public static void ApplyReset()
        {
            if (instance.ResetSettings == true)
            {

                instance.ResetSettings = false;
            }
        }
    }


}
