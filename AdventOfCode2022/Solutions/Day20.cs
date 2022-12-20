namespace AdventOfCode2022.Solutions;

public class Day20 : IAdventSolution
{
    public object PartOne(string input)
    {
        var originalOrder = new List<Node>();

        using var enumerator = input.Split(Environment.NewLine).Select(int.Parse).GetEnumerator();
        enumerator.MoveNext();

        var root = new Node(enumerator.Current);
        originalOrder.Add(root);
        var prev = root;

        while (enumerator.MoveNext())
        {
            var next = new Node(enumerator.Current, prev);
            originalOrder.Add(next);
            prev.Next = next;
            prev = next;
        }

        prev.Next = root;
        root.Prev = prev;

        var length = originalOrder.Count;
        Node? zero = null;
        
        foreach (var toMove in originalOrder)
        {
            if (toMove.Value == 0)
            {
                zero = toMove;
                continue;
            }

            toMove.Next.Prev = toMove.Prev;
            toMove.Prev.Next = toMove.Next;

            var newPrev = toMove.Prev;
            var distanceToMove = (toMove.Value + 2 * length - 2) % (length - 1);
            while (distanceToMove > 0)
            {
                newPrev = newPrev.Next;
                --distanceToMove;
            }

            toMove.Next = newPrev.Next;
            newPrev.Next.Prev = toMove;
            toMove.Prev = newPrev;
            newPrev.Next = toMove;
        }

        if (zero == null)
        {
            throw new ArgumentException("no zero value found in the file");
        }

        var sum = 0;
        var current = zero;
        for (int i = 1; i <= 3000; ++i)
        {
            current = current.Next;
            if (i % 1000 == 0)
            {
                sum += current.Value;
            }
        }

        return sum;
    }

    class Node
    {
        public readonly int Value;
        public Node?Next;
        public Node?Prev;

        public Node(int value, Node prev)
        {
            Value = value;
            Prev = prev;
        }
        
        public Node(int value)
        {
            Value = value;
        }

        public override string ToString() => $"Day20.Node[{Value}]";
    }

    public object PartTwo(string input) => 0;
}