using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop_clothes.Servises
{
    public record State: IEquatable<State>
    {
        public ICollection<Products> Products { get; init; }

        public ICollection<User> Users { get; init; }

        public State(ICollection<Products> products, ICollection<User> users)
        {
            Users = [.. users.Select(u => (User)u.Clone())];
            Products = [.. products.Select(p => p.Clone())];
        }

        public virtual bool Equals(State? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return Products.SequenceEqual(other.Products) &&
                   Users.SequenceEqual(other.Users);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            foreach (var product in Products.OrderBy(p => p.GetHashCode()))
                hash.Add(product);
            foreach (var user in Users.OrderBy(u => u.GetHashCode()))
                hash.Add(user);
            return hash.ToHashCode();
        }
    }

    internal class StateProvider
    {
        readonly Stack<State> _undo = [];
        readonly Stack<State> _redo = [];

        public void SaveState(State state)
        {
            _undo.Push(state);
            _redo.Clear();
        }

        public State? Undo(State state)
        {
            if(_redo.Count == 0) _redo.Push(state);
            else if (!_redo.Peek().Equals(state)) _redo.Push(state);
            var res = _undo.TryPop(out var result);
            return res ? result : null;
        }

        public State? Redo(State state)
        {
            if (_undo.Count == 0) _undo.Push(state);
            else if (!_undo.Peek().Equals(state)) _undo.Push(state);
            var res = _redo.TryPop(out var result);
            return res ? result : null;
        }
    }
}
