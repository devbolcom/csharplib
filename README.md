Overview
===========================================================================
This library can be used to do the following requests:
- Ping: Pings the OpenAPI server.
-	Search: Searches for products.
-	GetList: Gets the product or category/refinement results list.
-	GetProduct: Gets the product.
-	GetProductRecommendations: Gets the product recommendations.
-	GetAnonymousSession: Returns an anonymousSession, which can be use to manage an anonymous basket.
-	GetBasket: Return the basket associated with given sessionId
-	AddItemToBasket: Adds the offer with offerId to basket, associated with the sessionId.
-	ChangeBasketItemQuantity: Changes the quantity of basketItem to given quantity.
-	RemoveBasketItemFromBasket: Removes basketItem from basket that is associated with the sessionId
This library uses XML Serializer to convert the xml-responses to equivalent C# objects.

Requirements
===========================================================================
-	.NET Framework 4.5

Howto:
===========================================================================

Basic examples:
---------------------------------------------------------------------------
Create a new instance of the OpenApiClient class, its constructor requires two parameters:
-	AccessKeyId: user's accessKeyId
-	SecretAccessKey: user's secretAccessKey

OpenApiClient client = new OpenApiClient(ACCESS_KEY_ID, SECRET_ACCESS_KEY);
client.Ping(); //ping bol.com openapi server.

Extended example:
----------------------------------------------------------------------------
Please check Program.cs file in the Example-project for more.
