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

        private LinkedListNode lookup(int idx)
        {
            if (idx >= count)
                throw new Exception("idx (" + idx.ToString() + ") out of range.");

            if (count == 0)
                return null;

            LinkedListNode node = null;

            if (idx >= 0)
            {
                // start from whichever end of list is closest to idx
                int midIdx = (count - 1) / 2;
                if (idx < midIdx)
                {
                    node = head;
                    for (int i = 0; i < idx; ++i)
                        node = node.next;
                }
                else
                {
                    node = tail;
                    for (int i = 0; i < (count - 1) - idx; ++i)
                        node = node.prev;
                }
            }
            else
            {
                // handle negative idx
                node = tail;
                for (int i = 1; i < Math.Abs(idx); ++i)
                    node = node.prev;
            }

            return node;
        }

        public bool is_empty()
        {
            return count == 0;
        }

        public void push(int idx, LinkedListNode node)
        {
            if (idx == 0)
            {
                // set new head, order of operations matters
                node.prev = null;
                node.next = head;

                // if list not empty, point current head to new head
                if (head != null)
                    head.prev = node;
                else
                    tail = node;

                head = node;
            }
            else if (idx >= count - 1)
            {
                // set new tail, order of operations matters
                node.next = null;
                node.prev = tail;

                // if list not empty, point current tail to new tail
                if (tail != null)
                    tail.next = node;
                else
                    head = node;

                tail = node;
            }
            else
            {
                LinkedListNode curNode = lookup(idx);

                // handle empty list case
                if (curNode == null)
                {
                    head = node;
                    tail = node;
                    node.prev = null;
                    node.next = null;
                }
                else
                {
                    // splice new node in
                    node.next = curNode;
                    node.prev = curNode.prev;
                    curNode.prev.next = node;
                    curNode.prev = node;
                }
            }

            count += 1;
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
