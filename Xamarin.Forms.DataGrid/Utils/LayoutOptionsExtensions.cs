namespace Xamarin.Forms.DataGrid.Utils
{
    internal static class LayoutOptionsExtensions
    {
        internal static TextAlignment ToTextAlignment(this LayoutOptions layoutAlignment)
        {
            switch (layoutAlignment.Alignment)
            {
                case LayoutAlignment.Start:
                    return TextAlignment.Start;
                case LayoutAlignment.Center:
                    return TextAlignment.Center;
                case LayoutAlignment.End:
                    return TextAlignment.End;
                default:
                    return TextAlignment.Center;
            }
        }
    }
}