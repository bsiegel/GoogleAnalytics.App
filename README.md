# GoogleAnalytics.App - Google App Analytics library for Windows Phone and Windows RT
GoogleAnalytics.App is an unofficial SDK for interacting with Google Analytics. It is compatible with version 2 of the Google Analytics SDK for iOS and Android and uses Google Measurement Protocol to report app- and mobile-specific metrics.


# Get it on NuGet!
    Install-Package GoogleAnalytics.App


# API
The GoogleAnalytics.App API was designed to approximately match that of the official Google Analytics SDKs for iOS and Android. While it currently offers a subset of the features provided by the official SDKs, the hope is that it will grow over time to match the same level of functionality and to grow closer to the APIs offered by the official SDKs.

The tracking functions of the GoogleAnalytics.App API are all performed asynchronously, using the async/await functionality offered by C# 5. The Async Targetting Pack (Microsoft.Bcl.Async) is used to provide this functionality for Windows Phone 7 where async/await are not supported natively.


## Tracker
Tracker is the main object used to report analytics information. Obtain an instance of Tracker, and then call its `*Async` methods to track various types of activities.

### Constructors

### `Tracker(string trackingId)`
Initialize a new Tracker instance with the given tracking ID (e.g. "UA-XXXXXX-XX").

### Properties

### `bool UseHttps`
Optional. When true, send analytics messages over SSL. The default value for this property is `false`.

### `bool Anonymize`
Optional. When true, send a flag in each analytics message that instructs Google to anonymize the IP address of the sender. The default value for this property is `false`.

### `bool SessionStart`
Optional. When true, the next time a tracking call is made a parameter will be added to indicate that a new session should be started. This flag will then be cleared. When a Tracker is first instantiated, the default value for this property is `true`. Set this property to `false` before making a tracking call to prevent this default behavior.

### `bool ThrowOnErrors`
Optional. When true, if an error occurs during transmission of an analytics message, throw the exception. When false, if an error occurs during transmission of an analytics message, ignore the exception and return a TrackingResult object. The TrackingResult object will include the originating exception and will have its `Success` property set to `false`. The default value for this property is `false`.

### Methods

### `void SetCustom(int index, string dimension)`
Set the value of a custom dimension for all tracking calls. Each custom dimension has an associated index.

### `void SetCustom(int index, long metric)`
Set the value of a custom metric for all tracking calls. Each custom metric has an associated index.

### `async Task<TrackingResult> TrackViewAsync(string screen)`
Track that the specified view or screen was displayed.

### `async Task<TrackingResult> TrackEventAsync(string category, string action, [string label = null], [long? value = null])`
Track an event.

### `async Task<TrackingResult> TrackTransactionAsync(Transaction tran)`
Track a transaction.

## Transaction
Transaction is a structure that represents a transaction to report via Tracker's `TrackTransactionAsync` method. It contains various properties about the transaction itself, as well as zero or more TransactionItems.

### Properties

### `string OrderId`
Required. A unique identifier for the transaction.

### `string StoreName`
Optional. Specifies the affiliation or store name.

### `string Total`
Optional. Specifies the total revenue associated with the transaction.

### `string Tax`
Optional. Specifies the total tax of the transaction.

### `string Shipping`
Optional. Specifies the total shipping cost of the transaction.

### `List<TransactionItem> Items`
Optional. A list of items associated with the transaction.


## TransactionItem
TransactionItem is a structure that represents a single item associated with a transaction.

### Properties

### `string Price`
Optional. Specifies the price for a single item / unit.

### `long? Quantity`
Optional. Specifies the number of items purchased.

### `string Code`
Optional. Specifies the SKU or item code.

### `string Name`
Optional. Specifies the item name.

### `string Category`
Optional. Specifies the category that the item belongs to.


## TrackingResult
TrackingResult is a structure that represents the result of a call to one of Tracker's `*Async` tracking methods.

### Properties

### `string Url`
The URL to which the call was made.

### `Dictionary<string,string> Parameters`
The parameters sent during the call.

### `bool Success`
A flag indicating whether the call was successful.

### `Exception Exception`
The exception that occurred during the call, if any. This is only populated when the call fails (`Success` is set to `false`) and Tracker's `ThrowOnErrors` property is also set to `false` (the default).


# License
[MS-PL License](https://github.com/bsiegel/GoogleAnalytics.App/blob/master/LICENSE.md)


# Building the source
After cloning the repository, run build.cmd. A folder named "Build" will be created and populated with:

- Assemblies
- PDB files
- NuGet package
