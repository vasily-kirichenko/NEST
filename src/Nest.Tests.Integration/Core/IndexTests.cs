﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Nest.Tests.MockData;
using Nest.Tests.MockData.Domain;
using NUnit.Framework;

namespace Nest.Tests.Integration.Core
{
	[TestFixture]
	public class IndexTests : IntegrationTests
	{
		private readonly Random idGen = new Random();

		[Test]
		public void IndexUsingCreateFlag()
		{
			// Document to be indexed.
			var doc = new ElasticSearchProject
			{
				Country = "Mozambique",
				Followers = new List<Person>(),
				Id = idGen.Next(),
				Name = "Test Document for 'IndexDocument' Create Flag"
			};

			// Index the document
			var indexResult = this._client.Index<ElasticSearchProject>(doc, i => i.OpType(OpTypeOptions.Create));
			indexResult.IsValid.Should().BeTrue();

			// Grab the indexed document.
			var foundDoc = this._client.Source<ElasticSearchProject>(doc.Id);

			// Check that the document was successfully indexed.
			Assert.NotNull(foundDoc);
			Assert.AreEqual(doc.Country, foundDoc.Country);
			Assert.AreEqual(doc.Followers.Count, foundDoc.Followers.Count);
			Assert.AreEqual(doc.Id, foundDoc.Id);
			Assert.AreEqual(doc.Name, foundDoc.Name);

			// Now try to index the document again while using the Create Flag
			var response = this._client.Index<ElasticSearchProject>(doc, i => i.OpType(OpTypeOptions.Create));

			// Make sure the index request failed with HTTP status 409 since document with same id already exists.
			Assert.False(response.OK);
			Assert.AreEqual(System.Net.HttpStatusCode.Conflict, response.ConnectionStatus.Error.HttpStatusCode);
		}
	}
}
