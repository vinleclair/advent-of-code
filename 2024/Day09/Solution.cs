namespace AdventOfCode._2024.Day09;

//TODO Optimize this bad boy by using linked list
public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var blocks = GetBlocks(input);
        
        var array = blocks.SelectMany(b => b).ToList();

        /*var length = array.Count;
        int dotIndex = 0;

        for (var i = length - 1; i >= 0; i--)
        {
            // Skip if current position is already a dot
            if (array[i] == ".") continue;

            // Find next available dot position
            while (dotIndex < i && array[dotIndex] != ".")
            {
                dotIndex++;
            }

            // If no dots found before current position, skip
            if (dotIndex >= i) continue;

            // Swap the current number with the dot
            array[dotIndex] = array[i];
            array[i] = ".";
        }*/
        for (var i = array.Count - 1; i >= 0; i--)
        {
            array[array.IndexOf(".")] = array[i];
            array[array.LastIndexOf(array[i])] = ".";
        }
        
        long checksum = 0;

        foreach (var (c, i) in array.Select((b, i) => (b, i)))
        {
            if (c != ".")
                checksum += int.Parse(c) * i;
        }

        return checksum;
    }

    public object PartTwo(string input)
    {
        var blocks = GetBlocks(input);

        /*for (var i = blocks.Count - 1; i >= 0; i--)
        {
            // Quick check if current block is all dots
            bool allDots = true;
            foreach (var item in blocks[i])
            {
                if (item != ".")
                {
                    allDots = false;
                    break;
                }
            }
            if (allDots) continue;

            // Find block to update
            List<string> blockToUpdate = null;
            int targetIndex = -1;
            int currentBlockCount = blocks[i].Count;
            string valueToMove = blocks[i][0];

            // Find first eligible block from top
            for (var j = 0; j < i; j++)
            {
                int dotCount = 0;
                foreach (var item in blocks[j])
                {
                    if (item == ".") dotCount++;
                    if (dotCount >= currentBlockCount)
                    {
                        blockToUpdate = blocks[j];
                        targetIndex = j;
                        break;
                    }
                }
                if (blockToUpdate != null) break;
            }

            if (blockToUpdate == null) continue;

            // Fill dots directly
            int filled = 0;
            for (var j = 0; j < blockToUpdate.Count && filled < currentBlockCount; j++)
            {
                if (blockToUpdate[j] == ".")
                {
                    blocks[targetIndex][j] = valueToMove;
                    filled++;
                }
            }

            // Replace current block with dots
            for (var j = 0; j < currentBlockCount; j++)
            {
                blocks[i][j] = ".";
            }
        }*/
        for (var i = blocks.Count - 1; i >= 0; i--)
        {
            if (blocks[i].All(b => b == ".")) continue;

            var blockToUpdate =
                blocks.FirstOrDefault(b => b.Count(f => f == ".") >= blocks[i].Count && blocks.IndexOf(b) < i);

            if (blockToUpdate == null) continue;
            
            var dotIndices = blockToUpdate
                .Select((value, index) => (value, index))
                .Where(x => x.value == ".")
                .Select(x => x.index)
                .Take(blocks[i].Count)
                .ToList();

            foreach (var t in dotIndices)
                blockToUpdate[t] = blocks[i][0];

            blocks[i] = blocks[i].Select(_ => ".").ToList();
        }

        long checksum = 0;

        foreach (var (c, i) in blocks.SelectMany(b => b).Select((b, i) => (b, i)))
        {
            if (c != ".")
                checksum += int.Parse(c) * i;
        }

        return checksum;
    }

    private static List<List<string>> GetBlocks(string diskMap)
    {
        var blocks = new List<List<string>>();

        foreach (var (block, index) in diskMap.Select((c, i) => (c, i)))
        {
            var t = new List<string>();
            for (var i = 0; i < int.Parse(block.ToString()); i++)
            {
                t.Add(index % 2 == 0 ? (index / 2).ToString() : ".");
            }

            if (t.Count > 0)
                blocks.Add(t);
        }

        return blocks;
    }
}