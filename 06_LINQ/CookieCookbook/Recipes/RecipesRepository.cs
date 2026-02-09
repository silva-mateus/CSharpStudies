using CookieCookbook.DataAccess;
using CookieCookbook.Recipes.Ingredients;

namespace CookieCookbook.Recipes;

public class RecipesRepository : IRecipesRepository
{
    private readonly IStringsRepository _stringsRepository;
    private readonly IIngredientsRegister _ingredientsRegister;
    private const string Separator = ",";

    public RecipesRepository(
        IStringsRepository stringsRepository,
        IIngredientsRegister ingredientsRegister)
    {
        _stringsRepository = stringsRepository;
        _ingredientsRegister = ingredientsRegister;
    }

    public List<Recipe> Read(string filePath)
    {
        List<string> recipesFromFile = _stringsRepository.Read(filePath);

        return recipesFromFile
        .Select(recipeFromFile => RecipeFromString(recipeFromFile))
        .ToList();

        // foreach (var recipeFromFile in recipesFromFile)
        // {
        //     var recipe = RecipeFromString(recipeFromFile);
        //     recipes.Add(recipe);
        // }

    }

    private Recipe RecipeFromString(string recipeFromFile)
    {
        var textualIds = recipeFromFile.Split(Separator);
        var ingredients = new List<Ingredient>();

        return new Recipe(textualIds
                                    .Select(textualId => _ingredientsRegister
                                    .GetById(int.Parse(textualId))));

        // foreach (var textualId in textualIds)
        // {
        //     var id = int.Parse(textualId);
        //     var ingredient = _ingredientsRegister.GetById(id);
        //     ingredients.Add(ingredient);
        // }

        // return new Recipe(ingredients);
    }

    public void Write(string filePath, List<Recipe> allRecipes)
    {
        var recipesAsStrings = new List<string>();


        recipesAsStrings = allRecipes
                                    .Select(recipe => string.Join(Separator, recipe.Ingredients
                                    .Select(ingredient => ingredient.Id)))
                                    .ToList();
        // foreach (var recipe in allRecipes)
        // {
        //     var allIds = new List<int>();
        //     foreach (var ingredient in recipe.Ingredients)
        //     {
        //         allIds.Add(ingredient.Id);
        //     }
        //     recipesAsStrings.Add(string.Join(Separator, allIds));
        // }

        _stringsRepository.Write(filePath, recipesAsStrings);
    }
}
