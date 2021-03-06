﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace superqDotNet
{
    public class LinkedList : IEnumerable<LinkedListNode>
    {
        public LinkedListNode head = null;
        public LinkedListNode tail = null;
        public int count = 0;

        public bool circular = false;

        public LinkedList()
        {

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<LinkedListNode> GetEnumerator()
        {
            LinkedListNode node = head;
            while (node != null)
            {
                yield return node;
                node = node.next;
            }
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
            else if (idx >= count)
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
            push(0, node);
        }

        public void push_middle(int idx, LinkedListNode node)
        {
            push(idx, node);
        }

        public void push_tail(LinkedListNode node)
        {
            push(count, node);
        }

        public LinkedListNode pop(int idx)
        {
            if (count < 1)
                return null;

            LinkedListNode node = null;
        
            if (idx <= 0)
            {
                // get list head
                node = head;

                // point list head to next element
                head = head.next;

                // if list not empty, tell head it has no prev         
                if (head != null)
                    head.prev = null;

                // one less element in the list
                count -= 1;

                // if down to 1 element, set tail to head
                if (count == 1)
                    tail = head;
            }
            else if (idx >= count - 1)
            {
                // get list tail
                node = tail;

                // point list tail to previous element
                tail = tail.prev;

                // if list not empty, tell tail it has no next
                if (tail != null)
                    tail.next = null;

                // one less element in the list
                count -= 1;

                // if down to 1 element, set head to tail
                if (count == 1)
                    head = tail;
            }
            else
            {
                node = lookup(idx);

                // because node is not head or tail, these dereferences are safe
                node.prev.next = node.next;
                node.next.prev = node.prev;

                // one less element in the list
                count -= 1;
            }

            return node;
        }

        public LinkedListNode pop_head()
        {
            return pop(0);
        }

        public LinkedListNode pop_middle(int idx)
        {
            return pop(idx);
        }

        public LinkedListNode pop_tail()
        {
            return pop(count - 1);
        }

        public LinkedListNode pop_node(LinkedListNode node)
        {
            if (node == null)
                return null;

            if (node.prev != null)
                node.prev.next = node.next;
            else
                head = node.next;

            if (node.next != null)
                node.next.prev = node.prev;
            else
                tail = node.prev;

            count -= 1;

            return node;
        }

        public void insert_before(LinkedListNode oldNode, LinkedListNode newNode)
        {
            // handle case inserting new head
            if (head == oldNode)
            {
                newNode.prev = null;
                newNode.next = oldNode;
                oldNode.prev = newNode;
                head = newNode;
            }
            else
            {
                newNode.prev = oldNode.prev;
                newNode.next = oldNode;
                oldNode.prev.next = newNode;
                oldNode.prev = newNode;
            }

            count += 1;
        }

        public void insert_after(LinkedListNode oldNode, LinkedListNode newNode)
        {
            // handle case inserting new tail
            if (tail == oldNode)
            {
                newNode.next = null;
                newNode.prev = oldNode;
                oldNode.next = newNode;
                tail = newNode;
            }
            else
            {
                newNode.prev = oldNode;
                newNode.next = oldNode.next;
                oldNode.next.prev = newNode;
                oldNode.next = newNode;
            }

            count += 1;
        }

        public void move_up(LinkedListNode node)
        {
            // can't move list node up if it is already head
            if (node.prev == null)
                return;

            // these are aliases to the 4 starting elements involved
            LinkedListNode above_node_prev = node.prev.prev;
            LinkedListNode above_node = node.prev;
            LinkedListNode current_node = node;
            LinkedListNode current_node_next = node.next;

            // do the pointer swaps
            if (above_node_prev != null)
                above_node_prev.next = current_node;
            current_node.prev = above_node_prev;
            current_node.next = above_node;
            above_node.prev = current_node;
            above_node.next = current_node_next;
            above_node.next.prev = above_node;

            // if node is at top of list, set head to node
            if (current_node.prev == null)
                head = current_node;
        }

        public void move_down(LinkedListNode node)
        {
            // can't move list node up if it is already head
            if (node.prev == null)
                return;

            // these are aliases to the 4 starting elements involved
            LinkedListNode above_node_prev = node.prev.prev;
            LinkedListNode above_node = node.prev;
            LinkedListNode current_node = node;
            LinkedListNode current_node_next = node.next;

            // do the pointer swaps
            if (above_node_prev != null)
                above_node_prev.next = current_node;
            current_node.prev = above_node_prev;
            current_node.next = above_node;
            above_node.prev = current_node;
            above_node.next = current_node_next;
            above_node.next.prev = above_node;

            // if node is at top of list, set head to node
            if (current_node.prev == null)
                head = current_node;
        }
    }
}
