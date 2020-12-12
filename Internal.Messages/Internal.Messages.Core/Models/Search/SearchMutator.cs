using System;
using System.Collections.Generic;
using System.Linq;
using Internal.Messages.Core.Extensions;

namespace Internal.Messages.Core.Models.Search
{
    public class SearchMutator<TItem, TSearch>
    {
        public List<SearchFieldMutator<TItem, TSearch>> SearchFieldMutators { get; set; } = new List<SearchFieldMutator<TItem, TSearch>>();

        public void AddCondition(Predicate<TSearch> condition, QueryMutator<TItem, TSearch> mutator)
        {
            SearchFieldMutators.Add(new SearchFieldMutator<TItem, TSearch>(condition, mutator));
        }

        public IQueryable<TItem> Apply(TSearch search, IQueryable<TItem> query)
        {
            search.AllStringsToLower();

            foreach (var searchFieldMutator in SearchFieldMutators)
            {
                query = searchFieldMutator.Apply(search, query);
            }

            return query;
        }
    }
}
