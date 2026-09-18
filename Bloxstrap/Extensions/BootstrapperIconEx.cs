using System.Drawing;

namespace Bloxstrap.Extensions
{
    static class BootstrapperIconEx
    {
        public static IReadOnlyCollection<BootstrapperIcon> Selections => new BootstrapperIcon[]
        {
            BootstrapperIcon.IconBubblestrap,
            BootstrapperIcon.IconBloxstrapClassic,
            BootstrapperIcon.IconCustom
        };

        // small note on handling icon sizes
        // i'm using multisize icon packs here with sizes 16, 24, 32, 48, 64 and 128
        // use this for generating multisize packs: https://www.aconvert.com/icon/

        public static Icon GetIcon(this BootstrapperIcon icon)
        {
            const string LOG_IDENT = "BootstrapperIconEx::GetIcon";

            // load the custom icon file
            if (icon == BootstrapperIcon.IconCustom)
            {
                Icon? customIcon = null;
                string location = App.Settings.Prop.BootstrapperIconCustomLocation;

                if (String.IsNullOrEmpty(location))
                {
                    App.Logger.WriteLine(LOG_IDENT, "Warning: custom icon is not set.");
                }
                else
                {
                    try
                    {
                        customIcon = new Icon(location);
                    }
                    catch (Exception ex)
                    {
                        App.Logger.WriteLine(LOG_IDENT, $"Failed to load custom icon!");
                        App.Logger.WriteException(LOG_IDENT, ex);
                    }
                }

                return customIcon ?? Properties.Resources.IconBubblestrap;
            }

            return icon switch
            {
                BootstrapperIcon.IconBubblestrap => Properties.Resources.IconBubblestrap,
                BootstrapperIcon.IconBloxstrapClassic => Properties.Resources.IconBloxstrapClassic,
                _ => Properties.Resources.IconBubblestrap
            };
        }
    }
}
