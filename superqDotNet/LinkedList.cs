using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace superqDotNet
{
    public class LinkedList
    {
        public LinkedListNode head = null;
        public LinkedListNode tail = null;
        public int count = 0;

        public bool circular = false;

        public LinkedList()
        {

        }

        public bool is_empty()
        {
            return count == 0;
        }

        public void push(int idx, LinkedListNode node)
        {
            if idx == 0:
                // set new head, order of operations matters
                node.prev = None;
                node.next = self.head;

            # if list not empty, point current head to new head
            if self.head is not None:
                self.head.prev = node
            else:
                self.tail = node

            self.head = node
# TODO: this looks like an off by one. Should be count - 1. Test case?
# Also, there is the question of >.
        elif idx == self.__count:
            # set new tail, order of operations matters
            node.next = None
            node.prev = self.tail

            # if list not empty, point current tail to new tail
            if self.tail is not None:
                self.tail.next = node
            else:
                self.head = node

            self.tail = node
        else:
            curNode = self.__lookup(idx)

            # handle empty list case
            if curNode is None:
                self.head = node
                self.tail = node
                node.prev = None
                node.next = None
            else:
                # splice new node in
                node.next = curNode
                node.prev = curNode.prev
                curNode.prev.next = node
                curNode.prev = node

        self.__count += 1
        }

        public void push_head(LinkedListNode node)
        {

        }

        public void push_middle(int idx, LinkedListNode node)
        {

        }

        public void push_tail(LinkedListNode node)
        {

        }

        public LinkedListNode pop(int idx)
        {
            return null;
        }

        public LinkedListNode pop_head()
        {
            return null;
        }

        public LinkedListNode pop_tail()
        {
            return null;
        }

        public LinkedListNode pop_node(LinkedListNode node)
        {
            return null;
        }

        public void insert_before(LinkedListNode oldNode, LinkedListNode newNode)
        {

        }

        public void insert_after(LinkedListNode oldNode, LinkedListNode newNode)
        {

        }

        public void move_up(LinkedListNode node)
        {

        }

        public void move_down(LinkedListNode node)
        {

        }
    }
}
