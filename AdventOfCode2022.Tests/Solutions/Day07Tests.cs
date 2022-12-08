using AdventOfCode2022.Solutions;

namespace AdventOfCode2022.Tests.Solutions;

public class Day07Tests
{
    private const string ExampleInput = @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";
    
    [Fact]
    public void PartOneExample()
    {
        var result = new Day07().PartOne(ExampleInput);
        
        Assert.Equal(95437, result);
    }

    [Fact]
    public void PartTwoExample()
    {
        var result = new Day07().PartTwo(ExampleInput);
        
        Assert.Equal(24933642, result);
    }
}