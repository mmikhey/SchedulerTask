using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTask
{
    public class TreeIterator
    {
        public Party Current { get; set; }

        public TreeIterator(Party aRoot)
        {
            Current = aRoot;

            if (Current == null)
            {
                return;
            }
            if (!Current.isLeaf())
            {
                List<Party> subP = Current.getSubParty();
                foreach (Party subPart in subP)
                {
                    NodeQueue.Enqueue(subPart);
                }

            }
        }

        public bool next()
        {
            if (Current == null)
            {
                return false;
            }

            if (Current.isLeaf())
            {
                return false;
            }

            List<Party> subP = Current.getSubParty();

            if (NodeQueue.Count > 0)
            {
                Current = NodeQueue.Dequeue();

                foreach (Party subPart in subP)
                {
                    NodeQueue.Enqueue(subPart);
                }
            }
            else
            {
                Current = null;
            }

            return hasNext();
        }

        public bool hasNext()
        {
            return Current != null;
        }

        private Queue<Party> NodeQueue
        {
            get { return mNodeQueue; }
        }
        private readonly Queue<Party> mNodeQueue = new Queue<Party>();

    }    
}
