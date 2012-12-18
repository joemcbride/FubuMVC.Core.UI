using System.Collections.Generic;
using Bottles;
using Bottles.Diagnostics;
using FubuCore.Reflection;
using FubuMVC.Core.Bootstrapping;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.UI.Elements;
using FubuMVC.Core.UI.Forms;
using HtmlTags.Conventions;

namespace FubuMVC.Core.UI
{
    public class HtmlConventionsActivator : IActivator, ITagLibraryVisitor<ElementRequest>
    {
        private readonly AccessorRules _rules;
        private readonly BehaviorGraph _graph;
        private readonly IContainerFacility _facility;
        private string _category;

        public HtmlConventionsActivator(AccessorRules rules, BehaviorGraph graph, IContainerFacility facility)
        {
            _rules = rules;
            _graph = graph;
            _facility = facility;
        }

        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            var library = _graph.Settings.Get<HtmlConventionLibrary>();
            library.Import(new DefaultHtmlConventions().Library);

            library.For<ElementRequest>().AcceptVisitor(this);

            _facility.Register(typeof(HtmlConventionLibrary), ObjectDef.ForValue(library));
        }

        public void Category(string name, TagCategory<ElementRequest> category)
        {
            _category = name;
        }

        public void BuilderSet(string profile, BuilderSet<ElementRequest> builders)
        {
            var policy = new AccessorOverrideElementBuilderPolicy(_rules, _category, profile);
            builders.InsertFirst(policy);
        }
    }
}