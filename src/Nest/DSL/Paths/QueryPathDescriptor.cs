﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nest.Resolvers.Converters;
using System.Linq.Expressions;
using Nest.Resolvers;

namespace Nest
{

	public class QueryPathDescriptor<T> : QueryPathDescriptorBase<QueryPathDescriptor<T>, T, BulkQueryString>
		where T : class
	{

	}

	/// <summary>
	/// Provides a base for descriptors that need to describe a path in the form of 
	/// <pre>
	///	/{indices}/{types}
	/// </pre>
	/// all parameters are optional and will default to the defaults for <para>T</para>
	/// </summary>
	public class QueryPathDescriptorBase<P, T, K>
		where P : QueryPathDescriptorBase<P, T, K>, new() where T : class
		where K : FluentQueryString<K>, new()
	{
		internal IEnumerable<IndexNameMarker> _Indices { get; set; }
		internal IEnumerable<TypeNameMarker> _Types { get; set; }
		internal bool _AllIndices { get; set; }
		internal bool _AllTypes { get; set; }
		public P Indices(params Type[] indices)
		{
			if (indices == null) return (P) this;
			this._Indices = indices.Cast<IndexNameMarker>();
			return (P)this;
		}
		public P Indices(params string[] indices)
		{
			if (indices == null) return (P) this;
			this._Indices = indices.Cast<IndexNameMarker>();
			return (P)this;
		}
		public P Indices(IEnumerable<Type> indices)
		{
			if (indices == null) return (P) this;
			this._Indices = indices.Cast<IndexNameMarker>();
			return (P)this;
		}
		public P Indices(IEnumerable<string> indices)
		{
			if (indices == null) return (P) this;
			this._Indices = indices.Cast<IndexNameMarker>();
			return (P)this;
		}

		public P Index<TAlternative>() where TAlternative : class
		{
			return this.Indices(typeof(TAlternative));
		}

		public P Index(Type index)
		{
			return this.Indices(index);
		}
		public P Index(string index)
		{
			return this.Indices(index);
		}
		public P Types(IEnumerable<string> types)
		{
			if (types == null) return (P) this;
			this._Types = types.Select(s => (TypeNameMarker)s); ;
			return (P)this;
		}
		public P Types(params string[] types)
		{
			if (types == null) return (P) this;
			return this.Types((IEnumerable<string>) types);
		}
		public P Types(IEnumerable<Type> types)
		{
			if (types == null) return (P) this;
			this._Types = types.Cast<TypeNameMarker>();
			return (P)this;
		}
		public P Types(params Type[] types)
		{
			if (types == null) return (P) this;
			return this.Types((IEnumerable<Type>)types);
		}
		public P Type(string type)
		{
			return this.Types(type);
		}
		public P Type(Type type)
		{
			return this.Types(type);
		}
		public P Type<TAlternative>() where TAlternative : class
		{
			return this.Types(typeof(TAlternative));
		}
		public P AllIndices()
		{
			this._AllIndices = true;
			return (P)this;
		}
		public P AllTypes()
		{
			this._AllTypes = true;
			return (P)this;
		}

		internal virtual ElasticSearchPathInfo<K> ToPathInfo<K>(IConnectionSettings settings) 
			where K : FluentQueryString<K>, new()
		{
			//start out with defaults
			var inferrer = new ElasticInferrer(settings);
			var index = inferrer.IndexName<T>();
			var type = inferrer.TypeName<T>();
			var pathInfo = new ElasticSearchPathInfo<K>()
			{
				Index = index,
				Type = type
			};

			if (this._AllTypes)
				pathInfo.Type = null;
			else if (this._Types.HasAny())
				pathInfo.Type = string.Join(",", this._Types.Select(s=>s.Resolve(settings)));

			if (this._AllIndices && pathInfo.Type == inferrer.TypeName<T>())
				pathInfo.Type = null;

			if (this._AllIndices && !pathInfo.Type.IsNullOrEmpty())
				pathInfo.Index = "_all";
			else if (this._AllIndices)
				pathInfo.Index = null;
			else if (this._Indices.HasAny())
				pathInfo.Index = string.Join(",", this._Indices);

			pathInfo.QueryString = new K();
			return pathInfo;
		}

	}
}