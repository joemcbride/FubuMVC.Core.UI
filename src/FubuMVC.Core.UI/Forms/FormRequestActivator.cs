using System;
using FubuCore;
using FubuMVC.Core.Registration.Querying;
using HtmlTags.Conventions;

namespace FubuMVC.Core.UI.Forms
{
    public class FormRequestActivator : ITagRequestActivator
    {
        private readonly IChainResolver _resolver;

        public FormRequestActivator(IChainResolver resolver)
        {
            _resolver = resolver;
        }

        public bool Matches(Type requestType)
        {
            return requestType.CanBeCastTo<FormRequest>();
        }

        public void Activate(TagRequest request)
        {
            ActivateForm(request.As<FormRequest>());
        }

        public void ActivateForm(FormRequest request)
        {
            var chain = _resolver.Find(request.Search);
            request.Url = chain.Route.CreateUrlFromInput(request.Input);
            request.Chain = chain;
        }
    }
}