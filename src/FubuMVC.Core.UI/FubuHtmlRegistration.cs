using FubuMVC.Core.UI.Forms;
using FubuMVC.Core.UI.ViewEngine;
using FubuMVC.Core.View;
using HtmlTags.Conventions;

namespace FubuMVC.Core.UI
{
    public class FubuHtmlRegistration : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Services<UIServiceRegistry>();

            registry.ViewFacility(new HtmlDocumentViewFacility());

            registry.AlterSettings<HtmlConventionLibrary>(x => {
                x.RegisterService<IFieldChrome, DefinitionListFieldChrome>();
            });
        }
    }
}