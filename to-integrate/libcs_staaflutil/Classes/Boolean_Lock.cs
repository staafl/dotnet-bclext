using System;

namespace Fairweather.Service
{
    //[DebuggerStepThrough]
    public sealed class Block : IDisposable
    {
        public bool Locked { get; set; }
        readonly bool previous;
        Block parent;

        public Block() {

            Locked = false;
            parent = this;
        }

        Block(Block parent_p) {

            previous = parent_p.Locked;
            this.parent = parent_p;
            parent.Locked = true;
        }

        public Block Lock() {

            if (this.Locked)
                throw new InvalidOperationException("Lock already taken.");

            return new Block(this);
        }


        public void Dispose() {

            parent.Locked = previous;

            // GC.SuppressFinalize(this);
        }

        public static implicit operator bool(Block b_lock) {

            return b_lock.Locked;
        }
    }
}