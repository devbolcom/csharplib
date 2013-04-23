using Bol.OpenAPI;
using Bol.OpenAPI.Client;
using OpenAPI_Client.Request;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        private const string ACCESS_KEY_ID = "";
        private const string SECRET_ACCESS_KEY = "";

        static void Main(string[] args)
        {
            OpenApiClient client = new OpenApiClient(ACCESS_KEY_ID, SECRET_ACCESS_KEY);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Ping(client);
            Search(client);
            GetList(client);
            GetProduct(client);
            GetProductRecommendations(client);
            Basket(client);

            stopwatch.Stop();
            Console.WriteLine("< Execution time: " + stopwatch.Elapsed.Milliseconds + "ms");
            Console.ReadLine();
        }

        static void Ping(OpenApiClient client)
        {
            Console.WriteLine("====");
            Console.WriteLine("Performing Ping");
            Console.WriteLine("Ping responded with " + client.Ping());
            Console.WriteLine("====");
            Console.WriteLine();
        }

        static void Search(OpenApiClient client)
        {
            SearchResultsRequest searchResultsRequest = new SearchResultsRequest("halo");
            searchResultsRequest.IncludeCategories = true;
            searchResultsRequest.IncludeProducts = true;
            searchResultsRequest.IncludeRefinements = true;
            searchResultsRequest.IncludeAttributes = true;
            searchResultsRequest.NrProducts = 10;
            searchResultsRequest.Offset = 0;
            searchResultsRequest.SortingAscending = true;
            searchResultsRequest.SortingMethod = SearchResultsRequest.SearchSortingMethod.PRICE;

            Console.WriteLine("====");
            Console.WriteLine("Performing SearchResultsRequest");
            SearchResultsResponse searchResultsResponse = client.Search(searchResultsRequest);
            PrintSearch(searchResultsResponse);
            Console.WriteLine("====");
            Console.WriteLine();
        }

        private static void PrintSearch(SearchResultsResponse searchResultsResponse)
        {
            Console.WriteLine("Found " + searchResultsResponse.TotalResultSize + " products based on search term");
        }

        static void GetList(OpenApiClient client)
        {
            ListResultRequest listResultRequest = new ListResultRequest(ListResultRequest.ListType.TOPLIST_DEFAULT, "0");
            listResultRequest.IncludeCategories = true;
            listResultRequest.IncludeProducts = true;
            listResultRequest.IncludeRefinements = true;
            listResultRequest.IncludeAttributes = true;
            listResultRequest.NrProducts = 10;
            listResultRequest.Offset = 0;
            listResultRequest.SortingAscending = true;
            listResultRequest.SortingMethod = ListResultRequest.ListSortingMethod.PRICE;
            
            Console.WriteLine("====");
            Console.WriteLine("Performing ListResultRequest");
            ListResultResponse listResultResponse = client.GetList(listResultRequest);
            PrintList(listResultResponse);
            Console.WriteLine("====");
            Console.WriteLine();
        }

        private static void PrintList(ListResultResponse listResultResponse)
        {
            if (listResultResponse != null && listResultResponse.Category != null)
            {
                foreach (Category category in listResultResponse.Category)
                {
                    Console.WriteLine("- " + category.Name);
                }
                Console.WriteLine("Found " + listResultResponse.TotalResultSize + " products in product list");
            }
        }

        static void GetProduct(OpenApiClient client)
        {
            Console.WriteLine("====");
            Console.WriteLine("Performing GetProduct");
            ProductResponse productResponse = client.GetProduct("1004004011412184", null);
            Console.WriteLine("====");
            PrintProducts(productResponse);
            Console.WriteLine("====");
            Console.WriteLine();
        }

        private static void PrintProducts(ProductResponse productResponse)
        {
            if (productResponse != null && productResponse.Product != null)
            {
                PrintProduct(productResponse.Product);
            }
        }

        static void GetProductRecommendations(OpenApiClient client)
        {
            ProductRecommendationsRequest productRecommendationsRequest = new ProductRecommendationsRequest("1004004011412184a");
            productRecommendationsRequest.IncludeProducts = true;
            productRecommendationsRequest.IncludeAttributes = true;
            productRecommendationsRequest.NrProducts = 10;
            productRecommendationsRequest.Offset = 0;
                        
            Console.WriteLine("====");
            Console.WriteLine("Performing GetProductRecommendations");
            ProductRecommendationsResponse productRecommendationsResponse = client.GetProductRecommendations(productRecommendationsRequest);
            Console.WriteLine("====");
            PrintProductRecommendations(productRecommendationsResponse);
            Console.WriteLine("====");
            Console.WriteLine();
        }

        private static void PrintProductRecommendations(ProductRecommendationsResponse productRecommendationsResponse)
        {
            if (productRecommendationsResponse != null && productRecommendationsResponse.Product != null)
            {
                foreach (Product product in productRecommendationsResponse.Product)
                {
                    PrintProduct(product);
                }
            }
        }

        private static void PrintProduct(Product product)
        {
            Console.WriteLine("Id : " + product.Id);
            Console.WriteLine("Title : " + product.Title);
            Console.WriteLine("Price : " + product.Offers.Offer[0].Price);
        }

        static void Basket(OpenApiClient client)
        {
            Console.WriteLine("====");
            Console.WriteLine("Performing Basket operations");
            Console.WriteLine("====");
            Console.WriteLine("Performing GetAnonymousSession");
            string sessionId = client.GetAnonymousSession();            
            Console.WriteLine("Session Id : " + sessionId);
            Console.WriteLine("----");
            Console.WriteLine("Performing AddItemToBasket");
            client.AddItemToBasket(sessionId, 1004004011412184, 1, "127.0.0.1");
            BasketResponse basketResponse = client.GetBasket(sessionId);
            PrintBasket(basketResponse);
            Console.WriteLine("----");
            Console.WriteLine("Performing ChangeItemQuantity: 7");
            client.ChangeItemQuantity(sessionId, basketResponse.Basket.BasketItem[0].Id, 7);
            Console.WriteLine("----");
            Console.WriteLine("Performing GetBasket");
            basketResponse = client.GetBasket(sessionId);
            PrintBasket(basketResponse);
            Console.WriteLine("----");
            Console.WriteLine("Performing RemoveItemFromBasket");
            client.RemoveItemFromBasket(sessionId, basketResponse.Basket.BasketItem[0].Id);
            Console.WriteLine("----");
            Console.WriteLine("Performing GetBasket");
            basketResponse = client.GetBasket(sessionId);
            PrintBasket(basketResponse);            
            Console.WriteLine();
        }

        private static void PrintBasket(BasketResponse basketResponse)
        {
            if (basketResponse != null && basketResponse.Basket != null)
            {
                Console.WriteLine("Basket items : " + basketResponse.Basket.TotalQuantity);

                if (basketResponse.Basket.BasketItem != null)
                {
                    foreach (BasketItem basketItem in basketResponse.Basket.BasketItem)
                    {
                        Console.WriteLine("Title : " + basketItem.Product.Title);
                        Console.WriteLine("Quantity : " + basketItem.Quantity);
                        Console.WriteLine("Price : " + basketItem.Price);
                    }
                }
            }
        }
    }
}
