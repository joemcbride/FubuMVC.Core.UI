using FubuMVC.Core.UI.Forms;
using FubuMVC.TestingHarness;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Core.UI.Testing.Integration
{
    [TestFixture]
    public class can_override_the_field_chrome_bug : FubuRegistryHarness
    {
        protected override void configure(FubuRegistry registry)
        {
            registry.Actions.IncludeType<ShowEditEndpoints>();
            registry.Import<HtmlConventionRegistry>(x => {
                x.FieldChrome<TableRowFieldChrome>();
            });
        }


        [Test]
        public void field_chrome_is_not_the_default()
        {
            endpoints.GetByInput(new ShowModel { Name = "Jeremy" })
                     .ToString().ShouldEqual("<tr><td><label for=\"Name\">Name</label></td><td><span id=\"Name\">Jeremy</span></td></tr>");
        }
    }
}