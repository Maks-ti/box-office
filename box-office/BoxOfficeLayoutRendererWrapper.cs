using NLog;
using NLog.LayoutRenderers.Wrappers;

namespace box_office
{
    public class BoxOfficeLayoutRendererWrapper : WrapperLayoutRendererBase
    {
        protected override string RenderInner(LogEventInfo logEvent)
        {
            return "";
        }

        protected override string Transform(string text)
        {
            return text;
        }
    }
}
