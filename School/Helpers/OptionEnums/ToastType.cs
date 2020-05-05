using System.ComponentModel;

namespace School.Models.Helpers.OptionEnums
{
    public enum ToastType
    {
        //blue, red, green, yellow
        [Description("success")]
        Green,

        [Description("info")]
        Blue,

        [Description("warning")]
        Yellow,

        [Description("error")]
        Red,

        [Description("success2")]
        Green2,

    }


}
