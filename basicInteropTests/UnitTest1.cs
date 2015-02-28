using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using superqDotNet;

namespace basicInteropTests
{
    [TestClass]
    public class UnitTest1
    {
        // Simple custom class
        private class FooNode : LinkedListNode
        {
            public int a { get; set; }

            public FooNode(int a)
            {
                this.a = a;
            }
        }

        [TestMethod]
        public void LinkedListIntegrity()
        {
            // Instantiate list
            LinkedList<FooNode> llist = new LinkedList<FooNode>();

            // Add elements
            for (int i = 0; i < 10; ++i)
                llist.push_tail(new FooNode(i + 1));
            Assert.AreEqual(10, llist.count);
            Assert.AreEqual(1, llist[0].a);

            // Remove elements from head
            for (int i = 0; i < 5; ++i)
                llist.pop_head();
            Assert.AreEqual(5, llist.count);

            // Add elements to head
            for (int i = 0; i < 5; ++i)
                llist.push_head(new FooNode(i + 1));
            Assert.AreEqual(10, llist.count);
            Assert.AreEqual(5, llist[0].a);

            // Remove elements from tail
            for (int i = 0; i < 5; ++i)
                llist.pop_tail();
            Assert.AreEqual(5, llist.count);

            // Add into middle of list
            llist.push_middle(3, new FooNode(11));
            Assert.AreEqual(6, llist.count);
            Assert.AreEqual(11, llist[3].a);

            // Remove from middle of list
            FooNode fooNode = llist.pop_middle(3);
            Assert.AreEqual(5, llist.count);
            Assert.AreEqual(11, fooNode.a);

            // Push/pop from both sides of list
            llist = new LinkedList<FooNode>();
            for (int i = 0; i < 10; ++i)
                llist.push_tail(new FooNode(i + 1));
            Assert.AreEqual(10, llist.count);
        }

        [TestMethod]
        public void BasicInstantiation()
        {
            superq sq = superq.Create(new object[0]);
        }

        [TestMethod]
        public void InstantiateFromIntArray()
        {
            superq sq = superq.Create(new int[3] {1, 2, 3});
            Assert.AreEqual(3, sq.list.count);
        }

        [TestMethod]
        public void RetrieveElemByIndex()
        {
            superq sq = superq.Create(new int[] { 1, 2, 3 });
            Assert.AreEqual(2, sq[1]);
        }

        [TestMethod]
        public void InstantiateFromNonUniqueList()
        {
            superq sq = superq.Create(new int[] { 1, 1, 2 });
        }

        [TestMethod]
        public void InstantiateFromCustomObjects()
        {
            superq sq = superq.Create(new FooNode[] { new FooNode(1), new FooNode(2) });
        }

        [TestMethod]
        public void InstantiateWithKeyCol()
        {
            superq sq = superq.Create(new FooNode[] { new FooNode(1), new FooNode(2) }, "a");
        }
    }
}
