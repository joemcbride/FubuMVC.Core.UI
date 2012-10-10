using System;
using System.Linq.Expressions;
using FubuCore;
using FubuCore.Reflection;
using FubuCore.Util;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.UI.Elements;
using HtmlTags;
using HtmlTags.Conventions;

namespace FubuMVC.Core.UI.Templates
{
    // TODO -- add a feature to add partials as templates?
    public class TemplateWriter
    {
        private readonly HtmlConventionLibrary _library;
        private readonly ITagRequestActivator[] _activators;
        private readonly HtmlTag _tag = new HtmlTag("div").Hide().AddClass("templates");
        private readonly TagGeneratorFactory _factory;
        private readonly Lazy<ITagGenerator<ElementRequest>> _elements; 

        public TemplateWriter(ActiveProfile profile, HtmlConventionLibrary library, IElementNamingConvention naming, IServiceLocator services)
        {
            _library = library;
            _activators = new ITagRequestActivator[] {new ServiceLocatorTagRequestActivator(services), new ElementIdActivator(naming) };
            _factory = new TagGeneratorFactory(profile, library, _activators);
            _elements = new Lazy<ITagGenerator<ElementRequest>>(() => _factory.GeneratorFor<ElementRequest>());
        }

        public void AddTemplate(string subject, HtmlTag tag)
        {
            _tag.Add("div").Attr("data-subject", subject).Append(tag);
        }

        public void AddTemplate(string subject, string html)
        {
            AddTemplate(subject, new LiteralTag(html));
        }

        public void AddElement(Accessor accessor, string category)
        {
            var request = new ElementRequest(accessor);
            var tag = _elements.Value.Build(request, category: category,
                                            profile: ElementConstants.Templates);

            var subject = "{0}-{1}".ToFormat(category.ToLower(), request.ElementId);
            AddTemplate(subject, tag);
        }

        public void DisplayFor<T>(Expression<Func<T, object>> property)
        {
            AddElement(property.ToAccessor(), ElementConstants.Display);
        }

        public void InputFor<T>(Expression<Func<T, object>> property)
        {
            AddElement(property.ToAccessor(), ElementConstants.Editor);
        }

        public void LabelFor<T>(Expression<Func<T, object>> property) where T : class
        {
            AddElement(property.ToAccessor(), ElementConstants.Label);
        }

        public HtmlTag WriteAll()
        {
            return _tag;
        }
    }
}