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
