using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Pager.Test
{
    [TestFixture]
    public class PagingTests
    {
        public class Constructor
        {
            [SetUp]
            public void SetUp()
            {
                this.Queryable = new List<string>().AsQueryable();
            }

            public IQueryable<string> Queryable { get; set; }

            private void SetUpQueryableWithItems(int items)
            {
                var list = new List<string>();
                for (int i = 0; i < items; i++) list.Add(i.ToString());
                this.Queryable = list.AsQueryable();
            }

            [Test]
            public void When_Queryable_IsNull_Throw_ArgumentNullException()
            {
                Type expectedException = typeof(ArgumentNullException);

                TestDelegate codeUnderTest = () => new Paging<string>(null, 1, 1);

                Assert.Throws(expectedException, codeUnderTest);
            }

            [Test]
            public void When_Page_IsNegative_Set_Page_To_1()
            {
                int expectedResult = 1;

                Paging<string> pager = new Paging<string>(this.Queryable, -5, 1);

                Assert.That(pager.Page == expectedResult);
            }

            [Test]
            public void When_Page_IsZero__Set_Page_To_1()
            {
                int expectedResult = 1;

                Paging<string> pager = new Paging<string>(this.Queryable, 0, 1);

                Assert.That(pager.Page == expectedResult);
            }

            [Test]
            public void When_ItemsPerPage_Is_Zero_Set_ItemsPerPage_To_1()
            {
                int expectedResult = 1;

                Paging<string> pager = new Paging<string>(this.Queryable, 1, 0);

                Assert.That(pager.Page == expectedResult);
            }


            [Test]
            public void When_ItemsPerPage_Is_Negative_Set_ItemsPerPage_To_1()
            {
                int expectedResult = 1;

                Paging<string> pager = new Paging<string>(this.Queryable, 1, -4);

                Assert.That(pager.ItemsPerPage == expectedResult);
            }

            [Test]
            public void When_DisableTotalPageCount_Is_True_Set_TotalPageCount_To_Negative1()
            {
                int expectedResult = -1;

                Paging<string> pager = new Paging<string>(this.Queryable, 1, 10, true);

                Assert.That(pager.TotalPages == expectedResult);
            }


            [Test]
            public void When_DisableTotalPageCount_Is_False_Get_TotalPageCount()
            {
                int expectedResult = 1;

                Paging<string> pager = new Paging<string>(this.Queryable, 1, 10);

                Assert.That(pager.TotalPages == expectedResult);
            }

            [Test]
            public void When_Using_15Items_SplitInto_12PerPage_FirstPageShouldContain_12Items()
            {
                int expectedResult = 12;
                this.SetUpQueryableWithItems(15);

                Paging<string> pager = new Paging<string>(this.Queryable, 1, 12);

                Assert.That(pager.PagedResult.Count == expectedResult);
            }

            [Test]
            public void When_Using_15Items_SplitInto_12PerPage_SecondPage_ShouldContain_3Items()
            {
                int expectedResult = 3;
                this.SetUpQueryableWithItems(15);

                Paging<string> pager = new Paging<string>(this.Queryable, 2, 12);

                Assert.That(pager.PagedResult.Count == expectedResult);
            }


            [Test]
            public void When_Using_12Items_SplitInto_12PerPage_OnlyOnePage_ShouldExist()
            {
                int expectedResult = 1;
                this.SetUpQueryableWithItems(12);

                Paging<string> pager = new Paging<string>(this.Queryable, 1, 12);

                Assert.That(pager.TotalPages == expectedResult);
            }

            [Test]
            public void When_Using_15Items_SplitInto_12PerPage_FirstPage_ShouldSet_HasMorePages_To_True()
            {
                bool expectedResult = true;
                this.SetUpQueryableWithItems(15);

                Paging<string> pager = new Paging<string>(this.Queryable, 1, 12);

                Assert.That(pager.HasMorePages == expectedResult);
            }
            [Test]
            public void When_Using_15Items_SplitInto_12PerPage_SecondPage_ShouldSet_HasMorePages_To_False()
            {
                bool expectedResult = false;
                this.SetUpQueryableWithItems(15);

                Paging<string> pager = new Paging<string>(this.Queryable, 2, 12);

                Assert.That(pager.HasMorePages == expectedResult);
            }

            [Test]
            public void When_Using_0Items_HasMorePages_Returns_False()
            {
                bool expectedResult = false;

                Paging<string> pager = new Paging<string>(this.Queryable, 1, 12);

                Assert.That(pager.HasMorePages == expectedResult);
            }
        }

        public class NextPage
        {
            [SetUp]
            public void SetUp()
            {
                string[] strings = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
                this.Queryable = new List<string>(strings).AsQueryable();
            }

            public IQueryable<string> Queryable { get; set; }

            [Test]
            public void WhenMorePagesExist_NextPageIsRetrieved()
            {
                int page = 1;
                int itemsPerPage = 12;
                int expectedResult = 2;

                Paging<string> pager = new Paging<string>(this.Queryable, page, itemsPerPage);
                pager.NextPage();

                Assert.That(pager.Page == expectedResult);
            }
            [Test]
            public void When_NoMorePagesExist_SamePageIsReturned()
            {
                int page = 2;
                int itemsPerPage = 12;
                int expectedResult = 2;

                Paging<string> pager = new Paging<string>(this.Queryable, page, itemsPerPage);
                pager.NextPage();

                Assert.That(pager.Page == expectedResult);
            }
        }
        public class PreviousPage
        {
            [SetUp]
            public void SetUp()
            {
                string[] strings = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
                this.Queryable = new List<string>(strings).AsQueryable();
            }

            public IQueryable<string> Queryable { get; set; }

            [Test]
            public void When_PreviousPagesExist_PreviousPageIsReturned()
            {
                int page = 2;
                int itemsPerPage = 12;
                int expectedResult = 1;

                Paging<string> pager = new Paging<string>(this.Queryable, page, itemsPerPage);
                pager.PreviousPage();

                Assert.That(pager.Page == expectedResult);
            }

            [Test]
            public void When_AtLastPage_ViewingPreviousPage_Sets_HasMorePages_AsTrue()
            {
                int page = 2;
                int itemsPerPage = 12;
                bool expectedResult = true;

                Paging<string> pager = new Paging<string>(this.Queryable, page, itemsPerPage);
                pager.PreviousPage();

                Assert.That(pager.HasMorePages == expectedResult);
            }
        }

    }
}
