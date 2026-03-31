using System.Collections.Generic;

public class CraftingIngredients
{
    private readonly Dictionary<CraftingMaterial, int> _counts;

    public IReadOnlyDictionary<CraftingMaterial, int> Counts => _counts;

    public CraftingIngredients(IEnumerable<CraftingMaterial> materials)
    {
        _counts = new Dictionary<CraftingMaterial, int>();

        foreach (var mat in materials)
        {
            if (_counts.ContainsKey(mat))
                _counts[mat]++;
            else
                _counts[mat] = 1;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is not CraftingIngredients other)
            return false;

        if (_counts.Count != other._counts.Count)
            return false;

        foreach (var kvp in _counts)
        {
            if (!other._counts.TryGetValue(kvp.Key, out int otherCount))
                return false;

            if (kvp.Value != otherCount)
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        int hash = 17;

        foreach (var kvp in _counts)
        {
            // Combine material + count in an order-independent way
            int pairHash = kvp.Key.GetHashCode() ^ kvp.Value.GetHashCode();
            hash += pairHash;
        }

        return hash;
    }
}