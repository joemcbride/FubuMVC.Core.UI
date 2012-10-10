using System;
using System.Linq.Expressions;
using FubuCore;
using FubuCore.Reflection;
using FubuMVC.Core.UI.Elements;
using HtmlTags;
using HtmlTags.Conventions;

namespace FubuMVC.Core.UI.Templates
{
    // TODO -- add a feature to add partials as templates?
    public interface ITemplateWriter
    {
        void AddTemplate(string subject, HtmlTag tag);
        void AddTemplate(string subject, string html);
        void AddElement(Accessor accessor, string category);
        void DisplayFor<T>(Expression<Func<T, object>> property);
        void InputFor<T>(Expression<Func<T, object>> property);
        void LabelFor<T>(Expression<Func<T, object>> property) where T : class;
        HtmlTag WriteAll();
    }

    public class TemplateWriter : ITemplateWriter
    {
        private readonly ITagRequestActivator[] _activators;
        private readonly Lazy<ITagGenerator<ElementRequest>> _elements;
        private readonly HtmlConventionLibrary _library;
        private readonly HtmlTag _tag = new HtmlTag("div").Hide().AddClass("templates").Render(false);

        public TemplateWriter(ActiveProfile profile, HtmlConventionLibrary library, IElementNamingConvention naming,
                              IServiceLocator services)
        {
            _library = library;
            _activators = new ITagRequestActivator[]
            {new ServiceLocatorTagRequestActivator(services), new ElementIdActivator(naming)};
            var factory = new TagGeneratorFactory(profile, library, _activators);
            _elements = new Lazy<ITagGenerator<ElementRequest>>(factory.GeneratorFor<ElementRequest>);
        }

        #region ITemplateWriter Members

        public void AddTemplate(string subject, HtmlTag tag)
        {
            _tag.Render(true);
            _tag.Add("div").Attr("data-subject", subject).Append(tag);
        }

        public void AddTemplate(string subject, string html)
        {
            AddTemplate(subject, new LiteralTag(html));
        }

        public void AddElement(Accessor accessor, string category)
        {
            var request = new ElementRequest(accessor);
            HtmlTag tag = _elements.Value.Build(request, category: category,
                                                profile: ElementConstants.Templates);

            string subject = "{0}-{1}".ToFormat(category.ToLower(), request.ElementId);
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

        #endregion
    }
}