using MyBookPlannerAPI.ViewModels.UserBooks;

class DuplicatedItemsInCatalogListsRemover
{
    public static List<CatalogViewModel> RemoveDuplicates(List<CatalogViewModel> catalog)
    {
        // Group objects based on IdBook
        var groups = catalog.GroupBy(x => x.IdBook);

        // Creates a new list to keep the objects with the highest UserScore
        var result = new List<CatalogViewModel>();

        // Loops to each group of objects
        foreach (var group in groups)
        {
            // Gets the objecct with the highest UserScore in this group
            var objetoComMaiorScore = group.OrderByDescending(x => x.UserScore).First();

            // Adds the objects with the highest UserScore to the result
            result.Add(objetoComMaiorScore);
        }

        return result;
    }
}