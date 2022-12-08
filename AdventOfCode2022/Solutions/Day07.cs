namespace AdventOfCode2022.Solutions;

public class Day07 : IAdventSolution
{
    public object PartOne(string input)
    {
        var root = ParseFileSystem(input);
        return root.SumOfAllSubDirectoriesAboveMinSize(100000);
    }

    private FileSystem ParseFileSystem(string input)
    {
        var root = new Directory("/");
        var path = new Stack<Directory>();
        var wd = root;
        
        foreach (var instruction in input.Split(Environment.NewLine))
        {
            if (instruction.StartsWith("$ cd "))
            {
                var dirName = instruction[5..];
                switch (dirName)
                {
                    case "..":
                        wd = path.Pop();
                        break;
                    case "/":
                        wd = root;
                        path.Clear();
                        break;
                    default:
                        var dir = wd.GetDir(dirName);
                        path.Push(wd);
                        wd = dir;
                        break;
                }
            }
            else if (instruction.StartsWith("$ ls"))
            {
                // nop
            } 
            else if (instruction.StartsWith("dir"))
            {
                var dirName = instruction[4..];
                wd.AddDir(dirName);
            } 
            else
            {
                var bits = instruction.Split(" ");
                var size = int.Parse(bits[0]);
                var filename = bits[1];
                wd.AddFile(filename, size);
            }
        }

        return root;
    }

    public object PartTwo(string input)
    {
        var root = ParseFileSystem(input);
        var spaceRemaining = 70000000 - root.TotalSize;
        var toClear = 30000000 - spaceRemaining;
        return root.AllSubDirSizes.Where(s => s >= toClear).Min();
    }

    private abstract class FileSystem
    {
        public abstract int SumOfAllSubDirectoriesAboveMinSize(int min);
        public abstract int TotalSize { get; }
        public abstract bool IsDir { get; }
        public abstract IEnumerable<int> AllSubDirSizes { get; }
    }
    
    private class Directory : FileSystem
    {
        private readonly Dictionary<string, FileSystem> _children = new ();
        private readonly string _name;

        public Directory(string name)
        {
            _name = name;
        }

        public Directory AddDir(string dirName)
        {
            if (_children.ContainsKey(dirName))
            {
                return (Directory)_children[dirName];
            }

            var dir = new Directory(dirName);
            _children[dirName] = dir;
            return dir;
        }

        public Directory GetDir(string dirName) => (Directory)_children[dirName];

        public File AddFile(string fileName, int fileSize)
        {
            if (_children.ContainsKey(fileName))
            {
                throw new ArgumentException("dir already contains file of name {0}", fileName);
            }

            var file = new File(fileName, fileSize);
            _children[fileName] = file;
            return file;
        }

        public override int SumOfAllSubDirectoriesAboveMinSize(int min)
        {
            var sizesOfChildDirs = _children.Values.Where(c => c.IsDir).Select(f => f.TotalSize).Where(s => s <= min).Sum();
            var sizeOfSubDirsAboveMinSize = _children.Values.Select(f => f.SumOfAllSubDirectoriesAboveMinSize(min)).Sum();
            return sizesOfChildDirs + sizeOfSubDirsAboveMinSize;
        }

        public override int TotalSize => _children.Values.Select(f => f.TotalSize).Sum();

        public override bool IsDir => true;

        public override IEnumerable<int> AllSubDirSizes
        {
            get
            {
                var childSizes = _children.Values.SelectMany(d => d.AllSubDirSizes).ToList();
                childSizes.Add(TotalSize);
                return childSizes;
            }
        }

        public override string ToString() => $"Directory[{_name}]";
    }
    
    private class File : FileSystem
    {
        public File(string name, int size)
        {
            Name = name;
            TotalSize = size;
        }

        public override int SumOfAllSubDirectoriesAboveMinSize(int min) => 0;
        
        private string Name { get; }
        
        public override int TotalSize { get; }
        
        public override bool IsDir => false;

        public override IEnumerable<int> AllSubDirSizes => new List<int>();

        public override string ToString() => $"File[{Name}, size={TotalSize}]";
    }
}