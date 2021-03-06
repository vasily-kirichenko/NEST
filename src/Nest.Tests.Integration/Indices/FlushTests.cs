﻿using Nest.Tests.MockData.Domain;
using NUnit.Framework;

namespace Nest.Tests.Integration.Indices
{
	[TestFixture]
	public class FlushTests : IntegrationTests
	{
		[Test]
		public void FlushAll()
		{
			var r = this._client.Flush(f=>f.AllIndices());
			Assert.True(r.OK, r.ConnectionStatus.ToString());
		}
		[Test]
		public void FlushIndex()
		{
			var r = this._client.Flush(f=>f.Index(ElasticsearchConfiguration.DefaultIndex));
			Assert.True(r.OK);
		}
		[Test]
		public void FlushIndeces()
		{
			var r = this._client.Flush(f=>f
				.Indices( ElasticsearchConfiguration.DefaultIndex, ElasticsearchConfiguration.DefaultIndex + "_clone")
			);
			Assert.True(r.OK);
		}
		[Test]
		public void FlushTyped()
		{
			var r = this._client.Flush(f=>f.Index<ElasticSearchProject>());
			Assert.True(r.OK);
		}
		[Test]
		public void FlushAllRefresh()
		{
			var r = this._client.Flush(f=>f.AllIndices().Refresh());
			Assert.True(r.OK);
		}
		[Test]
		public void FlushIndexRefresh()
		{
			var r = this._client.Flush(f=>f.Index(ElasticsearchConfiguration.DefaultIndex).Refresh());
			Assert.True(r.OK);
		}
		[Test]
		public void FlushIndecesRefresh()
		{
			var r = this._client.Flush(f=>f
				.Indices(ElasticsearchConfiguration.DefaultIndex, ElasticsearchConfiguration.DefaultIndex + "_clone")
				.Refresh()
			);
			Assert.True(r.OK);
		}
		[Test]
		public void FlushTypedRefresh()
		{
			var r = this._client.Flush(f=>f.Index<ElasticSearchProject>().Refresh());
			Assert.True(r.OK);
		}
	}
}