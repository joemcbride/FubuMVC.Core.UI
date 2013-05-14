using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FubuCore;
using FubuCore.Reflection;
using FubuMVC.Core.UI.Elements;
using HtmlTags.Conventions;

namespace FubuMVC.Core.UI.Security
{
    public class FieldAccessService : IFieldAccessService
    {
        private readonly IFieldAccessRightsExecutor _accessRightsExecutor;
        private readonly ITypeResolver _types;
        private readonly ITagRequestBuilder _tagRequestBuilder;
        private readonly List<IFieldAccessRule> _rules = new List<IFieldAccessRule>();

        public FieldAccessService(IFieldAccessRightsExecutor accessRightsExecutor, IEnumerable<IFieldAccessRule> rules, ITypeResolver types,
            ITagRequestBuilder tagRequestBuilder)
        {
            _accessRightsExecutor = accessRightsExecutor;
            _types = types;
            _tagRequestBuilder = tagRequestBuilder;
            _rules.AddRange(rules);
        }

        public AccessRight RightsFor(ElementRequest request)
        {
            var matchingRules = _rules.Where(x => x.Matches(request.Accessor));
            var authorizationRules = matchingRules.Where(x => x.Category == FieldAccessCategory.Authorization);
            var logicRules = matchingRules.Where(x => x.Category == FieldAccessCategory.LogicCondition);
            return _accessRightsExecutor.RightsFor(request, authorizationRules, logicRules);
        }

        public AccessRight RightsFor(object target, PropertyInfo property)
        {
            var accessor = new SingleProperty(property, _types.ResolveType(target));

            var request = new ElementRequest(accessor){
                Model = target
            };

            _tagRequestBuilder.Build(request);

            return RightsFor(request);
        }
    }
}