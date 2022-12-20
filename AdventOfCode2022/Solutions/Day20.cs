namespace AdventOfCode2022.Solutions;

public class Day20 : IAdventSolution
{
    private const long Key = 811589153;
    
    public object PartOne(string input)
    {
        var (zero, originalOrder) =  ParseInput(input);

        Mix(originalOrder);

        return CoordinatesFromList(zero);
    }
    
    public object PartTwo(string input)
    {
        var (zero, originalOrder) = ParseInput(input, Key);

        Mix(originalOrder);
        Mix(originalOrder);
        Mix(originalOrder);
        Mix(originalOrder);
        Mix(originalOrder);
        Mix(originalOrder);
        Mix(originalOrder);
        Mix(originalOrder);
        Mix(originalOrder);
        Mix(originalOrder);

        return CoordinatesFromList(zero);
    }

    private Tuple<Node, List<Node>> ParseInput(string input, long key = 1)
    {
        var originalOrder = new List<Node>();

        using var enumerator = input.Split(Environment.NewLine)
            .Select(int.Parse)
            .Select(v => v * key)
            .GetEnumerator();
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

        return new Tuple<Node, List<Node>>(originalOrder.Find(n => n.Value == 0), originalOrder);
    }

    private void Mix(List<Node> originalOrder)
    {
        var length = originalOrder.Count;
        
        foreach (var toMove in originalOrder)
        {
            if (toMove.Value == 0)
            {
                continue;
            }

            toMove.Next.Prev = toMove.Prev;
            toMove.Prev.Next = toMove.Next;

            var newPrev = toMove.Prev;
            var distanceToMove = toMove.Value % (length - 1);
            while (distanceToMove < 0)
            {
                distanceToMove += (length - 1);
            }
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
    }

    private long CoordinatesFromList(Node zero)
    {
        var sum = 0L;
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
        public readonly long Value;
        public Node?Next;
        public Node?Prev;

        public Node(long value, Node prev)
        {
            Value = value;
            Prev = prev;
        }
        
        public Node(long value)
        {
            Value = value;
        }

        public override string ToString() => $"Day20.Node[{Value}]";
    }
}