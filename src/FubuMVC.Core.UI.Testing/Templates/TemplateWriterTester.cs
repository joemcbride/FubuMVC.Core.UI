using FubuCore;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.UI.Elements;
using FubuMVC.Core.UI.Templates;
using HtmlTags;
using HtmlTags.Conventions;
using NUnit.Framework;
using FubuTestingSupport;
using System.Linq;

namespace FubuMVC.Core.UI.Testing.Templates
{
    [TestFixture]
    public class TemplateWriterTester
    {
        private TemplateWriter theTemplates;

        [SetUp]
        public void SetUp()
        {
            var library = new DefaultHtmlConventions().Library;

            theTemplates = new TemplateWriter(library, new DefaultElementNamingConvention(),
                                              new InMemoryServiceLocator());
        }

        [Test]
        public void add_a_simple_template_directory_and_write()
        {
            theTemplates.AddTemplate("subject1", new HtmlTag("span").MustacheText("foo"));

            var templates = theTemplates.WriteAll();

        }
    }

    [TestFixture]
    public class when_writing_templates
    {
        private TemplateWriter theTemplates;
        private HtmlTag templates;

        [SetUp]
        public void SetUp()
        {
            var library = new DefaultHtmlConventions().Library;

            theTemplates = new TemplateWriter(library, new DefaultElementNamingConvention(),
                                              new InMemoryServiceLocator());

            theTemplates.AddTemplate("foo", new HtmlTag("span").MustacheText("foo"));
            theTemplates.AddTemplate("bar", "some {{bar}} text");

            templates = theTemplates.WriteAll();
        }

        [Test]
        public void outside_tag_should_have_the_templates_class()
        {
            templates.HasClass("templates");
        }

        [Test]
        public void outside_tag_is_not_visible()
        {
            templates.Style("display").ShouldEqual("none");
        }

        [Test]
        public void writes_template_tag_inside_a_holder_with_subject()
        {
            templates.FirstChild().Attr("data-subject").ShouldEqual("foo");
            templates.FirstChild().FirstChild().ToString().ShouldEqual("<span>{{foo}}</span>");
        }

        [Test]
        public void writes_html_in_a_literal_tag()
        {
            templates.Children.Last().FirstChild().ShouldBeOfType<LiteralTag>()
                .ToString().ShouldEqual("some {{bar}} text");
        }
    }
}