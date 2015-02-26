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
            Assert.AreEqual(llist.count, 10);
            Assert.AreEqual(llist[0].a, 1);

            // Remove elements from head
            for (int i = 0; i < 5; ++i)
                llist.pop_head();
            Assert.AreEqual(llist.count, 5);

            // Add elements to head
            for (int i = 0; i < 5; ++i)
                llist.push_head(new FooNode(i + 1));
            Assert.AreEqual(llist.count, 10);
            Assert.AreEqual(llist[0].a, 5);

            // Remove elements from tail
            for (int i = 0; i < 5; ++i)
                llist.pop_tail();
            Assert.AreEqual(llist.count, 5);

            // Add into middle of list
            llist.push_middle(3, new FooNode(11));
            Assert.AreEqual(llist.count, 6);
            Assert.AreEqual(llist[3].a, 11);

            // Remove from middle of list
            FooNode fooNode = llist.pop_middle(3);
            Assert.AreEqual(llist.count, 5);
            Assert.AreEqual(fooNode.a, 11);

            // Push/pop from both sides of list
            llist = new LinkedList<FooNode>();
            for (int i = 0; i < 10; ++i)
                llist.push_tail(new FooNode(i + 1));
            Assert.AreEqual(llist.count, 10);
        }

        [TestMethod]
        public void BasicInstantiation()
        {
            superq sq = superq.Create(new object[0]);
        }

        [TestMethod]
        public void InstantiateFromIntArray()
        {
            superq sq = superq.Create(new int[1, 2, 3]);
            Assert.AreEqual(sq.list.count, 3);
        }
    }
}
