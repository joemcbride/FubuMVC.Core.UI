using FubuMVC.Core.Runtime;
using FubuMVC.Core.UI.Elements;
using FubuMVC.Core.UI.Elements.Builders;
using HtmlTags.Conventions;

namespace FubuMVC.Core.UI
{
    public class DefaultHtmlConventions : HtmlConventionRegistry
    {
        public DefaultHtmlConventions()
        {
            Editors.Builder<CheckboxBuilder>();
            Editors.Builder<TextboxBuilder>();

            Editors.Modifier<AddNameModifier>();

            Displays.Builder<SpanDisplayBuilder>();

            Labels.Builder<DefaultLabelBuilder>();

            Templates.Displays.Builder<TemplateSpanBuilder>();
            Templates.Editors.Builder<TemplateTextboxBuilder>();

            Templates.Displays.Always.ModifyWith<DataFldModifier>();
            Templates.Editors.Always.ModifyWith<DataFldModifier>();
        }
    }
}