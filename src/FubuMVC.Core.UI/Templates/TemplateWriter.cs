using System;
using System.Linq.Expressions;
using FubuCore;
using FubuMVC.Core.UI.Elements;
using HtmlTags;
using HtmlTags.Conventions;

namespace FubuMVC.Core.UI.Templates
{
    // TODO -- add a feature to add partials as templates?
    public class TemplateWriter
    {
        private readonly HtmlConventionLibrary _library;
        private ITagRequestActivator[] _activators;
        private readonly HtmlTag _tag = new HtmlTag("div").Hide().AddClass("templates");

        public TemplateWriter(HtmlConventionLibrary library, IElementNamingConvention naming, IServiceLocator services)
        {
            _library = library;
            _activators = new ITagRequestActivator[] {new ServiceLocatorTagRequestActivator(services), new ElementIdActivator(naming) };
        }

        public void AddTemplate(string subject, HtmlTag tag)
        {
            _tag.Add("div").Attr("data-subject", subject).Append(tag);
        }

        public void AddTemplate(string subject, string html)
        {
            AddTemplate(subject, new LiteralTag(html));
        }

        public void DisplayFor<T>(Expression<Func<T, object>> property)
        {
            throw new NotImplementedException();
        }

        public void InputFor<T>(Expression<Func<T, object>> property)
        {
            throw new NotImplementedException();
        }

        public void LabelFor<T>(Expression<Func<T, object>> property)
        {
            throw new NotImplementedException();
        }

        public HtmlTag WriteAll()
        {
            return _tag;
        }
    }
}